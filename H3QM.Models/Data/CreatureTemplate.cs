using System;

namespace H3QM.Models.Data
{
    public class CreatureTemplate
    {
        #region C-tor & Properties

        public string Name { get; }

        public int Value { get; }

        public int Hp { get; }

        public int Speed { get; }

        public int DamageMin { get; }

        public int DamageMax { get; }

        public int Attack { get; }

        public int Defence { get; }

        public int Shots { get; }

        public CreatureTemplate(string name, int value = -1, int hp = -1, int speed = -1, int damageMin = -1, int damageMax = -1, int attack = -1, int defence = -1, int shots = -1)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
            Value = value;
            Hp = hp;
            Speed = speed;
            DamageMin = damageMin;
            DamageMax = damageMax;
            Attack = attack;
            Defence = defence;
            Shots = shots;
        }

        #endregion
    }
}