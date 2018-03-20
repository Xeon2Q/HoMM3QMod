using System;
using System.Globalization;
using System.IO;
using System.Text;
using H3QM.RunApp.Properties;
using H3QM.RunApp.QMod;
using H3QM.Services;

namespace H3QM.RunApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //QModMaker.ModGame(@"X:\Zoid");
            //QModMaker.ModGame(@"X:\Games\HoMM3X\");

            var dd = new LodArchiveService(Encoding.GetEncoding(1251));
            var adag = dd.GetFile(@"X:\Games\HoMM3X\Data\HotA.lod", "adag.def");

            var adagRes = Resources.ResourceManager..GetObject("ADAG");

            File.WriteAllBytes(@"X:\ADAG.DEF", adag.GetCompressedContentBytes());

            Console.WriteLine("DONE!");
            Console.ReadLine();
        }
    }
}
