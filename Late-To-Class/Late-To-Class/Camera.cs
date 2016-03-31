using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Late_To_Class
{
    /// <summary>
    /// basic camera that is locked to the player and follows him across the screen, while within the bounds of the map
    /// </summary>
   public class Camera
    {
        private Matrix transform;
        private Vector2 centre;
        private Viewport viewport;
        public Rectangle cameraView;
        

        

        public Matrix Transform
        {
            get { return transform; }
        }

       public Camera(Viewport newViewport)
        {
            viewport = newViewport;
            cameraView = new Rectangle(0, 0, viewport.Width, viewport.Height);
        }

        //fucks around with the camera position based on the player position and the size(in pixels) of the screen
       public void Update(Vector2 position, int xOffset, int yOffset)
       {
           if (position.X < viewport.Width / 2) { centre.X = viewport.Width / 2; }
           else if (position.X > xOffset - (viewport.Width / 2)) { centre.X = xOffset - viewport.Width / 2; }
           else { centre.X = position.X; }

           if (position.Y < viewport.Height / 2) { centre.Y = viewport.Height / 2; }
           else if (position.Y > yOffset - (viewport.Height / 2)) { centre.Y = yOffset - viewport.Height / 2; }
           else { centre.Y = position.Y; }

           cameraView = new Rectangle((int)position.X - viewport.Width / 2, (int)position.Y - viewport.Height / 2, viewport.Width, viewport.Height);

           //actually moves the camera
           transform = Matrix.CreateTranslation(new Vector3(-centre.X + (viewport.Width / 2),
                                                              -centre.Y + (viewport.Height / 2), 0));

            

       }
    }
}
