using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;



namespace Late_To_Class
{
    class StartScene : GameScene
    {
        private TextMenuComponet menu;
        private AudioComponet audioComponet;
        private Texture2D elements;
        private Cue backMusic;

        private SpriteBatch spriteBatch = null;

        //GUI
        private Rectangle LRect = new Rectangle(0, 0, 536, 131);
        private Vector2 LPos;
        private Rectangle cRect = new Rectangle(120, 165, 517, 130);
        private Vector2 cPos;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="game">Main game object</param>
        /// <param name="smallFont">Font for the menu items</param>
        /// <param name="largeFont">Font for the menu selcted item</param>
        /// <param name="background">Texture for background image</param>
        /// <param name="elements">Texture with the foreground elements</param>
        public StartScene(Game game, SpriteFont smallFont, SpriteFont largeFont, Texture2D background, Texture2D elements): base(game)
        {
            this.elements = elements;
            getComponet.Add(new ImageComponet(game, background,
                                            ImageComponet.DrawMode.Center));

            // Create the Menu
            string[] items = { "Play", "Help", "Quit" };
            menu = new TextMenuComponet(game, smallFont, largeFont);
            menu.SetMenuItems(items);
            getComponet.Add(menu);

            // Get the current spritebatch
            spriteBatch = (SpriteBatch)Game.Services.GetService(
                                            typeof(SpriteBatch));
            // Get the current audiocomponent and play the background music
            audioComponet = (AudioComponet)
                Game.Services.GetService(typeof(AudioComponet));
        }

        public override void Update(GameTime gameTime)
        {
            if (!menu.Visible)
            {
                if (LPos.X >= ((Game.Window.ClientBounds.Width - 595)/ 2))
                {
                    LPos.X -= 15;
                }
                if (LPos.X >= ((Game.Window.ClientBounds.Width - 715) / 2))
                {
                    LPos.X += 15;
                }
                else
                {
                    menu.Visible = true;
                    menu.Enabled = true;
                    backMusic.Play();
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
            spriteBatch.Draw(elements, LPos, LRect, Color.White);
            spriteBatch.Draw(elements, cPos, cRect, Color.White);
        }
        public override void Show()
        {
            //audioComponet.PlayCue("newmeteor");
            //backMusic = audioComponet.GetCue("startmusic");

            LRect.X = -1 * LRect.Width;
            LPos.Y = 40;
            cPos.X = Game.Window.ClientBounds.Width;
            cPos.Y = 180;
            // Put the menu centered in screen
            menu.Pos = new Vector2((Game.Window.ClientBounds.Width -
                                          menu.Width) / 2, 330);

            // These elements will be visible when the 'Rock Rain' title
            // is done.
            menu.Visible = false;
            menu.Enabled = false;

            base.Show();
        }

        /// <summary>
        /// Hide the start scene
        /// </summary>
        public override void Hide()
        {
            backMusic.Stop(AudioStopOptions.Immediate);
            base.Hide();
        }

        /// <summary>
        /// Gets the selected menu option
        /// </summary>
        public int SelectedMenuIndex
        {
            get { return menu.SelectedIndex; }
        }

    }
}
