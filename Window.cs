using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    public class Window
    {
        public int height { get; set; }
        public int width { get; set; }

        public Window()
        {
            // gets game window size
            GraphicsAdapter graphics = new GraphicsAdapter();
            DisplayMode window = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            // assumes window size does not change throughout game
            width = window.Width;
            height = window.Height;
        }
    }
}
