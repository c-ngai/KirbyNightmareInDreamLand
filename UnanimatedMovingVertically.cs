using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Sprint0
{
    public class UnanimatedMovingVertically : Sprite
    {
        // Texture atlas
        public Texture2D Texture { get; set; }
        // Number of rows in the texture atlas
        public int Rows { get; set; }
        // Number of columns in the texture atlas
        public int Columns { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        private int CurrentFrame;
        private int TotalFrames;


        public UnanimatedMovingVertically(Texture2D texture, int rows, int columns, Vector2 location)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            CurrentFrame = 0;
            TotalFrames = Rows * Columns;
            x = (int)location.X;
        }

        public void Update()
        {

            if (y < 450)
            {
                y+= 5;
            }
            else if (y >= 450)
            {
                y = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = CurrentFrame / Columns;
            int column = CurrentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int) location.X, y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
