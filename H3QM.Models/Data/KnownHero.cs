using H3QM.Models.Enums;

namespace H3QM.Models.Data
{
    public static class KnownHero
    {
        #region Orrin

        public static HeroTemplate Orrin { get; } = new HeroTemplate(
            "Orrin", "Brutallus",
            "HPL000KN.pcx", "HPL133Nc.pcx",

            // exe pattern
            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                7, 0, 0, 0,
                (byte) HeroClassEnum.Knight, 0, 0, 0,
                (byte) SecondarySkillEnum.Leadership, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                (byte) SecondarySkillEnum.Archery, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                0, 0, 0, 0,
                0xFF, 0xFF, 0xFF, 0xFF
            },

            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                7, 0, 0, 0,
                (byte) HeroClassEnum.DeathKnight, 0, 0, 0,
                (byte) SecondarySkillEnum.Wisdom, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                (byte) SecondarySkillEnum.Archery, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                1, 0, 0, 0,
                0xFF, 0xFF, 0xFF, 0xFF
            },

            // maped pattern
            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                7, 0, 0, 0,
                (byte) HeroClassEnum.Knight, 0, 0, 0,
                (byte) SecondarySkillEnum.Leadership, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                (byte) SecondarySkillEnum.Archery, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                0, 0, 0, 0,
                0xFF, 0xFF, 0xFF, 0xFF
            },

            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                7, 0, 0, 0,
                (byte) HeroClassEnum.DeathKnight, 0, 0, 0,
                (byte) SecondarySkillEnum.Wisdom, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                (byte) SecondarySkillEnum.Archery, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                1, 0, 0, 0,
                0xFF, 0xFF, 0xFF, 0xFF
            }
        );

        #endregion

        #region Sir Mullich

        public static HeroTemplate SirMullich { get; } = new HeroTemplate(
            "Sir Mullich", "Behemoth",
            "HPL130Kn.pcx", "HPL135Wi.pcx",

            // exe pattern
            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                7, 0, 0, 0,
                (byte) HeroClassEnum.Knight, 0, 0, 0,
                (byte) SecondarySkillEnum.Leadership, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Advanced, 0, 0, 0,
                0xFF, 0xFF, 0xFF, 0xFF
            },

            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                7, 0, 0, 0,
                (byte) HeroClassEnum.DeathKnight, 0, 0, 0,
                (byte) SecondarySkillEnum.Wisdom, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Advanced, 0, 0, 0,
                0xFF, 0xFF, 0xFF, 0xFF
            },

            // maped pattern
            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                7, 0, 0, 0,
                (byte) HeroClassEnum.Knight, 0, 0, 0,
                (byte) SecondarySkillEnum.Leadership, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Advanced, 0, 0, 0,
                0xFF, 0xFF, 0xFF, 0xFF
            },

            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                7, 0, 0, 0,
                (byte) HeroClassEnum.DeathKnight, 0, 0, 0,
                (byte) SecondarySkillEnum.Wisdom, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Advanced, 0, 0, 0,
                0xFF, 0xFF, 0xFF, 0xFF
            }
        );

        #endregion

        #region Dessa

        public static HeroTemplate Dessa { get; } = new HeroTemplate(
            "Dessa", "Anathema",
            "HPL106BM.pcx", "HPL000SH.pcx",

            // exe pattern
            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                0x0B, 0, 0, 0,
                (byte) HeroClassEnum.BattleMage, 0, 0, 0,
                (byte) SecondarySkillEnum.Wisdom, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                (byte) SecondarySkillEnum.Logistics, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                0x01, 0, 0, 0,
                0x2E, 0, 0, 0,
                0x54, 0, 0, 0,
                0x56, 0, 0, 0,
                0x58, 0, 0, 0,
                0x04, 0xE4, 0x67, 0
            },

            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                0x0B, 0, 0, 0,
                (byte) HeroClassEnum.DeathKnight, 0, 0, 0,
                (byte) SecondarySkillEnum.Wisdom, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                (byte) SecondarySkillEnum.Logistics, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                0x01, 0, 0, 0,
                0x2E, 0, 0, 0,
                0x54, 0, 0, 0,
                0x56, 0, 0, 0,
                0x58, 0, 0, 0,
                0x04, 0xE4, 0x67, 0
            },

            // maped pattern
            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                0x0B, 0, 0, 0,
                (byte) HeroClassEnum.BattleMage, 0, 0, 0,
                (byte) SecondarySkillEnum.Wisdom, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                (byte) SecondarySkillEnum.Logistics, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                0x01, 0, 0, 0,
                0x2E, 0, 0, 0,
                0x54, 0, 0, 0,
                0x56, 0, 0, 0,
                0x58, 0, 0, 0,
                0x34, 0xAE, 0x58, 0
            },

            new byte[]
            {
                (byte) HeroSexEnum.Male, 0, 0, 0,
                0x0B, 0, 0, 0,
                (byte) HeroClassEnum.DeathKnight, 0, 0, 0,
                (byte) SecondarySkillEnum.Wisdom, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                (byte) SecondarySkillEnum.Logistics, 0, 0, 0,
                (byte) SecondarySkillLevelEnum.Basic, 0, 0, 0,
                0x01, 0, 0, 0,
                0x2E, 0, 0, 0,
                0x54, 0, 0, 0,
                0x56, 0, 0, 0,
                0x58, 0, 0, 0,
                0x34, 0xAE, 0x58, 0
            }
        );

        #endregion
    }
}