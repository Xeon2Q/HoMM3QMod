using System;
using System.IO;
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
            var registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Heroes3.exe");
            var path = registryKey?.GetValue(string.Empty)?.ToString();
            if (!string.IsNullOrEmpty(path)) return Path.GetDirectoryName(path);

            registryKey = Registry.ClassesRoot.OpenSubKey(@"VirtualStore\MACHINE\SOFTWARE\WOW6432Node\New World Computing\Heroes of Might and Magic® III\1.0");
            path = registryKey?.GetValue(@"AppPath")?.ToString();
            if (!string.IsNullOrEmpty(path)) return path;

            return Environment.CurrentDirectory;
        }
    }
}
