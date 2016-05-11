using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
 using Microsoft.Xna.Framework.Graphics;
 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 
 namespace Late_To_Class
 {
     class NPC
     {
 
         Rectangle hitBox;
         public Rectangle bodyPosition;
         Rectangle hairPosition;
         Rectangle BodySource;
         Rectangle HairSource;
         Color hairTint;
         Texture2D image;
         Vector2 vPosition;
         Vector2 vVelocity;
         AnimationHelper helper;
         int nPlayerHeight;
         int moveSwitch;
         bool bDirRight;
         double timeCounter;
         Random movement;
 
 
         public NPC()
         {
             helper = new AnimationHelper(3, 3);
             bDirRight = true;
             vVelocity = Vector2.Zero;
             movement = new Random();
             moveSwitch = 0;
         }
 
         public void SetContent(int body, int hair, int r, int g, int b, Texture2D builder)
         {
             this.image = builder;
             HairSource = new Rectangle(1 * hair, 0, 32, 32);
             BodySource = new Rectangle(0, 32 + (nPlayerHeight * body), image.Width, nPlayerHeight);
             hairTint = new Color(r, g, b);
         }
 
         public void Update(GameTime gameTime)
         {
             vPosition += vVelocity;
             bodyPosition = new Rectangle((int)vPosition.X, (int)vPosition.Y, 32, nPlayerHeight);
             hairPosition = new Rectangle((int)vPosition.X, (int)vPosition.Y, 32, 32);
             if (moveSwitch > 32)
             {
                 if (vVelocity.X != 0)
                 {
                     UpdateMovement(gameTime, helper);
                     vVelocity.X = movement.Next(-2, 3);
                     
                 }
                 if (vVelocity.X < 0)
                 {
                     bDirRight = false;
                 }
                 else bDirRight = true;

                 moveSwitch = 0;
             }
             moveSwitch++;
 
         }
 
         public void Draw(SpriteBatch spriteBatch)
         {
             if (vVelocity.X == 0)
             {
                 DrawStand(spriteBatch);
             }
             else
             {
                 DrawRun(spriteBatch);
             }
         }
 
         public void DrawStand(SpriteBatch spriteBatch)
         {
             Rectangle bodyRectangle = BodySource;
             bodyRectangle.Width = 32;
             if (bDirRight)
             {
                 spriteBatch.Draw(image, bodyPosition, bodyRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                 spriteBatch.Draw(image, bodyPosition, HairSource, hairTint, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
             }
             else
            {
                 spriteBatch.Draw(image, bodyPosition, bodyRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
                 spriteBatch.Draw(image, bodyPosition, HairSource, hairTint, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
             }
         }
 
         public void DrawRun(SpriteBatch spriteBatch)
         {
             Rectangle bodyRectangle = BodySource;
             bodyRectangle.Y = helper.CurrentFrame * 32;
             bodyRectangle.Width = 32;
             if (bDirRight)
             {
                 spriteBatch.Draw(image, bodyPosition, bodyRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                 spriteBatch.Draw(image, bodyPosition, HairSource, hairTint, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
             }
             else
             {
                 spriteBatch.Draw(image, bodyPosition, bodyRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
                 spriteBatch.Draw(image, bodyPosition, HairSource, hairTint, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
             }
         }
 
         public void UpdateMovement(GameTime gameTime, AnimationHelper ah)
         {
             timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
 
             if (timeCounter >= ah.SPF)
             {
                 ah.CurrentFrame++;
 
                 if (ah.CurrentFrame >= ah.TotalFrames)
                 {
                     ah.CurrentFrame = 0;
                 }
 
                 timeCounter -= ah.SPF;
             }
         }
 
     }
 
 } 