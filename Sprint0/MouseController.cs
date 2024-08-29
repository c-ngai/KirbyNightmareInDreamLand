using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint0;
public class MouseController : IController
{
    private Rectangle firstQuadrant;
    private Rectangle secondQuadrant;
    private Rectangle thirdQuadrant;
    private Rectangle fourthQuadrant;

    private Dictionary<MouseControllerState, ICommand> controllerMappings;     

    public MouseController(int width, int height)
    {
        // creates starting quadrant point 
        Point origin = new Point(0, 0);
        Point topBorderMidPoint = new Point(width / 2, 0);
        Point leftBorderMidPoint = new Point(0, height / 2);
        Point center = new Point(width / 2, height / 2);

        // creates size of quadrant
        Point size = new Point(width / 2, height / 2);

        // creates quadrants
        firstQuadrant = new Rectangle(origin, size);
        secondQuadrant = new Rectangle(topBorderMidPoint, size);
        thirdQuadrant = new Rectangle(leftBorderMidPoint, size);
        fourthQuadrant = new Rectangle(center, size);

        controllerMappings = new Dictionary<MouseControllerState, ICommand>();
        
    }

    public void RegisterCommand(MouseControllerState state, ICommand command)
    {
        controllerMappings.Add(state, command);
    }

    public void Update()
    {
        MouseState mouse = GetState();
    }
}