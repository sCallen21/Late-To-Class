using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Late_To_Class
{
    /// <summary>
    /// TODO: Create Endless Mode
    /// </summary>
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
        const double dMaxHypoDistance = 10; //Maximum distance from the jump point on one building to the landing zone in the next
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

        #region SpawnControl
        /// <summary>
        /// Loads in a source rectangle from LevelManager that determines how the spritesheet is broken up
        /// </summary>
        /// <param name="Source"></param>
        public void LoadSource(Rectangle[,] Source)
        {
            this.Source = Source;
            LoadSpawn();
        }

        /// <summary>
        /// Creates a spawn area at the start of the endless mode
        /// </summary>
        /// TODO: Change loop to an array map
        public void LoadSpawn()
        {
            for (int i = 0; i < 5; i++)
            {
                List<Rectangle> column = new List<Rectangle>();
                column.Add(Source[0, i]);
                for (int j = 1; j < 10; j++)
                {
                    if (i == 0)
                    {
                        column.Add(Source[1, 1]);
                    }
                    else if (i == 4)
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
        #endregion

        #region Update and Draw
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
        #endregion

        #region Helpers
        /// <summary>
        /// Creates the next building in the list
        /// </summary>
        public void NextBuilding()
        {
            //TODO : create system for randomized building generation
        }

        /// <summary>
        /// determines if a building is in a suitable location. Will probably just move this into NextBuilding
        /// </summary>
        /// <param name="spacing"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public bool CalculateJumpable(double spacing, double height)
        {
            bool isOk = false;
            return isOk;
        }
        #endregion
    }
}
