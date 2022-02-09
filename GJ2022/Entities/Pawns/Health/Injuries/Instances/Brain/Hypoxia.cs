namespace GJ2022.Entities.Pawns.Health.Injuries.Instances.Brain
{
    public class Hypoxia : Injury
    {
        public Hypoxia(float damage) : base(damage)
        {
        }

        public override string Name => "Cerebral Hypoxia";

        public override bool Unique => false;

        public override float PainPerDamage => 0;

    }
}
