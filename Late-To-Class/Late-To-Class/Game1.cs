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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MenuComponet menuComponent;
        //enum for FNS machine 
        Scene activeScene;
        //helpscene 
        Texture2D helpBackgroundTexture;
        Texture2D helpForegroundTexture;
        HelpScene helpScene;

        //checks the users key presses 
        KeyboardState kbState, previousKbState;
        Texture2D backScene;
        SpriteFont font;
        Rectangle rec;
        string cameraNotes = "Stephen";

        Texture2D testLevel;
        Texture2D playerImage;
        Player player;
        Camera camera;
        Point screen;
        Point CameraOrigin;

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

            activeScene = Scene.Game;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //Main menu
            rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Tahoma_40");
            string[] menuItems = { "Start Game", "Help", "End Game" };
            
            //help menu
            helpBackgroundTexture = Content.Load<Texture2D>("helpBack");
            helpForegroundTexture = Content.Load<Texture2D>("helpFront");

            playerImage = Content.Load<Texture2D>("player.png");
            player.Tex = playerImage;
            
            //help menu
            helpScene = new HelpScene(this, helpBackgroundTexture);
            Components.Add(helpScene);
            backScene = helpBackgroundTexture;
            menuComponent = new MenuComponet(this, spriteBatch, font, menuItems);
            Components.Add(menuComponent);

            //DO NOT FUCKING TOUCH
            testLevel = Content.Load<Texture2D>("tiles.png");
            LevelBuilder.Instance.LoadMap("Test.txt");
            LevelBuilder.Instance.TileMaker(testLevel);
            camera = new Camera(GraphicsDevice.Viewport);
            screen.X = GraphicsDevice.Viewport.Width;
            screen.Y = GraphicsDevice.Viewport.Height;

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
                        player.Update(gameTime);
                        camera.Update(player.position, LevelBuilder.Instance.MapSize.X * 32, LevelBuilder.Instance.MapSize.Y * 32);
                        CameraOrigin.X = camera.cameraView.X + player.speed;
                        CameraOrigin.Y = camera.cameraView.Y + player.speed;
                        cameraNotes = CameraOrigin.X.ToString() + ";" + CameraOrigin.Y.ToString();

                        
    
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
                    //draws the main menu
                    GraphicsDevice.Clear(Color.SlateGray);
                    //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null); 
                    base.Draw(gameTime);
                   // spriteBatch.End();
                    break;
                case Scene.Help:
                    spriteBatch.Begin();
                    GraphicsDevice.Clear(Color.SlateGray);
                    spriteBatch.Draw(backScene, rec, Color.White);
                    spriteBatch.End();
                    break;
                case Scene.Game:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
                    GraphicsDevice.Clear(Color.SlateGray);
                    LevelBuilder.Instance.Draw(spriteBatch, screen, CameraOrigin);
                    spriteBatch.DrawString(font, cameraNotes, new Vector2(50, 50), Color.White);
                    player.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }
             //spriteBatch.End();
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
