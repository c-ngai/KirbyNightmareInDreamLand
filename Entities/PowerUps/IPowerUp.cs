using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.PowerUps
{
    public interface IPowerUp
    {
        public void Draw();
        public void Update();
        public void UsePowerUp();
    }
}
