using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KirbyNightmareInDreamLand
{
    public class PlayerAttackCollisionHandler : ICollidable
    {
        public Rectangle BoundingBox { get; private set; }
        public bool IsDynamic { get; private set; } = true;
        public bool IsActive { get; private set; } = true;
        private Dictionary<string, AttackHitBoxProperties> attackProperties;
        private AttackHitBoxProperties properties;
        bool isLeft;
        public PlayerAttackCollisionHandler(Vector2 position, string attack, bool isFacingLeft)
        {
            InitializeHitBoxDictionary();
            isLeft = isFacingLeft;
            attackProperties.TryGetValue(attack, out properties);
            position += properties.GetPosition(isLeft, position);
            BoundingBox = new Rectangle((int) position.X, (int) position.Y, properties.Width, properties.Height);

            CollisionDetection.Instance.RegisterDynamicObject(this);
        }
        public void InitializeHitBoxDictionary()
        {
           attackProperties = new Dictionary<string, AttackHitBoxProperties>
            {
                { "Beam", new AttackHitBoxProperties(Constants.HitBoxes.BEAM_OFFSET, Constants.HitBoxes.BEAM_OFFSET, 
                        Constants.HitBoxes.BEAM_SIZE, Constants.HitBoxes.BEAM_SIZE) },

                { "Fire", new AttackHitBoxProperties(Constants.HitBoxes.FIRE_OFFSET_LEFT, Constants.HitBoxes.FIRE_OFFSET_RIGHT, 
                        Constants.HitBoxes.FIRE_SIZE, Constants.HitBoxes.FIRE_SIZE) },

                { "Puff", new AttackHitBoxProperties(Constants.HitBoxes.PUFF_OFFSET, Constants.HitBoxes.PUFF_OFFSET, 
                        Constants.HitBoxes.PUFF_SIZE, Constants.HitBoxes.PUFF_SIZE) },

                { "Normal", new AttackHitBoxProperties(Constants.HitBoxes.NORMA_OFFSET_LEFT, Constants.HitBoxes.NORMAL_OFFSET_RIGHT, 
                        Constants.HitBoxes.NORMAL_SIZE, Constants.HitBoxes.NORMAL_SIZE) },

                { "Spark", new AttackHitBoxProperties(Constants.HitBoxes.SPARK_OFFSET, Constants.HitBoxes.SPARK_OFFSET, 
                        Constants.HitBoxes.SPARK_SIZE, Constants.HitBoxes.SPARK_SIZE) },

                { "Slide", new AttackHitBoxProperties(Constants.HitBoxes.SLIDE_OFFSET_LEFT, Constants.HitBoxes.SLIDE_OFFSET_RIGHT, 
                        Constants.HitBoxes.SLIDE_WIDTH, Constants.HitBoxes.SLIDE_HEIGHT) }
            };
        }
        public void DestroyHitBox()
        {
            IsActive = false;  // Mark enemy as inactive
        }
        public void EnableHitBox()
        {
            IsActive = true;  // Mark enemy as inactive
        }

        public void OnCollision(ICollidable other)
        {
            //does nothing
        }  
        public void UpdateBoundingBox(Vector2 position)
        {
            position = properties.GetPosition(isLeft, position);
            BoundingBox = new Rectangle((int) position.X, (int) position.Y, 
                properties.Width, properties.Height);
        }
    }
    public struct AttackHitBoxProperties
    {
        public Vector2 OffSetLeft { get; }
        public Vector2 OffSetRight { get; }
        public int Width { get; }
        public int Height { get; }

        public AttackHitBoxProperties(Vector2 offSetL,Vector2 offSetR, int width, int height)
        {
            OffSetLeft = offSetL;
            OffSetRight = offSetR;
            Width = width;
            Height = height;
        }

        public Vector2 GetOffset(bool isLeft)
        {
            return isLeft ? OffSetLeft : OffSetRight;
        }
        public Vector2 GetPosition(bool isLeft, Vector2 pos)
        {
            Vector2 offset = GetOffset(isLeft);
            return pos += offset;
        }
    }
}