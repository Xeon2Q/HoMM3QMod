using System;
using H3QM.Models.Enums;

namespace H3QM.Models.HoMM3
{
    public class Hero
    {
        #region C-tor & properties

        public string Name { get; set; }

        public HeroSexEnum Sex { get; }

        public HeroClassEnum Class { get; }

        public bool HasBook { get; }

        public SecondarySkillWithLevel[] Skills { get; }

        public Hero(string name, HeroSexEnum sex, HeroClassEnum @class, bool hasBook, params SecondarySkillWithLevel[] skills)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
            Sex = sex;
            Class = @class;
            HasBook = hasBook;
            Skills = skills ?? new SecondarySkillWithLevel[0];
        }

        #endregion
    }
}