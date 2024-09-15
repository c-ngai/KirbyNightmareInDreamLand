using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework;

namespace MasterGame.Entities.Enemies
{
    public interface IEnemy : IEntity
    {
        void ChangeDirection();

    }
}