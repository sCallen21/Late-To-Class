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
        private GameTime time = new GameTime();
        private double airTime;
        private int jumpHeight;
        private int gravity;
        private int baseHeight;


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


        bool dirRight; //bool indicating facing direction with right == true
        bool jumping;

        public Player()
        {
            pState = playerStates.Stand;
            dirRight = true;
            pos = new Rectangle(200, 100, 66, 66);
            jumpHeight = pos.Y;
            baseHeight = pos.Y;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            switch (pState)
            {
                case playerStates.Run:
                    if (kbState.IsKeyDown(Keys.A) && (dirRight == true || dirRight == false))
                    {
                        pos.X -= 5;
                        dirRight = false;
                    }

                    else if (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D) /*&& dirRight == true*/)
                    {
                        pState = playerStates.Stand;
                        //dirRight = false;
                    }

                    else if (kbState.IsKeyDown(Keys.D) && (dirRight == true || dirRight == false))
                    {
                        pos.X += 5;
                        dirRight = true;
                    }

                    else if (kbState.IsKeyUp(Keys.D) && kbState.IsKeyUp(Keys.A) /*&& dirRight == false*/)
                    {
                        pState = playerStates.Stand;
                        //dirRight = true;
                    }

                    break;


                case playerStates.Stand:
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        //dirRight = false;
                        pState = playerStates.Run;
                    }

                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        //dirRight = false;
                        pState = playerStates.Run;

                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        pState = playerStates.Jump;
                    }
                    break;

                case playerStates.Jump: //http://flatformer.blogspot.com/2010/02/making-character-jump-in-xnac-basic.html
                    if (jumping == true)
                    {
                        pos.Y += jumpHeight;
                        jumpHeight += 1;

                        if (pos.Y >= baseHeight)
                        {
                            pos.Y = baseHeight;
                            jumping = false;
                            pState = playerStates.Stand;
                        }
                    }

                    else if (jumping == true && kbState.IsKeyDown(Keys.D))
                    {
                        pos.Y += jumpHeight;
                        pos.X += jumpHeight;
                        jumpHeight += 1;

                        if (pos.Y >= baseHeight)
                        {
                            pos.Y = baseHeight;
                            jumping = false;

                            if (kbState.IsKeyDown(Keys.D))
                            {
                                pState = playerStates.Run;
                            }

                            else
                            {
                                pState = playerStates.Run;
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
        }

        public void Draw(SpriteBatch playerSprite)
        {
            playerSprite.Draw(tex, pos, Color.White);

        }

    }
}
