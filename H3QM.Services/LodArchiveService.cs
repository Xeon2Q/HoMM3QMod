﻿using System;
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

        private readonly Encoding _encoding;

        public LodArchiveService(Encoding encoding)
        {
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

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

                    if (file.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))
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
                    if (file != null) WriteLodFileContent(stream, file);
                }
            }
        }

        public byte[] Compress(byte[] data)
        {
            var buffer = new byte[4096];
            var zc = new ZlibCodec(CompressionMode.Compress);
            zc.InitializeDeflate();

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

        public byte[] Decompress(byte[] data)
        {
            return ZlibStream.UncompressBuffer(data);
        }

        public bool OptimizeLodArchive(string archivePath)
        {
            using (var stream = File.Open(archivePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var info = GetArchiveInfo(archivePath, stream);
                var contentPosition = stream.Position;

                // load files info
                var files = new List<LodFile>();
                while (true)
                {
                    var file = GetLodFile(stream);
                    if (file == null) break;
                    
                    files.Add(file);
                }
                // load files content
                files.ForEach(q => ReadLodFileContent(stream, q));

                // truncate the files
                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(contentPosition);

                // delete duplicates
                files = files.Where(q => q != null).Distinct().ToList();

                // write files info
                stream.Seek(0, SeekOrigin.End);
                files.ForEach(q =>
                {
                    q.Position = stream.Position;

                    WriteBytes(stream, q.GetNameBytes());
                    WriteBytes(stream, q.GetOffsetBytes());
                    WriteBytes(stream, q.GetOriginalSizeBytes());
                    WriteBytes(stream, q.GetTypeBytes());
                    WriteBytes(stream, q.GetCompressedSizeBytes());
                });
                
                // write 128 bytes zero delimiter
                WriteBytes(stream, new byte[128]);

                // write files content
                files.ForEach(q =>
                {
                    q.Offset = (uint) stream.Position;

                    WriteBytes(stream, q.GetCompressedContentBytes());
                });

                // update offset info
                files.ForEach(q =>
                {
                    stream.Seek(q.Position + q.GetNameBytes().Length, SeekOrigin.Begin);

                    WriteBytes(stream, q.GetOffsetBytes());
                });

                // update files count
                stream.Seek(0, SeekOrigin.Begin);
                var filesCount = new byte[4];
                BitConverter.GetBytes(files.Count).CopyTo(filesCount, 0);
                WriteBytes(stream, filesCount, 8);
            }

            return true;
        }

        #endregion

        #region Private methods

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

        private LodFile GetLodFile(Stream stream)
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

            return new LodFile(_encoding, position, name, type, offset, originalSize, compressedSize);
        }

        private void ReadLodFileContent(Stream stream, LodFile file)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (file == null) throw new ArgumentNullException(nameof(file));

            stream.Seek(file.Offset, SeekOrigin.Begin);

            var length = file.CompressedSize > 0 ? file.CompressedSize : file.OriginalSize;
            var compressedContent = ReadBytes(stream, length);
            var originalContent = file.CompressedSize > 0 ? Decompress(compressedContent) : compressedContent;

            file.SetContent(originalContent, compressedContent);
        }

        private void WriteLodFileContent(Stream stream, LodFile file, bool forceSave = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (file == null) throw new ArgumentNullException(nameof(file));

            if (!file.IsChanged && !forceSave) return;

            // update offset
            file.Offset = (uint) stream.Seek(0, SeekOrigin.End);

            // write content
            WriteBytes(stream, file.GetCompressedContentBytes());

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