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
    class GrapplingHook : PowerUp
    {
        #region Variables
        Texture2D Hook;
        Texture2D Rope;
        Texture2D Cursor;
        Vector2 CursorPosition;
        Rectangle HookPoint;
        SpriteBatch spriteBatch;
        #endregion

        #region Constructors
        public GrapplingHook()
        {
           
        }
        #endregion

        #region Load
        public void Load(ContentManager Content, SpriteBatch spriteBatch)
        {
            Hook = Content.Load<Texture2D>("Hook");
            Rope = Content.Load<Texture2D>("Rope");
            Cursor = Content.Load<Texture2D>("Cursor");
            this.spriteBatch = spriteBatch;
        }
        #endregion

        #region Update and Draw
        public void Update(Player player)
        {
            HookPoint = new Rectangle((int)CursorPosition.X, (int)CursorPosition.Y, Cursor.Width, Cursor.Height);
            MouseState mouse = Mouse.GetState();
            CursorPosition.X = mouse.X;
            CursorPosition.Y = mouse.Y;

            if(mouse.LeftButton == ButtonState.Pressed)
            {
                foreach(Rectangle box in LevelBuilder.Instance.collisionBoxes)
                {
                    if(HookPoint.Intersects(box))
                    {
                        CursorPosition.Y = box.Y;
                        CursorPosition.X = box.X;
                        if(mouse.RightButton == ButtonState.Pressed)
                        {
                            Zip(player.position);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Will draw the Aim Cursor on the screen for the player to guide with the mouse
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawCursor(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Cursor, HookPoint, Color.White);
        }

        /// <summary>
        /// Will draw a line between the player and the landing position of the hook
        /// </summary>
        /// <param name="dist"></param>
        /// <param name="playerPosition"></param>
        private void DrawLine(float dist, Vector2 playerPosition)
        {


            spriteBatch.Draw(Rope, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, (int)dist, 2),
                null,
                Color.White,
                (float)Math.Atan2(CursorPosition.Y - playerPosition.Y, CursorPosition.X - playerPosition.X),
                Vector2.Zero,
                SpriteEffects.None,
                0);

        }
        #endregion

        #region Helpers
        /// <summary>
        /// decrements the distance between player and grapple point 
        /// </summary>
        /// <param name="playerPosition"></param>
        public void Zip(Vector2 playerPosition)
        {
            float dist = Vector2.Distance(playerPosition, CursorPosition);
            float increments = 1 / dist;
            while(playerPosition != CursorPosition)
            {
                DrawLine(dist, playerPosition);
                playerPosition.X += increments;
                playerPosition.Y += increments;
                dist = Vector2.Distance(playerPosition, CursorPosition);
            }
        }
        #endregion


    }
}
