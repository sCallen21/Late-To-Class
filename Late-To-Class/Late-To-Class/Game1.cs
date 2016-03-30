using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Late_To_Class
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
<<<<<<< HEAD
        MenuComponet menuComponent;
        //enum for FNS machine 
        Scene activeScene;
        //helpscene 
        Texture2D helpBackgroundTexture;
        Texture2D helpForegroundTexture;
=======
        Player player;
        Texture2D pTex;
        Camera camera;
        Texture2D testLevel;
        private SpriteFont smallFont, largeFont, scoreFont, font;
        Point screenSize;
        Point cameraOrigin;
        string cameraNotes;
        
        enum GameStates // Ian Oliver, the main states throughout the game
        {
            Menu,
            Options,
            Game,
            Pause,
            GameOver,
            FinishLevel
        }

        GameStates gameState;
        GameStates prevGameState; 
        private Texture2D backHelpScene, frontHelpScene;
        private Texture2D startBackgroundTexture, startElementsTexture;
        private StartScene startScene;
        GameScene activeScene;
>>>>>>> 558c8be3bcf13246aa34ea2c8a40497b5aa9ef45
        HelpScene helpScene;
        //checks the users key presses 
        KeyboardState kbState, previousKbState;
        Texture2D backScene;
        SpriteFont font;
        Rectangle rec;

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
            // TODO: Add your initialization logic here
<<<<<<< HEAD
            activeScene = Scene.MainMenu;
=======
            //gameState = GameStates.Menu; // the menu will be the default state every time the game starts


            IsMouseVisible = true;
>>>>>>> 558c8be3bcf13246aa34ea2c8a40497b5aa9ef45
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch = new SpriteBatch(GraphicsDevice);
<<<<<<< HEAD
            font = Content.Load<SpriteFont>("Tahoma_40");
            // TODO: use this.Content to load your game content here
            string[] menuItems = { "Start Game", "Help", "End Game" };

            spriteBatch = new SpriteBatch(GraphicsDevice);

            helpBackgroundTexture = Content.Load<Texture2D>("helpBack");
            helpForegroundTexture = Content.Load<Texture2D>("helpFront");
            helpScene = new HelpScene(this, helpBackgroundTexture);
            Components.Add(helpScene);
=======
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            
            screenSize.X = GraphicsDevice.Viewport.Width;
            screenSize.Y = GraphicsDevice.Viewport.Height;
            font = Content.Load<SpriteFont>("font");



            GameControls.Instance.LoadControls(); //loads in any previously defined user controls, else, defaults to WASDJ
            
            testLevel = Content.Load<Texture2D>("tiles.png"); //replace this with our actual tilesheet when available
            LevelBuilder.Instance.LoadMap("Test.txt");        //Loads in the testing map.  
            LevelBuilder.Instance.TileMaker(testLevel);  //creates a new set of map tiles of specified size and using the testing tileSheet
            camera = new Camera(GraphicsDevice.Viewport);    //creates a new camera that follows the player within the bounds of the map
            player = new Player();                          

            pTex = Content.Load<Texture2D>("Kirby.png");
            player.Tex = pTex;

            
            /*
            //Create help scene
            frontHelpScene = Content.Load<Texture2D>("helpBack.png");
            backHelpScene = Content.Load<Texture2D>("helpFront.jpg.");
            helpScene = new HelpScene (this, backHelpScene, frontHelpScene);
            Components.Add(helpScene);
            

            // Create the Start Scene
            smallFont = Content.Load<SpriteFont>("Tahoma_40");
            largeFont = Content.Load<SpriteFont>("Tahoma_40");
            startBackgroundTexture = Content.Load<Texture2D>("6bd26c45-b949-4781-aaef-262d0132f60f");
            startElementsTexture = Content.Load<Texture2D>("titleForground");
            startScene = new StartScene(this, smallFont, largeFont,
                startBackgroundTexture, startElementsTexture);
            Components.Add(startScene);

            startScene.Show();
            activeScene = startScene;
            */

>>>>>>> 558c8be3bcf13246aa34ea2c8a40497b5aa9ef45

            backScene = Content.Load<Texture2D>("helpBack");

            menuComponent = new MenuComponet(this,
                spriteBatch,
                Content.Load<SpriteFont>("Tahoma_40"),
                menuItems);
            Components.Add(menuComponent);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
<<<<<<< HEAD
=======

             
            // Ian Oliver, Here is a basic skeleton for the overall finite state machine for the menus and the game as a whole.
            switch(gameState) 
            {
                case GameStates.Menu:
                    {

                    }
                    break;
                case GameStates.Options:
                    {

                    }
                    break;
                case GameStates.Game:
                    {

                    }
                    break;
                case GameStates.Pause:
                    {

                    }
                    break;
                case GameStates.GameOver:
                    {

                    }
                    break;
                case GameStates.FinishLevel:
                    {

                    }
                    break;
            }

            // this logic will be put in the 'Game' state enum
            
            

>>>>>>> 558c8be3bcf13246aa34ea2c8a40497b5aa9ef45

            // TODO: Add your update logic here
            switch (activeScene)
            {
                case Scene.MainMenu:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        switch (menuComponent.SelectedIndex)
                        {
                            case 0:
                                activeScene = Scene.Game;
                                break;
                            case 1:
                                activeScene = Scene.Help;
                                break;
                            case 2:
                                activeScene = Scene.Exit;
                                break;
                        }
                        
                    }
                    break;
                case Scene.Help:
                    if (SingleKeyPress(Keys.Back))
                    {
                        activeScene = Scene.MainMenu;
                    }
                        break;
                case Scene.Game:
                    break;
                case Scene.Exit:
                    Exit();
                    break;
            }
            base.Update(gameTime);
<<<<<<< HEAD
        }
=======
               
    }
>>>>>>> 558c8be3bcf13246aa34ea2c8a40497b5aa9ef45

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
<<<<<<< HEAD
             switch (activeScene)
            {
                case Scene.MainMenu:
                    //draws the main menu
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null);
                    base.Draw(gameTime);
                    spriteBatch.End();
                    break;
                case Scene.Help:
                    spriteBatch.Begin();
                    GraphicsDevice.Clear(Color.Beige);
                    spriteBatch.Draw(backScene, rec, Color.White);
                    spriteBatch.End();
                    break;
                case Scene.Game:
                    break;
            }
           
        }
=======
            GraphicsDevice.Clear(Color.SlateGray);

            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            LevelBuilder.Instance.Draw(spriteBatch, screenSize, cameraOrigin);
            player.Draw(spriteBatch);
            spriteBatch.DrawString(font, cameraNotes, new Vector2(0,0), Color.White);
            
            /*
            spriteBatch.Begin();
            base.Draw(gameTime);
             */ 
            spriteBatch.End();
>>>>>>> 558c8be3bcf13246aa34ea2c8a40497b5aa9ef45

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
