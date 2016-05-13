using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

//Chris Banks
namespace Late_To_Class
{
    /// <summary>
    /// manager class for bulding levels from text files
    /// </summary>
    public sealed class LevelBuilder
    {
        #region Variables
        private static LevelBuilder instance = null;
        StreamReader input = null;
        int[,] map;
        int[,] spawnMap;
        Texture2D tileSheet;
        Rectangle[,] spawnSource;
        Rectangle position;
        Rectangle toDraw;
        Point tileSize;
        public Rectangle[,] Source;
        public Point MapSize;
        public Point PlayerPosition;
        public Point GoalPosition;
        public List<Point> EnemySpawnPositions;
        public List<Rectangle> NPCSpawnPositions;
        public List<Point> PowerUpPositions;
        public List<Rectangle> collisionBoxes;
        #endregion

        #region Constructor
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
        #endregion

        #region Load
        /// <summary>
        /// loads in a map from a text file
        /// </summary>
        /// <param name="sMapName">name of map to be loaded</param>
        public void LoadMap(string sMapName)
        {
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

                    }
                    y++;

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
        /// Loads in the spawn Map data from the file
        /// </summary>
        /// <param name="sSpawnName"></param>
        public void LoadSpawns(string sSpawnName)
        {
            try
            {
                input = new StreamReader(sSpawnName);
                NPCSpawnPositions = new List<Rectangle>();
                EnemySpawnPositions = new List<Point>();
                PowerUpPositions = new List<Point>();
                collisionBoxes = new List<Rectangle>();
                int y = 0;
                string line = null;

                line = input.ReadLine(); //Removes the first line from the list

                spawnMap = new int[MapSize.Y, MapSize.X];
                spawnSource = new Rectangle[MapSize.Y, MapSize.X];
                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    for (int x = 0; x < data.Length; x++)
                    {
                        spawnMap[y, x] = int.Parse(data[x]);

                    }
                    y++;

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
        #endregion

        #region Spawn Creator and Tile Creator
        /// <summary>
        /// builds collision and spawn points out of the map provided
        /// </summary>
        public void SpawnMaker()
        {
            for (int y = 0; y < MapSize.Y; y++)
            {
                for (int x = 0; x < MapSize.X; x++)
                {
                    switch (spawnMap[y, x])
                    {
                        case -1: 
                            break; //Null Space Case
                        case 0: collisionBoxes.Add(new Rectangle(x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y));
                            break; // Collision Case
                        case 1: collisionBoxes.Add(new Rectangle(x * tileSize.X, y * tileSize.Y + 10, tileSize.X, tileSize.Y));
                            break; // Displaced Collision Case
                        case 2:  PlayerPosition = new Point(y * tileSize.X, x * tileSize.Y + 128);
                            break;//Player Spawn Location
                        case 3:  NPCSpawnPositions.Add(new Rectangle((x * tileSize.X), (tileSize.Y * y), tileSize.X, tileSize.Y));
                            break; //NPC Spawn Locations
                        case 4: EnemySpawnPositions.Add(new Point((x * tileSize.X), (tileSize.Y * y)));
                            break; //Enemy Spawn Locations
                        case 5:  GoalPosition = new Point(x * tileSize.X, y * tileSize.Y);
                            break;
                        default: 
                            break;
                    }
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
            for (int y = 0; y < MapSize.Y; y++)
            {
                for (int x = 0; x < MapSize.X; x++)
                {
                    tile = map[y, x];
                    int xPos = tile % tilesPerRow;
                    int yPos = (tile - xPos) / tilesPerRow;
                    toDraw = new Rectangle(xPos * tileSize.X, yPos * tileSize.Y, tileSize.X, tileSize.Y);
                    Source[y, x] = toDraw;
                }
            }
        }
        #endregion

        #region Draw
        /// <summary>
        /// draws all non empty tile spaces that are in the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="screen">Size of the render window as a point</param>
        public void Draw(SpriteBatch spriteBatch, Point screen, Point CameraOrigin)
        {
            for (int y = 0; y < MapSize.Y; y++)
            {
                for (int x = 0; x < MapSize.X; x++)
                {
                    if (map[y, x] != -1)
                    {
                        position = new Rectangle(x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y);
                        spriteBatch.Draw(tileSheet, position, Source[y, x], Color.White);
                    }
                }
            }
        }
        #endregion
    }
}