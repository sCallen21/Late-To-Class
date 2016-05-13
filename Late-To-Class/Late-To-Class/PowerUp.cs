using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Chris Banks
namespace Late_To_Class
{
    class PowerUp
    {
        #region Variables
        Texture2D texture;
        Rectangle hitBox;
        PowerUp powerUp;
        #endregion

        #region Load
        public void LoadTexture(int LoadType, ContentManager Content)
        {
            texture = Content.Load<Texture2D>("PowerUp" + LoadType);
        }
        #endregion

        #region Update
        public void Update()
        {
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(GameControls.Instance.powerUpKey))
            {
                powerUp.Update();
            }
        }
        #endregion

        #region Helpers
        public void CreateSpawn(Point spawnLocation, ContentManager Content)
        {
            Random rand = new Random();
            int load = rand.Next(0, 5);
            LoadTexture(load, Content);
            PowerUpType(load);
            hitBox = new Rectangle(spawnLocation.X, spawnLocation.Y, 32, 32);
        }

        public void PowerUpType(int type)
        {
            switch(type)
            {
                case 0: powerUp = new GrapplingHook();
                    break;
            }
        }
        #endregion       
    }
}
