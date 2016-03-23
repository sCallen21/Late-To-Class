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
        Player player;
        Texture2D pTex;
        Camera camera;
        Texture2D testLevel;
        SpriteFont font;
        Point screenSize;
        Point cameraOrigin;
        string cameraNotes;

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenSize.X = GraphicsDevice.Viewport.Width;
            screenSize.Y = GraphicsDevice.Viewport.Height;
            font = Content.Load<SpriteFont>("font");



            GameControls.Instance.LoadControls(); //loads in any previously defined user controls, else, defaults to WASDJ
            
            testLevel = Content.Load<Texture2D>("tiles.png"); //replace this with our actual tilesheet when available
            LevelBuilder.Instance.LoadMap("Test.txt");        //Loads in the testing map.  
            LevelBuilder.Instance.TileMaker(testLevel, 32);  //creates a new set of map tiles of specified size and using the testing tileSheet
            camera = new Camera(GraphicsDevice.Viewport);    //creates a new camera that follows the player within the bounds of the map
            player = new Player();                          

            pTex = Content.Load<Texture2D>("Kirby.png");
            player.Tex = pTex;



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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            player.Update(gameTime);
            camera.Update(player.position, 200 * 32, 40 * 32);
            cameraOrigin.X += camera.cameraView.X + player.speed;
            cameraOrigin.Y += camera.cameraView.Y;
            cameraNotes = cameraOrigin.X.ToString() + ";" + cameraOrigin.Y.ToString();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            LevelBuilder.Instance.Draw(spriteBatch, screenSize, cameraOrigin);
            player.Draw(spriteBatch);
            spriteBatch.DrawString(font, cameraNotes, new Vector2(0,0), Color.White);
            spriteBatch.End();

           
            
            base.Draw(gameTime);
        }
    }
}
