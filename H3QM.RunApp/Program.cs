using System;
using System.Linq;
using H3QM.Interfaces.Services;
using H3QM.Models.Data;
using H3QM.Models.Enums;
using H3QM.Services;

namespace H3QM.RunApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IChangeExeService exeService = new ChangeExeService();
            var orrin = exeService.ChangeHero(@"X:\Games\HoMM3X\h3hota_maped.exe", MarkerEnum.MapEditorMarker, KnownHero.Orrin.OriginalPattern, KnownHero.Orrin.ModifiedPattern);

            Console.WriteLine("[EXE] Orrin: " + (orrin ? " OK!" : "NOT CHANGED!"));

            ILodArchiveService lodArchiveService = new LodArchiveService();
            var files = lodArchiveService.GetFiles(@"X:\Games\HoMM3X\Data\H3bitmap.lod", out var _);

            var sfiles = files.Where(q => q.Name.Equals(KnownHero.Orrin.Icon)).ToArray();

            //File.WriteAllBytes($@"X:\{KnownHero.Orrin.Icon}.bmp", Encoding.GetEncoding(1251).GetBytes(file.OriginalContent));

            Console.ReadLine();
        }
    }
}
