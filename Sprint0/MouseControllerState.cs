using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    public class MouseControllerState
    {

        private bool leftClick { get; set; }
        private bool rightClick { get; set; }

        private int xPosition { get; set; }
        private int yPosition { get; set;  }

        public MouseControllerState()
        {
            leftClick = false;
            rightClick = false;

            xPosition = 0;
            yPosition = 0;

        }
    }
}
