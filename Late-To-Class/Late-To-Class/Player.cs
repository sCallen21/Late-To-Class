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
        private Rectangle pos;
        private Texture2D tex;
        public Vector2 position;
        int playerHeight = 64;
        int playerWidth = 64;

        private double airTime;
        private double jumpHeight;
        private int platformHeight;
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
            fpsRun = Math.Pow(speed, 5.0);
            fpsJump = 5;
            timePerFrameRun = 1 / fpsRun;
            timePerFrameJump = 1 / fpsJump;
            currentFrameRun = 0;
            currentFrameJump = 0;
            framesRun = 10;
            framesJump = 4;



        }

        public void Load(ContentManager Content)
        {
            tex = Content.Load<Texture2D>("player");
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
                            pos.X += speed;
                        }

                        else if (!dirRight)
                        {
                            pos.X -= speed;
                        }

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

                        else if(pos.Y < baseHeight)
                        {
                            foreach(Rectangle tile in LevelBuilder.Instance.collisionBoxes)
                            {
                                if (pos.Intersects(tile))
                                {
                                    platformHeight = tile.Top;
                                    pos.Y = platformHeight;
                                    jumping = false;
                                }
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

                    /*if(pState == playerStates.Stand || pState == playerStates.Run || pState == playerStates.Jump) //Beginning of collision code
                    {
                        foreach(Rectangle tiles in LevelBuilder.Instance.collisionBoxes)
                        {
                            if (pos.Intersects(tiles))
                            {
                                if(pos.Y <= tiles.Top)
                                {
                                    platformHeight = tiles.Top;
                                    pos.Y = platformHeight;
                                }
                                else if (pos.X <= tiles.Left)
                                {
                                    pos.X = tiles.Left;
                                }
                                else if (pos.X >= tiles.Right)
                                {
                                    pos.X = tiles.Right;
                                }
                            }
                        }

                        //OR

                        /*for(int i = 0; i < LevelBuilder.Instance.collisionBoxes.Count; i++)
                        {
                            if (pos.Intersects(LevelBuilderInstance.collisionBoxes[i]))
                            {
                                if(pos.Y <= LevelBuilderInstance.collisionBoxes[i].Top)
                                {
                                    platformHeight = LevelBuilderInstance.collisionBoxes[i].Top;
                                    pos.Y = platformHeight;
                                }

                                else
                                {
                                    if(dirRight == true)
                                    {
                                        pos.X = LevelBuilderInstance.collisionBoxes[i].Left;
                                    }
                                    else if (dirRight == false)
                                    {
                                        pos.X = LevelBuilderInstance.collisionBoxes[i].Right;
                                    }
                                }
                            }
                        }
                    }*///End of collision code

                    break;
            }

            fpsRun = speed * 1.8;
            timePerFrameRun = 1 / fpsRun; //this makes it so that when the player is just starting to run, his animation follows the speed and doesn't look too fast.
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



///
//This first bit should go in Game1 update
/* 
foreach (CollisionTiles tile in map.CollisionTiles)
            {
                player.Collision(tile.Rectangle, map.Width, map.Height);
*/

//this is the entire player class for that project. Make sure to take a look at how the rectangle is updated. You can also take a look at how movement is done here, in case that helps
//Shove the above code into the Update section of Game1, and it will check the player for collisions there, and pass in the tile it hits.
/*
{
    class Player
    {
        private Texture2D texture;
        private Vector2 position = new Vector2 (64, 384);
        private Vector2 velocity;
        private Rectangle rectangle;

        private bool hasJumped = false;

        public Vector2 Position
        {
            get { return position; }
        }

        public Player() { }

        public void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("player");
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
            rectangle = new Rectangle((int) position.X, (int)position.Y, texture.Width, texture.Height);

            Input(gameTime);

            if (velocity.Y < 10) { velocity.Y += 0.4f; }

        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            }
            else { velocity.X = 0f; }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && hasJumped == false)
            {
                position.Y -= 5f;
                velocity.Y = -9f;
                hasJumped = true;
            }
        }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            if (rectangle.TouchTopOf(newRectangle))
            {
                rectangle.Y = newRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }

            if (rectangle.TouchLeftOf(newRectangle))
            {
                position.X = newRectangle.X - rectangle.Width - 2;
            }

            if (rectangle.TouchRightOf(newRectangle))
            {
                position.X = newRectangle.X + newRectangle.Width + 2;
            }

            if (rectangle.TouchBottomOf(newRectangle)) {velocity.Y = 1f;}

            if (position.X < 0) { position.X = 0;}
            if(position.X > xOffset - rectangle.Width) { position.X = xOffset - rectangle.Width;}
            if (position.Y < 0) {velocity.Y = 1f; }
            if (position.Y > yOffset - rectangle.Height) { position.Y = yOffset - rectangle.Height; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
*/

// the rectangles Touch(thing)of is from this class here. There are four different options for how something collides with a thing, and determines what axis to use for moving the player
//away from the thing it hit, rather than through it. The first three reset position, the TouchBottomOf check forces a jump to stop if it hits say, a roof
//The camera should handle everything with those last four if statements, they simply keep the player from leaving the bounds of the map, but I would say don't include those right now, I will deal with them
//later, because I might make the camera handle that instead. not sure yet, so don't worry about them.


//Hopefully this is straightforward enough for you. Each time the player hits something, the game will check using these four options to see which side of the tile the player hit.
//Each of these simply checks a series of bounds to determine where, and returns true or false depending on the result.
//Honestly, it would be best just to take these four methods and shove them into the class listed below, and then dont worry about them
//Hope that all this code helps you out, feel free to just use it if it works well enough as is, but if not, hopefully it is a good starting place for you
/*
static class RectangleHelper
    {
        public static bool TouchTopOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Bottom >= r2.Top - 1 &&
                r1.Bottom <= r2.Top + (r2.Height / 2) &&
                r1.Right >= r2.Left + (r2.Width / 5) &&
                r1.Left <= r2.Right - (r2.Width / 5));
        }

        public static bool TouchBottomOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Top <= r2.Bottom + (r2.Height / 5) &&
                    r1.Top >= r2.Bottom - 1 &&
                    r1.Right >= r2.Left + r2.Width / 5 &&
                    r1.Left <= r2.Right - (r2.Width / 5));
        }

        public static bool TouchLeftOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Right <= r2.Right &&
                    r1.Right >= r2.Left - 5 &&
                    r1.Top <= r2.Bottom - (r2.Width / 4) &&
                    r1.Bottom >= r2.Top + (r2.Width / 4));
        }

        public static bool TouchRightOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Left >= r2.Left &&
                    r1.Left <= r2.Right + 5 &&
                    r1.Top <= r2.Bottom - (r2.Width / 4) &&
                    r1.Bottom >= r2.Top + (r2.Width / 4));
        }
    }
}
*/