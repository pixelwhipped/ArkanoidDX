using System;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Levels;
using ArkanoidDXUniverse.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDXUniverse.Arena
{
    public class LoadArena : BaseArena
    {
        public Vector2 ExitOffset;
        public Vector2 LeftButton;
        public bool Left;
        public Vector2 LeftLevelOffset;
        public int LevelNumber;
        public int MaxLevel;
        public Vector2 RightButton;
        public Vector2 RightLevelOffset;        
        public Vector2 TitleOffset;
        public LevelWad Wad;
        public BackGroundTypes BackGround;
        private bool TwoPlayer;

        public LoadArena(Arkanoid game, LevelWad wad, BackGroundTypes backGround, bool twoPlayer)
            : base(game)
        {
            TwoPlayer = twoPlayer;
            BackGround = backGround;
            Wad = wad;        
            LevelNumber = game.Settings.Unlocks.ContainsKey(wad.Name) ? game.Settings.Unlocks[wad.Name].LevelScores.Count : 0;
            MaxLevel = LevelNumber;
            LeftLevelOffset = new Vector2(Bounds.Center.X - (Textures.CmnSmallFrameCustom.Width + 20),
                Bounds.Center.Y - Textures.CmnSmallFrameCustom.Height/2f);

            RightLevelOffset = new Vector2(Bounds.Center.X, LeftLevelOffset.Y);
            RightButton = new Vector2(RightLevelOffset.X + (Textures.CmnSmallFrame.Width + 20),
                LeftLevelOffset.Y - Textures.CmnRight.Height + Textures.CmnSmallFrame.Height/2f);
            LeftButton = new Vector2(LeftLevelOffset.X - (Textures.CmnLeft.Width + 20),
                LeftLevelOffset.Y - Textures.CmnLeft.Height + Textures.CmnSmallFrame.Height/2f);
            ExitOffset = new Vector2(Bounds.Center.X - Textures.CmnBack.Width/2f,
                LeftLevelOffset.Y + Textures.CmnSmallFrame.Height + 20);

                TitleOffset = new Vector2(Bounds.Center.X - wad.Title.Width/2f,
                    LeftLevelOffset.Y - (wad.Title.Height + 20));
            
            Left = wad.Levels[LevelNumber].Key != null;
        }

        private void UpdateKeyboard()
        {            
            if (Game.KeyboardInput.TypedKey(Keys.Escape))
            {
                Game.Arena = new MenuArenaSelector(Game, TwoPlayer, BackGround);
                Game.Sounds.Menu.Play();
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Up) || Game.KeyboardInput.TypedKey(Keys.Down) || Game.KeyboardInput.TypedKey(Keys.Tab))
            {
                if (Left)
                {
                    if (Wad.Levels[LevelNumber].Value != null)
                    {
                        Left = false;
                    }
                }
                else
                {
                    if (Wad.Levels[LevelNumber].Key != null)
                    {
                        Left = true;
                    }
                }
                Game.Sounds.Menu.Play();
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Left))
            {
                LevelNumber = Math.Max(0, LevelNumber - 1);
                if (Left && Wad.Levels[LevelNumber].Key == null)
                {
                    Left = false;
                }
                else if (Wad.Levels[LevelNumber].Value == null)
                {
                    Left = true;
                }
                Game.Sounds.Menu.Play();
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Right))
            {
                LevelNumber = Math.Min(MaxLevel, LevelNumber + 1);
                if (Left && Wad.Levels[LevelNumber].Key == null)
                {
                    Left = false;
                }
                else if (Wad.Levels[LevelNumber].Value == null)
                {
                    Left = true;
                }
                Game.Sounds.Menu.Play();
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Enter))
            {
                Game.Arena = new PlayArena(Game,TwoPlayer, new LevelWadSelector(Game, TwoPlayer, Wad, LevelNumber, Left),null, 0);
                Game.Sounds.Menu.Play();
            }
        }

        private void UpdateUnifiedInput(Vector2 tap)
        {
            if (new Rectangle((int)ExitOffset.X, (int)ExitOffset.Y, Textures.CmnExit.Width,
                        Textures.CmnExit.Height).Contains(tap))
            {
                Game.Arena = new MenuArenaSelector(Game, TwoPlayer,BackGround);
                Game.Sounds.Menu.Play();
            }

            else if (new Rectangle((int)LeftLevelOffset.X, (int)LeftLevelOffset.Y, Textures.CmnSmallFrame.Width,
                Textures.CmnSmallFrame.Height).Contains(tap))
            {
                if (Wad.Levels[LevelNumber].Key != null)
                {
                    Game.Arena = new PlayArena(Game,TwoPlayer, new LevelWadSelector(Game, TwoPlayer, Wad, LevelNumber, true), null, 0);
                    Game.Sounds.Menu.Play();
                }
            }

            else if (new Rectangle((int)RightLevelOffset.X, (int)RightLevelOffset.Y, Textures.CmnSmallFrame.Width,
                Textures.CmnSmallFrame.Height).Contains(tap))
            {
                if (Wad.Levels[LevelNumber].Value != null)
                {
                    Game.Arena = new PlayArena(Game, TwoPlayer, new LevelWadSelector(Game, TwoPlayer, Wad, LevelNumber, false), null, 0);
                    Game.Sounds.Menu.Play();
                }
            }

            else if (new Rectangle((int)LeftButton.X, (int)LeftButton.Y, Textures.CmnLeft.Width,
                Textures.CmnLeft.Height).Contains(tap))
            {
                LevelNumber = Math.Max(0, LevelNumber - 1);
                if (Left && Wad.Levels[LevelNumber].Key == null)
                {
                    Left = false;
                }
                else if (Wad.Levels[LevelNumber].Value == null)
                {
                    Left = true;
                }
                Game.Sounds.Menu.Play();
            }
            else if (new Rectangle((int)RightButton.X, (int)RightButton.Y, Textures.CmnRight.Width,
                Textures.CmnRight.Height).Contains(tap))
            {
                LevelNumber = Math.Min(MaxLevel, LevelNumber + 1);
                if (Left && Wad.Levels[LevelNumber].Key == null)
                {
                    Left = false;
                }
                else if (Wad.Levels[LevelNumber].Value == null)
                {
                    Left = true;
                }
                Game.Sounds.Menu.Play();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.KeyboardInput.Any())
                UpdateKeyboard();
            var t = Game.UnifiedInput.VolatileTap;
            if (t != Vector2.Zero) UpdateUnifiedInput(t);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            DrawBackground(batch, BackGround);
            Arkanoid.Starfield.Draw(batch);
            batch.Draw(Wad.Title, TitleOffset, Color.White);
            
            if (Wad.Levels[LevelNumber].Key == null)
            {
                if (Wad.Box == null)
                {
                    batch.Draw(Textures.CmnSmallFrameCustom, LeftLevelOffset, Color.White);
                }
                else
                {
                    batch.Draw(Wad.Box,
                        new Rectangle((int) LeftLevelOffset.X, (int) LeftLevelOffset.Y, Textures.CmnSmallFrame.Width,
                            Textures.CmnSmallFrame.Height), Color.White);
                }
                batch.Draw(Textures.CmnSmallFrameEmpty, LeftLevelOffset, Color.White);
            }
            else
            {
                DrawLevel(batch, Wad.Levels[LevelNumber].Key, (int) LeftLevelOffset.X, (int) LeftLevelOffset.Y, Left);
            }
            if (Wad.Levels[LevelNumber].Value == null)
            {
                if (Wad.Box == null)
                {
                    batch.Draw(Textures.CmnSmallFrameCustom, RightLevelOffset, Color.White);
                }
                else
                {
                    batch.Draw(Wad.Box,
                        new Rectangle((int) RightLevelOffset.X, (int) RightLevelOffset.Y, Textures.CmnSmallFrame.Width,
                            Textures.CmnSmallFrame.Height), Color.White);
                }
                batch.Draw(Textures.CmnSmallFrameEmpty, RightLevelOffset, Color.White);
            }
            else
            {
                DrawLevel(batch, Wad.Levels[LevelNumber].Value, (int) RightLevelOffset.X, (int) RightLevelOffset.Y, !Left);
            }

            if (LevelNumber != 0)
            {
                batch.Draw(Textures.CmnLeft, LeftButton, Color.White);
            }
            if (LevelNumber != MaxLevel)
            {
                batch.Draw(Textures.CmnRight, RightButton, Color.White);
            }
            batch.Draw(Textures.CmnBack, ExitOffset, Color.White);

            DrawFrameLeft(batch);
            DrawFrameRight(batch);
            DrawEntries(batch);
            DrawWarps(batch);
            DrawShip(batch);
            DrawTitle(batch);

            base.Draw(batch);
        }


        public void DrawLevel(SpriteBatch batch, Level level, int xoff, int yoff, bool s)
        {
            BackGroundTypes bg;
            if (!Enum.TryParse(Enum.GetNames(typeof (BackGroundTypes))[level.Background], out bg))
                bg = BackGroundTypes.BlueCircuit;

            batch.Draw(Types.GetBackGround(bg).Map,
                new Rectangle(xoff, yoff, Textures.CmnSmallFrame.Width, Textures.CmnSmallFrame.Height), Color.White);
            batch.Draw(s ? Textures.CmnSmallFrame : Textures.CmnSmallFrameEmpty, new Vector2(xoff, yoff), Color.White);

            var by = level.BricksHigh;
            var bx = level.BricksWide;
            var yb = yoff;
            for (var y = 0; y < by; y++)
            {
                var xb = xoff;
                for (var x = 0; x < bx; x++)
                {
                    BrickTypes bt;
                    if (!Enum.TryParse(Enum.GetNames(typeof (BrickTypes))[level.GetBrickValue(y, x)], out bt))
                        bt = BrickTypes.Empty;
                    if (bt != BrickTypes.Empty)
                        batch.Draw(GetBrick(bt),
                            new Rectangle(xb + 10, yb + 10,
                                (int) (Textures.BrkWhite.Width*(185f/level.BricksWide/Textures.BrkWhite.Width)),
                                (int) (Textures.BrkWhite.Height*(185f/level.BricksWide/Textures.BrkWhite.Width))),
                            Color.White);
                    xb += (int) (Textures.BrkWhite.Width*(185f/level.BricksWide/Textures.BrkWhite.Width));
                }
                yb += (int) (Textures.BrkWhite.Height*(185f/level.BricksWide/Textures.BrkWhite.Width));
            }
        }

        public static Texture2D GetBrick(BrickTypes type)
        {
            switch (type)
            {
                case BrickTypes.White:
                {
                    return Textures.BrkWhite;
                }
                case BrickTypes.Yellow:
                {
                    return Textures.BrkYellow;
                }
                case BrickTypes.Pink:
                {
                    return Textures.BrkPink;
                }
                case BrickTypes.Blue:
                {
                    return Textures.BrkBlue;
                }
                case BrickTypes.Red:
                {
                    return Textures.BrkRed;
                }
                case BrickTypes.Green:
                {
                    return Textures.BrkGreen;
                }
                case BrickTypes.SkyBlue:
                {
                    return Textures.BrkSkyBlue;
                }
                case BrickTypes.Orange:
                {
                    return Textures.BrkOrange;
                }
                case BrickTypes.Silver:
                {
                    return Textures.BrkSilver;
                }
                case BrickTypes.Gold:
                {
                    return Textures.BrkGold;
                }
                case BrickTypes.DarkRed:
                    {
                        return Textures.BrkDarkRed;
                    }
                case BrickTypes.DarkBlue:
                    {
                        return Textures.BrkDarkBlue;
                    }
                case BrickTypes.Regen:
                {
                    return Textures.BrkRegen;
                }
                case BrickTypes.Teleport:
                {
                    return Textures.BrkTeleport;
                }
                case BrickTypes.SilverSwap:
                {
                    return Textures.BrkSilverSwap;
                }
                case BrickTypes.GoldSwap:
                {
                    return Textures.BrkGoldSwap;
                }
                case BrickTypes.BlueSwap:
                {
                    return Textures.BrkBlueSwap;
                }
                case BrickTypes.Black:
                {
                    return Textures.BrkBlack;
                }
                case BrickTypes.BlackRegen:
                {
                    return Textures.BrkBlackRegen;
                }
                case BrickTypes.Transmit:
                {
                    return Textures.BrkTransmit;
                }
                case BrickTypes.HorizonalMove:
                {
                    return Textures.BrkGold;
                }
                default:
                {
                    return Textures.BrkWhite;
                }
            }
        }
    }
}