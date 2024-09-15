using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework;
namespace MasterGame.Entities
{
    public interface IEntity{
        void takeDamage();
        void attack();
        void Update();
        void Draw();

    }
}