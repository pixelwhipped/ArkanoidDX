using System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using ArkanoidDX.Arena;
using ArkanoidDX.Audio;
using ArkanoidDX.Graphics;
using ArkanoidDX.Input;
using ArkanoidDX.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ArkanoidDX
{
    public delegate void Setter<in TValue>(TValue value);

    public delegate TValue Getter<out TValue>();

    public delegate void Procedure<in TValue>(TValue value);

    public delegate void Operation<in TValue>(TValue a, TValue b);

    public delegate void Routine();

    public delegate void Deflection(CollisionPoint c, Direction d);


    public class ArkanoidDX : Game
    {
        public static Random Random = new Random();
        public static Texture2D Pixel;

        public Rectangle ArenaArea
        {
            get{return new Rectangle((int) (8*Scale.X), (int) (8*Scale.X), FrameArea.Width - (int) ((8*Scale.X)*2),
                                      (int) (Height - (8*Scale.X)));}
        }
        public AudioFx Audio;

        public Rectangle FrameArea
        {
            get{return new Rectangle(0, 0, (int) (Width*.5), Height); }
        }

        public Rectangle MenuArea
        {
            get { return new Rectangle(FrameArea.Width, 0, Width - FrameArea.Width, Height); }
        }
        public Rectangle ScreenArea {
            get
            {
                return new Rectangle(
                    0, 0, Width, Height);
            }
        }

        public Rectangle Window { get; private set; }

        public Vector2 Shadow = new Vector2(8, 10);
        public Color ShadowColor = new Color(0, 0, 0, .5f);
        public SpriteBatch SpriteBatch;
        public static int MaxBalls = 10;

        public GameSettings Settings { get; protected set; }

        public BaseArena Arena;

        public GameMode GameMode;

        public int BlocksWide;
        public ArkanoidDX Game
        {
            get { return this; }
        }

        public readonly GraphicsDeviceManager GraphicsDeviceManager;

        public int Width
        {
            get { return GraphicsDevice.Viewport.Width; }
        }
        public int Height
        {
            get { return GraphicsDevice.Viewport.Height; }
        }

        public Vector2 Center
        {
            get { return new Vector2(Width / 2f, Height / 2f); }
        }
        public Vector2 Scale { get { return new Vector2(FrameArea.Width / (((BlocksWide + 1f)*16f)), FrameArea.Width / ((BlocksWide + 1f)*16)); } } //12f

        public static string AppData;

        public KeyboardInput KeyboardInput { get; private set; }
        public TouchInput TouchInput { get; private set; }
        

        public PauseArena PauseArena;
        public ArkanoidDX()
        {            
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Initialize();
        }

        protected override void Initialize()
        {
            //ApplicationView.Value = ApplicationViewState.FullScreenLandscape;
            ;// view = ApplicationView.Value.;
            
            IsMouseVisible = true;
            Audio = new AudioFx();            
            BlocksWide = Level.ClassicBricksWide;                        
            KeyboardInput = new KeyboardInput();
            TouchInput = new TouchInput();
            
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
                
            Settings = new GameSettings(this);
            SpriteBatch = new SpriteBatch(GraphicsDeviceManager.GraphicsDevice);
            Pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] {Color.White});

            Window = new Rectangle(0, 0,
                   (int)Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Bounds.Width,
                   (int)Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Bounds.Height);
            
            GraphicsDeviceManager.PreferredBackBufferWidth = Window.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = Window.Height;
            GraphicsDeviceManager.IsFullScreen = true;
            GraphicsDeviceManager.ApplyChanges();

            
            Levels.Levels.LoadContent(Content);
            Fonts.LoadContent(Content);
            Textures.LoadContent(Content);
            Sprites.LoadContent(this, Content);
            PauseArena = new PauseArena(this);
            Arena = new MenuArena(this);
            Arena.Initialise();
        }

        protected override void UnloadContent()
        {
        }

        

        protected override void Update(GameTime gameTime)
        {

            Window = new Rectangle(0, 0,
                   (int)Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Bounds.Width,
                   (int)Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Bounds.Height);
            if (Window.Width != GraphicsDeviceManager.PreferredBackBufferWidth && Window.Height != GraphicsDeviceManager.PreferredBackBufferHeight)
            {
                GraphicsDeviceManager.PreferredBackBufferWidth = Window.Width;
                GraphicsDeviceManager.PreferredBackBufferHeight = Window.Height;
                GraphicsDeviceManager.ApplyChanges();
            }
    
            Sprites.Update(gameTime);
            KeyboardInput.Update(gameTime, this);
            TouchInput.Update(gameTime);
            
            Arena.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();        
                Arena.Draw(SpriteBatch);                            
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}