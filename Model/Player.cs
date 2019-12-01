namespace MLG360.Model
{
    public class Player
    {
        public int Id { get; }
        public int Score { get; }

        public Player(int id, int score)
        {
            Id = id;
            Score = score;
        }

        public static Player ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new Player(reader.ReadInt32(), reader.ReadInt32());

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(Id);
            writer.Write(Score);
        }
    }
}
