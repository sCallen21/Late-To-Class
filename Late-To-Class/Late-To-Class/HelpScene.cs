using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Late_To_Class
{
    class HelpScene:GameScene
    {
        public HelpScene(Game game, Texture2D textureBack, Texture2D textureFront) : base(game)
        {
            getComponet.Add(new ImageComponet(game, textureBack, ImageComponet.DrawMode.Stretch));
            getComponet.Add(new ImageComponet(game, textureFront, ImageComponet.DrawMode.Center));
            
        }


    }
}
