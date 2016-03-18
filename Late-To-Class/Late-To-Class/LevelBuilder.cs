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
        Rectangle[,] Source;
        Rectangle position;
        Rectangle toDraw;
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
                map = new int[40, 40];
                Source = new Rectangle[40, 40];
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
                if (input != null)
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
            for (int y = 0; y < nMapHeight; y++)
            {
                for (int x = 0; x < nMapWidth; x++)
                {
                    tile = map[y, x];
                    int xPos = tile % tilesPerRow;
                    int yPos = (tile - xPos) / tilesPerRow;
                    toDraw = new Rectangle(xPos * tileSize, yPos * tileSize, tileSize, tileSize);
                    Source[y, x] = toDraw;

                }
            }


        }

        public void Draw(SpriteBatch spriteBatch, Point screen)
        {
            for (int y = 0; y < nMapHeight; y++)
            {
                for (int x = 0; x < nMapWidth; x++)
                {
                    if (map[y, x] != -1)
                    {
                        position = new Rectangle(x * 32, y * 32, 32, 32);
                        if (position.X < screen.X + 32 && position.X >= 0 && position.Y < screen.Y + 32 && position.Y >= 0)
                            spriteBatch.Draw(tileSheet, position, Source[y, x], Color.White);
                    }
                }
            }
        }
    }
}