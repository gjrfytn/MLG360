namespace MLG360.Model
{
    public class PlayerView
    {
        public int MyId { get; set; }
        public Game Game { get; set; }

        private PlayerView()
        {
        }

        public PlayerView(int myId, Game game)
        {
            MyId = myId;
            Game = game;
        }

        public static PlayerView ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new PlayerView();
            result.MyId = reader.ReadInt32();
            result.Game = Game.ReadFrom(reader);

            return result;
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
