using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Late_To_Class
{
    class ImageComponet
    {
        public enum DrawMode
        {
            center = 1, Stretch,
        };

        private Texture2D texture;
        private DrawMode drawmode;
        private Rectangle rect;
        private SpriteBatch spritebatch = null;



    }
}
