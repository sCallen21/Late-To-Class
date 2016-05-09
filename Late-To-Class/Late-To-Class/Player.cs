using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

//Steve Callen
namespace Late_To_Class
{
    public class Player
    {
        private Rectangle drawnTex;
        private Rectangle hitbox;
        private Texture2D tex;
        public Vector2 position;
        int playerHeight = 64;
        int playerWidth = 64;


        private double jumpHeight;



        /// <summary>
        /// Keys for all user input, set by default to W,A,D,S,J
        /// </summary>
        private Keys jumpKey;
        private Keys leftKey;
        private Keys rightKey;
        private Keys duckKey;
        private Keys powerUpKey;

        //these variables handle acceleration of the player
        public int speed; //determines how fast the player is moving. This value increases as the player continues to move.
        private int maxSpeed; //determines the player's max speed
        private double accTimer; //this counts how long it's been since the player speed increased. It will measure this and when it reaches a value, increment the player's speed.
        private double timeToNextAcc; //this is the time it takes for the player to increase his speed by 1. This is measured in seconds.
        private double timeToNextDec; //this is the time it takes for the player to decrease his speed by 1. This is measured in seconds.

        //these variables handle animation of the player
        Rectangle sourceRec; //this rectangle defines the portion of the texture it should grab.
        Dictionary<string, AnimationHelper> allAnims; //holds all animationhelpers for player
        double timeCounter; //counts ticks of the gametime

        public Texture2D Tex
        {
            set { tex = value; }
            get { return tex; }
        }

        enum playerStates
        {
            Run,
            Jump,
            Stand,
            Duck,
            Slide
        }

        playerStates pState;
        playerStates prevState; //this records the state the player had on the previous state


        bool dirRight; //bool indicating facing direction with right == true
        bool jumping;

        public Player()
        {

            pState = playerStates.Stand;
            dirRight = true;
            drawnTex = new Rectangle(0, 0, 66, 66);
            hitbox = new Rectangle(drawnTex.X + (drawnTex.Width / 4), drawnTex.Y, drawnTex.Width / 2, drawnTex.Height);
            jumpHeight = drawnTex.Y;
            position = new Vector2(hitbox.X, hitbox.Y);


            //acceleration stuff
            speed = 1; //initial speed of the player
            maxSpeed = 10; //maximum speed of the player
            accTimer = 0; //how long since last increment of speed
            timeToNextAcc = 0.1; //how long it takes to increment speed
            timeToNextDec = 0.05; //how long it takes to decrement speed

            //controls stuff
            jumpKey = GameControls.Instance.jumpKey;
            leftKey = GameControls.Instance.moveLeft;
            rightKey = GameControls.Instance.moveRight;
            duckKey = GameControls.Instance.duckKey;
            powerUpKey = GameControls.Instance.powerUpKey;

            //animation stuff
            allAnims = new Dictionary<string, AnimationHelper>();
            allAnims.Add("run", new AnimationHelper(5, 10));
            allAnims.Add("jump", new AnimationHelper(12, 10));
        }

        public void Load(ContentManager Content)
        {
            tex = Content.Load<Texture2D>("player");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            drawnTex.X = hitbox.X - (drawnTex.Width / 4);
            drawnTex.Y = hitbox.Y;
            position.X = drawnTex.X;
            position.Y = drawnTex.Y;


            hitbox.Y += (int)jumpHeight;

            jumpHeight++;

            for (int i = 0; i < LevelBuilder.Instance.collisionBoxes.Count; i++)
            {
                if (hitbox.Intersects(LevelBuilder.Instance.collisionBoxes[i]) && Math.Abs(LevelBuilder.Instance.collisionBoxes[i].Top - hitbox.Bottom) < hitbox.Height / 2)
                {

                    if (jumpHeight < 0 && dirRight == true)
                    {
                        hitbox.X += speed;
                    }
                    else if (jumpHeight < 0 && dirRight == false)
                    {
                        hitbox.X -= speed;
                    }
                    else
                    {
                        if (jumpHeight < 0 && dirRight == true)
                        {
                            hitbox.X += speed;
                        }
                        else if (jumpHeight < 0 && dirRight == false)
                        {
                            hitbox.X -= speed;
                        }

                        hitbox.Y = LevelBuilder.Instance.collisionBoxes[i].Top - hitbox.Height;
                        jumpHeight = 0;
                    }
                }
            }

            if (jumpHeight >= 30) //This will prevent the player from gaining infinite vertical momentum if he is too high, primarily a concern for clipping through the floor at too high of speeds
            {
                jumpHeight = 30;
            }

            switch (pState)
            {
                case playerStates.Run:
                    UpdateAnimation(gameTime, allAnims["run"]);
                    if (kbState.IsKeyDown(leftKey) && (dirRight == true || dirRight == false)) //running left
                    {
                        dirRight = false;

                        //this increments the player's speed as long as the speed is less than max speed. This increment happens every timeToNextAcc seconds
                        if (prevState == playerStates.Run && speed < maxSpeed && accTimer >= timeToNextAcc)
                        {
                            speed++;
                            accTimer = 0;
                        }

                        else
                        {
                            for (int i = 0; i < LevelBuilder.Instance.collisionBoxes.Count; i++)
                            {
                                if (hitbox.Intersects(LevelBuilder.Instance.collisionBoxes[i]) && hitbox.Left <= LevelBuilder.Instance.collisionBoxes[i].Right && hitbox.Bottom <= LevelBuilder.Instance.collisionBoxes[i].Bottom)
                                {
                                    hitbox.X = LevelBuilder.Instance.collisionBoxes[i].Right;
                                    hitbox.X += speed;
                                    speed = 0;
                                    jumpHeight = 0;
                                    pState = playerStates.Jump;
                                }
                            }
                        }


                        accTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if (kbState.IsKeyDown(rightKey) && (dirRight == true || dirRight == false)) //running right
                    {
                        dirRight = true;

                        //this increments the player's speed as long as the speed is less than max speed. This increment happens every timeToNextAcc seconds
                        if (prevState == playerStates.Run && speed < maxSpeed && accTimer >= timeToNextAcc)
                        {
                            speed++;
                            accTimer = 0;
                        }

                        else
                        {
                            for (int i = 0; i < LevelBuilder.Instance.collisionBoxes.Count; i++)
                            {
                                if (hitbox.Intersects(LevelBuilder.Instance.collisionBoxes[i]) && hitbox.Right >= LevelBuilder.Instance.collisionBoxes[i].Left && hitbox.Bottom <= LevelBuilder.Instance.collisionBoxes[i].Bottom)
                                {

                                    hitbox.X = LevelBuilder.Instance.collisionBoxes[i].Left - hitbox.Width;
                                    hitbox.X -= speed;
                                    speed = 0;
                                    jumpHeight = 0;
                                    pState = playerStates.Jump;
                                }
                            }
                        }


                        accTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if (kbState.IsKeyUp(leftKey) && kbState.IsKeyUp(rightKey) /*&& dirRight == true*/) //not running.
                    {
                        if (speed > 0)
                        {
                            //this decrements the player's speed as long as the speed is greater than 0, when it will change the state to standing. This increment happens every timeToNextDec seconds
                            if (accTimer >= timeToNextDec)
                            {
                                speed--;
                                accTimer = 0;
                            }

                            accTimer += gameTime.ElapsedGameTime.TotalSeconds;

                        }
                        else
                            pState = playerStates.Stand;
                    }

                    if (kbState.IsKeyDown(leftKey) && kbState.IsKeyDown(rightKey)) //if both keys are pressed
                    {
                        if (speed > 0)
                        {
                            //this decrements the player's speed very quickly, so that the character turns around quickly.
                            if (accTimer >= 0.01)
                            {
                                speed--;
                                accTimer = 0;
                            }

                            accTimer += gameTime.ElapsedGameTime.TotalSeconds;

                        }

                        if (!dirRight) //Leaving the code untouched here, but when commented out, doesn't affect draw animation so may not be needed
                        {
                            dirRight = false;
                        }
                    }

                    if (kbState.IsKeyDown(duckKey))
                    {
                        pState = playerStates.Slide;
                    }

                    if (kbState.IsKeyDown(jumpKey))
                    {
                        jumpHeight = -20;
                        pState = playerStates.Jump;
                    }

                    //this checks to see what direction the player is moving, and then adds the speed accordingly
                    if (dirRight)
                    {
                        hitbox.X += speed;
                    }

                    else if (!dirRight)
                    {
                        hitbox.X -= speed;
                    }

                    break;





                case playerStates.Stand:

                    speed = 0;
                    if (kbState.IsKeyDown(leftKey))
                    {
                        //dirRight = false;
                        pState = playerStates.Run;
                    }

                    if (kbState.IsKeyDown(rightKey))
                    {
                        //dirRight = false;
                        pState = playerStates.Run;

                    }
                    if (kbState.IsKeyDown(jumpKey))
                    {
                        jumpHeight = -20;
                        pState = playerStates.Jump;
                    }

                    for (int i = 0; i < LevelBuilder.Instance.collisionBoxes.Count; i++)
                    {
                        if (hitbox.Intersects(LevelBuilder.Instance.collisionBoxes[i]))
                        {
                            if (hitbox.Right > LevelBuilder.Instance.collisionBoxes[i].Left && (hitbox.Right - hitbox.Width / 2) <= LevelBuilder.Instance.collisionBoxes[i].Left)
                            {
                                hitbox.X = LevelBuilder.Instance.collisionBoxes[i].Left - drawnTex.Width;
                            }
                            else if (hitbox.Left < LevelBuilder.Instance.collisionBoxes[i].Right && (hitbox.Left + hitbox.Width / 2) >= LevelBuilder.Instance.collisionBoxes[i].Right)
                            {
                                hitbox.X = LevelBuilder.Instance.collisionBoxes[i].Right;
                            }
                        }
                    }

                    if (kbState.IsKeyDown(duckKey))
                    {
                        pState = playerStates.Duck;
                    }


                    break;






                case playerStates.Jump: //http://flatformer.blogspot.com/2010/02/making-character-jump-in-xnac-basic.html
                    UpdateAnimation(gameTime, allAnims["jump"]);
                    if (jumping == true)
                    {
                        //jumpHeight = -20;
                        //drawnTex.Y += (int)jumpHeight;
                        position.Y += (int)jumpHeight;

                        if (kbState.IsKeyUp(jumpKey))
                        {
                            jumpHeight += 0.75;

                        }

                        if (kbState.IsKeyDown(leftKey))
                        {
                            dirRight = false;
                            //pos.X -= 4;
                        }
                        else if (kbState.IsKeyDown(rightKey))
                        {
                            dirRight = true;
                            //pos.X += 4;
                        }

                        //this decelerates the player while jumping
                        if (kbState.IsKeyUp(leftKey) && kbState.IsKeyUp(rightKey) /*&& dirRight == true*/)
                        {
                            if (speed > 0)
                            {
                                //this decrements the player's speed as long as the speed is greater than 0, when it will change the state to standing. This increment happens every timeToNextDec seconds
                                if (accTimer >= timeToNextDec)
                                {
                                    speed--;
                                    accTimer = 0;
                                }

                                accTimer += gameTime.ElapsedGameTime.TotalSeconds;
                            }
                        }

                        //this checks to see what direction the player is moving, and then adds the speed accordingly
                        if (dirRight)
                        {
                            drawnTex.X += speed;
                            hitbox.X += speed;
                        }

                        else if (!dirRight)
                        {
                            drawnTex.X -= speed;
                            hitbox.X -= speed;
                        }


                        for (int i = 0; i < LevelBuilder.Instance.collisionBoxes.Count; i++)
                        {
                            if (hitbox.Bottom == LevelBuilder.Instance.collisionBoxes[i].Top && hitbox.TouchTopOf(LevelBuilder.Instance.collisionBoxes[i])) //Find another condition to prevent this, might rectangleHelper's touchTopOf
                            {
                                jumping = false;

                                if (dirRight == true)
                                {
                                    hitbox.X += speed;
                                }
                                else if (dirRight == false)
                                {
                                    hitbox.X -= speed;
                                }

                                //this determines if the player had any speed during the jump, and if so to put them back in the running state as opposed to the standing state
                                if (speed > 0)
                                {
                                    allAnims["jump"].CurrentFrame = 0; //resets jump animation
                                    pState = playerStates.Run;
                                }
                                else
                                {
                                    allAnims["jump"].CurrentFrame = 0; //resets jump animation
                                    pState = playerStates.Stand;
                                }
                            }
                        }
                    }

                    else
                    {
                        jumping = true;
                    }
                    break;




                case playerStates.Duck:
                    if (kbState.IsKeyUp(duckKey))
                    {
                        pState = playerStates.Stand;
                    }
                    if (kbState.IsKeyDown(jumpKey))
                    {
                        // after collision is implemented, some code will go here for the player jumping down off a platform
                    }
                    break;

                case playerStates.Slide:
                    if (kbState.IsKeyUp(duckKey))
                    {
                        pState = playerStates.Run;
                    }
                    else if (speed > 0)
                    {
                        if (accTimer >= timeToNextDec)
                        {
                            speed--;
                            accTimer = 0;
                        }

                        accTimer += gameTime.ElapsedGameTime.TotalSeconds;

                        if (dirRight)
                        {
                            hitbox.X += speed;
                        }

                        else if (!dirRight)
                        {
                            hitbox.X -= speed;
                        }

                    }

                    else
                    {
                        pState = playerStates.Duck;
                    }
                    break;

            }

            allAnims["run"].FPS = (speed + 1) * 1.8;
            allAnims["run"].UpdateSPF(); //this makes it so that when the player is just starting to run, his animation follows the speed and doesn't look too fast.
        }

        public void Draw(SpriteBatch playerSprite)
        {

            switch (pState)
            {
                case playerStates.Stand:
                    DrawStand(playerSprite);
                    break;
                case playerStates.Run:
                    DrawRun(playerSprite);
                    break;
                case playerStates.Jump:
                    DrawJump(playerSprite);
                    break;
                case playerStates.Duck:
                    DrawDuck(playerSprite);
                    break;
                case playerStates.Slide:
                    DrawSlide(playerSprite);
                    break;

            }
        }

        public void DrawStand(SpriteBatch spriteBatch)
        {
            sourceRec = new Rectangle(0, 0, playerWidth, playerHeight);

            if (dirRight)
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            else
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        }

        public void DrawRun(SpriteBatch spriteBatch)
        {
            sourceRec = new Rectangle(allAnims["run"].CurrentFrame * playerWidth, playerHeight, playerWidth, playerHeight);

            if (dirRight)
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            else
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        }

        public void DrawJump(SpriteBatch spriteBatch)
        {
            sourceRec = new Rectangle(allAnims["jump"].CurrentFrame * playerWidth, playerHeight * 2, playerWidth, playerHeight);

            if (dirRight)
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            else
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        }
        public void DrawDuck(SpriteBatch spriteBatch)
        {
            sourceRec = new Rectangle(0, playerHeight * 3, playerWidth, playerHeight);

            if (dirRight)
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            else
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        }
        public void DrawSlide(SpriteBatch spriteBatch)
        {
            sourceRec = new Rectangle(0, playerHeight * 4, playerWidth, playerHeight);

            if (dirRight)
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            else
                spriteBatch.Draw(tex, drawnTex, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        }

        public void UpdateAnimation(GameTime gameTime, AnimationHelper ah)
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