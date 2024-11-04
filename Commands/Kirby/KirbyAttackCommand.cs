using KirbyNightmareInDreamLand.Entities.Players;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackCommand : ICommand
    {
        // Reference to player list
        private List<IPlayer> _players;
        // Index of player to execute on
        private int playerIndex;
        public KirbyAttackCommand(int _playerIndex)
        {
            _players = ObjectManager.Instance.Players;
            playerIndex = _playerIndex;
        }

        public void Execute()
        {
            // If a player of this index exists
            if (playerIndex < _players.Count)
            {
                _players[playerIndex].Attack();
            }
        }
    }
}
