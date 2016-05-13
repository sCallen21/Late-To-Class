using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 //Chris Banks
 namespace Late_To_Class
 {
     class NPC
     {
         #region Variables
         Rectangle hitBox;
         Rectangle hairPosition;
         Rectangle BodySource;
         Rectangle HairSource;
         Color hairTint;
         Texture2D image;
         Vector2 vPosition;
         Vector2 vVelocity;
         AnimationHelper helper;
         int nPlayerHeight;

         int bodyType;
         int moveSwitch;

         int nMoveSwitch;

         bool bDirRight;
         double dTimeCounter;
         Random movement;
         public Rectangle bodyPosition;
         #endregion

         #region Constructor
         public NPC()
         {
             helper = new AnimationHelper(4, 4);
             bDirRight = true;
             vVelocity = Vector2.Zero;
             movement = new Random();

             moveSwitch = 0;
             nPlayerHeight = 64;

             nMoveSwitch = 0;

         }
         #endregion

         #region Load
         public void SetContent(int body, int hair, int r, int g, int b, Texture2D builder)
         {
             this.image = builder;
             bodyType = body;
             HairSource = new Rectangle(25 * hair, 3 * nPlayerHeight, 25, 64);
             BodySource = new Rectangle(helper.CurrentFrame * 25, (nPlayerHeight * bodyType), 25, nPlayerHeight);
             hairTint = new Color(r, g, b);
         }
#endregion

         #region Update and Draw
         public void Update(GameTime gameTime)
         {
             vPosition += vVelocity;
             bodyPosition = new Rectangle((int)vPosition.X, (int)vPosition.Y, 32, nPlayerHeight);
             hairPosition = new Rectangle((int)vPosition.X, (int)vPosition.Y, 32, 32);

            BodySource = new Rectangle(helper.CurrentFrame * 25, (nPlayerHeight * bodyType), 25, nPlayerHeight);
            if (moveSwitch > 32)

             if (nMoveSwitch > 32)

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

                 nMoveSwitch = 0;
             }
             nMoveSwitch++;
 
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
                 spriteBatch.Draw(image, bodyPosition, BodySource, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                 spriteBatch.Draw(image, bodyPosition, HairSource, hairTint, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
             }
             else
            {
                 spriteBatch.Draw(image, bodyPosition, BodySource, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
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
             dTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
 
             if (dTimeCounter >= ah.SPF)
             {
                 ah.CurrentFrame++;
 
                 if (ah.CurrentFrame >= ah.TotalFrames)
                 {
                     ah.CurrentFrame = 0;
                 }
 
                 dTimeCounter -= ah.SPF;
             }
         }
#endregion
     } 
 } 