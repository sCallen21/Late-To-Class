using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Late_To_Class
{
    /// <summary>
    /// manager class for bulding levels from text files
    /// </summary>
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
        Point tileSize;
        Point MapSize;
        public List<Rectangle> collisionBoxes;


        private LevelBuilder() { }

        /// <summary>
        /// Creates and returns a single instance of the LevelBuilder class
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

        /// <summary>
        /// loads in a map from a text file
        /// </summary>
        /// <param name="sMapName">name of map to be loaded</param>
        public void LoadMap(string sMapName)
        {
            collisionBoxes = new List<Rectangle>();
            try
            {
                input = new StreamReader(sMapName);
                int y = 0;
                string line = null;
                
                line = input.ReadLine();
                string[] size = line.Split(',');
                tileSize.Y = int.Parse(size[0]);
                tileSize.X = int.Parse(size[1]);
                MapSize.Y = int.Parse(size[2]);
                MapSize.X = int.Parse(size[3]);

                map = new int[MapSize.Y, MapSize.X];
                Source = new Rectangle[MapSize.Y, MapSize.X];

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

        /// <summary>
        /// uses the information from LoadMap to create the tiles of the map
        /// </summary>
        /// <param name="image">the tileSheet to be used</param>
        /// <param name="tileSize">the size(in pixels) of the tiles</param>
        public void TileMaker(Texture2D image)
        {
            tileSheet = image;
            int tilesPerRow = tileSheet.Width / tileSize.X;
            int tilesPerCol = tileSheet.Height / tileSize.Y;
            int tile;
            for (int y = 0; y < nMapHeight; y++)
            {
                for (int x = 0; x < nMapWidth; x++)
                {
                    tile = map[y, x];
                    int xPos = tile % tilesPerRow;
                    int yPos = (tile - xPos) / tilesPerRow;
                    toDraw = new Rectangle(xPos * tileSize.X, yPos * tileSize.Y, tileSize.X, tileSize.Y);
                    Source[y, x] = toDraw;
                    collisionBoxes.Add(toDraw);

                }
            }


        }
        
        /// <summary>
        /// draws all non empty tile spaces that are in the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="screen">Size of the render window as a point</param>
        public void Draw(SpriteBatch spriteBatch, Point screen, Point CameraOrigin)
   
        {
            for (int y = 0; y < nMapHeight; y++)
            {
                for (int x = 0; x < nMapWidth; x++)
                {
                    if (map[y, x] != -1)
                    {
                        position = new Rectangle(x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y);

                        if (position.X <  CameraOrigin.X + screen.X + tileSize.X && position.X >= CameraOrigin.X && position.Y < CameraOrigin.Y + screen.Y + tileSize.Y && position.Y >= CameraOrigin.Y)
                            spriteBatch.Draw(tileSheet, position, Source[y, x], Color.White);
                    }
                }
            }
        }
    }
}