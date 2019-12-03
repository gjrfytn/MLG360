namespace MLG360.Model
{
    public class PlayerView
    {
        public int MyId { get; }
        public Game Game { get; }

        public PlayerView(int myId, Game game)
        {
            MyId = myId;
            Game = game;
        }

        public static PlayerView ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            return new PlayerView(reader.ReadInt32(), Game.ReadFrom(reader));
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(MyId);
            Game.WriteTo(writer);
        }
    }
}
