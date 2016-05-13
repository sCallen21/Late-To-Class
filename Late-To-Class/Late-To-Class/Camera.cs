using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Chris Banks
namespace Late_To_Class
{
    /// <summary>
    /// basic camera that is locked to the player and follows him across the screen, while within the bounds of the map
    /// </summary>
   public class Camera
   {
       #region Variables
       Matrix transform;
       Vector2 vCentre;
       Viewport viewport;
       public Rectangle cameraView;
       #endregion

       #region Properties
       public Matrix Transform
        {
            get { return transform; }
        }
       #endregion

       #region Constructors
       public Camera(Viewport newViewport)
        {
            viewport = newViewport;
            cameraView = new Rectangle(0, 0, viewport.Width, viewport.Height);
        }
       #endregion

       #region Update
       /// <summary>
       /// Controls the Camera, so that the player is always on screen no matter where they go in the map
       /// </summary>
       /// <param name="position"></param>
       /// <param name="xOffset"></param>
       /// <param name="yOffset"></param>
       public void Update(Vector2 position, int xOffset, int yOffset)
       {
           if (position.X < viewport.Width / 2) { vCentre.X = viewport.Width / 2; }
           else if (position.X > xOffset - (viewport.Width / 2)) { vCentre.X = xOffset - viewport.Width / 2; }
           else { vCentre.X = position.X; }

           if (position.Y < viewport.Height / 2) { vCentre.Y = viewport.Height / 2; }
           else if (position.Y > yOffset - (viewport.Height / 2)) { vCentre.Y = yOffset - viewport.Height / 2; }
           else { vCentre.Y = position.Y; }

           cameraView = new Rectangle((int)position.X - viewport.Width / 2, (int)position.Y - viewport.Height / 2, viewport.Width, viewport.Height);

           //actually moves the camera
           transform = Matrix.CreateTranslation(new Vector3(-vCentre.X + (viewport.Width / 2),
                                                              -vCentre.Y + (viewport.Height / 2), 0));
       }
       #endregion
   }
}
