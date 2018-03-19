using H3QM.Models.Enums;

namespace H3QM.Models.HoMM3
{
    public class SecondarySkillWithLevel
    {
        #region C-tor & properties

        public SecondarySkillEnum Skill { get; }

        public SecondarySkillLevelEnum Level { get; }

        public SecondarySkillWithLevel(SecondarySkillEnum skill, SecondarySkillLevelEnum level)
        {
            Skill = skill;
            Level = level;
        }

        #endregion
    }
}