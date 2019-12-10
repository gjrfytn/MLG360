namespace MLG360.Strategy
{
    internal class Action
    {
        public Movement Movement { get; }
        public WeaponOperation WeaponOperation { get; }

        public Action(Movement movement, WeaponOperation weaponOperation)
        {
            //if (aim.LengthSquared() > 1)
            //    throw new System.ArgumentException("Should aim with unit vector.", nameof(aim));

            Movement = movement;
            WeaponOperation = weaponOperation;
        }
    }
}