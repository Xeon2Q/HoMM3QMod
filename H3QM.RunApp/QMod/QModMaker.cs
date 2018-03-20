using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using H3QM.Models.Data;
using H3QM.Models.Enums;
using H3QM.Services;

namespace H3QM.RunApp.QMod
{
    public static class QModMaker
    {
        #region C-tor & Private fields

        private static Encoding Encoding { get; } = Encoding.GetEncoding(1251);

        #endregion

        #region Public methods

        public static void ModGame(string gameFolder)
        {
            Console.Clear();
            Console.WriteLine();

            // update game exe's
            GetGameFiles(gameFolder).ToList().ForEach(UpdateGameExe);

            // update mapeditor exe's
            GetMapEditorFiles(gameFolder).ToList().ForEach(UpdateMapEditor);

            // update hero portraits
            var files = GetPortraitsLodFiles(gameFolder).ToList();
            var mainStorageFile = files.FirstOrDefault(q => q.EndsWith("h3bitmap.lod", StringComparison.OrdinalIgnoreCase));
            files.ForEach(q => UpdateHeroPortraits(q, mainStorageFile));

            // update hero information
            GetHeroInfoFiles(gameFolder).ToList().ForEach(UpdateHeroInfos);

            // update creature
            GetCreatureFiles(gameFolder).ToList().ForEach(UpdateCreatures);
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

        private static void UpdateHeroPortraits(string lodFile, string mainStorageFile)
        {
            if (!File.Exists(lodFile)) return;

            if (string.IsNullOrWhiteSpace(mainStorageFile)) mainStorageFile = lodFile;

            var file = Path.GetFileName(lodFile);
            var service = new LodArchiveService(Encoding);

            var action = new Func<HeroTemplate, bool>(hero => {
                var largeIcon = service.GetFile(lodFile, hero.Icon);
                var largeIconNew = service.GetFile(mainStorageFile, hero.NewIcon);
                var smallIcon = service.GetFile(lodFile, hero.SmallIcon);
                var smallIconNew = service.GetFile(mainStorageFile, hero.NewSmallIcon);

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

                if (largeIcon == null && smallIcon == null) return false;

                Console.Write($@"[{file}] Updating portraits ""{hero.Name}"": ");
                service.SaveFiles(lodFile, largeIcon, smallIcon);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK!");
                Console.ResetColor();
                return true;
            });

            var a1 = action(KnownHero.Orrin);
            a1 = action(KnownHero.SirMullich) || a1;
            a1 = action(KnownHero.Dessa) || a1;

            if (a1) Console.WriteLine();
        }

        private static void UpdateHeroInfos(string lodFile)
        {
            if (!File.Exists(lodFile)) return;

            var lodArchiveService = new LodArchiveService(Encoding);
            var biosFile = lodArchiveService.GetFile(lodFile, @"HeroBios.txt");
            var nameFile = lodArchiveService.GetFile(lodFile, @"HOTRAITS.TXT");
            if (biosFile == null && nameFile == null) return;

            var file = Path.GetFileName(lodFile);
            var biosString = biosFile?.OriginalContent;
            var nameString = nameFile?.OriginalContent;

            var action = new Action<HeroTemplate>(hero => {
                if (hero.Name.Equals(hero.NewName)) return;
                Console.Write($@"[{file}] Updating hero info ""{hero.Name}"" -> ""{hero.NewName}"" (Name/Bios): ");

                var hasName = nameString?.Contains(hero.Name) ?? false;
                if (hasName)
                {
                    nameString = nameString.Replace(hero.Name, hero.NewName);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("NOT CHANGED");
                }

                Console.ResetColor();
                Console.Write("/");

                var hasBios = biosString?.Contains(hero.Name) ?? false;
                if (hasBios)
                {
                    biosString = biosString.Replace(hero.Name, hero.NewName);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("NOT CHANGED");
                }

                Console.ResetColor();
                Console.WriteLine();
            });

            action(KnownHero.Orrin);
            action(KnownHero.SirMullich);
            action(KnownHero.Dessa);

            nameFile?.SetContent(Encoding.GetBytes(nameString), lodArchiveService.Compress(Encoding.GetBytes(nameString)));
            biosFile?.SetContent(Encoding.GetBytes(biosString), lodArchiveService.Compress(Encoding.GetBytes(biosString)));
            lodArchiveService.SaveFiles(lodFile, nameFile, biosFile);

            Console.WriteLine();
        }

        private static void UpdateCreatures(string lodFile)
        {
            if (!File.Exists(lodFile)) return;

            var lodArchiveService = new LodArchiveService(Encoding);
            var creatureFile = lodArchiveService.GetFile(lodFile, @"CRTRAITS.TXT");
            if (creatureFile == null) return;

            var file = Path.GetFileName(lodFile);
            var creatureService = new CreatureService();
            var originalContentString = creatureFile.OriginalContent;

            var action = new Action<CreatureTemplate>(creature => {
                Console.Write($@"[{file}] Updating creaute ""{creature.Name}"": ");
                var result = creatureService.Update(creature, ref originalContentString);

                if (result > 0) Console.ForegroundColor = ConsoleColor.Green;
                else if (result == 0) Console.ForegroundColor = ConsoleColor.Yellow;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result >= 0 ? "OK!" : "NOT CHANGED");
                Console.ResetColor();
            });

            action(KnownCreature.BoneDragon);
            action(KnownCreature.GhostDragon);
            action(KnownCreature.PowerLich);
            action(KnownCreature.Titan);
            action(KnownCreature.VampireLord);
            action(KnownCreature.WalkingDead);
            action(KnownCreature.Zombie);

            creatureFile.SetContent(Encoding.GetBytes(originalContentString), lodArchiveService.Compress(Encoding.GetBytes(originalContentString)));
            lodArchiveService.SaveFiles(lodFile, creatureFile);

            Console.WriteLine();
        }

        private static void UpdateMovementArrows(string lodFile)
        {
            if (!File.Exists(lodFile)) return;

            var file = Path.GetFileName(lodFile);
            var service = new LodArchiveService(Encoding);

            var action = new Func<HeroTemplate, bool>(hero => {
                var largeIcon = service.GetFile(lodFile, hero.Icon);
                var largeIconNew = service.GetFile("", hero.NewIcon);

                if (largeIcon != null && largeIconNew != null)
                {
                    largeIcon.SetContent(largeIconNew.GetOriginalContentBytes(), largeIconNew.GetCompressedContentBytes());
                }
                else
                {
                    largeIcon = null;
                }

                if (largeIcon == null) return false;

                Console.Write($@"[{file}] Updating movement arrows: ");
                service.SaveFiles(lodFile, largeIcon);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK!");
                Console.ResetColor();
                return true;
            });

            var a1 = action(KnownHero.Orrin);
            a1 = action(KnownHero.SirMullich) || a1;
            a1 = action(KnownHero.Dessa) || a1;

            if (a1) Console.WriteLine();
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

        private static IEnumerable<string> GetCreatureFiles(string folder)
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

        private static IEnumerable<string> GetHeroInfoFiles(string folder)
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

        private static IEnumerable<string> GetMovementArrowsFiles(string folder)
        {
            var files = new List<string>();

            var action = new Action<string>(fileName => {
                var path = Path.Combine(folder, "Data", fileName);
                if (File.Exists(path)) files.Add(path);
            });

            action("H3sprite.lod");
            action("HotA.lod");

            return files.Distinct().AsEnumerable();
        }

        #endregion
    }
}