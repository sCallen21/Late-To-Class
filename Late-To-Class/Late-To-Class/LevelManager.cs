using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Late_To_Class
{
    public sealed class LevelManager
    {
        private static LevelManager instance = null;
        private Texture2D tileSheet;
        private Texture2D timerBG; //looks like a blackboard, goes behind the timer
        private Camera camera;
        Point CameraOrigin;
        Player player;
        private double dTimer;
        string cameraNotes = "Stephen";
        const int difficulty = 200;
        List<PowerUp> powerUps = new List<PowerUp>();
       

        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LevelManager();
                return instance;
            }
        }

        /// <summary>
        /// Loads up a map level to play
        /// </summary>
        /// <param name="level">the name of the level, will be used with a menu to select from many levels</param>
        /// <param name="Content">Content Folder</param>
        /// <param name="newViewport">passes in the viewport for the camera</param>
        /// <param name="player">passes in the player so each level handles it</param>
        public void LoadLevel(string level, string spawns, ContentManager Content, Viewport newViewport, Player player)
        {
            tileSheet = Content.Load<Texture2D>("lvl1Tiles.png");
            LevelBuilder.Instance.LoadMap(level);
            LevelBuilder.Instance.LoadSpawns(spawns);
            LevelBuilder.Instance.TileMaker(tileSheet);
            LevelBuilder.Instance.SpawnMaker();
            camera = new Camera(newViewport);
            this.player = player;
            player.drawnTex = new Rectangle(LevelBuilder.Instance.PlayerPosition.X, LevelBuilder.Instance.PlayerPosition.Y, 66, 66);

            foreach(Point cluster in LevelBuilder.Instance.NPCSpawnPositions)
            {
                //npcBuilder.CreateSpawn(cluster);
            }
            foreach(Point guard in LevelBuilder.Instance.EnemySpawnPositions)
            {
                //CampoBuilder.CreateSpawn(guard);
            }
            powerUps.Clear();
            foreach(Point powerUp in LevelBuilder.Instance.PowerUpPositions)
            {
                PowerUp Power = new PowerUp();
                Power.CreateSpawn(powerUp, Content);
                powerUps.Add(Power);
            }
            dTimer = (LevelBuilder.Instance.MapSize.X * LevelBuilder.Instance.MapSize.Y) / difficulty * (1 / 1); //replace 1/1 with 1/level once multiple levels exist

            timerBG = Content.Load<Texture2D>("timerBlackboard.png");
        }


        /// <summary>
        /// Updates everything in a map, the timer, the rendering, the player.
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateLevel(GameTime gameTime)
        {
            player.Update(gameTime);
            camera.Update(player.position, LevelBuilder.Instance.MapSize.X * 32, LevelBuilder.Instance.MapSize.Y * 32);
            dTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            dTimer = (Math.Round(dTimer, 2));
            CameraOrigin.X = camera.cameraView.X + (int)player.position.X;
            if (CameraOrigin.X < 0) { CameraOrigin.X = 0; }
            CameraOrigin.Y = camera.cameraView.Y + (int)player.position.Y;
            cameraNotes = CameraOrigin.X.ToString() + ";" + CameraOrigin.Y.ToString();


        }

        /// <summary>
        /// Draws the level on screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="screen"></param>
        /// <param name="font"></param>
        public void DrawLevel(SpriteBatch spriteBatch, Point screen, SpriteFont font)
        {
            //this spritebatch will follow the player as per camera.Transform
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            LevelBuilder.Instance.Draw(spriteBatch, screen, CameraOrigin);
            player.Draw(spriteBatch);
            spriteBatch.End();
            //this spritebatch will not follow the player, and will always be drawn where they say they'll be
            spriteBatch.Begin();
            spriteBatch.Draw(timerBG, new Rectangle(8, 13, 160, 80), Color.White);
            spriteBatch.DrawString(font, dTimer.ToString(), new Vector2(20, 20), Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// used to determine if the timer has hit zero, ie LATE TO CLASS(roll credits)*ding*
        /// </summary>
        /// <returns></returns>
        public bool Late()
        {
            if (dTimer <= 0) { return true; }
            else { return false; }
        }

    }
}
