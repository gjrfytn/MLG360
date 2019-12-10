using MLG360.Model;
using MLG360.Strategy;
using System.Linq;

namespace MLG360
{
    internal class Scoretable : IScoretable
    {
        private readonly Game _Game;

        public Scoretable(Game game)
        {
            _Game = game;
        }

        public int GetPlayerScore(int id) => _Game.Players.Single(p => p.Id == id).Score;
    }
}
