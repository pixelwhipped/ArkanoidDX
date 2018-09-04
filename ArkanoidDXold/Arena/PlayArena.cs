using System;
using System.Collections.Generic;
using System.Linq;
using ArkanoidDX.Audio;
using ArkanoidDX.Graphics;
using ArkanoidDX.Levels;
using ArkanoidDX.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDX.Arena
{
    public class PlayArena : PlayableArena
    {
        
        public LevelWadSelector LevelSelector;
        public Map LevelMap { get { return LevelSelector.Map; } }
        public TimeSpan NextBrickFlash;
        public TimeSpan NextEnimyRelease;
        private readonly Level _editLevel;

        public virtual bool IsOver { get { return LevelMap.IsOver; } }
        public TimeSpan EndDemo;
        public bool DemoMode;
        private readonly int _editLevelNum;
        private readonly string _editPath;
        private readonly List<Enemy> _enimieQue = new List<Enemy>();

        public PlayArena(ArkanoidDX game, LevelWadSelector selector, Vaus vaus, int editLevelNumber, Level editLevel = null, string editPath = null, bool demo = false)
            : base(game)
        {
            _editLevelNum = editLevelNumber;
            _editLevel = editLevel;
            _editPath = editPath;
            if(demo)
            {
                var w = Levels.Levels.Wads.Where(v => v.Levels.Count != 0).ToList();
                LevelSelector = new DemoLevelWadSelector(game, w[w.Count-1], 0, false);
                
            }
            else
            {
                LevelSelector = selector;
            }
            if (LevelSelector != null)
            {
                LevelSelector.Initialise(this);

                if (LevelMap.TopLeftEntryEnable) EnimyEntries.Add(TopLeftEntry);
                if (LevelMap.TopRightEntryEnable) EnimyEntries.Add(TopRightEntry);

                if (LevelMap.SideRightTopEntryEnable) EnimyEntries.Add(SideRightTopEntry);
                if (LevelMap.SideRightMidEntryEnable) EnimyEntries.Add(SideRightMidEntry);
                if (LevelMap.SideLeftTopEntryEnable) EnimyEntries.Add(SideLeftTopEntry);
                if (LevelMap.SideLeftMidEntryEnable) EnimyEntries.Add(SideLeftMidEntry);

                NextEnimyRelease = new TimeSpan(0, 0,
                                                ArkanoidDX.Random.Next(LevelMap.MinEnimyRealeaseTime,
                                                                     LevelMap.MaxEnimyRealeaseTime));
            }
            NextBrickFlash = new TimeSpan(0, 0, 1);
            DemoMode = demo;
            EndDemo = new TimeSpan(0, 0, 2, 30);
            Vaus = vaus ?? ((demo) ? new DemoVaus(game, this) : new Vaus(game, this));
            Vaus.PlayArena = this;
            Vaus.Reset();

        }

        public override void Update(GameTime gameTime)
        {
             if (DemoMode)
                EndDemo -= gameTime.ElapsedGameTime;
             if (Game.KeyboardInput.TypedKey(Keys.Escape) || EndDemo < TimeSpan.Zero)
            {
                if (_editLevel == null)
                    Game.Arena = new MenuArena(Game);
                else
                {
                    Game.Arena = new EditArena(Game, _editLevelNum, _editPath);
                }
                    
                return;
            }
            if (DemoMode && Game.TouchInput.TouchLocations.Count > 0)
                EndDemo = TimeSpan.Zero;
            Enimies.AddRange(_enimieQue);
            _enimieQue.Clear();
            UpdateEnimies(gameTime);
            UpdateEntires(gameTime);
            UpdateWarps(gameTime);
            UpdateMap(gameTime);
            
             if (!Balls.Any(b => b.IsAlive) && Vaus.Life != 0 && !IsOver)
                Vaus.Die();
            else if (Vaus.Life == 0)
            {
                if(Fade.FadeIn)
                Fade.DoFadeOut(()=>{
                    Game.Arena = new MenuArena(Game);
                });

            }
            
            base.Update(gameTime);

            if (!IsOver) return;

            if (Vaus.ExitLeft())
            {
                Vaus.ActionExitLeft = true;
                Game.Audio.Play(Sounds.VausLong);
            }
            if (Vaus.ExitRight())
            {
                Vaus.ActionExitRight = true;
                Game.Audio.Play(Sounds.VausLong);

            }
            if (!Utilities.IsCollision(Vaus.Bounds, Bounds, true))
            {
                if (Fade.Finished)
                {
                    if(LeftWarp.IsOpen || LeftWarp.IsOpening)
                        LeftWarp.Close(() => { });
                    if (RightWarp.IsOpen || RightWarp.IsOpening)
                        RightWarp.Close(() => { });
                    if(Vaus.ActionExitRight)
                    {
                        Fade.DoFadeOut(() =>
                        {
                            if (DemoMode)
                            {
                                Game.Arena = new MenuArena(Game);
                            }
                            else
                            {
                                Game.Arena = LevelSelector.WarpRight(Vaus);
                                Game.Arena.Initialise();
                                Game.Arena.Fade.DoFadeIn(() => { });
                            }
                        });
                    }else
                    {
                        Fade.DoFadeOut(() =>
                        {
                            if (DemoMode)
                            {
                                Game.Arena = new MenuArena(Game);
                            }
                            else
                            {
                                Game.Arena = LevelSelector.WarpLeft(Vaus);
                                Game.Arena.Initialise();
                                Game.Arena.Fade.DoFadeIn(() => { });
                            }
                        });
                        
                    }                    
                }

                
                return;
            }
            if (!(LeftWarp.IsOpen || LeftWarp.IsOpening))
                LeftWarp.Open();
            if (!(RightWarp.IsOpen || RightWarp.IsOpening))
                RightWarp.Open();
            
        }


        public void UpdateMap(GameTime gameTime)
        {
            Game.Game.BlocksWide = LevelMap.BricksWide;
            NextBrickFlash -= gameTime.ElapsedGameTime;
            if (NextBrickFlash <= TimeSpan.Zero)
            {
                foreach (var brick in LevelMap.BrickMap)
                    brick.Flash();
                NextBrickFlash = new TimeSpan(0, 0, ArkanoidDX.Random.Next(15));
            }
            foreach (var brick in LevelMap.BrickMap)
                brick.Update(gameTime);
        }

        public void UpdateEnimies(GameTime gameTime)
        {
            NextEnimyRelease -= gameTime.ElapsedGameTime;
            if (NextEnimyRelease <= TimeSpan.Zero)
            {
                if (Enimies.Count <= LevelMap.MaxEnimies && EnimyEntries.Count != 0)
                    EnimyEntries[ArkanoidDX.Random.Next(EnimyEntries.Count)].Spawn(LevelSelector.Map.EnemyType,this,Enimies);
                NextEnimyRelease = new TimeSpan(0, 0,
                                                ArkanoidDX.Random.Next(LevelMap.MinEnimyRealeaseTime,
                                                                     LevelMap.MaxEnimyRealeaseTime));
            }
            foreach (var e in Enimies)
                e.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            DrawBackground(batch, LevelMap.BackGround);
            DrawFrameLeft(batch);
            DrawMap(batch);
            DrawFrameRight(batch);
            DrawEntries(batch);
            DrawWarps(batch);
            DrawEmimies(batch);
            DrawBalls(batch);
            DrawLasers(batch);
            DrawVaus(batch);
            DrawCapsules(batch);
            DrawShip(batch);
            DrawTitle(batch);
            DrawScore(batch);
            if (DemoMode)
                batch.Draw(Textures.CmnDemo, Center - new Vector2(Textures.CmnDemo.Width / 2f, Textures.CmnDemo.Height/2f), Color.White * FadeRotator);
            if (LevelMap.BricksWide>Level.ClassicBricksWide)
                batch.Draw(Textures.CmnTournament,new Vector2(Bounds.Right - Textures.CmnTournament.Width,Bounds.Bottom-Textures.CmnTournament.Height),Color.White*FadeRotator);

           // batch.DrawString(Fonts.SmallFont, (Vaus.Bounds.Right/(float) Bounds.Width) + "", new Vector2(20, 20),
           //     Color.Green);
           // batch.DrawString(Fonts.SmallFont, (Vaus.Bounds.Left / (float)Bounds.Width) + "", new Vector2(20, 40),
           //     Color.Green);
           // batch.DrawString(Fonts.SmallFont, (Mouse.GetState().X / (float)Game.Width) + "", new Vector2(20, 60),
           //     Color.Green);
        }

        public void DrawMap(SpriteBatch batch)
        {
            foreach (var brick in LevelMap.BrickMap)
                brick.Draw(batch);
            if (!(Vaus.ActionExitLeft || Vaus.ActionExitRight) && Vaus.Life != 0)
            {
                var strSize = Fonts.TextFont.MeasureString(LevelSelector.Name);
                batch.DrawString(Fonts.ArtFontGrey, LevelSelector.Name, new Vector2(Center.X-(strSize.X/2),Center.Y), Color.White*(1 - Fade.Fade));
            }
        }
        public void DrawScore(SpriteBatch batch)
        {
            var s = (Game.Width - Game.FrameArea.Width) / Sprites.CmnArkanoidDxLogoA.Width;
            batch.DrawString(Fonts.GameFont, "Score", new Vector2(Game.FrameArea.Width, Sprites.CmnArkanoidDxLogoA.Height * s), Color.Blue);
            batch.DrawString(Fonts.GameFont, Vaus.Score + "", new Vector2(Game.FrameArea.Width, (Sprites.CmnArkanoidDxLogoA.Height * s) + Fonts.GameFont.MeasureString("aZj").Y), Color.Blue);
            batch.DrawString(Fonts.GameFont, LevelSelector.Name,new Vector2(Game.FrameArea.Width,
                                   (Sprites.CmnArkanoidDxLogoA.Height*s) + (Fonts.GameFont.MeasureString("aZj").Y*2)),Color.Blue);
            batch.DrawString(Fonts.GameFont, "Highscore", new Vector2(Game.FrameArea.Width,
                                   (Sprites.CmnArkanoidDxLogoA.Height * s) + (Fonts.GameFont.MeasureString("aZj").Y * 3)), Color.Blue);
             batch.DrawString(Fonts.GameFont, Game.Settings.Unlocks[LevelSelector.Wad.Name].HighScore + "", new Vector2(Game.FrameArea.Width,
                                   (Sprites.CmnArkanoidDxLogoA.Height * s) + (Fonts.GameFont.MeasureString("aZj").Y * 4)), Color.Blue);
        }

        
        public void AddNewEnimies(Enemy[] enemies)
        {
            _enimieQue.AddRange(enemies);
        }
    }
}
