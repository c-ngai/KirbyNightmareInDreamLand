//using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Projectiles;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class PlayerAttack
    {
        private KirbyBeam beam;
        private KirbyFlamethrower flame;
        private IProjectile puff;

        private int counter = 0;
        public PlayerAttack(Player kirby)
        {
            beam = new KirbyBeam(GetBeamPosition(kirby), !kirby.IsLeft());
            flame = new KirbyFlamethrower();
            puff = new KirbyPuff(GetPuffPosition(kirby), new Vector2(kirby.IsLeft()? -1 : 1, 0));

        }
        public Vector2 GetBeamPosition(Player kirby)
        {
            Vector2 position = kirby.GetKirbyPosition();
            if(kirby.IsLeft()){
                position += Constants.Kirby.BEAM_ATTACK_OFFSET_LEFT;
            } else {
                position += Constants.Kirby.BEAM_ATTACK_OFFSET_RIGHT;
            }
            return position;
        }
        public Vector2 GetFlamePosition(Player kirby)
        {
            Vector2 position = kirby.GetKirbyPosition();
            if(kirby.IsLeft())
            {
                position += Constants.Kirby.FLAME_ATTACK_OFFSET_LEFT; //only need the x value to flip; 
            } else {
                position += Constants.Kirby.FLAME_ATTACK_OFFSET_RIGHT;
            }
            return position;
        }

        public Vector2 GetPuffPosition(Player kirby)
        {
            Vector2 position = kirby.GetKirbyPosition();
            if(kirby.IsLeft())
            {
                position += Constants.Kirby.PUFF_ATTACK_OFFSET  * new Vector2(-1, 0); //only need the x value to flip; 
            } else {
                position += Constants.Kirby.PUFF_ATTACK_OFFSET;
            }
            return position;
        }

        public void Update(GameTime gameTime, Player kirby)
        {
            if(kirby.GetKirbyPose().Equals("JumpFalling") && kirby.IsFloating()){
                puff.Update();
            } else if (kirby.GetKirbyType().Equals("Beam")){
                beam.Update();
            } else if (kirby.GetKirbyType().Equals("Fire")){
                flame.Update(gameTime, GetFlamePosition(kirby), new Vector2(kirby.IsLeft()? -1 : 1, 0));
            } else {
                //electric is not being implemented and dead does not have an attack
            }
        }
        public void DrawPuff(SpriteBatch spriteBatch, Player kirby)
        {
            if(counter < Constants.Kirby.PUFF_ATTACK_FRAMES){
                puff.Draw(spriteBatch);
            } else {
                counter = 0;
                kirby.ChangeAttackBool(false);
            }
        }
        public void DrawBeam(SpriteBatch spriteBatch, Player kirby)
        {
            if(counter < Constants.Kirby.BEAM_ATTACK_FRAMES){
                beam.Draw(spriteBatch);
            } else {
                counter = 0;
                kirby.ChangeAttackBool(false);
            }
        }
        public void DrawFire(SpriteBatch spriteBatch, Player kirby)
        {
            if(counter < Constants.Kirby.FLAME_ATTACK_FRAMES){
                flame.Draw(spriteBatch);
            } else {
                counter = 0;
                kirby.ChangeAttackBool(false);
            }
        }
        //this is specific for this sprint, sprint 3 is going to have all the 
        //kirby projectiles under the same interface so 
        //we can go through it in a list and run throguh the list
        public void Draw(SpriteBatch spriteBatch, Player kirby)
        {
            counter ++;
            if(kirby.GetKirbyPose().Equals("JumpFalling") && kirby.IsFloating()){
                DrawPuff(spriteBatch, kirby);
            } else if (kirby.GetKirbyType().Equals("Beam")){
                DrawBeam(spriteBatch, kirby);
            } else if (kirby.GetKirbyType().Equals("Fire")){
                DrawFire(spriteBatch, kirby);
            } else {
                //electric is not being implemented and dead does not have an attack
            }
        }

        


    }
}