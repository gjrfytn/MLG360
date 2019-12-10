namespace MLG360.Strategy
{
    internal class Movement
    {
        public HorizontalMovement Horizontal { get; }
        public VerticalMovement Vertical { get; }

        public Movement(HorizontalMovement horizontal, VerticalMovement vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}
