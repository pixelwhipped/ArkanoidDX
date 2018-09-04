 using System;
using System.Collections.Generic;
using System.Linq;
 using Windows.Devices.Sensors;
 using ArkanoidDX.Arena;
using ArkanoidDX.Audio;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDX.Objects
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
        public bool ActionExitLeft;
        public bool ActionExitRight;
        public bool Exit;

        public override bool IsAlive
        {
            get { return Life > 0; }
        }

        public Vector2 IllusionLocationA;
        public Vector2 IllusionLocationB;
        public Vector2 IllusionMotionA;
        public Vector2 IllusionMotionB;

        private bool _isCheat;
        public float ExtPVal;
        public float ExtVal;
        public bool IsCatch;
        public bool IsExtension;
        public bool IsExtraExtension;
        public bool IsLaser;
        public bool IsElectricFence {get { return ElectricFenceTimer > TimeSpan.Zero; }}
        public TimeSpan ElectricFenceTimer = TimeSpan.Zero;
        public int Lasers = 0;
        public bool IsReduce;
        public bool IsIllusion;
        public bool IsMegaLaser;
        public TimeSpan MegaLaserTimer = TimeSpan.Zero;
        public TimeSpan MegaLaserFireTimer = TimeSpan.Zero;

        public Vaus Twin;
        public Vaus TwinParent;
        public TimeSpan LastTouch;
        public bool IsTwin { 
            get { return Twin != null; } 
                set{
                    if(value)
                    {
                        Twin = new Vaus(Game, PlayArena, false)
                                   {
                                       TwinParent = this,
                                       Life = 1,
                                       IsCatch = IsCatch,
                                       IsLaser = IsLaser,
                                       Lasers = Lasers,
                                       IsMegaLaser = IsMegaLaser,
                                       IsReduce = IsReduce,
                                       IsExtension = IsExtension,
                                       IsExtraExtension = IsExtraExtension,
                                       IsIllusion = IsIllusion,
                                       X = PlayArena.Width - (X + Width),
                                       Y = Y - (Height*2)
                                   };
                    }else
                    {
                        Twin = null;
                    }
            } 
        }
        
        public int Life;
        public int LifeScore;
        public float PaddleSpeed = 8f;
        public bool Restting;
        public int Score;
        public float SideVal = 1;
        public bool Started;
        public VausState VausState;
        public PlayArena PlayArena;
        public bool TapFired;

        public MouseState LastMouseState;
        public int MouseX;
        public bool MouseOn;

        public float inertia;

        public Vaus(ArkanoidDX game, PlayArena playArena, bool reset = true):base(game)
        {
            PlayArena = playArena;
            Life = 3;
            VausState = VausState.Normal;
            if (!reset) return;
            ResetXY();
            PlayArena.Balls.Clear();
            PlayArena.Balls.Add(new Ball(Game, PlayArena, false, false));
            foreach (var b in PlayArena.Balls)
                b.SetStartVausLocation(new Vector2(Center.X - (b.Width/2), Y - b.Height));
            Game.TouchInput.TapListeners.Add(t =>
            {
                if (LastTouch >= TimeSpan.Zero) return;
                {
                    LastTouch = new TimeSpan(0, 0, 0, 0,200);
                    TapFired = true;
                }
            });
            LastTouch = new TimeSpan(0, 0, 0,0,200);
            LastMouseState = Mouse.GetState();
            MouseX = LastMouseState.X;
        }

        public override Sprite Texture
        {
            get { return VCenter; }
        }

        public Sprite VCenter
        {
            get { return (IsLaser) ? Sprites.VaCenterLaser : Sprites.VaCenterNormal; }
        }
        public Sprite VSide
        {
            get { return (IsLaser) ? Sprites.VaSideLaser : Sprites.VaSideNormal; }
        }
        public Sprite VExtension
        {
            get { return (IsLaser) ? Sprites.VaExtensionLaser : Sprites.VaExtensionNormal; }
        }
        public Sprite VExtensionPlus
        {
            get { return (IsLaser) ? Sprites.VaExtensionPlusLaser : Sprites.VaExtensionPlusNormal; }
        }
        public Sprite VEnd
        {
            get { return (IsLaser) ? Sprites.VaEndLaser : Sprites.VaEndNormal; }
        }

        #region ILocatable Members

        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Location.X,
                    (int)Location.Y,
                    (int)Width,
                    (int)Height);
            }
        }

        public Rectangle IllusionABounds
        {
            get
            {
                return new Rectangle(
                    (int)IllusionLocationA.X,
                    (int)IllusionLocationA.Y,
                    (int)Width,
                    (int)Height);
            }
        }

        public Rectangle IllusionBBounds
        {
            get
            {
                return new Rectangle(
                    (int)IllusionLocationB.X,
                    (int)IllusionLocationB.Y,
                    (int)Width,
                    (int)Height);
            }
        }

        public override float Width
        {
            get
            {
                return (VEnd.Width*2) + VCenter.Width + ((VSide.Width*SideVal)*2) + ((VExtension.Width*ExtVal)*2) +
                       ((VExtensionPlus.Width*ExtPVal)*2);
            }
        }

        public override float Height
        {
            get { return VCenter.Height; }
        }

        #endregion

        public void AddScore(int score)
        {
            Score += score;
            LifeScore += score;
            if (LifeScore < 5000) return;
            Life = (int)MathHelper.Clamp(Life + 1, 0, 12);
            LifeScore -= 5000;
        }

        public bool ExitRight()
        {
            return PlayArena.LevelMap.IsOver &&
                   (X + Width >= ((PlayArena.Bounds.Width + PlayArena.Bounds.X) - 3) && (PlayArena.RightWarp.IsOpen || PlayArena.RightWarp.IsOpening) &&
                    !ActionExitRight);
        }

        public bool ExitLeft()
        {
            return PlayArena.LevelMap.IsOver &&
                   ((X <= PlayArena.Bounds.X + 3) && (PlayArena.LeftWarp.IsOpen || PlayArena.LeftWarp.IsOpening) && !ActionExitLeft);
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
                if (MegaLaserFireTimer <= TimeSpan.Zero && !(ActionExitLeft || ActionExitRight ))
                {
                    MegaLaserFireTimer = new TimeSpan(0, 0, 0, 0, 100);
                    AddMegaLaser();
                }
            }
            if(!Started)
                foreach (var b in PlayArena.Balls)
                b.SetStartVausLocation(new Vector2(Center.X - (b.Width / 2),Y - b.Height));
            SideVal = (IsReduce) ? MathHelper.Clamp(SideVal - 0.1f, 0, 1) : MathHelper.Clamp(SideVal + 0.1f, 0, 1);
            ExtVal = (IsExtension) ? MathHelper.Clamp(ExtVal + 0.1f, 0, 1) : MathHelper.Clamp(ExtVal - 0.1f, 0, 1);
            ExtPVal = (IsExtraExtension)
                          ? MathHelper.Clamp(ExtPVal + 0.1f, 0, 1)
                          : MathHelper.Clamp(ExtPVal - 0.1f, 0, 1);

            if (Game.KeyboardInput.Pressed(Keys.LeftControl) && Game.KeyboardInput.TypedKey(Keys.Tab))
            {
                _isCheat = !_isCheat;
            }
            else if(_isCheat){
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
                AddPowerUp(CapsuleTypes.Slow );
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

            if (ActionExitLeft || ActionExitRight )
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
            
            foreach (var l in Game.TouchInput.TouchLocations)
            {
                
                MouseOn = false;
                if (l.X+10 < Bounds.Center.X)
                {
                    Motion = new Vector2(-.5f, Motion.Y);
                    if (inertia > 0) inertia = 0;
                    inertia -= 0.035f;
                }
                if (l.X-10 > Bounds.Center.X)
                {
                    Motion = new Vector2(.5f, Motion.Y);
                    if (inertia < 0) inertia = 0;
                    inertia += 0.035f;
                }
            }
            
            if (Game.KeyboardInput.Pressed(Keys.Left))
            {
                Motion = new Vector2(-.5f, Motion.Y);
                if (inertia > 0) inertia = 0;
                inertia -= 0.035f;
                MouseOn = false;
            }

            if (Game.KeyboardInput.Pressed(Keys.Right))
            {
                Motion = new Vector2(.5f, Motion.Y);
                if (inertia < 0) inertia = 0;
                inertia += 0.035f;
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
            
            if (m.X != LastMouseState.X)
            {
                MouseX = m.X;
                MouseOn = true;               
            }
            if (MouseOn)
            {
                var p = (float)m.X/(float)Game.Width;

                if (m.X < Game.Center.X)
                {
                   // if (Math.Abs(Bounds.Left - MathHelper.Clamp(m.X, PlayArena.Bounds.Left, PlayArena.Bounds.Left + PlayArena.Bounds.Width)) > 1)
                    {
                       var p2 = (float)Bounds.Left/(float)PlayArena.Bounds.Width;
                       if (p2>p)
                        {
                            Motion = new Vector2(-.5f, Motion.Y);
                            if (inertia > 0) inertia = 0;
                            inertia -= 0.035f;
                        }
                    }
                }
                else
                {
                  //  if (Math.Abs(Bounds.Left - MathHelper.Clamp(m.X, PlayArena.Bounds.Left, PlayArena.Bounds.Left + PlayArena.Bounds.Width)) > 1)
                    {
                        var p2 = (float)Bounds.Left / (float)PlayArena.Bounds.Width;
                        if (p2<p)
                        {
                            Motion = new Vector2(.5f, Motion.Y);
                            if (inertia < 0) inertia = 0;
                            inertia += 0.035f;
                        }
                    }
                }
            }
            #endregion
            LastMouseState = m;

            if (Game.KeyboardInput.TypedKey(Keys.Space)||TapFired)
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
            Motion = new Vector2(inertia + Motion.X * PaddleSpeed, Motion.Y);
            Location += Motion;// + new Vector2(inertia,0f);
            if (inertia > 0)
            {
                inertia -= 0.01f;
            }
            else
            {
                inertia += 0.01f;
            }
           // Location = new Vector2(Mouse.GetState().X, Location.Y);
            
            if(IsIllusion)
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
                }else
                {
                    if (IllusionLocationA.X + Width < Bounds.Left)
                    {
                        IllusionLocationA = new Vector2(Bounds.Left-Width, Y);
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
            }else
            {
                IllusionLocationA = Location;
                IllusionLocationB = Location;
            }
            

            if (X < PlayArena.Bounds.X )
                X = PlayArena.Bounds.X;
            if (X + Width > (PlayArena.Bounds.Width + PlayArena.Bounds.X) )
                X = (PlayArena.Bounds.Width + PlayArena.Bounds.X) - Width;

            if (ActionExitLeft || ActionExitRight)
            {

                IllusionLocationA = Location;
                IllusionLocationB = Location;
            }

            if(IsTwin)
            {
                Twin.X = (PlayArena.X + PlayArena.Width) - (X - PlayArena.X + Twin.Width);
                Twin.Y = Y - (Height * 2);
                Twin.Motion = new Vector2(Motion.X*-1, Motion.Y);
                if (Twin.X < PlayArena.Bounds.X )
                    Twin.X = PlayArena.Bounds.X;
                if (Twin.X + Twin.Width > (PlayArena.Bounds.Right))
                    Twin.X = (PlayArena.Bounds.Right) - (Width);

                Twin.SideVal = (Twin.IsReduce) ? MathHelper.Clamp(Twin.SideVal - 0.1f, 0, 1) : MathHelper.Clamp(Twin.SideVal + 0.1f, 0, 1);
                Twin.ExtVal = (Twin.IsExtension) ? MathHelper.Clamp(Twin.ExtVal + 0.1f, 0, 1) : MathHelper.Clamp(Twin.ExtVal - 0.1f, 0, 1);
                Twin.ExtPVal = (Twin.IsExtraExtension)
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
            Game.Audio.Play(Sounds.VausLaser);
            PlayArena.Lasers.Add(new MegaLaser(Game, PlayArena,
                                           new Vector2(Center.X - (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(ArkanoidDX.Random.Next(10)-5, - 6)));
            if(IsTwin)
                PlayArena.Lasers.Add(new MegaLaser(Game, PlayArena,
                                           new Vector2(Twin.Center.X - (Sprites.VaLaser.Width / 2),
                                                       Twin.Y - Sprites.VaLaser.Height), new Vector2(ArkanoidDX.Random.Next(10) - 5, -6)));
        }
        
        public void AddLaser()
        {
            Game.Audio.Play(Sounds.VausLaser);
            if (Lasers == 1)
            {
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2(Center.X - (Sprites.VaLaser.Width/2),
                                                       Y - Sprites.VaLaser.Height),new Vector2(0, -6)));
            }else if (Lasers == 2)
            {
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2((Center.X - (VCenter.Width / 2)) - (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2((Center.X + (VCenter.Width / 2))- (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
            }
            else if (Lasers == 3)
            {
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2(Center.X - (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2((Center.X - (VCenter.Width / 2)) - (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2((Center.X + (VCenter.Width / 2)) - (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
            }
            else if (Lasers >= 3)
            {
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2(Center.X - (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2((Center.X - (VCenter.Width / 2)) - (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2((Center.X + (VCenter.Width / 2)) - (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2((Bounds.Left + VEnd.Width) - (Sprites.VaLaser.Width / 2),
                                                       Y - Sprites.VaLaser.Height), new Vector2(0, -6)));
                PlayArena.Lasers.Add(new Laser(Game, PlayArena,
                                           new Vector2((Bounds.Right - VEnd.Width) - (Sprites.VaLaser.Width / 2),
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
            else if (IsLaser)
            {
                IsLaser = false;
                Lasers = 0;
                if(IsTwin)
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
            if(TwinParent!=null)
            {
                TwinParent.AddPowerUp(type);
                return;
            }
            Game.Audio.Play(Sounds.VausCollect);
            switch (type)
            {
                case CapsuleTypes.Reduce:
                    Game.Audio.Play(Sounds.VausLong);
                    IsExtension = false;
                    IsExtraExtension = false;
                    IsReduce = true;
                    IsCatch = false;
                    Started = true;
                    if(IsTwin)
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
                        Game.Audio.Play(Sounds.VausLong);
                        if (IsTwin)
                        {
                            Twin.IsExtraExtension = true;
                        }
                    }
                    else
                    {
                        IsExtension = true;
                        Game.Audio.Play(Sounds.VausLong);
                        if (IsTwin)
                        {
                            Twin.IsExtension = true;
                        }
                    }
                    break;
                case CapsuleTypes.Fast:
                    foreach (var b in PlayArena.Balls)
                    {
                        b.BallSpeed = MathHelper.Clamp(b.BallSpeed * 1.5f, b.MinBallSpeed, b.MaxBallSpeed);
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
                        b.BallSpeed = MathHelper.Clamp(b.BallSpeed * .5f, b.MinBallSpeed, b.MaxBallSpeed);
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
                    Life = (int) MathHelper.Clamp(Life + 1, 0, 12);
                    break;
                case CapsuleTypes.MegaBall:
                    foreach (var b in PlayArena.Balls)
                    {
                        b.IsMegaBall = !b.IsMegaBall;
                    }
                    break;
                case CapsuleTypes.Disrupt:
                    {
                        if (PlayArena.Balls.Count > ArkanoidDX.MaxBalls) break;
                        var addballs = new List<Ball>();
                        foreach (var b in PlayArena.Balls)
                        {
                            var nb = new Ball(Game,PlayArena, b.IsMegaBall, b.IsCaught);
                            nb.SetStartBallLocation(b.Location,b);
                            addballs.Add(nb);
                            nb = new Ball(Game, PlayArena,b.IsMegaBall, b.IsCaught);
                            nb.SetStartBallLocation(b.Location,b);
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
                    AddPowerUp(Utilities.RandomEnum<CapsuleTypes>());
                    break;
            }
        }

        

        public void ResetXY()
        {
            X = (PlayArena.Bounds.X + (PlayArena.Bounds.Width / 2)) - (Width / 2);
            Y = (Game.FrameArea.Height - (Sprites.FrmSideWarp.Height*2)) +
                ((Sprites.FrmSideWarp.Height/2) - (Height/2));
            inertia = 0;
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
            Game.Audio.Play(Sounds.GameStart);
            PlayArena.Balls.Clear();
            PlayArena.Balls.Add(new Ball(Game, PlayArena,false, false));
            
            Restting = false;
        }

        public void Die()
        {
            if (Restting) return;
            if (TwinParent != null)
            {
                VausState = VausState.Explode;
                Sprites.VaExplode.ToStart();
                Sprites.VaExplode.SetAnimation(AnimationState.Play);
                Sprites.VaExplode.OnFinish = () => { TwinParent.IsTwin = false; };
                return;
            }
            
            Restting = true;
            if (IsTwin)
            {
                Twin.VausState = VausState.Explode;
                Sprites.VaExplode.ToStart();
                Sprites.VaExplode.SetAnimation(AnimationState.Play);
                Sprites.VaExplode.OnFinish = () => { Twin.Life--; };
            }
            Life--;
            if(Life<=0)
                PlayArena.Capsules.Clear();
            Game.Audio.Play(Sounds.VausDeath);
            VausState = VausState.Explode;
            Sprites.VaExplode.ToStart();
            Sprites.VaExplode.SetAnimation(AnimationState.Play);
            Sprites.VaExplode.OnFinish = () =>
                                             {                                                 
                                                 Game.Audio.Play(Life<=0?Sounds.GameOver:Sounds.VausEnter);
                                                 ResetXY();
                                                 VausState = VausState.Reassemble;
                                                 

                                                 Sprites.VaExplode.SetAnimation(AnimationState.Stop);
                                                 Sprites.VaRevive.ToStart();
                                                 Sprites.VaRevive.SetAnimation(AnimationState.Play);
                                                 Sprites.VaRevive.OnFinish = () =>
                                                                                 {
                                                                                     Sprites.VaRevive.SetAnimation(
                                                                                         AnimationState.Stop);
                                                                                     if (Life != 0)
                                                                                     {
                                                                                         VausState = VausState.Normal;
                                                                                         Reset();
                                                                                     }
                                                                                 };
                                             };
        }        

        public override void Draw(SpriteBatch batch)
        {

            DrawVaus(batch);
            if(IsTwin)
            {
                Twin.DrawVaus(batch);
            }

            if (IsElectricFence)
            {
                batch.Draw(Sprites.CmnElectricFence,
                           new Rectangle(PlayArena.Bounds.X, (int) PlayArena.Height - 35, PlayArena.Bounds.Width,
                                         Textures.CmnElectricFence.Height), Color.White*PlayArena.Fade.Fade);
            }

            for (var i = 0; i < Life-1; i++)
            {
                batch.Draw(Sprites.CmnLife,
                           new Vector2(PlayArena.Bounds.X + (Sprites.CmnLife.Width * i),
                                       PlayArena.Bounds.Height - Sprites.CmnLife.Height), Color.White * PlayArena.Fade.Fade);
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
                            lEnd = new Rectangle((int) ((lExtP.Left - VEnd.Width) + 1), (int) Y, (int) VEnd.Width,
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
                                lEnd = new Rectangle((int) ((lExtP.Left - VEnd.Width) + 1), (int) Y, (int) VEnd.Width,
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
                        center = new Rectangle((int)(Bounds.Center.X - VCenter.Width / 2f), (int)Y, (int)VCenter.Width,
                                                   (int)VCenter.Height);
                        lSide = new Rectangle(center.Left - (int)(VSide.Width * SideVal), (int)Y,
                                                  (int)(VSide.Width * SideVal), (int)VCenter.Height);
                        rSide = new Rectangle(center.Right, (int)Y, (int)(VSide.Width * SideVal), (int)VCenter.Height);
                        lExt = new Rectangle(lSide.Left - (int)(VExtension.Width * ExtVal), (int)Y,
                                                 (int)(VExtension.Width * ExtVal), (int)VCenter.Height);
                        rExt = new Rectangle(rSide.Right, (int)Y, (int)(VExtension.Width * ExtVal), (int)VCenter.Height);
                        lExtP = new Rectangle(lExt.Left - (int)(VExtensionPlus.Width * ExtPVal), (int)Y,
                                                  (int)(VExtensionPlus.Width * ExtPVal), (int)VCenter.Height);
                        rExtP = new Rectangle(rExt.Right, (int)Y, (int)(VExtensionPlus.Width * ExtPVal),
                                                  (int)VCenter.Height);
                        lEnd = new Rectangle((int)((lExtP.Left - VEnd.Width) + 1), (int)Y, (int)VEnd.Width,
                                                 (int)VCenter.Height);
                        rEnd = new Rectangle(rExtP.Right - 1, (int)Y, (int)VEnd.Width, (int)VCenter.Height);

                        batch.Draw(VExtensionPlus,
                                   new Rectangle(lExtP.X + (int)Game.Shadow.X, lExtP.Y + (int)Game.Shadow.Y, lExtP.Width,
                                                 lExtP.Height), Game.ShadowColor * PlayArena.Fade.Fade);
                        batch.Draw(VExtensionPlus,
                                   new Rectangle(rExtP.X + (int)Game.Shadow.X, rExtP.Y + (int)Game.Shadow.Y, rExtP.Width,
                                                 rExtP.Height), Game.ShadowColor * PlayArena.Fade.Fade, 1, true);
                        batch.Draw(VExtension,
                                   new Rectangle(lExt.X + (int)Game.Shadow.X, lExt.Y + (int)Game.Shadow.Y, lExt.Width,
                                                 lExt.Height), Game.ShadowColor * PlayArena.Fade.Fade);
                        batch.Draw(VExtension,
                                   new Rectangle(rExt.X + (int)Game.Shadow.X, rExt.Y + (int)Game.Shadow.Y, rExt.Width,
                                                 rExt.Height), Game.ShadowColor * PlayArena.Fade.Fade, 1, true);
                        batch.Draw(VSide,
                                   new Rectangle(lSide.X + (int)Game.Shadow.X, lSide.Y + (int)Game.Shadow.Y, lSide.Width,
                                                 lSide.Height), Game.ShadowColor * PlayArena.Fade.Fade);
                        batch.Draw(VSide,
                                   new Rectangle(rSide.X + (int)Game.Shadow.X, rSide.Y + (int)Game.Shadow.Y, rSide.Width,
                                                 rSide.Height), Game.ShadowColor * PlayArena.Fade.Fade, 1, true);
                        batch.Draw(VCenter,
                                   new Rectangle(center.X + (int)Game.Shadow.X, center.Y + (int)Game.Shadow.Y, center.Width,
                                                 center.Height), Game.ShadowColor * PlayArena.Fade.Fade);
                        batch.Draw(VEnd,
                                   new Rectangle(lEnd.X + (int)Game.Shadow.X, lEnd.Y + (int)Game.Shadow.Y, lEnd.Width,
                                                 lEnd.Height), Game.ShadowColor * PlayArena.Fade.Fade);
                        batch.Draw(VEnd,
                                   new Rectangle(rEnd.X + (int)Game.Shadow.X, rEnd.Y + (int)Game.Shadow.Y, rEnd.Width,
                                                 rEnd.Height), Game.ShadowColor * PlayArena.Fade.Fade, 1, true);

                        batch.Draw(VExtensionPlus, lExtP, Color.White * PlayArena.Fade.Fade);
                        batch.Draw(VExtensionPlus, rExtP, Color.White * PlayArena.Fade.Fade, 1, true);
                        batch.Draw(VExtension, lExt, Color.White * PlayArena.Fade.Fade);
                        batch.Draw(VExtension, rExt, Color.White * PlayArena.Fade.Fade, 1, true);
                        batch.Draw(VSide, lSide, Color.White * PlayArena.Fade.Fade);
                        batch.Draw(VSide, rSide, Color.White * PlayArena.Fade.Fade, 1, true);
                        batch.Draw(VCenter, center, Color.White * PlayArena.Fade.Fade);
                        batch.Draw(VEnd, lEnd, Color.White * PlayArena.Fade.Fade);
                        batch.Draw(VEnd, rEnd, Color.White* PlayArena.Fade.Fade, 1, true);
                        if (IsCatch)
                        {
                            batch.Draw(Sprites.VaExtensionPlusCatch, lExtP, Color.White* PlayArena.Fade.Fade);
                            batch.Draw(Sprites.VaExtensionPlusCatch, rExtP, Color.White* PlayArena.Fade.Fade, 1, true);
                            batch.Draw(Sprites.VaExtensionCatch, lExt, Color.White* PlayArena.Fade.Fade);
                            batch.Draw(Sprites.VaExtensionCatch, rExt, Color.White * PlayArena.Fade.Fade, 1, true);
                            batch.Draw(Sprites.VaSideCatch, lSide, Color.White * PlayArena.Fade.Fade);
                            batch.Draw(Sprites.VaSideCatch, rSide, Color.White * PlayArena.Fade.Fade, 1, true);
                            batch.Draw(Sprites.VaCenterCatch, center, Color.White * PlayArena.Fade.Fade);
                        }
                        if(IsMegaLaser)
                        {
                            batch.Draw(Sprites.VaCenterMegaLaser, center, Color.White * PlayArena.Fade.Fade);
                        }
                    }
                        #endregion

                    break;
                case VausState.Explode:
                    batch.Draw(Sprites.VaExplode, Bounds, Color.White * PlayArena.Fade.Fade);
                    break;
                case VausState.Reassemble:
                    batch.Draw(Sprites.VaRevive, Bounds, Color.White* PlayArena.Fade.Fade);
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
        
        
    }
}