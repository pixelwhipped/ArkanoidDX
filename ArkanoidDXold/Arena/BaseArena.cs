using ArkanoidDX.Graphics;
using ArkanoidDX.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = ArkanoidDX.Graphics.IDrawable;

namespace ArkanoidDX.Arena
{
    public class BaseArena : IDrawable, ILocatable
    {

       // public static int BricksWide = ArkanoidLevel.Level.BricksWide;
       // public static int BricksHigh = ArkanoidLevel.Level.BricksHigh;

        public ArkanoidDX Game;

        public Fader Fade;

        public float FadeRotator;
        public bool FadeRotatorIn;

        public Warp LeftWarp;
        public Warp RightWarp;
        public SideEntry SideLeftMidEntry;
        public SideEntry SideLeftTopEntry;
        public SideEntry SideRightMidEntry;
        public SideEntry SideRightTopEntry;
        public TopEntry TopLeftEntry;
        public TopEntry TopRightEntry;

        public Vector2 Motion { get; set; }
        public Rectangle Bounds { get { return Game.ArenaArea; } }
        public Vector2 Location
        {
            get { return new Vector2(X, Y); }
            set { }
        }
        public Vector2 Center
        {
            get { return new Vector2(Game.ArenaArea.Center.X, Game.ArenaArea.Center.Y); }
        }

        public float Width { get { return Game.ArenaArea.Width; } }
        public float Height { get { return Game.ArenaArea.Height; } }

        public float X
        {
            get { return Game.ArenaArea.X; }
            set { }
        }

        public float Y
        {
            get { return Game.ArenaArea.Y; }
            set { }
        }

        public BaseArena(ArkanoidDX game)
        {
            Game = game;
            TopLeftEntry = new TopEntry(Game, new Vector2(Sprites.FrmCorner.Width + Sprites.FrmTopOpen.Width, 0));
            TopRightEntry = new TopEntry(Game,
                                         new Vector2(
                                             Game.FrameArea.Width - (Sprites.FrmCorner.Width + (Sprites.FrmTopOpen.Width*2)),
                                             0));
            SideLeftTopEntry = new SideEntry(Game, new Vector2(0, Sprites.FrmCorner.Height), false);
            SideLeftMidEntry = new SideEntry(Game,
                                              new Vector2(0,
                                                          (Game.FrameArea.Height / 2f) - (Sprites.FrmSideWarp.Height / 2)), false);
            SideRightTopEntry = new SideEntry(Game,
                                             new Vector2(Game.FrameArea.Width - Sprites.FrmSideOpen.Width,
                                                         Sprites.FrmCorner.Height), true);
            SideRightMidEntry = new SideEntry(Game,
                                             new Vector2(Game.FrameArea.Width - Sprites.FrmSideOpen.Width,
                                                         (Game.FrameArea.Height / 2f) - (Sprites.FrmSideWarp.Height/2)), true);
            LeftWarp = new Warp(new Vector2(0, Game.FrameArea.Height - (Sprites.FrmSideWarp.Height*2)), false);
            RightWarp = new Warp(new Vector2(Game.FrameArea.Width - Sprites.FrmSideWarp.Width,
                                             Game.FrameArea.Height - (Sprites.FrmSideWarp.Height*2)), true);
            Fade = new Fader(true, true);

        }


        public virtual void Initialise()
        {
            if (RightWarp.IsOpen || RightWarp.IsOpening)
                RightWarp.Close(() => { });
            if (LeftWarp.IsOpen || LeftWarp.IsOpening)
                LeftWarp.Close(() => { });
        }

         public virtual void Update(GameTime gameTime)
         {
             Fade.Update();
             FadeRotator = (FadeRotatorIn) ? MathHelper.Clamp(FadeRotator += 0.01f, .2f, .8f) : MathHelper.Clamp(FadeRotator -= 0.01f, .2f, .8f);
             if (FadeRotator >= .8f) FadeRotatorIn = false;
             if (FadeRotator <= .2f) FadeRotatorIn = true;
             UpdateWarps(gameTime);
             UpdateEntires(gameTime);
         }

         public void UpdateWarps(GameTime gameTime)
         {
             LeftWarp.Update(gameTime);
             RightWarp.Update(gameTime);
         }

         public void UpdateEntires(GameTime gameTime)
         {
             TopLeftEntry.Update(gameTime);
             TopRightEntry.Update(gameTime);
             SideRightTopEntry.Update(gameTime);
             SideRightMidEntry.Update(gameTime);
             SideLeftTopEntry.Update(gameTime);
             SideLeftMidEntry.Update(gameTime);
         }

         public virtual void Draw(SpriteBatch batch)
         {

         }

         public void DrawTitle(SpriteBatch batch)
         {
             var s = (Game.Width - Game.FrameArea.Width) / Sprites.CmnArkanoidDxLogoA.Width;
             batch.Draw(Sprites.CmnArkanoidDxLogoA, new Vector2(Game.FrameArea.Width, 0), Color.White, s);
             batch.Draw(Sprites.CmnArkanoidDxLogoB, new Vector2(Game.FrameArea.Width, 0), (Color.White * FadeRotator), s);

         }

         public void DrawShip(SpriteBatch batch)
         {
             var s = (Game.Width - Game.FrameArea.Width) / Sprites.CmnArkanoidShip.Width;
             batch.Draw(Sprites.CmnArkanoidShip,
                        new Rectangle(Game.FrameArea.Width - 1, (int)(Height - (Sprites.CmnArkanoidShip.Height * s)),
                                      (int)(Sprites.CmnArkanoidShip.Width * s), (int)(Sprites.CmnArkanoidShip.Height * s)), Color.White);
         }

         public void DrawEntries(SpriteBatch batch)
         {
             TopLeftEntry.Draw(batch);
             TopRightEntry.Draw(batch);
             SideRightTopEntry.Draw(batch);
             SideRightMidEntry.Draw(batch);
             SideLeftTopEntry.Draw(batch);
             SideLeftMidEntry.Draw(batch);
         }

         public void DrawBackground(SpriteBatch batch, BackGroundTypes bgType)
         {
             var y = 0;
             for (var j = 0; j < (int)(Game.Height / Types.GetBackGround(bgType).Height) + 1; j++)
             {
                 var x = (Sprites.BrkWhite.Width * Game.Game.BlocksWide);
                 for (var i = 0; i < Bounds.Width / Types.GetBackGround(bgType).Width; i++)
                 {
                     x -= (int)Types.GetBackGround(bgType).Width;
                     batch.Draw(Types.GetBackGround(bgType),
                                new Vector2(Bounds.X + x, Bounds.Y + y),
                                Color.Lerp(Color.Black, Color.White, Fade.Fade));
                 }
                 y += (int)Types.GetBackGround(bgType).Height;
             }
         }

         public void DrawFrameLeft(SpriteBatch batch)
         {
             batch.Draw(Sprites.FrmTop,
                        new Rectangle((int)Game.Shadow.X, (int)Game.Shadow.Y,
                                      Game.FrameArea.Width - (int)(Game.Shadow.X + Sprites.FrmSide.Width),
                                      (int)Sprites.FrmTop.Height), Game.ShadowColor);
             batch.Draw(Sprites.FrmSide,
                        new Rectangle((int)Game.Shadow.X, (int)(Sprites.FrmTop.Height + Game.Shadow.Y),
                                      (int)Sprites.FrmSide.Width,
                                      Game.FrameArea.Height - (int)(Sprites.FrmTop.Height + Game.Shadow.Y)),
                        Game.ShadowColor);

             batch.Draw(Sprites.FrmCorner, new Vector2(Game.FrameArea.X, Game.FrameArea.Y), Color.White);
             batch.Draw(Sprites.FrmTop,
                        new Rectangle((int)Sprites.FrmSide.Width, 0,
                                      Game.FrameArea.Width - ((int)Sprites.FrmSide.Width * 2), (int)Sprites.FrmTop.Height),
                        Color.White);
             batch.Draw(Sprites.FrmCorner, new Vector2(Game.FrameArea.Width - Sprites.FrmCorner.Width, Game.FrameArea.Y),
                        Color.White, 1, true);
             batch.Draw(Sprites.FrmSide,
                        new Rectangle(0, (int)Sprites.FrmTop.Height, (int)Sprites.FrmSide.Width, Game.FrameArea.Height),
                        Color.White);
         }

         public void DrawFrameRight(SpriteBatch batch)
         {
             batch.Draw(Sprites.FrmSide,
                        new Rectangle((int)(Game.FrameArea.Width - Sprites.FrmSide.Width), (int)Sprites.FrmTop.Height,
                                      (int)Sprites.FrmSide.Width, Game.FrameArea.Height), Color.White, 1, true);
         }

         public void DrawWarps(SpriteBatch batch)
         {
             LeftWarp.Draw(batch);
             RightWarp.Draw(batch);
         }
    }
}
