namespace MLG360.Strategy
{
    internal class Action
    {
        public HorizontalMovement HorizontalMovement { get; }
        public VerticalMovement VerticalMovement { get; }
        public WeaponOperation WeaponOperation { get; }

        public Action(HorizontalMovement horizontalMovement, VerticalMovement verticalMovement, WeaponOperation weaponOperation)
        {
            //if (aim.LengthSquared() > 1)
            //    throw new System.ArgumentException("Should aim with unit vector.", nameof(aim));

            HorizontalMovement = horizontalMovement;
            VerticalMovement = verticalMovement;
            WeaponOperation = weaponOperation;
        }
    }
}