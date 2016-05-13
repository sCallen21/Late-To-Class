using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

//Chris Banks
namespace Late_To_Class
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Variables
        //Base Game Types
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        //Menu State items
        MenuComponent menuComponent;
        Scene activeScene;
        Rectangle textRec;
        Texture2D textTex;
        //parallax menu
        Rectangle texRec;
        Rectangle texRecTwo;
        Texture2D menuTex;

        //helpscene 
        HelpScene helpScene;
        Texture2D helpForegroundTexture;
        OptionComponet OptionComponet;
        KeyboardState kbState, previousKbState;
        Rectangle rec;

        //pause
        Texture2D pausedTex;
        Rectangle pausedRec;
        Button btnPlay, btnQuit;
        Texture2D playTex;
        Texture2D quitTex;

        //win 
        Texture2D winTex;
              
        //death
        Texture2D deathTex;
        Rectangle deathRec;

        //mouse
        MouseState mouse = Mouse.GetState();
        static public Vector2 pos;
        static private Texture2D tex;
        static public MouseState mouseState;
        static public MouseState previousState;


        //level details
        Player player;
        Point screen;

        #endregion

        #region Constructors
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion

        #region Initialize and Load
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player = new Player();
            activeScene = Scene.MainMenu;

            //menu parallax 
            texRec = new Rectangle(0, 0, 1500, 600);
            texRecTwo = new Rectangle(1500, 0, 1500, 600);
 
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            #region FullGame Content
            //Full Game Content - This section is for content the entire game needs        
            rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Tahoma_40");
            screen.X = GraphicsDevice.Viewport.Width;
            screen.Y = GraphicsDevice.Viewport.Height;
            player.Load(Content);
            #endregion

            #region Options Menu
            helpForegroundTexture = Content.Load<Texture2D>("HelpTex.png");
            GameControls.Instance.LoadControls();
            string[] OptionItems = new string[6];
            Keys[] controls = new Keys[5];
            GameControls.Instance.conflicting.CopyTo(controls, 0);
            for (int i = 0; i < controls.Length; i++)
            {
                OptionItems[i] = controls[i].ToString();
            }
            OptionItems[5] = "Exit";
            OptionComponet = new OptionComponet(this, spriteBatch, font, OptionItems);
            Components.Add(OptionComponet);
            #endregion

            #region Main Menu
            string[] menuItems = { "Start Game", "Endless", "Options", "End Game" };
            menuComponent = new MenuComponent(this, spriteBatch, font, menuItems);
            menuTex = Content.Load<Texture2D>("Menu.png");
            Components.Add(menuComponent);
            textTex = Content.Load<Texture2D>("TexTitle.png");
            textRec = new Rectangle(200, 5, textTex.Width, 150);
            #endregion

            #region Pause Menu
            pausedTex = Content.Load<Texture2D>("Pause.png");
            pausedRec = new Rectangle(100, 0, pausedTex.Width, pausedTex.Height);
            btnPlay = new Button();
            btnQuit = new Button();
            playTex = Content.Load<Texture2D>("Play.jpg");
            btnPlay.Load(playTex, new Vector2(350, 225));
            quitTex = Content.Load<Texture2D>("Quit.jpg");
            btnQuit.Load(quitTex, new Vector2(350, 275));
            #endregion

            #region Death Screen
            //death
            deathTex = Content.Load<Texture2D>("Death.png");
            deathRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            tex = Content.Load<Texture2D>("Mouse.PNG");
            #endregion

            #region Win Screen
            winTex = Content.Load<Texture2D>("Wintex.png");
            #endregion
        }     

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }
        #endregion

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            previousState = mouseState;
            mouseState = Mouse.GetState(); //Needed to find the most current mouse states.
            pos.X = mouseState.X; //Change x pos to mouseX
            pos.Y = mouseState.Y; //Change y pos to mouseY

            switch (activeScene)
            {
                #region MainMenu
                case Scene.MainMenu:
                    if (texRec.X + menuTex.Width <= 0)
                        texRec.X = texRecTwo.X + menuTex.Width;
                    // Then repeat this check for rectangle2.
                    if (texRecTwo.X + menuTex.Width <= 0)
                        texRecTwo.X = texRec.X + menuTex.Width;

                    texRec.X -= 3;
                    texRecTwo.X -= 3;

                    if (SingleKeyPress(Keys.Enter))
                    {
                        switch (menuComponent.SelectedIndex)
                        {
                            case 0:
                                LevelManager.Instance.LoadLevel("Levelv2.txt", "SpawnAndCollisionv2.txt", Content, GraphicsDevice.Viewport, player);
                                activeScene = Scene.Game;
                                break;
                            case 1:
                                LevelManager.Instance.LoadEndless(Content, GraphicsDevice.Viewport, player);
                                activeScene = Scene.Endless;
                                break;
                            case 2:
                                activeScene = Scene.Options;
                                break;
                            case 3:
                                activeScene = Scene.Exit;
                                break;
                        }
                    }
                    break;
#endregion
                  
                #region Options
                case Scene.Options:

                    if (SingleKeyPress(Keys.Enter))
                    {

                        switch (OptionComponet.SelectedIndex)
                        {
                            case 0:
                                try
                                {
                                    Keys[] pressed = kbState.GetPressedKeys();
                                    Keys up = pressed[1];
                                    GameControls.Instance.ConfigureControls("jump", up);
                                    GameControls.Instance.SaveControls();
                                    GameControls.Instance.LoadControls();
                                    OptionComponet.MenuItem[0] = up.ToString();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }


                                break;
                            case 1:
                                try
                                {
                                    Keys[] pressed = kbState.GetPressedKeys();
                                    Keys down = pressed[1];
                                    GameControls.Instance.ConfigureControls("duck", down);
                                    GameControls.Instance.SaveControls();
                                    GameControls.Instance.LoadControls();
                                    OptionComponet.MenuItem[1] = down.ToString();
                                   
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case 2:
                                try
                                {
                                    Keys[] pressed = kbState.GetPressedKeys();
                                    Keys left = pressed[1];
                                    GameControls.Instance.ConfigureControls("moveLeft", left);
                                    GameControls.Instance.SaveControls();
                                    GameControls.Instance.LoadControls();
                                    OptionComponet.MenuItem[2] = left.ToString();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case 3:
                                try
                                {
                                    Keys[] pressed = kbState.GetPressedKeys();
                                    Keys right = pressed[1];
                                    GameControls.Instance.ConfigureControls("moveRight", right);
                                    GameControls.Instance.SaveControls();
                                    GameControls.Instance.LoadControls();
                                    OptionComponet.MenuItem[3] = right.ToString();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case 4:
                                try
                                {
                                    Keys[] pressed = kbState.GetPressedKeys();
                                    Keys powerUp = pressed[1];
                                    GameControls.Instance.ConfigureControls("powerUp", powerUp);
                                    GameControls.Instance.SaveControls();
                                    GameControls.Instance.LoadControls();
                                    OptionComponet.MenuItem[4] = powerUp.ToString();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case 5:
                                GameControls.Instance.SaveControls();
                                GameControls.Instance.LoadControls();
                                activeScene = Scene.MainMenu;
                                break;
                        }

                    }

                    break;
                #endregion

                #region Game
                case Scene.Game:
                    LevelManager.Instance.UpdateLevel(gameTime);
                    
                    if (LevelManager.Instance.Late()) { activeScene = Scene.Death; }

                   if (player.position.X >= LevelBuilder.Instance.GoalPosition.X) { activeScene = Scene.Win;}

                    //pause 
                    btnPlay.Color = new Color(255, 255, 255, 255);
                    btnQuit.Color = new Color(255, 255, 255, 255);
                    btnPlay.isClicked = false;
                    btnQuit.isClicked = false;
                    if (SingleKeyPress(Keys.P))
                    {
                        activeScene = Scene.Paused;
                    }

                    break;
                #endregion

                #region Endless
                case Scene.Endless:
                    
                    break;
                #endregion

                #region PauseMenu
                case Scene.Paused:
                    if (SingleKeyPress(Keys.P))
                    {
                        activeScene = Scene.Game;
                    }
                    if (btnPlay.isClicked)
                    {
                        activeScene = Scene.Game;
                    }
                    if (btnQuit.isClicked)
                    {
                        activeScene = Scene.MainMenu;
                    }
                    btnPlay.Update(mouse);
                    btnQuit.Update(mouse);
                    break;
                #endregion

                #region Death
                case Scene.Death:
                    if (btnQuit.isClicked)
                    {
                        activeScene = Scene.MainMenu;
                    }
                    btnQuit.Update(mouse);
                    break;
                #endregion

                #region Win
                case Scene.Win:
                    if (btnQuit.isClicked)
                    {
                        activeScene = Scene.MainMenu;
                    }
                    btnQuit.Update(mouse);
                    break;
                #endregion

                #region Exit
                case Scene.Exit:
                    Exit();
                    break;
                #endregion
            }
            base.Update(gameTime);
        }
        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            switch (activeScene)
            {
                case Scene.MainMenu:
                    GraphicsDevice.Clear(Color.SlateGray);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null);
                    spriteBatch.Draw(menuTex, texRecTwo, Color.White);
                    spriteBatch.Draw(menuTex, texRec, Color.White);
                    spriteBatch.Draw(textTex, textRec, Color.White);
                    base.Draw(gameTime);
                    spriteBatch.End();
                    break;
                case Scene.Options:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null);
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Draw(helpForegroundTexture, rec, Color.White);
                    OptionComponet.Draw();
                    spriteBatch.End();
                    break;
                case Scene.Game:
                    GraphicsDevice.Clear(Color.DeepSkyBlue);
                    LevelManager.Instance.DrawLevel(spriteBatch, screen, font);
                    break;
                case Scene.Paused:
                    GraphicsDevice.Clear(Color.SlateGray);
                    spriteBatch.Begin();
                    spriteBatch.Draw(pausedTex, pausedRec, Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnQuit.Draw(spriteBatch);
                    spriteBatch.Draw(tex, pos, Color.White);
                    spriteBatch.End();
                    break;
                case Scene.Death:
                    GraphicsDevice.Clear(Color.SlateGray);
                    spriteBatch.Begin();
                    spriteBatch.Draw(deathTex, deathRec, Color.White);
                    btnQuit.Draw(spriteBatch);
                    spriteBatch.Draw(tex, pos, Color.White);
                    spriteBatch.End();
                    break;
                case Scene.Win:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    spriteBatch.Draw(winTex, rec, Color.White);
                    spriteBatch.DrawString(font, (LevelManager.Instance.Timer + "s"), (new Vector2(330, 205)), Color.Red, 0f, new Vector2(0,0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font, PlayerGrade(), (new Vector2(550, 205)), Color.Red, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    btnQuit.Draw(spriteBatch);
                    spriteBatch.Draw(tex, pos, Color.White);
                    spriteBatch.End();
                    break;

            }
        }

        #endregion

        #region Helpers
        public bool SingleKeyPress(Keys k)
        {
            //gets the current keyboard state
            kbState = Keyboard.GetState();
            //checks if the key was pressed 
            if (kbState.IsKeyDown(k) && previousKbState.IsKeyUp(k))
            {
                previousKbState = kbState;
                return true;
            }
            previousKbState = kbState;
            return false;
        }

        public string PlayerGrade()
        {
            if (LevelManager.Instance.Timer <= 40 && LevelManager.Instance.Timer >= 30)
            {
                return "A";
            }
            if (LevelManager.Instance.Timer < 30 && LevelManager.Instance.Timer >= 20)
            {
                return "B";
            }
            if (LevelManager.Instance.Timer < 20 && LevelManager.Instance.Timer >= 15)
            {
                return "C";
            }
            if (LevelManager.Instance.Timer < 15 && LevelManager.Instance.Timer >= 10)
            {
                return "D";
            }
            if (LevelManager.Instance.Timer < 10 && LevelManager.Instance.Timer >= 0)
            {
                return "F";
            }
            return "F";
        }
        #endregion
    }
}
