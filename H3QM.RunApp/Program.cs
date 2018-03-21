using System;
using H3QM.RunApp.QMod;
using Microsoft.Win32;

namespace H3QM.RunApp
{
    class Program
    {
        static void Main()
        {
            QModMaker.ModGame(FindGameFolder());

            Console.ReadLine();
        }

        private static string FindGameFolder()
        {
            var registryKey = Registry.ClassesRoot.OpenSubKey(@"VirtualStore\MACHINE\SOFTWARE\WOW6432Node\New World Computing\Heroes of Might and Magic® III\1.0");
            var path = registryKey?.GetValue(@"AppPath")?.ToString();
            if (!string.IsNullOrEmpty(path)) return path;

            return Environment.CurrentDirectory;
        }
    }
}
