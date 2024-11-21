using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyEnterDoorCommand : ICommand
    {
        Game1 _game;
        // Reference to player list
        private List<IPlayer> _players;
        // Index of player to execute on
        private int playerIndex;
        public KirbyEnterDoorCommand(int _playerIndex)
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
                if (_game.Level.atDoor(_players[playerIndex].GetKirbyPosition()))
                {
                    SoundManager.Play("enterdoor");
                }
                else
                {
                    SoundManager.Play("float");
                }

                _players[playerIndex].ChangePose(StateMachines.KirbyPose.EnterDoor);
                _game.Level.EnterDoorAt(_players[playerIndex].GetKirbyPosition());
            } 
        }
    }
}
