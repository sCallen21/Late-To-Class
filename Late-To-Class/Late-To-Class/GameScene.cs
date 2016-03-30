using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Late_To_Class
{
    class GameScene: DrawableGameComponent
    {
        private List<GameComponent> componets;

        public List<GameComponent> getComponet
        {
            get { return componets; }
        }

        public GameScene(Game game) : base(game)
        {
            componets = new List<GameComponent>();
            Visible = false;
            Enabled = false;

        }

        public virtual void Show()
        {
            Visible = true;
            Enabled = true;

        }
        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent gc in componets) {
                if (gc.Enabled)
                {
                    gc.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent gc in componets)
            {
                if (gc is DrawableGameComponent && ((DrawableGameComponent)gc).Visible)
                {
                    ((DrawableGameComponent)gc).Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }
    }
}
