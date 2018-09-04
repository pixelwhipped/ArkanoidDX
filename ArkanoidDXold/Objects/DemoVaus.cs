using System;
using System.Linq;
using ArkanoidDX.Arena;
using Microsoft.Xna.Framework;

namespace ArkanoidDX.Objects
{
    public class DemoVaus : Vaus
    {

        public TimeSpan ActionTimer ;
        public DemoVaus(ArkanoidDX game, PlayArena playArena) : base(game, playArena)
        {
            ActionTimer = new TimeSpan(0, 0, 0, 1);
        }

        public override void Update(GameTime gameTime)
        {
            ActionTimer -= gameTime.ElapsedGameTime;
            CheckCapsuleVausCollision();

            if (!Started)
            {
            foreach (var b in PlayArena.Balls)
                b.SetStartVausLocation(new Vector2(Center.X - (b.Width/2), Y - b.Height));
            }

            SideVal = (IsReduce) ? MathHelper.Clamp(SideVal - 0.1f, 0, 1) : MathHelper.Clamp(SideVal + 0.1f, 0, 1);
            ExtVal = (IsExtension) ? MathHelper.Clamp(ExtVal + 0.1f, 0, 1) : MathHelper.Clamp(ExtVal - 0.1f, 0, 1);
            ExtPVal = (IsExtraExtension)
                          ? MathHelper.Clamp(ExtPVal + 0.1f, 0, 1)
                          : MathHelper.Clamp(ExtPVal - 0.1f, 0, 1);


            Motion = Vector2.Zero;
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
            if(PlayArena.IsOver)
            {
                Motion = new Vector2(1, 0);
            }
            else if (PlayArena.Balls.All(p => p.Motion.Y < 0)&&PlayArena.Capsules.Count !=0)
            {
                if (PlayArena.Balls.Count != 0)
                {
                    Vector2 nerrest =
                        PlayArena.Capsules.Select(
                            vect => new {distance = Vector2.Distance(vect.Center, Center), vect.Center})
                            .OrderBy(x => x.distance)
                            .First().Center;
                    if (nerrest.X < X + VEnd.Width && nerrest.X < (X + (Width - VEnd.Width)))
                    {
                        Motion = new Vector2(-.5f, Motion.Y); //Motion = new Vector2(PlayArena.Balls.First().BallSpeed * -.5f, Motion.Y);
                    }
                    if (nerrest.X > X + VEnd.Width && nerrest.X > (X + (Width - VEnd.Width)))
                    {
                        Motion = new Vector2(.5f, Motion.Y); //Motion = new Vector2(PlayArena.Balls.First().BallSpeed * .5f, Motion.Y);
                    }
                }
            }else
            {
                if (PlayArena.Balls.Count != 0)
                {
                    Vector2 nerrest =
                        PlayArena.Balls.Select(
                            vect => new {distance = Vector2.Distance(vect.Center, Center), vect.Center})
                            .OrderBy(x => x.distance)
                            .First().Center;
                    if (nerrest.X < X + VEnd.Width && nerrest.X < (X + (Width - VEnd.Width)))
                    {
                        Motion = new Vector2(-.5f, Motion.Y);
                       // Motion = new Vector2(PlayArena.Balls.First().BallSpeed*-.5f, Motion.Y);
                    }
                    if (nerrest.X > X + VEnd.Width && nerrest.X > (X + (Width - VEnd.Width)))
                    {
                        Motion = new Vector2(.5f, Motion.Y);
                        //Motion = new Vector2(PlayArena.Balls.First().BallSpeed*.5f, Motion.Y);
                    }
                }
            }

            if (ActionTimer < TimeSpan.Zero)
            {
                ActionTimer = new TimeSpan(0, 0, 0, 0,800);
                Started = true;
                if (IsLaser)
                {
                    AddLaser();
                }
                foreach (var b in PlayArena.Balls)
                    b.IsCaught = false;
                    
            }


            Motion = new Vector2(Motion.X * PaddleSpeed, Motion.Y);
            Location += Motion;



            if (X < PlayArena.Bounds.X)
                X = PlayArena.Bounds.X;
            if (X + Width > (PlayArena.Bounds.Width + PlayArena.Bounds.X))
                X = (PlayArena.Bounds.Width + PlayArena.Bounds.X) - Width;


        }
    }
}
