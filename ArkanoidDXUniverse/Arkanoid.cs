//https://plus.google.com/u/0/101017533262625040441
//http://pixelwhipped.my-free.website/
//pixelwhippedme@gmail.com
using System;
using Windows.UI.ViewManagement;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Input;
using ArkanoidDXUniverse.Levels;
using ArkanoidDXUniverse.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Arkanoid : Game
    {
        public static int MaxBalls = 10;

        public int BlocksWide;

        public Vector2 Shadow = new Vector2(8, 10);

        public Color ShadowColor = new Color(0, 0, 0, .5f);

        public static Random Random = new Random();
               
        public readonly GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch { get; set; }

        public static Texture2D Pixel;

        public readonly Sounds Sounds;

        public BaseArena Arena;

        public PauseArena PauseArena;        

        public GameMode GameMode;
       
        public bool IsPaused => ApplicationView.Value != ApplicationViewState.FullScreenLandscape;

        public Vector2 Scale
            => new Vector2(FrameArea.Width / ((BlocksWide + 1f) * 16f), FrameArea.Width / ((BlocksWide + 1f) * 16));

        public Rectangle ArenaArea
            => new Rectangle((int)(8 * Scale.X), (int)(8 * Scale.X), FrameArea.Width - (int)(8 * Scale.X * 2),
                (int)(Height - 8 * Scale.X));

        public Rectangle FrameArea => new Rectangle(0, 0, (int)(Width * .5), (int)Height);
        public Rectangle Bounds => new Rectangle(0, 0, (int)Width, (int)Height);
        public TouchInput TouchInput { get; private set; }
        public MouseInput MouseInput { get; private set; }
        public float Width => Graphics.GraphicsDevice.Viewport.Width;
        public float Height => Graphics.GraphicsDevice.Viewport.Height;               
        public Vector2 Center => new Vector2(Width / 2f, Height / 2f);
        public KeyboardInput KeyboardInput { get; private set; }
        public UnifiedInput UnifiedInput { get; private set; }
        public GameSettings Settings { get; set; }
        public Arkanoid Game => this;

        public static Starfield Starfield;
        public Arkanoid()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            Sounds = new Sounds(this);
            Branding.BackgroundColor = Color.Black;
        }


        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            BlocksWide = Level.ClassicBricksWide;
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Settings = new GameSettings(this);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Sounds.LoadContent();
            Pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] {Color.White});

            MouseInput = new MouseInput();
            TouchInput = new TouchInput();
            UnifiedInput = new UnifiedInput(this);
            KeyboardInput = new KeyboardInput(this);            
            UnifiedInput.TapListeners.Add(t => Arena?.OnTap(t));

            Levels.Levels.LoadContent(Content);
            Fonts.LoadContent(Content);
            Textures.LoadContent(Content);
            Sprites.LoadContent(this, Content);
            Starfield = new Starfield(100, ArenaArea);
            PauseArena = new PauseArena(this);
            Arena = new MenuArena(this);
            Arena.Initialise();
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (IsPaused)
            {
                PauseArena.Update(gameTime);
                return;
            }
            Starfield.Update(gameTime);
            if (!ApplicationView.GetForCurrentView().IsFullScreenMode)
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            TouchInput.Update(gameTime);
            MouseInput.Update(gameTime);
            UnifiedInput.Update(gameTime);
            KeyboardInput.Update(gameTime);
            Arena.Update(gameTime);
            Sprites.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Branding.BackgroundColor);
            SpriteBatch.Begin();
            if (IsPaused)
            {
                PauseArena.Draw(SpriteBatch);
            }
            else
            {
                Arena.Draw(SpriteBatch);
                
            }
            SpriteBatch.End();
            KeyboardInput.Draw(SpriteBatch);
            base.Draw(gameTime);
        }
    }
}