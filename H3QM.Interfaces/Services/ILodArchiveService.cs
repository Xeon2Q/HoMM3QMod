using System.Collections.Generic;
using H3QM.Models.HoMM3;

namespace H3QM.Interfaces.Services
{
    public interface ILodArchiveService
    {
        IEnumerable<LodFile> GetFiles(string archivePath, out LodArchive archiveInfo);

        LodFile GetFile(string archivePath, string fileName);

        void SaveFiles(string archivePath, params LodFile[] files);

        byte[] Compress(byte[] data);

        byte[] Decompress(byte[] data);

        bool OptimizeLodArchive(string archivePath);
    }
}