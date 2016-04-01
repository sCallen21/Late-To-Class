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
    class Player
    {
        private Rectangle pos;
        private Texture2D tex;
        public Vector2 position;
        int playerHeight = 64;
        int playerWidth = 64;

        private double airTime;
        private double jumpHeight;
        private int gravity;
        private int baseHeight;

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
        double fpsRun; //how many frames of the run animation play per second
        double fpsJump;
        double timePerFrameRun; //how much time per frame (1/fpsRun)
        double timePerFrameJump;
        double timeCounter; //counts ticks of the gametime
        int currentFrameRun;
        int currentFrameJump;
        int framesRun; //how many frames are in the run animation
        int framesJump; //how many frames are in the jump animation


        public Texture2D Tex
        {
            set { tex = value; }
            get { return tex; }
        }

        enum playerStates
        {
            Run,
            Jump,
            Stand
        }

        playerStates pState;
        playerStates prevState; //this records the state the player had on the previous state


        bool dirRight; //bool indicating facing direction with right == true
        bool jumping;

        public Player()
        {
            pState = playerStates.Stand;
            dirRight = true;
            pos = new Rectangle(200, 300, 66, 66);
            jumpHeight = pos.Y;
            baseHeight = pos.Y;

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
            fpsRun = Math.Pow(speed * 3.5, 2.0);
            fpsJump = 5;
            timePerFrameRun = 1 / fpsRun;
            timePerFrameJump = 1 / fpsJump;
            currentFrameRun = 0;
            currentFrameJump = 0;
            framesRun = 10;
            framesJump = 4;


        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            position.X = pos.X;
            position.Y = pos.Y;
            switch (pState)
            {
                case playerStates.Run:
                    UpdateRunAnimation(gameTime);
                    if (kbState.IsKeyDown(leftKey) && (dirRight == true || dirRight == false)) //running left
                    {
                        dirRight = false;

                        //this increments the player's speed as long as the speed is less than max speed. This increment happens every timeToNextAcc seconds
                        if (prevState == playerStates.Run && speed < maxSpeed && accTimer >= timeToNextAcc)
                        {
                            speed++;
                            accTimer = 0;
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

                    //if (kbState.IsKeyUp(rightKey) && kbState.IsKeyUp(leftKey) /*&& dirRight == false*/) //also not running
                    //{
                    //    if (speed > 0)
                    //    {
                    //        //this decrements the player's speed as long as the speed is less than max speed. This increment happens every timeToNextDec seconds
                    //        if (accTimer >= timeToNextDec)
                    //        {
                    //            speed--;
                    //            accTimer = 0;
                    //        }

                    //        accTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    //    }
                    //    else
                    //        pState = playerStates.Stand;
                    //}

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

                        if (!dirRight)
                        {
                            dirRight = false;
                        }
                    }

                    if (kbState.IsKeyDown(jumpKey))
                    {
                        pState = playerStates.Jump;
                    }

                    //this checks to see what direction the player is moving, and then adds the speed accordingly
                    if (dirRight)
                    {
                        pos.X += speed;
                    }

                    else if (!dirRight)
                    {
                        pos.X -= speed;
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
                        pState = playerStates.Jump;
                    }
                    break;

                case playerStates.Jump: //http://flatformer.blogspot.com/2010/02/making-character-jump-in-xnac-basic.html
                    UpdateJumpAnimation(gameTime);
                    if (jumping == true)
                    {
                        pos.Y += (int)jumpHeight;
                        jumpHeight += 0.75;

                        if (kbState.IsKeyUp(jumpKey))
                        {
                            jumpHeight += 0.75;

                        }

                        if (kbState.IsKeyDown(leftKey))
                        {
                            dirRight = false;
                            pos.X -= 4;
                        }
                        else if (kbState.IsKeyDown(rightKey))
                        {
                            dirRight = true;
                            pos.X += 4;
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
                            pos.X += speed;
                        }

                        else if (!dirRight)
                        {
                            pos.X -= speed;
                        }

                        //if (kbState.IsKeyDown(leftKey))
                        //{
                        //    pos.X -= speed;
                        //}
                        //if (kbState.IsKeyDown(rightKey))
                        //{
                        //    pos.X += speed;
                        //}

                        if (pos.Y >= baseHeight)
                        {
                            pos.Y = baseHeight;
                            jumping = false;

                            //this determines if the player had any speed during the jump, and if so to put them back in the running state as opposed to the standing state
                            if (speed > 0)
                            {
                                currentFrameJump = 0; //resets jump animation
                                pState = playerStates.Run;
                            }
                            else
                            {
                                currentFrameJump = 0; //resets jump animation
                                pState = playerStates.Stand;
                            }
                        }
                    }

                    else if (jumping == true && kbState.IsKeyDown(rightKey))
                    {
                        pos.Y += (int)jumpHeight;
                        pos.X += (int)jumpHeight;
                        jumpHeight += 1;

                        if (pos.Y >= baseHeight)
                        {
                            pos.Y = baseHeight;
                            jumping = false;

                            if (kbState.IsKeyDown(rightKey) || kbState.IsKeyDown(leftKey))
                            {
                                pState = playerStates.Run;
                            }

                            else
                            {
                                pState = playerStates.Stand;
                            }
                        }
                    }
                    else
                    {
                        jumping = true;
                        jumpHeight = -20;
                    }

                    break;
            }

            fpsRun = Math.Pow(speed * 3.5, 2.0); //this makes it so that when the player is just starting to run, his animation follows the speed and doesn't look too fast.
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
            }
        }

        public void DrawStand(SpriteBatch spriteBatch)
        {
            sourceRec = new Rectangle(0, 0, playerWidth, playerHeight);

            if (dirRight)
                spriteBatch.Draw(tex, pos, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            else
                spriteBatch.Draw(tex, pos, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        }

        public void DrawRun(SpriteBatch spriteBatch)
        {
            sourceRec = new Rectangle(currentFrameRun * playerWidth, playerHeight, playerWidth, playerHeight);

            if (dirRight)
                spriteBatch.Draw(tex, pos, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            else
                spriteBatch.Draw(tex, pos, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        }

        public void DrawJump(SpriteBatch spriteBatch)
        {
            sourceRec = new Rectangle(currentFrameJump * playerWidth, playerHeight * 2, playerWidth, playerHeight);

            if (dirRight)
                spriteBatch.Draw(tex, pos, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            else
                spriteBatch.Draw(tex, pos, sourceRec, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        }

        public void UpdateRunAnimation(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= timePerFrameRun)
            {
                currentFrameRun++;

                if (currentFrameRun >= framesRun)
                {
                    currentFrameRun = 0;
                }

                timeCounter -= timePerFrameRun;
            }
        }

        public void UpdateJumpAnimation(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= timePerFrameJump)
            {
                if(currentFrameJump < framesJump - 1)
                    currentFrameJump++;
                timeCounter -= timePerFrameJump;
            }
        }

    }
}