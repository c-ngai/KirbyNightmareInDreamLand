using KirbyNightmareInDreamLand.Entities.Players;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyFloatCommand : ICommand
    {
        Game1 _game;
        // Reference to player list
        private List<IPlayer> _players;
        // Index of player to execute on
        private int playerIndex;
        public KirbyFloatCommand(int _playerIndex)
        {
            _game = Game1.Instance;
            _players = ObjectManager.Instance.Players;
            playerIndex = _playerIndex;
        }


        public void Execute()
        {
            // If a player of this index exists
            if (playerIndex < _players.Count)
            {
                // If kirby is not in a door, float
                if (!_game.Level.atDoor(_players[playerIndex].GetKirbyPosition()))
                {
                    _players[playerIndex].Float();
                }
            }
        }
    }
}
