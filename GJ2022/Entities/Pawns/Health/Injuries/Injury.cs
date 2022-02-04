namespace GJ2022.Entities.Pawns.Health.Injuries
{
    public abstract class Injury
    {

        //Unique:
        //If an injury is unique it will never merge/stack with other identical injuries.
        //Set to false for injuries that are gradual (brain damage), but true for injuries that
        //are applied 1 time (cuts / regular damage)
        public abstract bool Unique { get; }

        //Amount of pain applied per point of damage
        public abstract float PainPerDamage { get; }

        //Amount of damage on this injury
        public float Damage { get; private set; }

    }
}
