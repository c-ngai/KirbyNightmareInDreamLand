﻿using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyFloatCommand : ICommand
    {
        Game1 _game;
        IPlayer _player;

        public KirbyFloatCommand()
        {
            _game = Game1.Instance;
            _player = _game.Players[0];
        }

        public void Execute()
        {
            // If kirby is not in a door, float
            if (!_game.Level.atDoor(_player.GetKirbyPosition()))
            {
                _player.Float();
            }
        }
    }
}
