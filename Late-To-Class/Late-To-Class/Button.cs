using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Late_To_Class
{
    class Button
    {
        #region Variables
        Texture2D texture;
        Vector2 posistion;
        Rectangle rectangle;

        Color color = new Color(255, 255, 255, 255);
        bool down;
        public bool isClicked;
        #endregion

        #region Properties
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }            
        #endregion

        #region Load
        public void Load(Texture2D newTexture, Vector2 newPos)
        {
            texture = newTexture;
            posistion = newPos;

        }
        #endregion

        #region Update and Draw
        public void Update(MouseState mouse)
        {
            mouse = Mouse.GetState();

            rectangle = new Rectangle((int)posistion.X, (int)posistion.Y, texture.Width, texture.Height);
            Rectangle mouseRec = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRec.Intersects(rectangle))
            {
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                if (down) color.A += 3; else color.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                    color.A = 255;
                }
                else if (color.A < 255)
                {
                    color.A += 1;
                }

            }
            else
            {
                color.A = 255;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, posistion, color);
        }
        #endregion
    }
}
