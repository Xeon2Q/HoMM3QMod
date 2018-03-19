using System;

namespace H3QM.Models.HoMM3
{
    public class LodArchive
    {
        #region C-tor & properties

        public string ArchivePath { get; }

        public byte[] LodPrefix { get; }

        public byte[] Type { get; }

        public byte[] FilesCount { get; }

        public byte[] UnknownBytes { get; }

        public LodArchive(string archivePath, byte[] lodPrefix, byte[] type, byte[] filesCount, byte[] unknownBytes)
        {
            if (string.IsNullOrWhiteSpace(archivePath)) throw new ArgumentNullException(nameof(archivePath));

            ArchivePath = archivePath;
            LodPrefix = lodPrefix ?? throw new ArgumentNullException(nameof(lodPrefix));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            FilesCount = filesCount ?? throw new ArgumentNullException(nameof(filesCount));
            UnknownBytes = unknownBytes ?? throw new ArgumentNullException(nameof(unknownBytes));
        }

        #endregion
    }
}