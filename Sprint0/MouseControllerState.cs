using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    public class MouseControllerState
    {
        public int leftClick { get; set; }
        public int rightClick { get; set; }
        public int xPosition { get; set; }
        public int yPosition { get; set;  }
        public int quadrant { get; set; }

        public MouseControllerState()
        {
            leftClick = 0;
            rightClick = 0;

            xPosition = 0;
            yPosition = 0;
            quadrant = 0;

        }
    }
}
