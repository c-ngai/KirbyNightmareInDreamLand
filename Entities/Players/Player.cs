using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class Player : IPlayer
    {
        //no axis aligned collison?? 
        //BSP trees for collision optimization
        
        //this class will be refactored in next sprint to make another class: State management
        // and movement management so it is not doing this much
        // TODO: Is it possible to make this a public property so commands can access it?
        private PlayerStateMachine state;
        private PlayerMovement movement;
        private Sprite playerSprite ;
        private ICollidable collidable;

        private PlayerAttack attack;

        //health stuffs -- will be taken to another class connected to kirby in next sprint
        private int health = Constants.Kirby.MAX_HEALTH;
        private int lives = Constants.Kirby.MAX_LIVES;
        private bool invincible = false;
        private double timer = 0;

        //others
        private string oldState;
        private bool attackIsActive = false;

        //constructor
        public Player(Vector2 pos)
        {
            state = new PlayerStateMachine();
            movement = new NormalPlayerMovement(pos);
            oldState = state.GetStateString();
            collidable = new PlayerCollisionHandler((int) pos.X, (int) pos.Y, this);
        }
        public Sprite PlayerSprite
        {
            set{playerSprite = value;}
        }

        //changes kiry's texture if he is in a different state than before
        //only called by Draw
        public void UpdateTexture()
        {
            if(!state.GetStateString().Equals(oldState)){
                playerSprite = SpriteFactory.Instance.CreateSprite(state.GetSpriteParameters());
                oldState = state.GetStateString();
            } 
        }

        #region KirbyState
        public void ChangePose(KirbyPose pose)
        {
            state.ChangePose(pose);
        }
        public void ChangeMovement()
        {
            movement = new NormalPlayerMovement(movement.GetPosition());
        }
        public string GetKirbyPose()
        {
            return state.GetPose().ToString();
        }
        public string GetKirbyType()
        {
            return state.GetKirbyType().ToString();
        }
        public bool IsLeft(){
            return state.IsLeft();
        }
        public bool IsFloating()
        {
            return state.IsFloating();
        }

        public void ChangeAttackBool(bool activate)
        {
            attackIsActive = activate;
        }
        public Vector2 GetKirbyPosition()
        {
            return movement.GetPosition();
        }
        #endregion

        #region Power-Up
        public void ChangeToNormal()
        {
            state.ChangeType(KirbyType.Normal);
        }
        public void ChangeToBeam()
        {
            //attackSprite = new KirbyBeam(movement.GetPosition(), IsLeft());
            state.ChangeType(KirbyType.Beam);
        }
        public void ChangeToFire()
        {
            state.ChangeType(KirbyType.Fire);
        }
        public void ChangeToSpark()
        {
            state.ChangeType(KirbyType.Spark);
        }
        #endregion
        #region direction
        public void SetDirectionLeft()
        {
            if(!state.IsAttacking()){
                state.SetDirectionLeft();
            }
        }
        public void SetDirectionRight()
        {
            if(!state.IsAttacking()){
                state.SetDirectionRight();
            }
        }
        #endregion

        #region health 
        //health will be moved to another class
        public void Death() //does nothing this sprint
        {
            //state.ChangeType(KirbyType.Dead);
        }
        public void DecreaseHealth()
        {
            if(timer == 0)
            {
                health --;
            }
            if(health == 0)
            {
                health = Constants.Kirby.MAX_HEALTH;
                lives--;
            }
            if(lives == 0)
            {
                Death();
            }
        }
        //calls method to drecease health & changes kirby pose
        public async void TakeDamageAnimation()
        {
            ChangePose(KirbyPose.Hurt);
            await Task.Delay(Constants.Physics.DELAY);
            StopMoving();
        }
        public void EndInvinciblility(GameTime gameTime)
        {
            if(invincible){
                timer += gameTime.ElapsedGameTime.TotalSeconds; 
                if(timer > 5){
                    invincible = false;
                    timer = 0;
                }
            }
        }
        public void TakeDamage()
        {
            invincible = true;
            DecreaseHealth();
            TakeDamageAnimation();
            movement.ReceiveDamage(state.IsLeft());
        }
        #endregion

        #region Movement
        public void MoveLeft()
        {   
            SetDirectionLeft();
            movement.Walk(state.IsLeft());
            //check if kirby should change pose
            if(state.CanMove()){
                ChangePose(KirbyPose.Walking);
            }
        }

        public void MoveRight()
        {
            SetDirectionRight();
            movement.Walk(state.IsLeft());
            //walk connot override walking, jumping, floating, crouching, and attack
            if(state.CanMove()){
                ChangePose(KirbyPose.Walking);
            }
        }
        public void StopMoving() 
        {
            movement.StopMovement();
            if(state.CanStand())
            {
                ChangePose(KirbyPose.Standing);
            }
        }
        #region running
        public void RunLeft()
        {
            SetDirectionLeft();
            movement.Run(state.IsLeft());
           if(state.CanMove()){
                ChangePose(KirbyPose.Running);
            }
        }
        public void RunRight()
        {
            SetDirectionRight();
            movement.Run(state.IsLeft());
            if(state.CanMove()){
                ChangePose(KirbyPose.Running);
            }
        }
        #endregion

        #region jumping
        public void Jump()
        {
            if(state.CanJump()){ //not floating, not jumping, not crouching
                movement = new JumpMovement(movement.GetPosition());
                ChangePose(KirbyPose.JumpRising);
            }else if (state.IsJumping() && !state.IsFloating()){ //if jumping and x is pressed again
                //Float();
                movement.Jump(state.IsLeft());
            } else {
                //does nothing: sprint 3 will have a controller refactor 
                //that will allow for a use of this spot
            }
        }
        #endregion
        public void Float()
        {
            //crouching and sliding cannot be overwritten by float 
            if(state.CanFloat()){
                movement.StartFloating(this);
                movement = new FloatingMovement(movement.GetPosition());
                ChangePose(KirbyPose.FloatingRising);
            } 
            if(!state.IsCrouching()){ //if float is up arrow is pressed again it goes up
                ChangePose(KirbyPose.FloatingRising);
                movement.Jump(state.IsLeft()); //change this to flowting geenral movement
            }
        }

        #region crouch
        public void Crouch()
        {
            if(state.CanCrouch()){ //crouch does not overwrite jump and floating
                ChangePose(KirbyPose.Crouching);
                movement = new CrouchingMovement(movement.GetPosition());
            }
        }
        public void EndCrouch()
        {
            if(state.CanCrouch()){
                StopMoving();
                ChangeMovement();
            }
        }
        #endregion
        
        public async void Slide()
        {
            if(state.IsCrouching()){
                ChangePose(KirbyPose.Sliding);
                movement.Slide(state.IsLeft());
                await Task.Delay(Constants.Physics.DELAY);
            }
        }
        #endregion //movement region

        public void Attack()
        {
            ChangeAttackBool(true);
            attack = new PlayerAttack(this);
            movement.Attack(this);
        }

        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last?
        public void Update(GameTime gameTime)
        {
            movement.MovePlayer(this, gameTime);
            EndInvinciblility(gameTime);
            playerSprite.Update();
            collidable.UpdateBoundingBox(movement.GetPosition());
            if(attackIsActive){
                attack.Update(gameTime, this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateTexture();
            if(invincible){
                playerSprite.DamageDraw(movement.GetPosition(), spriteBatch);
            } else {
                playerSprite.Draw(movement.GetPosition(), spriteBatch);
            }
            if(attackIsActive){
                attack.Draw(spriteBatch, this);
            }
        }
    }

}

