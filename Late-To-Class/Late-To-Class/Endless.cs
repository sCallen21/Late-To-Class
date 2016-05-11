using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Late_To_Class
{
    public sealed class Endless
    {
        #region Variables
        static Endless instance = null;
        Rectangle[,] Source;
        Texture2D tile;
        List<List<Rectangle>> visibleColumns = new List<List<Rectangle>>();
        List<Rectangle> Collisions = new List<Rectangle>();
        #endregion

        #region Constants
        const double dMaxHypoDistance = 10;
        #endregion

        #region Constructor
        public static Endless Instance
        {
            get
            {
                if (instance == null)
                    instance = new Endless();
                return instance;
            }
        }
        #endregion

        public void LoadSource(Rectangle[,] Source)
        {
            this.Source = Source;
            LoadSpawn();
        }

        public void Update(Player player, GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(List<Rectangle> column in visibleColumns)
                foreach(Rectangle location in column)
                {
           // spriteBatch.Draw(tile, ,);
                }
        }

        public void NextBuilding()
        {

        }

        public bool CalculateJumpable(double spacing, double height)
        {
            bool isOk = false;
            return isOk;
        }

        public void LoadSpawn()
        {
            for (int i = 0; i < 5; i++)
            {
                List<Rectangle> column = new List<Rectangle>();
                column.Add(Source[0,i]);
                for (int j = 1; j < 10; j++)
                {
                    if(i == 0)
                    {
                        column.Add(Source[1,1]);
                    }
                    else if(i == 4)
                    {
                        column.Add(Source[1, 3]);
                    }
                    else if (j % 2 == 0)
                    {
                        column.Add(Source[1, 1]);
                    }
                    else if (j % 2 == 1)
                    {
                        column.Add(Source[1, 2]);
                    }
                }
                visibleColumns.Add(column);
            }
        }

    }
}
