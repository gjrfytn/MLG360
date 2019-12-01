namespace MLG360.Model
{
    public class LootBox
    {
        public Vec2Double Position { get; set; }
        public Vec2Double Size { get; set; }
        public Items.Item Item { get; set; }

        public LootBox(Vec2Double position, Vec2Double size, Items.Item item)
        {
            Position = position;
            Size = size;
            Item = item;
        }

        public static LootBox ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new LootBox(Vec2Double.ReadFrom(reader), Vec2Double.ReadFrom(reader), Items.Item.ReadFrom(reader));

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            Position.WriteTo(writer);
            Size.WriteTo(writer);
            Item.WriteTo(writer);
        }
    }
}
