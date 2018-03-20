using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using H3QM.Models.Data;
using H3QM.Models.Enums;
using H3QM.Services;

namespace H3QM.RunApp.QMod
{
    public static class QModMaker
    {
        #region Public methods

        public static void ModGame(string gameFolder)
        {
            Console.Clear();
            Console.WriteLine();

            // update game exe's
            GetGameFiles(gameFolder).ToList().ForEach(UpdateGameExe);

            // update mapeditor exe's
            GetMapEditorFiles(gameFolder).ToList().ForEach(UpdateMapEditor);

            // update portraits
            GetPortraitsLodFiles(gameFolder).ToList().ForEach(UpdateHeroPortraits);
        }

        #endregion

        #region Private methods

        private static void UpdateGameExe(string exeFile)
        {
            if (!File.Exists(exeFile)) return;

            var file = Path.GetFileName(exeFile);
            var service = new ChangeExeService();

            var func = new Func<HeroTemplate, bool>(hero => {
                Console.Write($@"[{file}] Updating ""{hero.Name}"": ");

                var result = service.ChangeHero(exeFile, MarkerEnum.GameMarker, hero.OriginalPattern, hero.ModifiedPattern);

                Console.ForegroundColor = result ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(result ? "OK!" : "NOT CHANGED");
                Console.ResetColor();
                return result;
            });

            func(KnownHero.Orrin);
            func(KnownHero.SirMullich);
            func(KnownHero.Dessa);

            Console.WriteLine();
        }

        private static void UpdateMapEditor(string exeFile)
        {
            if (!File.Exists(exeFile)) return;

            var file = Path.GetFileName(exeFile);
            var service = new ChangeExeService();

            var func = new Func<HeroTemplate, bool>(hero => {
                Console.Write($@"[{file}] Updating ""{hero.Name}"": ");

                var result = service.ChangeHero(exeFile, MarkerEnum.MapEditorMarker, hero.OriginalMapEdPattern, hero.ModifiedMapEdPattern);

                Console.ForegroundColor = result ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(result ? "OK!" : "NOT CHANGED");
                Console.ResetColor();
                return result;
            });

            func(KnownHero.Orrin);
            func(KnownHero.SirMullich);
            func(KnownHero.Dessa);

            Console.WriteLine();
        }

        private static void UpdateHeroPortraits(string lodFile)
        {
            if (!File.Exists(lodFile)) return;

            var file = Path.GetFileName(lodFile);
            var service = new LodArchiveService();

            var action = new Action<HeroTemplate>(hero => {
                var largeIcon = service.GetFile(lodFile, hero.Icon);
                var largeIconNew = service.GetFile(lodFile, hero.NewIcon);
                var smallIcon = service.GetFile(lodFile, hero.SmallIcon);
                var smallIconNew = service.GetFile(lodFile, hero.NewSmallIcon);

                if (largeIcon != null && largeIconNew != null)
                {
                    largeIcon.SetContent(largeIconNew.GetOriginalContentBytes(), largeIconNew.GetCompressedContentBytes());
                }
                else
                {
                    largeIcon = null;
                }

                if (smallIcon != null && smallIconNew != null) {
                    smallIcon.SetContent(smallIconNew.GetOriginalContentBytes(), smallIconNew.GetCompressedContentBytes());
                }
                else
                {
                    smallIcon = null;
                }

                if (largeIcon == null && smallIcon == null) return;

                Console.Write($@"[{file}] Updating portraits ""{hero.Name}"": ");
                service.SaveFiles(lodFile, largeIcon, smallIcon);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK!");
                Console.ResetColor();
            });

            action(KnownHero.Orrin);
            action(KnownHero.SirMullich);
            action(KnownHero.Dessa);

            Console.WriteLine();
        }

        #endregion

        #region Info methods

        private static IEnumerable<string> GetGameFiles(string folder)
        {
            var files = new List<string>();

            var action = new Action<string>(fileName => {
                var path = Path.Combine(folder, fileName);
                if (File.Exists(path)) files.Add(path);
            });

            action("h3wog.exe");
            action("h3hota.exe");
            action("Heroes3.exe");

            return files.Distinct().AsEnumerable();
        }

        private static IEnumerable<string> GetMapEditorFiles(string folder)
        {
            var files = new List<string>();

            var action = new Action<string>(fileName => {
                var path = Path.Combine(folder, fileName);
                if (File.Exists(path)) files.Add(path);
            });

            action("h3maped.exe");
            action("h3wmaped.exe");
            action("h3hota_maped.exe");

            return files.Distinct().AsEnumerable();
        }

        private static IEnumerable<string> GetPortraitsLodFiles(string folder)
        {
            var files = new List<string>();

            var action = new Action<string>(fileName => {
                var path = Path.Combine(folder, "Data", fileName);
                if (File.Exists(path)) files.Add(path);
            });

            action("H3bitmap.lod");
            action("H3sprite.lod");
            action("HotA.lod");
            action("HotA_lng.lod");

            return files.Distinct().AsEnumerable();
        }

        #endregion
    }
}