using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework;
namespace MasterGame.Entities.PlayerSprites
{
    public interface IPlayer:IEntity
    {
        void setDirection();

    }
}