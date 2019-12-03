namespace MLG360.Model
{
    public class LootBox
    {
        public Vec2Double Position { get; }
        public Vec2Double Size { get; }
        public Items.Item Item { get; }

        public LootBox(Vec2Double position, Vec2Double size, Items.Item item)
        {
            Position = position;
            Size = size;
            Item = item;
        }

        public static LootBox ReadFrom(System.IO.BinaryReader reader)
        {
            return new LootBox(Vec2Double.ReadFrom(reader), Vec2Double.ReadFrom(reader), Items.Item.ReadFrom(reader));
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            Position.WriteTo(writer);
            Size.WriteTo(writer);
            Item.WriteTo(writer);
        }
    }
}
