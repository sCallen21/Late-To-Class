using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
/// <summary>
/// XNA GAME PROGRAMMING BOOK by: Alendro Labao
/// </summary>
namespace Late_To_Class
{
    class ImageComponent:DrawableGameComponent
    {
        public enum DrawMode
        {
            Center = 1,
            Stretch
        }

        private Texture2D texture;
        private DrawMode drawMode;
        private SpriteBatch spriteBatch = null;
        private Rectangle imageRect;

        public ImageComponent(Game game, Texture2D texture, DrawMode drawMode) : base(game)
        {
            this.texture = texture;
            this.drawMode = drawMode;
            
            //gets current spritebatch
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            //create  a rect. with the size and position of the image
            switch (drawMode)
            {
                case DrawMode.Center:
                    imageRect = new Rectangle((Game.Window.ClientBounds.Width - texture.Width) / 2, (Game.Window.ClientBounds.Height - texture.Height) / 2, texture.Width, texture.Height);
                    break;
                case DrawMode.Stretch:
                    imageRect = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
                    break;

            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, imageRect, Color.White);
            base.Draw(gameTime);
            spriteBatch.End();

        }
    }
}
