namespace H3QM.Models.Data
{
    public static class KnownCreature
    {
        public static CreatureTemplate Titan { get; } = new CreatureTemplate("Titan", hp: 300, speed: 18, shots: 36, damageMin: 40, damageMax: 60, attack: 24, defence: 24);

        public static CreatureTemplate BoneDragon { get; } = new CreatureTemplate("Bone Dragon", hp: 200, speed: 13, damageMin: 25, damageMax: 50, attack: 25, defence: 20);

        public static CreatureTemplate GhostDragon { get; } = new CreatureTemplate("Ghost Dragon", hp: 300, speed: 19, damageMin: 50, damageMax: 50, attack: 30, defence: 30);

        public static CreatureTemplate PowerLich { get; } = new CreatureTemplate("Power Lich", speed: 9);

        public static CreatureTemplate VampireLord { get; } = new CreatureTemplate("Vampire Lord", speed: 12, damageMin: 8, damageMax: 10);

        public static CreatureTemplate WalkingDead { get; } = new CreatureTemplate("Walking Dead", speed: 5);

        public static CreatureTemplate Zombie { get; } = new CreatureTemplate("Zombie", speed: 6);
    }
}