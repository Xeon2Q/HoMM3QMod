using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using H3QM.Interfaces.Services;
using H3QM.Models.Data;

namespace H3QM.Services
{
    public class CreatureService : ICreatureService
    {
        #region Consts

        private static char Tab { get; } = (char) ConsoleKey.Tab;
        private static char Enter { get; } = (char) ConsoleKey.Enter;

        private static int ValueX { get; } = 10;
        private static int HpX { get; } = 13;
        private static int SpeedX { get; } = 14;
        private static int AttackX { get; } = 15;
        private static int DefenceX { get; } = 16;
        private static int DamageMinX { get; } = 17;
        private static int DamageMaxX { get; } = 18;
        private static int ShotsX { get; } = 19;

        #endregion

        #region ICreatureService implementation

        public int Update(CreatureTemplate creature, ref string data)
        {
            if (creature == null) throw new ArgumentNullException(nameof(creature));
            if (data == null) throw new ArgumentNullException(nameof(data));

            var creatureIndex = GetCreaturePosition(creature.Name, data);
            if (creatureIndex < 0) return -1;

            creatureIndex += 2; // skip carriage return symbols

            var results = new List<bool>();
            if (creature.Value >= 0) results.Add(UpdateStat(ValueX, creature.Value, creatureIndex, ref data));
            if (creature.Hp >= 0) results.Add(UpdateStat(HpX, creature.Hp, creatureIndex, ref data));
            if (creature.Speed >= 0) results.Add(UpdateStat(SpeedX, creature.Speed, creatureIndex, ref data));
            if (creature.Attack >= 0) results.Add(UpdateStat(AttackX, creature.Attack, creatureIndex, ref data));
            if (creature.Defence >= 0) results.Add(UpdateStat(DefenceX, creature.Defence, creatureIndex, ref data));
            if (creature.DamageMin >= 0) results.Add(UpdateStat(DamageMinX, creature.DamageMin, creatureIndex, ref data));
            if (creature.DamageMax >= 0) results.Add(UpdateStat(DamageMaxX, creature.DamageMax, creatureIndex, ref data));
            if (creature.Shots >= 0) results.Add(UpdateStat(ShotsX, creature.Shots, creatureIndex, ref data));

            if (results.All(q => q)) return 1;
            if (results.All(q => !q)) return -1;
            return 0;
        }

        #endregion

        #region Private methods

        private int GetCreaturePosition(string name, string data)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(data) || name.Length > data.Length) return -1;

            var nameForm = string.Concat((char) 13, (char) 10, name);
            return data.IndexOf(nameForm, 0, StringComparison.Ordinal);
        }

        private bool UpdateStat(int tabNo, int value, int datax, ref string data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (datax < 0 || data.Length < datax) throw new ArgumentOutOfRangeException(nameof(datax));
            if (tabNo < 0) throw new ArgumentOutOfRangeException(nameof(tabNo));

            var sb = new StringBuilder(data);
            var statx = -1;
            var tabsLeft = 0;
            for (var i = datax; i < sb.Length; i++)
            {
                if (sb[i] == Enter) break;

                if (tabNo == tabsLeft)
                {
                    statx = i;
                    break;
                }

                if (sb[i] == Tab && sb.Length > i + 1 && sb[i + 1] != Tab) tabsLeft++;
            }
            if (statx < 0) return false;

            var statxe = -1;
            for (var i = statx; i < sb.Length; i++)
            {
                if (char.IsDigit(sb[i])) continue;
                
                statxe = i;
                break;
            }
            if (statxe <= statx) return false;

            sb.Remove(statx, statxe - statx);
            sb.Insert(statx, value);

            data = sb.ToString();
            return true;
        }

        #endregion
    }
}