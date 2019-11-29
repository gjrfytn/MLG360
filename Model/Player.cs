namespace MLG360.Model
{
    public class Player
    {
        public int Id { get; set; }
        public int Score { get; set; }

        private Player()
        {
        }

        public Player(int id, int score)
        {
            Id = id;
            Score = score;
        }

        public static Player ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new Player();
            result.Id = reader.ReadInt32();
            result.Score = reader.ReadInt32();

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
