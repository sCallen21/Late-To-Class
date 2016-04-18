using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Chris Banks
namespace Late_To_Class
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Base Game Types
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        //Menu State items
        MenuComponent menuComponent;
        Scene activeScene;
        
        //helpscene 
        HelpScene helpScene;
        Texture2D helpForegroundTexture;
        Texture2D backScene;
               
        KeyboardState kbState, previousKbState;
        Rectangle rec;

        //pause
        Texture2D pausedTex;
        Rectangle pausedRec;
        Button btnPlay, btnQuit;
        Texture2D playTex;
        Texture2D quitTex;

        //death
        Texture2D deathTex;
        Rectangle deathRec;

        
        
        //level details
        Player player;
        Point screen;
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

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
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            //Full Game Content - This section is for content the entire game needs
            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();
            rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Tahoma_40");
            screen.X = GraphicsDevice.Viewport.Width;
            screen.Y = GraphicsDevice.Viewport.Height;
            player.Load(Content);


            //help menu - TODO - replace with options menu
            backScene = Content.Load<Texture2D>("helpBack");
            helpForegroundTexture = Content.Load<Texture2D>("helpFront");
            helpScene = new HelpScene(this, backScene);
            Components.Add(helpScene);
            
            //Main Menu
            string[] menuItems = { "Start Game", "Options", "End Game" };
            menuComponent = new MenuComponent(this, spriteBatch, font, menuItems);
            Components.Add(menuComponent);

            //pause 
            pausedTex = Content.Load<Texture2D>("Pause.png");
            pausedRec = new Rectangle(100, 0, pausedTex.Width, pausedTex.Height);
            btnPlay = new Button();
            btnQuit = new Button();
            playTex = Content.Load<Texture2D>("Quit.jpg");
            btnPlay.load(playTex, new Vector2(350, 225));
            quitTex = Content.Load<Texture2D>("Play.jpg");
            btnQuit.load(quitTex, new Vector2(350, 275));

            //death
            deathTex = Content.Load<Texture2D>("Death.png");
            deathRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);





            //Game
            LevelManager.Instance.LoadLevel("Test.txt", Content, GraphicsDevice.Viewport, player);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            switch (activeScene)
            {
                case Scene.MainMenu:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        switch (menuComponent.SelectedIndex)
                        {
                            case 0:
                                LevelManager.Instance.LoadLevel("Test.txt", Content, GraphicsDevice.Viewport, player);
                                activeScene = Scene.Game;
                                break;
                            case 1:
                                activeScene = Scene.Options;
                                break;
                            case 2:
                                activeScene = Scene.Exit;
                                break;
                        }
                    }

                    break;
                case Scene.Options:
                    if (SingleKeyPress(Keys.Escape))
                    {
                        activeScene = Scene.MainMenu;
                    }
                    break;

                case Scene.Game:
                    LevelManager.Instance.UpdateLevel(gameTime);
                    if (LevelManager.Instance.Late()) { activeScene = Scene.Death; }


                    //pause 
                    btnPlay.isClicked = false;
                    btnQuit.isClicked = false;
                    if (SingleKeyPress(Keys.P))
                    {
                        activeScene = Scene.Paused;
                    }

                    break;

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
                case Scene.Death:
                    if (btnQuit.isClicked)
                    {
                        activeScene = Scene.MainMenu;
                    }
                    btnQuit.Update(mouse);
                    break;

                case Scene.Exit:
                    Exit();
                    break;
            }
            base.Update(gameTime);
        }

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
                    base.Draw(gameTime);
                    spriteBatch.End();
                    break;
                case Scene.Options:
                    spriteBatch.Begin();
                    GraphicsDevice.Clear(Color.SlateGray);
                    spriteBatch.Draw(backScene, rec, Color.White);
                    spriteBatch.End();
                    break;
                case Scene.Game:
                    GraphicsDevice.Clear(Color.IndianRed);
                    LevelManager.Instance.DrawLevel(spriteBatch, screen, font);
                    break;
                case Scene.Paused:
                    spriteBatch.Begin();
                    spriteBatch.Draw(pausedTex, pausedRec, Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnQuit.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case Scene.Death:
                    spriteBatch.Begin();
                    spriteBatch.Draw(deathTex, deathRec, Color.White);
                    btnQuit.Draw(spriteBatch);
                    spriteBatch.End();
                    break;

            }
        }
            

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
    }
}
