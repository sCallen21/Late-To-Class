﻿using System;
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
        Texture2D texture;
        Vector2 posistion;
        Rectangle rectangle;

        Color color = new Color(255, 255, 255, 255);
        bool down;
        public bool isClicked;

        public bool IsClicked {
            get { return isClicked; }
            set { isClicked = value; }
        }

        public void load(Texture2D newTexture, Vector2 newPos)
        {
            texture = newTexture;
            posistion = newPos;

        }

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
                else if (color.A < 255) {
                    color.A += 1;
                }

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, posistion, color);
        }
    }
}