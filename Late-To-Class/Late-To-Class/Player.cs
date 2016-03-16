using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Late_To_Class
{
    class Player
    {
        private Rectangle pos;
        private Texture2D tex;
        //private GameTime time = new GameTime();

        private double airTime;
        private int jumpHeight;
        private int gravity;
        private int baseHeight;

        private Keys jumpKey;
        private Keys leftKey;
        private Keys rightKey;
        private Keys duckKey;
        private Keys powerUpKey;

        //these variables handle acceleration of the player
        private int speed; //determines how fast the player is moving. This value increases as the player continues to move.
        private int maxSpeed; //determines the player's max speed
        private double accTimer; //this counts how long it's been since the player speed increased. It will measure this and when it reaches a value, increment the player's speed.
        private double timeToNextAcc; //this is the time it takes for the player to increase his speed by 1. This is measured in seconds.
        private double timeToNextDec; //this is the time it takes for the player to decrease his speed by 1. This is measured in seconds.


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

            speed = 1; //initial speed of the player
            maxSpeed = 10; //maximum speed of the player
            accTimer = 0; //how long since last increment of speed
            timeToNextAcc = 0.1; //how long it takes to increment speed
            timeToNextDec = 0.05; //how long it takes to decrement speed

            jumpKey = GameControls.Instance.jumpKey;
            leftKey = GameControls.Instance.moveLeft;
            rightKey = GameControls.Instance.moveRight;
            duckKey = GameControls.Instance.duckKey;
            powerUpKey = GameControls.Instance.powerUpKey;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            switch (pState)
            {
                case playerStates.Run:
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


                    //THIS DOES NOT WORK PROPERLY YET
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
                            dirRight = false;
                            
                    }

                    if (kbState.IsKeyDown(jumpKey))
                    {
                        pState = playerStates.Jump;
                    }

                    //this checks to see what direction the player is moving, and then adds the speed accordingly
                    if (dirRight)
                        pos.X += speed;
                    else if (!dirRight)
                        pos.X -= speed;

                    

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
                    if (jumping == true)
                    {
                        pos.Y += jumpHeight;
                        jumpHeight += 1;

                        if (kbState.IsKeyDown(leftKey))
                        {
                            dirRight = false;
                        }
                        else if (kbState.IsKeyDown(rightKey))
                        {
                            dirRight = true;
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
                            else
                                pState = playerStates.Stand;
                        }

                        //this checks to see what direction the player is moving, and then adds the speed accordingly
                        if (dirRight)
                            pos.X += speed;
                        else if (!dirRight)
                            pos.X -= speed;

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
                                pState = playerStates.Run;
                            else
                                pState = playerStates.Stand;
                        }
                    }

                    else if (jumping == true && kbState.IsKeyDown(rightKey))
                    {
                        pos.Y += jumpHeight;
                        pos.X += jumpHeight;
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
                        jumpHeight = -14;
                    }

                    break;


            }
            //I don't think this is necessary
            //prevState = pState; //this sets prevState to the state the player was in last frame (still technically this frame)
        }

        public void Draw(SpriteBatch playerSprite)
        {
            playerSprite.Draw(tex, pos, Color.White);

        }

    }
}
