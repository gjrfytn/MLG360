namespace MLG360.Model
{
    public class ServerMessageGame
    {
        public PlayerView PlayerView { get; set; }

        public ServerMessageGame(PlayerView playerView)
        {
            PlayerView = playerView;
        }

        public static ServerMessageGame ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            PlayerView playerView;
            if (reader.ReadBoolean())
            {
                playerView = PlayerView.ReadFrom(reader);
            }
            else
            {
                playerView = null;
            }

            var result = new ServerMessageGame(playerView);

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            if (PlayerView == null)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                PlayerView.WriteTo(writer);
            }
        }
    }
}
