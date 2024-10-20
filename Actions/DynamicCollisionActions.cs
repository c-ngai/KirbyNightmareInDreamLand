using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Actions
{
    public class DynamicCollisionActions
    {
        public static void EntityCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player player = (Player)object2;
            Enemy enemy = (Enemy)object1;
            player.TakeDamage();
            enemy.TakeDamage();
        }
    }
}
