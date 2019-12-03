using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace MLG360
{
    public class Runner : System.IDisposable
    {
        private readonly TcpClient _TcpClient;
        private readonly BinaryReader _Reader;
        private readonly BinaryWriter _Writer;

        public Runner(string host, int port, string token)
        {
            _TcpClient = new TcpClient(host, port) { NoDelay = true };
            var stream = new BufferedStream(_TcpClient.GetStream());

            _Reader = new BinaryReader(stream);
            _Writer = new BinaryWriter(stream);

            var tokenData = Encoding.UTF8.GetBytes(token);

            _Writer.Write(tokenData.Length);
            _Writer.Write(tokenData);
            _Writer.Flush();
        }

        public void Run()
        {
            var myStrategy = new MyStrategy();
            var debug = new Debug(_Writer);
            while (true)
            {
                var message = Model.ServerMessageGame.ReadFrom(_Reader);
                if (message.PlayerView == null)
                {
                    break;
                }

                var playerView = message.PlayerView;
                var actions = new Dictionary<int, Model.UnitAction>();
                foreach (var unit in playerView.Game.Units)
                {
                    if (unit.PlayerId == playerView.MyId)
                    {
                        actions.Add(unit.Id, myStrategy.GetAction(unit, playerView.Game, debug));
                    }
                }

                new Model.PlayerMessageGame.ActionMessage(new Model.Versioned(actions)).WriteTo(_Writer);
                _Writer.Flush();
            }
        }
        public static void Main(string[] args)
        {
            var host = args.Length < 1 ? "127.0.0.1" : args[0];
            var port = args.Length < 2 ? 31001 : int.Parse(args[1]);
            var token = args.Length < 3 ? "0000000000000000" : args[2];
            using (var runner = new Runner(host, port, token))
            {
                runner.Run();
            }
        }

        #region IDisposable Support

        private bool _Disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    _Reader.Dispose();
                    _Writer.Dispose();
                    _TcpClient.Dispose();
                }

                _Disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        #endregion
    }
}