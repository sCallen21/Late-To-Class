using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Late_To_Class
{
    public sealed class LevelBuilder
    {
        private static LevelBuilder instance = null;
        StreamReader input = null;
        int[,] map;
        Texture2D tileSheet;
        Vector2 position;
        Texture2D[,] tiles;
        int nMapWidth;
        int nMapHeight;
            

        private LevelBuilder() { }

        /// <summary>
        /// Creates and returns a single instance of the GameControls class
        /// </summary>
        public static LevelBuilder Instance
        {
            get
            {
                if (instance == null)
                    instance = new LevelBuilder();
                return instance;
            }
        }

        public void LoadMap(string sMapName)
        {
            try
            {
                input = new StreamReader(sMapName);
                int y = 0;
                string line = null;
                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    for (int x = 0; x < data.Length; x++)
                    {
                        map[y, x] = int.Parse(data[x]);
                        nMapWidth = x;
                    }
                    y++;
                    nMapHeight = y;
                }
            }
            catch
            {

            }
            finally
            {
                if(input != null)
                {
                    input.Close();
                }
            }
        }

        public void TileMaker(Texture2D image, int tileSize)
        {
            tileSheet = image;
            int tilesPerRow = tileSheet.Width / tileSize;
            int tilesPerCol = tileSheet.Height / tileSize;
            int tile;
            Rectangle toDraw;
            for(int y = 0; y < nMapHeight; y++)
            {
                for (int x = 0; x < nMapWidth; x++)
                {
                    tile = map[y, x];
                    toDraw = new Rectangle(tile / tilesPerCol, tile / tilesPerRow, tileSize, tileSize);
                }
            }

        }

        public void DrawTile(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tileSheet, position, Color.White);
        }


    }
}
