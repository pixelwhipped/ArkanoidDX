using System;
using System.Collections.Generic;
using System.Linq;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDXUniverse.Objects
{
    public enum VausState
    {
        Normal,
        Explode,
        Reassemble,
        WarpIn,
        WarpOut
    }

    public class Vaus : GameObject
    {
        private bool _isCheat;
        public bool ActionExitLeft;
        public bool ActionExitRight;
        public TimeSpan ElectricFenceTimer = TimeSpan.Zero;
        public bool Exit;
        public float ExtPVal;
        public float ExtVal;

        public Vector2 IllusionLocationA;
        public Vector2 IllusionLocationB;
        public Vector2 IllusionMotionA;
        public Vector2 IllusionMotionB;

        public float Inertia;
        public bool IsCatch;
        public bool IsExtension;
        public bool IsExtraExtension;
        public bool IsIllusion;
        public bool IsLaser;
        public bool IsMegaLaser;
        public bool IsReduce;
        public int Lasers;

        public MouseState LastMouseState;
        public TimeSpan LastTouch;

        public int CurrentPlayer;
        public int Life1;
        public int LifeScore1;
        public int Life2;
        public int LifeScore2;

        public TimeSpan MegaLaserFireTimer = TimeSpan.Zero;
        public TimeSpan MegaLaserTimer = TimeSpan.Zero;
        public bool MouseOn;
        public int MouseX;
        public float PaddleSpeed = 8f;
        public PlayArena PlayArena;
        public bool Restting;
        public int Score;
        public float SideVal = 1;
        public bool Started;
        public bool TapFired;

        public Vaus Twin;
        public Vaus TwinParent;
        public VausState VausState;

        public Vaus(Arkanoid game, PlayArena playArena, bool reset = true, bool twoPlayer =false) : base(game)
        {
            PlayArena = playArena;
            CurrentPlayer = 1;
            Life1 = 3;
            Life2 = twoPlayer ? 3 : 0;

            VausState = VausState.Normal;
            if (!reset) return;
            ResetXY();
            PlayArena.Balls.Clear();
            PlayArena.Balls.Add(new Ball(Game, PlayArena, false, false,false));
            foreach (var b in PlayArena.Balls)
                b.SetStartVausLocation(new Vector2(Center.X - b.Width/2, Y - b.Height));
            Game.TouchInput.TapListeners.Add(t =>
            {
                if (LastTouch >= TimeSpan.Zero) return;
                {
                    LastTouch = new TimeSpan(0, 0, 0, 0, 200);
                    TapFired = true;
                }
            });
            LastTouch = new TimeSpan(0, 0, 0, 0, 200);
            LastMouseState = Mouse.GetState();
            MouseX = LastMouseState.X;
        }

        public override bool IsAlive => Life1 > 0 || Life2 > 0;

        public bool IsElectricFence => ElectricFenceTimer > TimeSpan.Zero;

        public bool IsTwin
        {
            get { return Twin != null; }
            set
            {
                if (value)
                {
                    Twin = new Vaus(Game, PlayArena, false)
                    {
                        TwinParent = this,
                        Life1 = 1,
                        CurrentPlayer = (CurrentPlayer==1)?1:2,
                        IsCatch = IsCatch,
                        IsLaser = IsLaser,
                        Lasers = Lasers,
                        IsMegaLaser = IsMegaLaser,
                        IsReduce = IsReduce,
                        IsExtension = IsExtension,
                        IsExtraExtension = IsExtraExtension,
                        IsIllusion = IsIllusion,
                        X = PlayArena.Width - (X + Width),
                        Y = Y - Height*2
                    };
                }
                else
                {
                    Twin = null;
                }
            }
        }

        public override Sprite Texture => VCenter;

        public Sprite VCenter => IsLaser ? Sprites.VaCenterLaser : Sprites.VaCenterNormal;

        public Sprite VSide => IsLaser ? Sprites.VaSideLaser : Sprites.VaSideNormal;

        public Sprite VExtension => IsLaser ? Sprites.VaExtensionLaser : Sprites.VaExtensionNormal;

        public Sprite VExtensionPlus => IsLaser ? Sprites.VaExtensionPlusLaser : Sprites.VaExtensionPlusNormal;

        public Sprite VEnd
        {
            get
            {
                if(IsLaser)return Sprites.VaEndLaser;
                if(CurrentPlayer==1)return TwinParent == null ? Sprites.VaEndNormal : Sprites.VaEndNormalP2;
                return TwinParent == null ? Sprites.VaEndNormalP2 : Sprites.VaEndNormal;
            }
        }

        public Sprite VExplosion
        {
            get
            {
                if (CurrentPlayer == 1)
                {
                    if (Life2 > 0)
                    {
                        return (TwinParent == null) ? Sprites.VaExplodeP2 : Sprites.VaExplode;
                    }
                    return (TwinParent == null) ? Sprites.VaExplode : Sprites.VaExplodeP2;
                }
                if (Life1 > 0)
                {
                    return (TwinParent == null) ? Sprites.VaExplode : Sprites.VaExplodeP2;
                }
                return (TwinParent == null) ? Sprites.VaExplodeP2 : Sprites.VaExplode;
            }
        } 
        public void AddScore(int score)
        {
            Score += score;
            if (CurrentPlayer == 1)
            {
                LifeScore1 += score;
                if (LifeScore1 < 5000) return;
                Life1 = MathHelper.Clamp(Life1 + 1, 0, 12);
                LifeScore1 -= 5000;
            }
            else
            {
                LifeScore2 += score;
                if (LifeScore2 < 5000) return;
                Life2 = MathHelper.Clamp(Life2 + 1, 0, 12);
                LifeScore2 -= 5000;
            }
        }

        public bool ExitRight()
        {
            return PlayArena.LevelMap.IsOver && X + Width >= PlayArena.Bounds.Width + PlayArena.Bounds.X - 3 &&
                   (PlayArena.RightWarp.IsOpen || PlayArena.RightWarp.IsOpening) && !ActionExitRight;
        }

        public bool ExitLeft()
        {
            return PlayArena.LevelMap.IsOver && (X <= PlayArena.Bounds.X + 3) &&
                   (PlayArena.LeftWarp.IsOpen || PlayArena.LeftWarp.IsOpening) && !ActionExitLeft;
        }

        public virtual void Update(GameTime gameTime)
        {
            LastTouch -= gameTime.ElapsedGameTime;
            CheckCapsuleVausCollision();
            CheckCapsuleVausTwinCollision();
            ElectricFenceTimer -= gameTime.ElapsedGameTime;
            if (IsMegaLaser)
                MegaLaserTimer -= gameTime.ElapsedGameTime;
            if (MegaLaserTimer <= TimeSpan.Zero)
            {
                IsMegaLaser = false;
                if (IsTwin)
                    Twin.IsMegaLaser = false;
            }
            if (IsMegaLaser)
            {
                MegaLaserFireTimer -= gameTime.ElapsedGameTime;
                if (MegaLaserFireTimer <= TimeSpan.Zero && !(ActionExitLeft || ActionExitRight))
                {
                    MegaLaserFireTimer = new TimeSpan(0, 0, 0, 0, 100);
                    AddMegaLaser();
                }
            }
            if (!Started)
                foreach (var b in PlayArena.Balls)
                    b.SetStartVausLocation(new Vector2(Center.X - b.Width/2, Y - b.Height));
            SideVal = IsReduce ? MathHelper.Clamp(SideVal - 0.1f, 0, 1) : MathHelper.Clamp(SideVal + 0.1f, 0, 1);
            ExtVal = IsExtension ? MathHelper.Clamp(ExtVal + 0.1f, 0, 1) : MathHelper.Clamp(ExtVal - 0.1f, 0, 1);
            ExtPVal = IsExtraExtension
                ? MathHelper.Clamp(ExtPVal + 0.1f, 0, 1)
                : MathHelper.Clamp(ExtPVal - 0.1f, 0, 1);

            if ((Game.KeyboardInput.Pressed(Keys.LeftControl) && Game.KeyboardInput.TypedKey(Keys.Tab)) || (Game.KeyboardInput.TypedKey(Keys.LeftControl) && Game.KeyboardInput.Pressed(Keys.Tab)))
            {
                _isCheat = !_isCheat;
            }
            else if (_isCheat)
            {
                if (Game.KeyboardInput.TypedKey(Keys.G))
                    AddPowerUp(CapsuleTypes.MagnaBall);
                if (Game.KeyboardInput.TypedKey(Keys.R))
                    AddPowerUp(CapsuleTypes.Reduce);
                if (Game.KeyboardInput.TypedKey(Keys.E))
                    AddPowerUp(CapsuleTypes.Expand);
                if (Game.KeyboardInput.TypedKey(Keys.C))
                    AddPowerUp(CapsuleTypes.Catch);
                if (Game.KeyboardInput.TypedKey(Keys.L))
                    AddPowerUp(CapsuleTypes.Laser);
                if (Game.KeyboardInput.TypedKey(Keys.D))
                    AddPowerUp(CapsuleTypes.Disrupt);
                if (Game.KeyboardInput.TypedKey(Keys.M))
                    AddPowerUp(CapsuleTypes.MegaBall);
                if (Game.KeyboardInput.TypedKey(Keys.F))
                    AddPowerUp(CapsuleTypes.Fast);
                if (Game.KeyboardInput.TypedKey(Keys.S))
                    AddPowerUp(CapsuleTypes.Slow);
                if (Game.KeyboardInput.TypedKey(Keys.I))
                    AddPowerUp(CapsuleTypes.Illusion);
                if (Game.KeyboardInput.TypedKey(Keys.O))
                    AddPowerUp(CapsuleTypes.Orbit);
                if (Game.KeyboardInput.TypedKey(Keys.T))
                    AddPowerUp(CapsuleTypes.Twin);
                if (Game.KeyboardInput.TypedKey(Keys.R))
                    AddPowerUp(CapsuleTypes.Random);
                if (Game.KeyboardInput.TypedKey(Keys.P))
                    AddPowerUp(CapsuleTypes.Life);


                if (Game.KeyboardInput.TypedKey(Keys.W))
                    AddPowerUp(CapsuleTypes.Exit);
                if (Game.KeyboardInput.TypedKey(Keys.X))
                    AddPowerUp(CapsuleTypes.MegaLaser);
                if (Game.KeyboardInput.TypedKey(Keys.A))
                    AddPowerUp(CapsuleTypes.ElectricFence);
            }

            Motion = Vector2.Zero;

            if (ActionExitLeft || ActionExitRight)
            {
                if (ActionExitLeft)
                {
                    Location -= new Vector2(4, 0);
                }
                if (ActionExitRight)
                {
                    Location += new Vector2(4, 0);
                }
                return;
            }

            foreach (var l in Game.TouchInput.Touches)
            {
                MouseOn = false;
                if (l.X + 10 < Bounds.Center.X)
                {
                    Motion = new Vector2(-.5f, Motion.Y);
                    if (Inertia > 0) Inertia = 0;
                    Inertia -= 0.035f;
                }
                if (l.X - 10 > Bounds.Center.X)
                {
                    Motion = new Vector2(.5f, Motion.Y);
                    if (Inertia < 0) Inertia = 0;
                    Inertia += 0.035f;
                }
            }

            if (Game.KeyboardInput.Pressed(Keys.Left))
            {
                Motion = new Vector2(-.5f, Motion.Y);
                if (Inertia > 0) Inertia = 0;
                Inertia -= 0.035f;
                MouseOn = false;
            }

            if (Game.KeyboardInput.Pressed(Keys.Right))
            {
                Motion = new Vector2(.5f, Motion.Y);
                if (Inertia < 0) Inertia = 0;
                Inertia += 0.035f;
                MouseOn = false;
            }

            var m = Mouse.GetState();

            if (m.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed)
            {
                TapFired = true;
            }
            if (m.RightButton == ButtonState.Released && LastMouseState.RightButton == ButtonState.Pressed)
            {
                TapFired = true;
            }
            if (m.MiddleButton == ButtonState.Released && LastMouseState.MiddleButton == ButtonState.Pressed)
            {
                TapFired = true;
            }

            #region Mouse Ver 2
            /*
            if (m.X != LastMouseState.X)
            {
                MouseX = m.X;
                MouseOn = true;
            }
            if (MouseOn)
            {
                var p = m.X / Game.Width;

                if (m.X < Game.Center.X)
                {
                    // if (Math.Abs(Bounds.Left - MathHelper.Clamp(m.X, PlayArena.Bounds.Left, PlayArena.Bounds.Left + PlayArena.Bounds.Width)) > 1)
                    {
                        var p2 = Bounds.Left / (float)PlayArena.Bounds.Width;
                        if (p2 > p)
                        {
                            Motion = new Vector2(-.5f, Motion.Y);
                            if (Inertia > 0) Inertia = 0;
                            Inertia -= 0.035f;
                        }
                    }
                }
                else
                {
                    //  if (Math.Abs(Bounds.Left - MathHelper.Clamp(m.X, PlayArena.Bounds.Left, PlayArena.Bounds.Left + PlayArena.Bounds.Width)) > 1)
                    {
                        var p2 = Bounds.Left / (float)PlayArena.Bounds.Width;
                        if (p2 < p)
                        {
                            Motion = new Vector2(.5f, Motion.Y);
                            if (Inertia < 0) Inertia = 0;
                            Inertia += 0.035f;
                        }
                    }
                }
            }
             */
            #endregion

            #region Mouse Ver 

            if (m.X != LastMouseState.X)
            {
                MouseX = m.X;
                MouseOn = true;
            }
            if (MouseOn)
            {
                var mod = Math.Min(Math.Abs(Center.X/(Center.X-m.X)), 1f);
                if (m.X < Center.X-50)
                {
                    Motion = new Vector2(-.5f*mod, Motion.Y);
                    if (Inertia > 0) Inertia = 0;
                    Inertia -= 0.035f * mod;
                }
                else if (m.X > Center.X + 50)
                {
                    Motion = new Vector2(.5f * mod, Motion.Y);
                    if (Inertia < 0) Inertia = 0;
                    Inertia += 0.035f * mod;
                }                             
            }

            #endregion

            LastMouseState = m;

            if (Game.KeyboardInput.TypedKey(Keys.Space) || TapFired)
            {
                TapFired = false;
                Started = true;
                if (IsLaser)
                {
                    AddLaser();
                }
                foreach (var b in PlayArena.Balls)
                    b.IsCaught = false;
            }
            Motion = new Vector2(Inertia + Motion.X*PaddleSpeed, Motion.Y);
            Location += Motion; // + new Vector2(inertia,0f);
            if (Inertia > 0)
            {
                Inertia -= 0.01f;
            }
            else
            {
                Inertia += 0.01f;
            }
            // Location = new Vector2(Mouse.GetState().X, Location.Y);

            if (IsIllusion)
            {
                if (IllusionLocationA.X > Location.X)
                {
                    if (IllusionLocationA.X > Bounds.Right)
                    {
                        IllusionLocationA = new Vector2(Bounds.Right, Y);
                    }
                    else
                    {
                        IllusionLocationA += new Vector2(-.8f, 0);
                    }
                }
                else
                {
                    if (IllusionLocationA.X + Width < Bounds.Left)
                    {
                        IllusionLocationA = new Vector2(Bounds.Left - Width, Y);
                    }
                    else
                    {
                        IllusionLocationA += new Vector2(.8f, 0);
                    }
                }


                if (IllusionLocationB.X > Location.X)
                {
                    if (IllusionLocationB.X > Center.X)
                    {
                        IllusionLocationB = new Vector2(Center.X, Y);
                    }
                    else
                    {
                        IllusionLocationB += new Vector2(-.6f, 0);
                    }
                }
                else
                {
                    if (IllusionLocationB.X + Width < Center.X)
                    {
                        IllusionLocationB = new Vector2(Center.X - Width, Y);
                    }
                    else
                    {
                        IllusionLocationB += new Vector2(.6f, 0);
                    }
                }
            }
            else
            {
                IllusionLocationA = Location;
                IllusionLocationB = Location;
            }


            if (X < PlayArena.Bounds.X)
                X = PlayArena.Bounds.X;
            if (X + Width > PlayArena.Bounds.Width + PlayArena.Bounds.X)
                X = PlayArena.Bounds.Width + PlayArena.Bounds.X - Width;

            if (ActionExitLeft || ActionExitRight)
            {
                IllusionLocationA = Location;
                IllusionLocationB = Location;
            }

            if (IsTwin)
            {
                Twin.X = PlayArena.X + PlayArena.Width - (X - PlayArena.X + Twin.Width);
                Twin.Y = Y - Height*2;
                Twin.Motion = new Vector2(Motion.X*-1, Motion.Y);
                if (Twin.X < PlayArena.Bounds.X)
                    Twin.X = PlayArena.Bounds.X;
                if (Twin.X + Twin.Width > PlayArena.Bounds.Right)
                    Twin.X = PlayArena.Bounds.Right - Width;

                Twin.SideVal = Twin.IsReduce
                    ? MathHelper.Clamp(Twin.SideVal - 0.1f, 0, 1)
                    : MathHelper.Clamp(Twin.SideVal + 0.1f, 0, 1);
                Twin.ExtVal = Twin.IsExtension
                    ? MathHelper.Clamp(Twin.ExtVal + 0.1f, 0, 1)
                    : MathHelper.Clamp(Twin.ExtVal - 0.1f, 0, 1);
                Twin.ExtPVal = Twin.IsExtraExtension
                    ? MathHelper.Clamp(Twin.ExtPVal + 0.1f, 0, 1)
                    : MathHelper.Clamp(Twin.ExtPVal - 0.1f, 0, 1);

                if (Twin.IsIllusion)
                {
                    if (Twin.IllusionLocationA.X > Twin.Location.X)
                    {
                        if (Twin.IllusionLocationA.X > Twin.Bounds.Right)
                        {
                            Twin.IllusionLocationA = new Vector2(Twin.Bounds.Right, Y);
                        }
                        else
                        {
                            Twin.IllusionLocationA += new Vector2(-.8f, 0);
                        }
                    }
                    else
                    {
                        if (Twin.IllusionLocationA.X + Twin.Width < Twin.Bounds.Left)
                        {
                            Twin.IllusionLocationA = new Vector2(Twin.Bounds.Left - Twin.Width, Twin.Y);
                        }
                        else
                        {
                            Twin.IllusionLocationA += new Vector2(.8f, 0);
                        }
                    }


                    if (Twin.IllusionLocationB.X > Twin.Location.X)
                    {
                        if (Twin.IllusionLocationB.X > Twin.Center.X)
                        {
                            Twin.IllusionLocationB = new Vector2(Twin.Center.X, Twin.Y);
                        }
                        else
                        {
                            Twin.IllusionLocationB += new Vector2(-.6f, 0);
                        }
                    }
                    else
                    {
                        if (Twin.IllusionLocationB.X + Twin.Width < Twin.Center.X)
                        {
                            Twin.IllusionLocationB = new Vector2(Twin.Center.X - Twin.Width, Twin.Y);
                        }
                        else
                        {
                            Twin.IllusionLocationB += new Vector2(.6f, 0);
                        }
                    }
                }
                else
                {
                    Twin.IllusionLocationA = Twin.Location;
                    Twin.IllusionLocationB = Twin.Location;
                }
            }
        }

        public void AddMegaLaser()
        {
            Game.Sounds.VausLaser.Play();
            PlayArena.Lasers.Add(new MegaLaser(Game, PlayArena,
                new Vector2(Center.X - Sprites.VaLaser.Width/2,
                    Y - Sprites.VaLaser.Height), new Vector2(Arkanoid.Random.Next(10) - 5, -6)));
            if (IsTwin)
                PlayArena.Lasers.Add(new MegaLaser(Game, PlayArena,
                    new Vector2(Twin.Center.X - Sprites.VaLaser.Width/2,
                        Twin.Y - Sprites.VaLaser.Height), new Vector2(Arkanoid.Random.Next(10) - 5, -6)));
        }

        public void AddLaser()
        {
            Game.Sounds.VausLaser.Play();
            if (Lasers == 1)
            {
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Center.X - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
            }
            else if (Lasers == 2)
            {
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Center.X - VCenter.Width/2 - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Center.X + VCenter.Width/2 - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
            }
            else if (Lasers == 3)
            {
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Center.X - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Center.X - VCenter.Width/2 - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Center.X + VCenter.Width/2 - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
            }
            else if (Lasers >= 3)
            {
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Center.X - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Center.X - VCenter.Width/2 - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Center.X + VCenter.Width/2 - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Bounds.Left + VEnd.Width - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                    new Vector2(Bounds.Right - VEnd.Width - Sprites.VaLaser.Width/2,
                        Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
            }
            if (IsTwin)
                Twin.AddLaser();
        }

        public void RemovePower()
        {
            if (PlayArena.Balls.Any(p => p.IsMegaBall))
            {
                foreach (var b in PlayArena.Balls)
                {
                    b.IsMegaBall = false;
                }
            }
            else if (PlayArena.Balls.Any(p => p.IsMagnaBall))
            {
                foreach (var b in PlayArena.Balls)
                {
                    b.IsMagnaBall = false;
                }
            }
            else if (IsLaser)
            {
                IsLaser = false;
                Lasers = 0;
                if (IsTwin)
                {
                    Twin.IsLaser = false;
                    Twin.Lasers = 0;
                }
            }
            else if (IsCatch)
            {
                IsCatch = false;
                Started = true;
                if (IsTwin)
                {
                    Twin.IsCatch = false;
                    Twin.Started = true;
                }
            }
            else if (IsExtraExtension)
            {
                IsExtraExtension = false;
                if (IsTwin)
                {
                    Twin.IsExtraExtension = false;
                }
            }
            else if (IsExtension)
            {
                IsExtension = false;
                if (IsTwin)
                {
                    Twin.IsExtension = false;
                }
            }
        }

        public void AddPowerUp(CapsuleTypes type)
        {
            if (TwinParent != null)
            {
                TwinParent.AddPowerUp(type);
                return;
            }
            Game.Sounds.VausCollect.Play();
            switch (type)
            {
                case CapsuleTypes.Reduce:
                    Game.Sounds.VausLong.Play();
                    IsExtension = false;
                    IsExtraExtension = false;
                    IsReduce = true;
                    IsCatch = false;
                    Started = true;
                    if (IsTwin)
                    {
                        Twin.IsExtension = false;
                        Twin.IsExtraExtension = false;
                        Twin.IsReduce = true;
                        Twin.IsCatch = false;
                        Twin.Started = true;
                    }
                    break;
                case CapsuleTypes.Exit:
                    PlayArena.LevelMap.Leavable = true;
                    break;
                case CapsuleTypes.Expand:
                    if (IsExtension && IsExtraExtension)
                    {
                        IsExtraExtension = false;
                        Started = true;
                        if (IsTwin)
                        {
                            Twin.IsExtraExtension = false;
                            Twin.Started = true;
                        }
                    }
                    else if (IsExtension)
                    {
                        IsExtraExtension = true;
                        Game.Sounds.VausLong.Play();
                        if (IsTwin)
                        {
                            Twin.IsExtraExtension = true;
                        }
                    }
                    else
                    {
                        IsExtension = true;
                        Game.Sounds.VausLong.Play();
                        if (IsTwin)
                        {
                            Twin.IsExtension = true;
                        }
                    }
                    break;
                case CapsuleTypes.Fast:
                    foreach (var b in PlayArena.Balls)
                    {
                        b.BallSpeed = MathHelper.Clamp(b.BallSpeed*1.5f, b.MinBallSpeed, b.MaxBallSpeed);
                    }
                    break;
                case CapsuleTypes.MegaLaser:
                    if (IsMegaLaser)
                    {
                        IsMegaLaser = false;
                        if (IsTwin)
                        {
                            Twin.IsMegaLaser = false;
                        }
                    }
                    else
                    {
                        IsMegaLaser = true;
                        MegaLaserTimer = new TimeSpan(0, 0, 0, 3);
                        if (IsTwin)
                        {
                            Twin.IsMegaLaser = true;
                            Twin.MegaLaserTimer = new TimeSpan(0, 0, 0, 3);
                        }
                    }
                    break;
                case CapsuleTypes.Slow:
                    foreach (var b in PlayArena.Balls)
                    {
                        b.BallSpeed = MathHelper.Clamp(b.BallSpeed*.5f, b.MinBallSpeed, b.MaxBallSpeed);
                    }
                    break;
                case CapsuleTypes.Laser:
                    IsLaser = true;
                    Lasers++;
                    if (IsTwin)
                    {
                        Twin.IsLaser = true;
                        Twin.Lasers++;
                    }
                    break;
                case CapsuleTypes.Life:
                    if (CurrentPlayer == 1)
                    {
                        Life1 = MathHelper.Clamp(Life1 + 1, 0, 12);
                    }
                    else
                    {
                        Life2 = MathHelper.Clamp(Life2 + 1, 0, 12);
                    }
                    break;
                case CapsuleTypes.MegaBall:
                    foreach (var b in PlayArena.Balls)
                    {
                        b.IsMegaBall = !b.IsMegaBall;
                    }
                    break;
                case CapsuleTypes.MagnaBall:
                    foreach (var b in PlayArena.Balls)
                    {
                        b.IsMagnaBall = !b.IsMagnaBall;
                    }
                    break;
                case CapsuleTypes.Disrupt:
                {
                    if (PlayArena.Balls.Count > Arkanoid.MaxBalls) break;
                    var addballs = new List<Ball>();
                    foreach (var b in PlayArena.Balls)
                    {
                        var nb = new Ball(Game, PlayArena, b.IsMegaBall, b.IsCaught, b.IsMagnaBall);
                        nb.SetStartBallLocation(b.Location, b);
                        addballs.Add(nb);
                        nb = new Ball(Game, PlayArena, b.IsMegaBall, b.IsCaught,b.IsMagnaBall);
                        nb.SetStartBallLocation(b.Location, b);
                        addballs.Add(nb);
                    }
                    PlayArena.Balls.AddRange(addballs);
                }
                    break;
                case CapsuleTypes.Catch:
                    if (IsCatch)
                    {
                        IsCatch = false;
                        Started = true;
                        if (IsTwin)
                        {
                            Twin.IsCatch = false;
                            Twin.Started = true;
                        }
                    }
                    else
                    {
                        IsCatch = true;
                        if (IsTwin)
                        {
                            Twin.IsCatch = true;
                        }
                    }
                    break;
                case CapsuleTypes.Illusion:
                    if (IsIllusion)
                    {
                        IsIllusion = false;
                        if (IsTwin)
                        {
                            Twin.IsIllusion = false;
                        }
                    }
                    else
                    {
                        IsIllusion = true;
                        if (IsTwin)
                        {
                            Twin.IsIllusion = true;
                        }
                    }
                    break;
                case CapsuleTypes.Orbit:
                    foreach (var b in PlayArena.Balls)
                    {
                        b.IsOrbit = !b.IsOrbit;
                    }
                    break;
                case CapsuleTypes.Twin:
                    IsTwin = !IsTwin;
                    break;
                case CapsuleTypes.ElectricFence:
                    ElectricFenceTimer = new TimeSpan(0, 0, 0, 20);
                    break;
                case CapsuleTypes.Random:
                    AddPowerUp(RandomUtils.RandomEnum<CapsuleTypes>());
                    break;
            }
        }


        public void ResetXY()
        {
            X = PlayArena.Bounds.X + PlayArena.Bounds.Width/2 - Width/2;
            Y = Game.FrameArea.Height - Sprites.FrmSideWarp.Height*2 +
                (Sprites.FrmSideWarp.Height/2 - Height/2);
            Inertia = 0;
        }

        public void Reset()
        {
            TapFired = false;
            Started = false;
            IsReduce = false;
            IsLaser = false;
            Lasers = 0;
            IsCatch = false;
            IsIllusion = false;
            IsTwin = false;
            IsMegaLaser = false;
            IsExtraExtension = false;
            IsExtension = false;
            ElectricFenceTimer = TimeSpan.Zero;
            ExtVal = 0;
            ExtPVal = 0;
            SideVal = 1;
            ActionExitLeft = false;
            ActionExitRight = false;
            ResetXY();
            Game.Sounds.GameStart.Play();
            PlayArena.Balls.Clear();
            PlayArena.Balls.Add(new Ball(Game, PlayArena, false, false,false));

            Restting = false;
        }

        public void Die()
        {
            if (Restting) return;
            Restting = true;
            if (TwinParent != null)
            {
                VausState = VausState.Explode;
                VExplosion.ToStart();
                VExplosion.SetAnimation(AnimationState.Play);
                VExplosion.OnFinish = () =>
                {                    
                    VExplosion.SetAnimation(AnimationState.Stop);
                    TwinParent.Twin = null;
                    Life1--;
                    Life2--;
                };
                return;
            }                        
            if (IsTwin)
            {
                Twin.Die();
           //     Twin.VausState = VausState.Explode;
           //     VExplosion.ToStart();
           //     VExplosion.SetAnimation(AnimationState.Play);
           //     VExplosion.OnFinish = () => { Twin.Life--; };
            }
            if (CurrentPlayer == 1)
            {
                Life1--;
                if (Life2 > 0) CurrentPlayer = 2;                
                
            }
            else
            {
                Life2--;
                if (Life1 > 0) CurrentPlayer = 1;                
            }
            if (!IsAlive)            
                PlayArena.Capsules.Clear();            
            Game.Sounds.VausDeath.Play();
            VausState = VausState.Explode;
            VExplosion.ToStart();
            VExplosion.SetAnimation(AnimationState.Play);
            VExplosion.OnFinish = () =>
            {
                
                if(!IsAlive) {
                    Game.Sounds.GameOver.Play();
                }else
                {                    
                    Game.Sounds.VausEnter.Play();
                        ResetXY();
                        VausState = VausState.Reassemble;
                        VExplosion.SetAnimation(AnimationState.Stop);
                        Sprites.VaRevive.ToStart();
                        Sprites.VaRevive.SetAnimation(AnimationState.Play);
                        Sprites.VaRevive.OnFinish = () =>
                        {
                            Sprites.VaRevive.SetAnimation(
                                AnimationState.Stop);
                            if (IsAlive)
                            {
                                VausState = VausState.Normal;
                                Reset();
                            }
                        };
                    }
                };            
        }

        public override void Draw(SpriteBatch batch)
        {
            DrawVaus(batch);
            if (IsTwin)
            {
                Twin.DrawVaus(batch);
            }

            if (IsElectricFence)
            {
                batch.Draw(Sprites.CmnElectricFence,
                    new Rectangle(PlayArena.Bounds.X, (int) PlayArena.Height - 35, PlayArena.Bounds.Width,
                        Textures.CmnElectricFence.Height), Color.White*PlayArena.Fade.Fade);
            }

            for (var i = 0; i < Life1-1; i++)
            {
                batch.Draw(Sprites.CmnLife,
                    new Vector2(PlayArena.Bounds.X + Sprites.CmnLife.Width*i,
                        PlayArena.Bounds.Height - Sprites.CmnLife.Height), Color.White*PlayArena.Fade.Fade * ((CurrentPlayer == 1) ? 1f : 0.33f));
            }

            for (var i = 0; i < Life2-1; i++)
            {
                batch.Draw(Sprites.CmnLife2,
                    new Vector2(PlayArena.Bounds.X + Sprites.CmnLife.Width * i,
                        PlayArena.Bounds.Height - (Sprites.CmnLife.Height*2.5f)), Color.White * PlayArena.Fade.Fade * ((CurrentPlayer==2)?1f:0.33f));
            }
        }


        public void DrawVaus(SpriteBatch batch)
        {
            switch (VausState)
            {
                case VausState.Normal:
                {
                    Rectangle center;
                    Rectangle lSide;
                    Rectangle rSide;
                    Rectangle lExt;
                    Rectangle rExt;
                    Rectangle lExtP;
                    Rectangle rExtP;
                    Rectangle lEnd;
                    Rectangle rEnd;
                    if (IsIllusion)
                    {
                        #region IllusionA

                        center = new Rectangle((int) (IllusionABounds.Center.X - VCenter.Width/2f), (int) Y,
                            (int) VCenter.Width,
                            (int) VCenter.Height);
                        lSide = new Rectangle(center.Left - (int) (VSide.Width*SideVal), (int) Y,
                            (int) (VSide.Width*SideVal), (int) VCenter.Height);
                        rSide = new Rectangle(center.Right, (int) Y, (int) (VSide.Width*SideVal),
                            (int) VCenter.Height);
                        lExt = new Rectangle(lSide.Left - (int) (VExtension.Width*ExtVal), (int) Y,
                            (int) (VExtension.Width*ExtVal), (int) VCenter.Height);
                        rExt = new Rectangle(rSide.Right, (int) Y, (int) (VExtension.Width*ExtVal),
                            (int) VCenter.Height);
                        lExtP = new Rectangle(lExt.Left - (int) (VExtensionPlus.Width*ExtPVal), (int) Y,
                            (int) (VExtensionPlus.Width*ExtPVal), (int) VCenter.Height);
                        rExtP = new Rectangle(rExt.Right, (int) Y, (int) (VExtensionPlus.Width*ExtPVal),
                            (int) VCenter.Height);
                        lEnd = new Rectangle((int) (lExtP.Left - VEnd.Width + 1), (int) Y, (int) VEnd.Width,
                            (int) VCenter.Height);
                        rEnd = new Rectangle(rExtP.Right - 1, (int) Y, (int) VEnd.Width, (int) VCenter.Height);
                        if (!(ActionExitLeft || ActionExitRight))
                        {
                            batch.Draw(VExtensionPlus,
                                new Rectangle(lExtP.X + (int) Game.Shadow.X, lExtP.Y + (int) Game.Shadow.Y,
                                    lExtP.Width,
                                    lExtP.Height), Game.ShadowColor*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VExtensionPlus,
                                new Rectangle(rExtP.X + (int) Game.Shadow.X, rExtP.Y + (int) Game.Shadow.Y,
                                    rExtP.Width,
                                    rExtP.Height), Game.ShadowColor*.25f*PlayArena.Fade.Fade, 1,
                                true);
                            batch.Draw(VExtension,
                                new Rectangle(lExt.X + (int) Game.Shadow.X, lExt.Y + (int) Game.Shadow.Y,
                                    lExt.Width,
                                    lExt.Height), Game.ShadowColor*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VExtension,
                                new Rectangle(rExt.X + (int) Game.Shadow.X, rExt.Y + (int) Game.Shadow.Y,
                                    rExt.Width,
                                    rExt.Height), Game.ShadowColor*.25f*PlayArena.Fade.Fade, 1,
                                true);
                            batch.Draw(VSide,
                                new Rectangle(lSide.X + (int) Game.Shadow.X, lSide.Y + (int) Game.Shadow.Y,
                                    lSide.Width,
                                    lSide.Height), Game.ShadowColor*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VSide,
                                new Rectangle(rSide.X + (int) Game.Shadow.X, rSide.Y + (int) Game.Shadow.Y,
                                    rSide.Width,
                                    rSide.Height), Game.ShadowColor*.25f*PlayArena.Fade.Fade, 1,
                                true);
                            batch.Draw(VCenter,
                                new Rectangle(center.X + (int) Game.Shadow.X, center.Y + (int) Game.Shadow.Y,
                                    center.Width,
                                    center.Height), Game.ShadowColor*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VEnd,
                                new Rectangle(lEnd.X + (int) Game.Shadow.X, lEnd.Y + (int) Game.Shadow.Y,
                                    lEnd.Width,
                                    lEnd.Height), Game.ShadowColor*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VEnd,
                                new Rectangle(rEnd.X + (int) Game.Shadow.X, rEnd.Y + (int) Game.Shadow.Y,
                                    rEnd.Width,
                                    rEnd.Height), Game.ShadowColor*.25f*PlayArena.Fade.Fade, 1,
                                true);

                            batch.Draw(VExtensionPlus, lExtP, Color.White*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VExtensionPlus, rExtP, Color.White*.25f*PlayArena.Fade.Fade, 1, true);
                            batch.Draw(VExtension, lExt, Color.White*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VExtension, rExt, Color.White*.25f*PlayArena.Fade.Fade, 1, true);
                            batch.Draw(VSide, lSide, Color.White*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VSide, rSide, Color.White*.25f*PlayArena.Fade.Fade, 1, true);
                            batch.Draw(VCenter, center, Color.White*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VEnd, lEnd, Color.White*.25f*PlayArena.Fade.Fade);
                            batch.Draw(VEnd, rEnd, Color.White*.25f*PlayArena.Fade.Fade, 1, true);
                            if (IsCatch)
                            {
                                batch.Draw(Sprites.VaExtensionPlusCatch, lExtP, Color.White*.25f*PlayArena.Fade.Fade);
                                batch.Draw(Sprites.VaExtensionPlusCatch, rExtP, Color.White*.25f*PlayArena.Fade.Fade,
                                    1, true);
                                batch.Draw(Sprites.VaExtensionCatch, lExt, Color.White*.25f*PlayArena.Fade.Fade);
                                batch.Draw(Sprites.VaExtensionCatch, rExt, Color.White*.25f*PlayArena.Fade.Fade, 1,
                                    true);
                                batch.Draw(Sprites.VaSideCatch, lSide, Color.White*.25f*PlayArena.Fade.Fade);
                                batch.Draw(Sprites.VaSideCatch, rSide, Color.White*.25f*PlayArena.Fade.Fade, 1, true);
                                batch.Draw(Sprites.VaCenterCatch, center, Color.White*.25f*PlayArena.Fade.Fade);
                            }
                            if (IsMegaLaser)
                            {
                                batch.Draw(Sprites.VaCenterMegaLaser, center, Color.White*.25f*PlayArena.Fade.Fade);
                            }

                            #endregion

                            #region IllusionB

                            center = new Rectangle((int) (IllusionBBounds.Center.X - VCenter.Width/2f), (int) Y,
                                (int) VCenter.Width,
                                (int) VCenter.Height);
                            lSide = new Rectangle(center.Left - (int) (VSide.Width*SideVal), (int) Y,
                                (int) (VSide.Width*SideVal), (int) VCenter.Height);
                            rSide = new Rectangle(center.Right, (int) Y, (int) (VSide.Width*SideVal),
                                (int) VCenter.Height);
                            lExt = new Rectangle(lSide.Left - (int) (VExtension.Width*ExtVal), (int) Y,
                                (int) (VExtension.Width*ExtVal), (int) VCenter.Height);
                            rExt = new Rectangle(rSide.Right, (int) Y, (int) (VExtension.Width*ExtVal),
                                (int) VCenter.Height);
                            lExtP = new Rectangle(lExt.Left - (int) (VExtensionPlus.Width*ExtPVal), (int) Y,
                                (int) (VExtensionPlus.Width*ExtPVal), (int) VCenter.Height);
                            rExtP = new Rectangle(rExt.Right, (int) Y, (int) (VExtensionPlus.Width*ExtPVal),
                                (int) VCenter.Height);
                            lEnd = new Rectangle((int) (lExtP.Left - VEnd.Width + 1), (int) Y, (int) VEnd.Width,
                                (int) VCenter.Height);
                            rEnd = new Rectangle(rExtP.Right - 1, (int) Y, (int) VEnd.Width, (int) VCenter.Height);

                            batch.Draw(VExtensionPlus,
                                new Rectangle(lExtP.X + (int) Game.Shadow.X, lExtP.Y + (int) Game.Shadow.Y,
                                    lExtP.Width,
                                    lExtP.Height), Game.ShadowColor*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VExtensionPlus,
                                new Rectangle(rExtP.X + (int) Game.Shadow.X, rExtP.Y + (int) Game.Shadow.Y,
                                    rExtP.Width,
                                    rExtP.Height), Game.ShadowColor*.5f*PlayArena.Fade.Fade, 1,
                                true);
                            batch.Draw(VExtension,
                                new Rectangle(lExt.X + (int) Game.Shadow.X, lExt.Y + (int) Game.Shadow.Y,
                                    lExt.Width,
                                    lExt.Height), Game.ShadowColor*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VExtension,
                                new Rectangle(rExt.X + (int) Game.Shadow.X, rExt.Y + (int) Game.Shadow.Y,
                                    rExt.Width,
                                    rExt.Height), Game.ShadowColor*.5f*PlayArena.Fade.Fade, 1, true);
                            batch.Draw(VSide,
                                new Rectangle(lSide.X + (int) Game.Shadow.X, lSide.Y + (int) Game.Shadow.Y,
                                    lSide.Width,
                                    lSide.Height), Game.ShadowColor*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VSide,
                                new Rectangle(rSide.X + (int) Game.Shadow.X, rSide.Y + (int) Game.Shadow.Y,
                                    rSide.Width,
                                    rSide.Height), Game.ShadowColor*.5f*PlayArena.Fade.Fade, 1,
                                true);
                            batch.Draw(VCenter,
                                new Rectangle(center.X + (int) Game.Shadow.X, center.Y + (int) Game.Shadow.Y,
                                    center.Width,
                                    center.Height), Game.ShadowColor*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VEnd,
                                new Rectangle(lEnd.X + (int) Game.Shadow.X, lEnd.Y + (int) Game.Shadow.Y,
                                    lEnd.Width,
                                    lEnd.Height), Game.ShadowColor*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VEnd,
                                new Rectangle(rEnd.X + (int) Game.Shadow.X, rEnd.Y + (int) Game.Shadow.Y,
                                    rEnd.Width,
                                    rEnd.Height), Game.ShadowColor*.5f*PlayArena.Fade.Fade, 1, true);

                            batch.Draw(VExtensionPlus, lExtP, Color.White*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VExtensionPlus, rExtP, Color.White*.5f*PlayArena.Fade.Fade, 1, true);
                            batch.Draw(VExtension, lExt, Color.White*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VExtension, rExt, Color.White*.5f*PlayArena.Fade.Fade, 1, true);
                            batch.Draw(VSide, lSide, Color.White*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VSide, rSide, Color.White*.5f*PlayArena.Fade.Fade, 1, true);
                            batch.Draw(VCenter, center, Color.White*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VEnd, lEnd, Color.White*.5f*PlayArena.Fade.Fade);
                            batch.Draw(VEnd, rEnd, Color.White*.5f*PlayArena.Fade.Fade, 1, true);
                            if (IsCatch)
                            {
                                batch.Draw(Sprites.VaExtensionPlusCatch, lExtP, Color.White*.5f*PlayArena.Fade.Fade);
                                batch.Draw(Sprites.VaExtensionPlusCatch, rExtP, Color.White*.5f*PlayArena.Fade.Fade,
                                    1, true);
                                batch.Draw(Sprites.VaExtensionCatch, lExt, Color.White*.5f*PlayArena.Fade.Fade);
                                batch.Draw(Sprites.VaExtensionCatch, rExt, Color.White*.5f*PlayArena.Fade.Fade, 1,
                                    true);
                                batch.Draw(Sprites.VaSideCatch, lSide, Color.White*.5f*PlayArena.Fade.Fade);
                                batch.Draw(Sprites.VaSideCatch, rSide, Color.White*.5f*PlayArena.Fade.Fade, 1, true);
                                batch.Draw(Sprites.VaCenterCatch, center, Color.White*.5f*PlayArena.Fade.Fade);
                            }
                            if (IsMegaLaser)
                            {
                                batch.Draw(Sprites.VaCenterMegaLaser, center, Color.White*.5f*PlayArena.Fade.Fade);
                            }
                        }

                        #endregion
                    }

                    #region Standard

                    center = new Rectangle((int) (Bounds.Center.X - VCenter.Width/2f), (int) Y, (int) VCenter.Width,
                        (int) VCenter.Height);
                    lSide = new Rectangle(center.Left - (int) (VSide.Width*SideVal), (int) Y,
                        (int) (VSide.Width*SideVal), (int) VCenter.Height);
                    rSide = new Rectangle(center.Right, (int) Y, (int) (VSide.Width*SideVal), (int) VCenter.Height);
                    lExt = new Rectangle(lSide.Left - (int) (VExtension.Width*ExtVal), (int) Y,
                        (int) (VExtension.Width*ExtVal), (int) VCenter.Height);
                    rExt = new Rectangle(rSide.Right, (int) Y, (int) (VExtension.Width*ExtVal), (int) VCenter.Height);
                    lExtP = new Rectangle(lExt.Left - (int) (VExtensionPlus.Width*ExtPVal), (int) Y,
                        (int) (VExtensionPlus.Width*ExtPVal), (int) VCenter.Height);
                    rExtP = new Rectangle(rExt.Right, (int) Y, (int) (VExtensionPlus.Width*ExtPVal),
                        (int) VCenter.Height);
                    lEnd = new Rectangle((int) (lExtP.Left - VEnd.Width + 1), (int) Y, (int) VEnd.Width,
                        (int) VCenter.Height);
                    rEnd = new Rectangle(rExtP.Right - 1, (int) Y, (int) VEnd.Width, (int) VCenter.Height);

                    batch.Draw(VExtensionPlus,
                        new Rectangle(lExtP.X + (int) Game.Shadow.X, lExtP.Y + (int) Game.Shadow.Y, lExtP.Width,
                            lExtP.Height), Game.ShadowColor*PlayArena.Fade.Fade);
                    batch.Draw(VExtensionPlus,
                        new Rectangle(rExtP.X + (int) Game.Shadow.X, rExtP.Y + (int) Game.Shadow.Y, rExtP.Width,
                            rExtP.Height), Game.ShadowColor*PlayArena.Fade.Fade, 1, true);
                    batch.Draw(VExtension,
                        new Rectangle(lExt.X + (int) Game.Shadow.X, lExt.Y + (int) Game.Shadow.Y, lExt.Width,
                            lExt.Height), Game.ShadowColor*PlayArena.Fade.Fade);
                    batch.Draw(VExtension,
                        new Rectangle(rExt.X + (int) Game.Shadow.X, rExt.Y + (int) Game.Shadow.Y, rExt.Width,
                            rExt.Height), Game.ShadowColor*PlayArena.Fade.Fade, 1, true);
                    batch.Draw(VSide,
                        new Rectangle(lSide.X + (int) Game.Shadow.X, lSide.Y + (int) Game.Shadow.Y, lSide.Width,
                            lSide.Height), Game.ShadowColor*PlayArena.Fade.Fade);
                    batch.Draw(VSide,
                        new Rectangle(rSide.X + (int) Game.Shadow.X, rSide.Y + (int) Game.Shadow.Y, rSide.Width,
                            rSide.Height), Game.ShadowColor*PlayArena.Fade.Fade, 1, true);
                    batch.Draw(VCenter,
                        new Rectangle(center.X + (int) Game.Shadow.X, center.Y + (int) Game.Shadow.Y, center.Width,
                            center.Height), Game.ShadowColor*PlayArena.Fade.Fade);
                    batch.Draw(VEnd,
                        new Rectangle(lEnd.X + (int) Game.Shadow.X, lEnd.Y + (int) Game.Shadow.Y, lEnd.Width,
                            lEnd.Height), Game.ShadowColor*PlayArena.Fade.Fade);
                    batch.Draw(VEnd,
                        new Rectangle(rEnd.X + (int) Game.Shadow.X, rEnd.Y + (int) Game.Shadow.Y, rEnd.Width,
                            rEnd.Height), Game.ShadowColor*PlayArena.Fade.Fade, 1, true);

                    batch.Draw(VExtensionPlus, lExtP, Color.White*PlayArena.Fade.Fade);
                    batch.Draw(VExtensionPlus, rExtP, Color.White*PlayArena.Fade.Fade, 1, true);
                    batch.Draw(VExtension, lExt, Color.White*PlayArena.Fade.Fade);
                    batch.Draw(VExtension, rExt, Color.White*PlayArena.Fade.Fade, 1, true);
                    batch.Draw(VSide, lSide, Color.White*PlayArena.Fade.Fade);
                    batch.Draw(VSide, rSide, Color.White*PlayArena.Fade.Fade, 1, true);
                    batch.Draw(VCenter, center, Color.White*PlayArena.Fade.Fade);
                    batch.Draw(VEnd, lEnd, Color.White*PlayArena.Fade.Fade);
                    batch.Draw(VEnd, rEnd, Color.White*PlayArena.Fade.Fade, 1, true);
                    if (IsCatch)
                    {
                        batch.Draw(Sprites.VaExtensionPlusCatch, lExtP, Color.White*PlayArena.Fade.Fade);
                        batch.Draw(Sprites.VaExtensionPlusCatch, rExtP, Color.White*PlayArena.Fade.Fade, 1, true);
                        batch.Draw(Sprites.VaExtensionCatch, lExt, Color.White*PlayArena.Fade.Fade);
                        batch.Draw(Sprites.VaExtensionCatch, rExt, Color.White*PlayArena.Fade.Fade, 1, true);
                        batch.Draw(Sprites.VaSideCatch, lSide, Color.White*PlayArena.Fade.Fade);
                        batch.Draw(Sprites.VaSideCatch, rSide, Color.White*PlayArena.Fade.Fade, 1, true);
                        batch.Draw(Sprites.VaCenterCatch, center, Color.White*PlayArena.Fade.Fade);
                    }
                    if (IsMegaLaser)
                    {
                        batch.Draw(Sprites.VaCenterMegaLaser, center, Color.White*PlayArena.Fade.Fade);
                    }
                }

                    #endregion

                    break;
                case VausState.Explode:
                    batch.Draw(VExplosion, Bounds, Color.White*PlayArena.Fade.Fade);
                    break;
                case VausState.Reassemble:
                    batch.Draw(Sprites.VaRevive, Bounds, Color.White*PlayArena.Fade.Fade);
                    break;
            }
        }

        public void CheckCapsuleVausCollision()
        {
            foreach (var cap in PlayArena.Capsules)
            {
                if (!cap.IsAlive) continue;
                Direction d;
                CollisionPoint c;
                if (!Collisions.IsCollision(cap, this, out d, out c)) continue;
                cap.Die();
            }
        }

        public void CheckCapsuleVausTwinCollision()
        {
            if (!IsTwin) return;
            foreach (var cap in PlayArena.Capsules)
            {
                if (!cap.IsAlive) continue;
                Direction d;
                CollisionPoint c;
                if (!Collisions.IsCollision(cap, Twin, out d, out c)) continue;
                cap.Die();
            }
        }

        #region ILocatable Members

        public override Rectangle Bounds => new Rectangle(
            (int) Location.X,
            (int) Location.Y,
            (int) Width,
            (int) Height);

        public Rectangle IllusionABounds => new Rectangle(
            (int) IllusionLocationA.X,
            (int) IllusionLocationA.Y,
            (int) Width,
            (int) Height);

        public Rectangle IllusionBBounds => new Rectangle(
            (int) IllusionLocationB.X,
            (int) IllusionLocationB.Y,
            (int) Width,
            (int) Height);

        public override float Width => VEnd.Width*2 + VCenter.Width + VSide.Width*SideVal*2 + VExtension.Width*ExtVal*2 +
                                       VExtensionPlus.Width*ExtPVal*2;

        public override float Height => VCenter.Height;

        #endregion
    }
}