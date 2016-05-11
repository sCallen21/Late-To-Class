using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Late_To_Class
{
    class GrapplingHook : PowerUp
    {
        Texture2D Hook;
        Texture2D Rope;
        Texture2D Cursor;
        Vector2 CursorPosition;
        Rectangle HookPoint;
        SpriteBatch spriteBatch;

        public GrapplingHook()
        {
           
        }

        public void Load(ContentManager Content, SpriteBatch spriteBatch)
        {
            Hook = Content.Load<Texture2D>("Hook");
            Rope = Content.Load<Texture2D>("Rope");
            Cursor = Content.Load<Texture2D>("Cursor");
            this.spriteBatch = spriteBatch;
        }

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


        public void DrawCursor(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Cursor, HookPoint, Color.White);
            
        }
        
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
    }
}
