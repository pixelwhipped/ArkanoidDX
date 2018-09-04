using System;
using ArkanoidDX.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ArkanoidDX.Graphics;

namespace ArkanoidDX.Arena
{
    public class LoadArena : BaseArena
    {
        public Starfield Starfield;
        public TimeSpan Show;
        public LevelWad Wad;
        public TimeSpan LastTouch;
        public int level;
        public int maxLevel;
        public Vector2 TitleOffset;
        public Vector2 leftLevelOffset;
        public Vector2 rightLevelOffset;
        public Vector2 exitOffset;
        public Boolean left;
        private Vector2 rbtn;
        private Vector2 lbtn;

        public LoadArena(ArkanoidDX game, LevelWad wad)
            : base(game)
        {
            Wad = wad;            
            LastTouch = new TimeSpan(0, 0, 0, 0, 200);
            Starfield = new Starfield(100, new Rectangle(0, 0, Game.Width, Game.Height));
            if (game.Settings.Unlocks.ContainsKey(wad.Name))
            {
                level = game.Settings.Unlocks[wad.Name].LevelScores.Count;
            }
            else
            {
                level = 0;
            }
            maxLevel = level;
            leftLevelOffset = new Vector2(Game.Center.X - (Textures.CmnSmallFrameCustom.Width + 20),
                Game.Center.Y - (Textures.CmnSmallFrameCustom.Height / 2f));

            rightLevelOffset = new Vector2(Game.Center.X, leftLevelOffset.Y);
            rbtn = new Vector2(rightLevelOffset.X + (Textures.CmnSmallFrame.Width + 20),
                leftLevelOffset.Y - (Textures.CmnRight.Height) + (Textures.CmnSmallFrame.Height/2f));
            lbtn = new Vector2(leftLevelOffset.X - (Textures.CmnLeft.Width + 20),
                leftLevelOffset.Y - (Textures.CmnLeft.Height) + (Textures.CmnSmallFrame.Height/2f));
            exitOffset = new Vector2(Game.Center.X - (Textures.CmnExit.Width/2f),
                leftLevelOffset.Y + Textures.CmnSmallFrame.Height + 20);
            if (wad.Title == null)
            {
                //editing
            }
            else
            {
                TitleOffset = new Vector2(Game.Center.X - (wad.Title.Width/2f), leftLevelOffset.Y - (wad.Title.Height + 20));
            }
            if (wad.Levels[level].Key == null)
            {
                left = false;
            }else
            {
                left = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            LastTouch -= gameTime.ElapsedGameTime;
            Starfield.Update(gameTime);
            Show -= gameTime.ElapsedGameTime;
            if (Game.KeyboardInput.TypedKey(Keys.Escape))
            {
                Game.Arena = new MenuArena(Game);
            }
            if (Game.KeyboardInput.TypedKey(Keys.Up) || Game.KeyboardInput.TypedKey(Keys.Down))
            {
                if (left)
                {
                    if (!(Wad.Levels[level].Value == null))
                    {
                        left = false;
                    }
                }
                else
                {
                    if (!(Wad.Levels[level].Key == null))
                    {
                        left = true;
                    } 
                }
            }
            if (Game.KeyboardInput.TypedKey(Keys.Left))
            {
                level = Math.Max(0, level - 1);
                if (left && Wad.Levels[level].Key == null)
                {
                    left = false;
                }
                else if (Wad.Levels[level].Value == null)
                {
                    left = true;
                }
            }
            if (Game.KeyboardInput.TypedKey(Keys.Right))
            {
                level = Math.Min(maxLevel, level + 1);
                if (left && Wad.Levels[level].Key == null)
                {
                    left = false;
                }
                else if (Wad.Levels[level].Value == null)
                {
                    left = true;
                }
            }
            if (Game.KeyboardInput.TypedKey(Keys.Enter))
            {
                Game.Arena = new PlayArena(Game, new LevelWadSelector(Game, Wad, level,left), null, 0);
            }

            if (LastTouch < TimeSpan.Zero)
            foreach (var l in Game.TouchInput.TouchLocations)
            {
                LastTouch = new TimeSpan(0, 0, 0, 0, 200);
                var p = new Point((int) l.X, (int) l.Y);
                if (new Rectangle((int) exitOffset.X, (int) exitOffset.Y, Textures.CmnExit.Width,
                    Textures.CmnExit.Height).Contains(p))
                {
                    Game.TouchInput.TapLocations.Clear();
                    Game.Arena = new MenuArena(Game);    
                }

                if (new Rectangle((int)leftLevelOffset.X, (int)leftLevelOffset.Y, Textures.CmnSmallFrame.Width,
                    Textures.CmnSmallFrame.Height).Contains(p))
                {
                    if (!(Wad.Levels[level].Key == null))
                    {
                        Game.Arena = new PlayArena(Game, new LevelWadSelector(Game, Wad, level, true), null, 0);
                    }
                }

                if (new Rectangle((int)rightLevelOffset.X, (int)rightLevelOffset.Y, Textures.CmnSmallFrame.Width,
                    Textures.CmnSmallFrame.Height).Contains(p))
                {
                    if (!(Wad.Levels[level].Value == null))
                    {
                        Game.Arena = new PlayArena(Game, new LevelWadSelector(Game, Wad, level, false), null, 0);
                    }
                }

                if (new Rectangle((int)lbtn.X, (int)lbtn.Y, Textures.CmnLeft.Width,
                    Textures.CmnLeft.Height).Contains(p))
                {
                    level = Math.Max(0, level - 1);
                    if (left && Wad.Levels[level].Key == null)
                    {
                        left = false;
                    }
                    else if (Wad.Levels[level].Value == null)
                    {
                        left = true;
                    }
                }
                if (new Rectangle((int)rbtn.X, (int)rbtn.Y, Textures.CmnRight.Width,
                    Textures.CmnRight.Height).Contains(p))
                {
                    level = Math.Min(maxLevel, level + 1);
                    if (left && Wad.Levels[level].Key == null)
                    {
                        left = false;
                    }
                    else if (Wad.Levels[level].Value == null)
                    {
                        left = true;
                    }
                }
            }
            //Game.Arena = new PlayArena(Game, new LevelWadSelector(Game, Levels.Levels.Wads[Select], 0, true), null, 0);
            //Game.Arena.Fade.DoFadeIn(() => { });
            base.Update(gameTime);

        }

        public override void Draw(SpriteBatch batch)
        {
            Starfield.Draw(batch);
            if (Wad.Title == null)
            {

            }
            else
            {
                batch.Draw(Wad.Title, TitleOffset, Color.White);
            }
            if(Wad.Levels[level].Key == null)
            {
                if (Wad.Box == null)
                {
                    batch.Draw(Textures.CmnSmallFrameCustom, leftLevelOffset, Color.White);
                }
                else
                {
                    batch.Draw(Wad.Box,
                        new Rectangle((int) leftLevelOffset.X, (int) leftLevelOffset.Y, Textures.CmnSmallFrame.Width,
                            Textures.CmnSmallFrame.Height),Color.White);
                }
                batch.Draw(Textures.CmnSmallFrameEmpty,leftLevelOffset,Color.White);               
            }else
            {
                DrawLevel(batch, Wad.Levels[level].Key, (int)leftLevelOffset.X, (int)leftLevelOffset.Y,left);
            }
            if(Wad.Levels[level].Value == null)
            {
                if (Wad.Box == null)
                {
                    batch.Draw(Textures.CmnSmallFrameCustom, rightLevelOffset, Color.White);
                }
                else
                {
                    batch.Draw(Wad.Box,
                        new Rectangle((int)rightLevelOffset.X, (int)rightLevelOffset.Y, Textures.CmnSmallFrame.Width,
                            Textures.CmnSmallFrame.Height), Color.White);
                }
                batch.Draw(Textures.CmnSmallFrameEmpty, rightLevelOffset, Color.White);        
            }else
            {                
                DrawLevel(batch, Wad.Levels[level].Value, (int)rightLevelOffset.X, (int)rightLevelOffset.Y,!left);
            }

            if (level != 0)
            {
                batch.Draw(Textures.CmnLeft, lbtn, Color.White);
            }
            if (level != maxLevel)
            {
                batch.Draw(Textures.CmnRight, rbtn, Color.White);            
            }
            batch.Draw(Textures.CmnExit, exitOffset, Color.White);

            
            base.Draw(batch);
        }

        
        public void DrawLevel(SpriteBatch batch, Level level, int xoff, int yoff, bool s)
        {

            BackGroundTypes bg;
            if (!Enum.TryParse(Enum.GetNames(typeof(BackGroundTypes))[level.Background], out bg))
                bg = BackGroundTypes.BlueCircuit;

            batch.Draw(Types.GetBackGround(bg).Map, new Rectangle(xoff, yoff, Textures.CmnSmallFrame.Width, Textures.CmnSmallFrame.Height), Color.White);
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
                    if (!Enum.TryParse(Enum.GetNames(typeof(BrickTypes))[level.GetBrickValue(y, x)], out bt))
                        bt = BrickTypes.Empty;
                    if (bt != BrickTypes.Empty)
                        batch.Draw(GetBrick(bt),
                                   new Rectangle((xb + 10), (yb + 10),
                                                 (int)(Textures.BrkWhite.Width * ((185f / level.BricksWide) / Textures.BrkWhite.Width)),
                                                 (int)(Textures.BrkWhite.Height * ((185f / level.BricksWide) / Textures.BrkWhite.Width))), Color.White);
                    xb += (int)((Textures.BrkWhite.Width * ((185f / level.BricksWide) / Textures.BrkWhite.Width)));
                }
                yb += (int)((Textures.BrkWhite.Height * ((185f / level.BricksWide) / Textures.BrkWhite.Width)));
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
