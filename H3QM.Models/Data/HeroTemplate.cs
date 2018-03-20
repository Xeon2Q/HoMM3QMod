using System;

namespace H3QM.Models.Data
{
    public class HeroTemplate
    {
        #region C-tor & Properties

        public string Name { get; }

        public string NewName { get; }

        public string Icon { get; }

        public string NewIcon { get; }

        public string SmallIcon { get; }

        public string NewSmallIcon { get; }

        public byte[] OriginalPattern { get; }

        public byte[] OriginalMapEdPattern { get; }

        public byte[] ModifiedPattern { get; }

        public byte[] ModifiedMapEdPattern { get; }

        public HeroTemplate(string name, string newName,
            string icon, string newIcon,
            string smallIcon, string newSmallIcon,
            byte[] originalPattern, byte[] modifiedPattern,
            byte[] originalMapEdPattern, byte[] modifiedMapEdPattern)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentNullException(nameof(name));

            NewName = newName;
            if (string.IsNullOrWhiteSpace(NewName)) NewName = Name;

            Icon = icon ?? throw new ArgumentNullException(nameof(icon));
            if (string.IsNullOrWhiteSpace(Icon)) throw new ArgumentNullException(nameof(icon));

            NewIcon = newIcon ?? throw new ArgumentNullException(nameof(newIcon));
            if (string.IsNullOrWhiteSpace(NewIcon)) NewIcon = Icon;

            SmallIcon = smallIcon ?? throw new ArgumentNullException(nameof(smallIcon));
            if (string.IsNullOrWhiteSpace(SmallIcon)) throw new ArgumentNullException(nameof(smallIcon));

            NewSmallIcon = newSmallIcon ?? throw new ArgumentNullException(nameof(newSmallIcon));
            if (string.IsNullOrWhiteSpace(NewSmallIcon)) NewSmallIcon = SmallIcon;

            OriginalPattern = originalPattern ?? throw new ArgumentNullException(nameof(originalPattern));
            OriginalMapEdPattern = originalMapEdPattern ?? throw new ArgumentNullException(nameof(originalMapEdPattern));

            ModifiedPattern = modifiedPattern ?? throw new ArgumentNullException(nameof(modifiedPattern));
            ModifiedMapEdPattern = modifiedMapEdPattern ?? throw new ArgumentNullException(nameof(modifiedMapEdPattern));
        }

        #endregion
    }
}