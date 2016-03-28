using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace Late_To_Class
{
    class TextMenuComponet : DrawableGameComponent
    {
        private SpriteBatch spriebatch = null;
        private SpriteFont regularFont, selectedFont;
        private Color regularColor = Color.White, selectedColor = Color.Red;
        private Vector2 pos = new Vector2();
        //items
        private int selectedIndex = 0;
        private StringCollection  menuItems;
        //size
        private int width, height;
        private KeyboardState oldKBState;
        private AudioComponet audioComponet;
      


        public TextMenuComponet(Game game, SpriteFont normalFont, SpriteFont selectedFont):base(game) {
            regularFont = selectedFont;
            this.selectedFont = selectedFont;
            menuItems = new StringCollection();

            spriebatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            audioComponet = (AudioComponet)Game.Services.GetService(typeof(AudioComponet));
            oldKBState = Keyboard.GetState();

        }


        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);
            CalculateBound();    
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }
        public Color RegularColor
        {
            get { return regularColor; }
            set { regularColor = value; }
        }
        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        private void CalculateBound()
        {
            width = 0;
            height = 0;
            foreach (string item in menuItems)
            {
                Vector2 size = selectedFont.MeasureString(item);
                if (size.X > width)
                {
                    width = (int)size.X;
                }
                height += selectedFont.LineSpacing;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            float y = pos.Y;
            for (int i = 0; i < menuItems.Count; i++)
            {
                SpriteFont font;
                Color aColor;
                if (i == selectedIndex)
                {
                    font = selectedFont;
                    aColor = selectedColor;
                }
                else
                {
                    font = regularFont;
                    aColor = regularColor;
                }
                spriebatch.DrawString(font, menuItems[i], new Vector2(pos.X + 1, y + 1), Color.Black);
                spriebatch.DrawString(font, menuItems[i], new Vector2(pos.X, y), aColor);
                y += font.Spacing;
            }
            base.Draw(gameTime);
        }


        public override void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            bool down, up;
            down = (oldKBState.IsKeyDown(Keys.Down) && kbState.IsKeyUp(Keys.Down));
            up = (oldKBState.IsKeyDown(Keys.Up) && kbState.IsKeyUp(Keys.Up));

            if (up || down)
            {
                audioComponet.PlayCue("menu_scroll");
            }
            if (down)
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Count)
                {
                    selectedIndex = 0;
                }
            }
            if (up)
            {
                selectedIndex--;
                if (selectedIndex == -1)
                {
                    selectedIndex = menuItems.Count - 1;
                }
            }
            oldKBState = kbState;
            base.Update(gameTime);
        }

    }
}
