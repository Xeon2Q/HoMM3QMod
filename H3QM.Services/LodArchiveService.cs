using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using H3QM.Interfaces.Services;
using H3QM.Models.HoMM3;
using Ionic.Zlib;

namespace H3QM.Services
{
    public class LodArchiveService : ILodArchiveService
    {
        #region C-tor & Private fields

        private static readonly Encoding Encoding = Encoding.GetEncoding(1251);

        #endregion

        #region ILodArchiveService implementation

        public IEnumerable<LodFile> GetFiles(string archivePath, out LodArchive archiveInfo)
        {
            var files = new List<LodFile>();

            using (var stream = File.Open(archivePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                stream.Seek(0, SeekOrigin.Begin);

                archiveInfo = GetArchiveInfo(archivePath, stream);

                // load file info
                while (true)
                {
                    var file = GetLodFile(stream);
                    if (file == null) break;
                    
                    files.Add(file);
                }
                // load file content
                files.ForEach(q => ReadLodFileContent(stream, q));
            }

            return files;
        }

        public LodFile GetFile(string archivePath, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));

            using (var stream = File.Open(archivePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                stream.Seek(0, SeekOrigin.Begin);

                GetArchiveInfo(archivePath, stream);

                // load file info
                while (true)
                {
                    var file = GetLodFile(stream);
                    if (file == null) break;

                    if (file.Name.Equals(fileName))
                    {
                        ReadLodFileContent(stream, file);
                        return file;
                    }
                }
            }

            return null;
        }

        public void SaveFiles(string archivePath, params LodFile[] files)
        {
            if (files == null || !files.Any()) throw new ArgumentNullException(nameof(files));

            using (var stream = File.Open(archivePath, FileMode.Open, FileAccess.Write, FileShare.Read))
            {
                foreach (var file in files)
                {
                    WriteLodFileContent(stream, file);
                }
            }
        }

        public byte[] Compress(byte[] data)
        {
            var buffer = new byte[4096];
            var zc = new ZlibCodec(CompressionMode.Compress);
            zc.InitializeDeflate(CompressionLevel.Default, 15, true);

            zc.InputBuffer = data;
            zc.NextIn = 0;
            zc.AvailableBytesIn = data.Length;
            zc.OutputBuffer = buffer;

            using (var ms = new MemoryStream())
            {
                do
                {
                    zc.NextOut = 0;
                    zc.AvailableBytesOut = buffer.Length;
                    zc.Deflate(FlushType.None);

                    ms.Write(zc.OutputBuffer, 0, buffer.Length - zc.AvailableBytesOut);
                }
                while (zc.AvailableBytesIn > 0 || zc.AvailableBytesOut == 0);

                do {
                    zc.NextOut = 0;
                    zc.AvailableBytesOut = buffer.Length;
                    zc.Deflate(FlushType.Finish);

                    if (buffer.Length - zc.AvailableBytesOut > 0) ms.Write(buffer, 0, buffer.Length - zc.AvailableBytesOut);
                }
                while (zc.AvailableBytesIn > 0 || zc.AvailableBytesOut == 0);

                zc.EndDeflate();
                return ms.ToArray();
            }
        }

        #endregion

        #region Private methods
 
        private static byte[] Decompress(byte[] data)
        {
            return ZlibStream.UncompressBuffer(data);
        }

        private static byte[] ReadBytes(Stream stream, ulong byteCount)
        {
            var bytes = new byte[byteCount];
            for (ulong i = 0; i < byteCount; i++)
            {
                bytes[i] = (byte)stream.ReadByte();
            }
            return bytes;
        }

        private static void WriteBytes(Stream stream, byte[] bytes, long offset = -1)
        {
            if (offset >= 0) stream.Seek(offset, SeekOrigin.Begin);

            stream.Write(bytes, 0, bytes.Length);
        }

        private static LodArchive GetArchiveInfo(string archivePath, Stream stream)
        {
            var lodPrefix = ReadBytes(stream, 4);
            var type = ReadBytes(stream, 4);
            var filesCount = ReadBytes(stream, 4);
            var unknownBytes = ReadBytes(stream, 80);

            return new LodArchive(archivePath, lodPrefix, type, filesCount, unknownBytes);
        }

        private static LodFile GetLodFile(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var position = stream.Position;
            var name = ReadBytes(stream, 16);
            // not a file
            if (name.All(q => q == 0)) return null;

            var offset = ReadBytes(stream, 4);
            var originalSize = ReadBytes(stream, 4);
            var type = ReadBytes(stream, 4);
            var compressedSize = ReadBytes(stream, 4);

            return new LodFile(Encoding, position, name, type, offset, originalSize, compressedSize);
        }

        private static void ReadLodFileContent(Stream stream, LodFile file)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (file == null) throw new ArgumentNullException(nameof(file));

            stream.Seek(file.Offset, SeekOrigin.Begin);

            var length = file.CompressedSize > 0 ? file.CompressedSize : file.OriginalSize;
            var compressedContent = ReadBytes(stream, length);
            var originalContent = file.CompressedSize > 0 ? Decompress(compressedContent) : compressedContent;

            file.SetContent(originalContent, compressedContent);
        }

        private static void WriteLodFileContent(Stream stream, LodFile file)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (file == null) throw new ArgumentNullException(nameof(file));

            if (!file.IsChanged) return;

            // write content
            file.Offset = (uint) stream.Seek(0, SeekOrigin.End);
            var content = Encoding.GetBytes(file.CompressedContent);
            WriteBytes(stream, content);

            // write info
            stream.Seek(file.Position, SeekOrigin.Begin);
            WriteBytes(stream, file.GetNameBytes());
            WriteBytes(stream, file.GetOffsetBytes());
            WriteBytes(stream, file.GetOriginalSizeBytes());
            WriteBytes(stream, file.GetTypeBytes());
            WriteBytes(stream, file.GetCompressedSizeBytes());
        }

        #endregion
    }
}