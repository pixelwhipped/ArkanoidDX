using System;
using System.Linq;
using ArkanoidDX.Arena;
using ArkanoidDX.Audio;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Objects
{
    public class Ball : GameObject
    {
        private const float BallStartSpeed = 2.5f;
        public float MinBallSpeed = 1.7f;
        public float MaxBallSpeed = 6.3f;
        public float BallSpeed;
        public float CoughtAt;
        public bool IsCaught;
        public bool IsMegaBall;
        public bool CoughtByTwin;
        public bool IsOrbit;
        public bool Orbitplus;
        public float OrbitX;

        public override Sprite Texture
        {
            get { return IsMegaBall ? Sprites.BallMega : Sprites.BallNormal; }
        }

        public override bool IsAlive
        {
            get { return Location.Y <= Game.Height; }
        }

        public PlayArena PlayArena;

        public Ball(ArkanoidDX game, PlayArena playArena, bool megaBall, bool isCaught):base(game)
        {
            PlayArena = playArena;
            IsMegaBall = megaBall;
            X = Y = 0;
            IsCaught = isCaught;
        }

        public void Update(GameTime gameTime)
        {
            CheckWallCollision();
            CheckBrickCollision();
            CheckEnimyCollsion();
            CheckVausCollision();
            CheckVausTwinCollision();
            if (IsOrbit)
            {
                if (Orbitplus)
                {
                    OrbitX += 1f;
                    if (OrbitX > 4)
                    {
                        Orbitplus = false;
                    }
                }
                else
                {
                    OrbitX -= 1f;
                    if (OrbitX < -4)
                    {
                        Orbitplus = true;
                    }
                }
            }

            if (PlayArena.Vaus.Started && !IsCaught)
            {
                Location += (Motion*BallSpeed) + new Vector2(OrbitX,0);
                BallSpeed = MathHelper.Clamp(BallSpeed + 0.0009f, MinBallSpeed , MaxBallSpeed);                
            }
            else if (IsCaught)
            {
                if (CoughtByTwin&&PlayArena.Vaus.IsTwin)
                    Location = new Vector2(PlayArena.Vaus.Twin.X + CoughtAt, PlayArena.Vaus.Twin.Y - Height);
                else
                    Location = new Vector2(PlayArena.Vaus.X + CoughtAt, PlayArena.Vaus.Y - Height);
            }
            else
            {
                Location = new Vector2(PlayArena.Vaus.Center.X - (Width / 2), Location.Y);
            }
            
          
        }

        public void SetStartVausLocation(Vector2 location)
        {
            
            BallSpeed = BallStartSpeed;
            var bounceAngle = location.X / PlayArena.Bounds.Right;
            bounceAngle = MathHelper.Pi + (bounceAngle * MathHelper.Pi);
            Motion = new Vector2((float)Math.Cos(bounceAngle), (float)-Math.Abs(-Math.Sin(bounceAngle)));
            Motion.Normalize();
            Location = location;
        }

        public void SetStartBallLocation(Vector2 location, Ball ball)
        {

            Motion = new Vector2(ball.Motion.X + (float)(ArkanoidDX.Random.NextDouble() - ArkanoidDX.Random.NextDouble()), ball.Motion.Y + (float)(ArkanoidDX.Random.NextDouble() - ArkanoidDX.Random.NextDouble()));
            Motion.Normalize();
            BallSpeed = ball.BallSpeed;
            Location = location;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location + Game.Shadow, Game.ShadowColor * PlayArena.Fade.Fade);
            spriteBatch.Draw(Texture, Location, Color.White * PlayArena.Fade.Fade);
            //spriteBatch.DrawString(Fonts.SmallFont, BallSpeed + "", Location + new Vector2(5, 0), Color.IndianRed);
        }

        public void CheckEnimyCollsion()
        {
            foreach (var e in PlayArena.Enimies)
            {
                if (!e.IsAlive || e.IsExploding) continue;
                Direction d;
                CollisionPoint c;
                if (!Collisions.IsCollision(this, e, out d, out c)) continue;
                e.Die();
                DeflectEnemy(c, d);
            }
        }

        public void DeflectEnemy(CollisionPoint c, Direction d)
        {
            Deflect(c, d);
        }

        public void CheckVausCollision()
        {
            if (!PlayArena.Vaus.IsAlive) return;
            Direction d;
            CollisionPoint c;
            if (PlayArena.Vaus.IsElectricFence)
            {
                if (Collisions.IsCollision(this, new Rectangle(PlayArena.Bounds.X, (int)PlayArena.Height - 35, PlayArena.Bounds.Width,
                                         Textures.CmnElectricFence.Height), out d, out c) && IsAlive)
                {
                    Game.Audio.Play(Sounds.BallMetalBounce);
                    Motion = new Vector2(Motion.X, Motion.Y * -1f);
                    return;
                }

            }
            if (
                !(Collisions.IsCollision(this, PlayArena.Vaus.Bounds, out d, out c) ||
                  Collisions.IsCollision(this, PlayArena.Vaus.IllusionBBounds, out d, out c) ||
                  Collisions.IsCollision(this, PlayArena.Vaus.IllusionABounds, out d, out c)) || !IsAlive) return;


            switch (c)
            {
                case CollisionPoint.Top:
                case CollisionPoint.TopLeft:
                case CollisionPoint.TopRight:
                    if (!IsCaught)
                        DeflectVaus(PlayArena.Vaus);
                    if (!PlayArena.Vaus.IsCatch) return;
                    IsCaught = true;
                    CoughtAt = Location.X - PlayArena.Vaus.X;
                    CoughtByTwin = false;
                    break;
                case CollisionPoint.Left:
                    Motion = new Vector2(Motion.X * -1f, Motion.Y);
                    Location = new Vector2(PlayArena.Vaus.X - Width, Location.Y);
                    Game.Audio.Play(Sounds.BallMetalBounce);
                    break;
                case CollisionPoint.Right:
                    Motion = new Vector2(Motion.X * -1f, Motion.Y);
                    Location = new Vector2(PlayArena.Vaus.Bounds.Right, Location.Y);
                    Game.Audio.Play(Sounds.BallMetalBounce);
                    break;
                case CollisionPoint.Bottom:
                case CollisionPoint.BottomLeft:
                case CollisionPoint.BottomRight:
                    Motion = new Vector2(Motion.X, Motion.Y * -1f);
                    Location = new Vector2(Location.X, PlayArena.Vaus.Bounds.Bottom);
                    Game.Audio.Play(Sounds.BallMetalBounce);
                    break;
                case CollisionPoint.None:
                    break;
            }
        }

        public void CheckVausTwinCollision()
        {
            if (!PlayArena.Vaus.IsAlive) return;
            if (!PlayArena.Vaus.IsTwin) return;
            Direction d;
            CollisionPoint c;
            if (!(Collisions.IsCollision(this, PlayArena.Vaus.Twin.Bounds, out d, out c) || Collisions.IsCollision(this, PlayArena.Vaus.Twin.IllusionBBounds, out d, out c) || Collisions.IsCollision(this, PlayArena.Vaus.Twin.IllusionABounds, out d, out c)) || !IsAlive) return;

            switch (c)
            {
                case CollisionPoint.Top:
                case CollisionPoint.TopLeft:
                case CollisionPoint.TopRight:
                    if (!IsCaught)
                        DeflectVaus(PlayArena.Vaus.Twin);
     
                    if (!PlayArena.Vaus.Twin.IsCatch) return;
                    IsCaught = true;
                    CoughtAt = Location.X - PlayArena.Vaus.Twin.X;
                    CoughtByTwin = true;
                    break;
                case CollisionPoint.Left:
                    Motion = new Vector2(Motion.X * -1f, Motion.Y);
                    Location = new Vector2(PlayArena.Vaus.Twin.X - Width, Location.Y);
                    Game.Audio.Play(Sounds.BallMetalBounce);
                    break;
                case CollisionPoint.Right:
                    Motion = new Vector2(Motion.X * -1f, Motion.Y);
                    Location = new Vector2(PlayArena.Vaus.Twin.Bounds.Right, Location.Y);
                    Game.Audio.Play(Sounds.BallMetalBounce);
                    break;
                case CollisionPoint.Bottom:
                case CollisionPoint.BottomLeft:
                case CollisionPoint.BottomRight:
                    Motion = new Vector2(Motion.X, Motion.Y * -1f);
                    Location = new Vector2(Location.X, PlayArena.Vaus.Twin.Bounds.Bottom);
                    Game.Audio.Play(Sounds.BallMetalBounce);
                    break;
                case CollisionPoint.None:
                    break;
            }
        }

        

        public void DeflectVaus(Vaus v)
        {
            if (Motion.Y > 0)
            {
                var relativeIntersectX = Center.X - v.X;
                var normalizedRelativeIntersectionX = relativeIntersectX / (v.Width);
                var bounceAngle = MathHelper.Clamp(MathHelper.Pi + ((normalizedRelativeIntersectionX + (float)((ArkanoidDX.Random.NextDouble() / 8f) * (ArkanoidDX.Random.NextDouble() > .5 ? 1 : -1))) * MathHelper.Pi), MathHelper.Pi + .2f, MathHelper.TwoPi - .2f);
                Motion = new Vector2((float)Math.Cos(bounceAngle), (float)-Math.Abs(-Math.Sin(bounceAngle)));
                Motion.Normalize();
                Location = new Vector2(Location.X, v.Y - (Height));
            }
            else
            {
                Motion = new Vector2(Motion.X, Motion.Y*-1f);
            }
            Game.Audio.Play(Sounds.BallMetalBounce);
        }

        public void CheckWallCollision()
        {
            if (!IsAlive) return;
            if (X <= PlayArena.X)
            {
                X = PlayArena.X;
                Motion = new Vector2(Motion.X*-1, Motion.Y);
                if (!(PlayArena.Vaus.ActionExitLeft || PlayArena.Vaus.ActionExitRight))
                Game.Audio.Play(Sounds.BallMetalBounce);
            }
            else if (X >= (PlayArena.X + PlayArena.Width) - Width)
            {
                X = (PlayArena.X + PlayArena.Width - Width);
                Motion = new Vector2(Motion.X * -1, Motion.Y);
                if (!(PlayArena.Vaus.ActionExitLeft || PlayArena.Vaus.ActionExitRight))
                Game.Audio.Play(Sounds.BallMetalBounce);
            }
            else if (Y <= PlayArena.Y)
            {
                Y = PlayArena.Y;
                Motion = new Vector2(Motion.X, Motion.Y*-1);
                if (!(PlayArena.Vaus.ActionExitLeft || PlayArena.Vaus.ActionExitRight))
                Game.Audio.Play(Sounds.BallMetalBounce);
            }

        }

        public void CheckBrickCollision()
        {
            if (Game.GameMode == GameMode.Boss) return;
            foreach (var brick in PlayArena.LevelMap.BrickMap)
            {
                if (!brick.IsAlive) continue;
                
                Direction dx;
                CollisionPoint cx;
                if (brick.Bounds.Contains(Bounds))
                {
                    while (brick.Bounds.Contains(Bounds))
                    {
                        Location += ((Motion*-1));// * BallSpeed);
                    }
                    Location += ((Motion));// * BallSpeed);
                }
                if (!Collisions.IsCollision(this, brick, out dx, out cx)) continue;
                if (!brick.IsActivated)
                    brick.IsActivated = true;
                cx = GetCollisionArea(brick);
                if(brick.IsTeleport)
                {
                    var t = PlayArena.LevelMap.BrickMap.Where(b => b.IsTeleport && b != brick).ToList();
                    var br = t.Count==0? brick:t[ArkanoidDX.Random.Next(t.Count)];
                    Brick ob;
                    switch(cx)
                    {
                        case CollisionPoint.Left:
                            if (PlayArena.LevelMap.GetBrickToRight(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick,cx,dx);
                            }
                            else
                            {
                                Location = new Vector2(br.Bounds.Right, br.Center.Y);
                            }
                            break;
                        case CollisionPoint.Right:
                            if (PlayArena.LevelMap.GetBrickToLeft(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick,cx,dx);
                            }
                            else
                            {
                                Location = new Vector2(br.Bounds.Left - Width, br.Center.Y);
                            }
                            break;
                        case CollisionPoint.Top:
                            if (PlayArena.LevelMap.GetBrickBelow(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick,cx,dx);
                            }
                            else
                            {
                                Location = new Vector2(br.Center.X, br.Bounds.Bottom);
                            }
                            break;
                        case CollisionPoint.Bottom:
                            if (PlayArena.LevelMap.GetBrickAbove(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick,cx,dx);
                            }
                            else
                            {
                                Location = new Vector2(br.Center.X, br.Bounds.Top - (Height));
                            }
                            break;
                        case CollisionPoint.TopLeft:
                            if (PlayArena.LevelMap.GetBrickToBelowRight(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick,cx,dx);
                            }
                            else
                            {
                                Location = new Vector2(br.Bounds.Right, br.Bounds.Bottom);
                            }
                            break;
                        case CollisionPoint.TopRight:
                            if (PlayArena.LevelMap.GetBrickToBelowLeft(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick,cx,dx);
                            }
                            else
                            {
                                Location = new Vector2(br.Bounds.Left - Width, br.Bounds.Bottom);
                            }
                            break;
                        case CollisionPoint.BottomLeft:
                            if (PlayArena.LevelMap.GetBrickToUpperRight(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick,cx,dx);
                            }
                            else
                            {
                                Location = new Vector2(br.Bounds.Right, br.Bounds.Top - Height);
                            }
                            break;
                        case CollisionPoint.BottomRight:
                            if (PlayArena.LevelMap.GetBrickToUpperLeft(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick,cx,dx);
                            }
                            else
                            {
                                Location = new Vector2(br.Bounds.Left - Width, br.Bounds.Top - Height);
                            }
                            break;
                        case CollisionPoint.None:
                            break;
                    }
                    return;
                }
                brick.Die();
               
                if ((IsMegaBall&& !brick.IsAlive) && !brick.IsInvincible) continue;
                DeflectBallFromBrick(brick,cx,dx);
            }
        }

        public void DeflectBallFromBrick(Brick brick,CollisionPoint cx, Direction dx)
        {
            Direction d;
            CollisionPoint c;
            while (Collisions.IsCollision(this, brick, out d, out c))
            {
                switch (d)
                {
                    case Direction.Up:
                        Location += new Vector2(Motion.X * BallSpeed, Math.Abs(Motion.Y * BallSpeed));
                        break;
                    case Direction.Down:
                        Location += new Vector2(Motion.X * BallSpeed, -1 * Math.Abs(Motion.Y * BallSpeed));
                        break;
                    case Direction.Left:
                        Location += new Vector2(Math.Abs(Motion.X * BallSpeed), Motion.Y * BallSpeed);
                        break;
                    case Direction.Right:
                         Location += new Vector2(-1 * Math.Abs(Motion.X * BallSpeed), Motion.Y * BallSpeed);
                        break;
                    case Direction.DownLeft:
                        Location += new Vector2(Math.Abs(Motion.X * BallSpeed), -1 * Math.Abs(Motion.Y * BallSpeed));
                        break;
                    case Direction.DownRight:
                        Location += new Vector2(-1 * Math.Abs(Motion.X * BallSpeed), -1 * Math.Abs(Motion.Y * BallSpeed));
                        break;
                    case Direction.UpLeft:
                        Location += new Vector2(Math.Abs(Motion.X * BallSpeed), Math.Abs(Motion.Y * BallSpeed));
                        break;
                    case Direction.UpRight:
                        Location += new Vector2(-1 * Math.Abs(Motion.X * BallSpeed), Math.Abs(Motion.Y * BallSpeed));
                        break;
                    case Direction.Stop:
                        break;
                }
            }
            
            DeflectBrick(cx,dx,brick);
        }
        public CollisionPoint GetCollisionArea(Brick brick)
        {
            
            Rectangle rx = brick.Bounds;
            Rectangle ry = brick.Bounds;
            Brick n;
            var b = brick;
            while (PlayArena.LevelMap.GetBrickToLeft(b, out n))
            {
                rx = new Rectangle(n.Bounds.X, n.Bounds.Y, rx.Width + n.Bounds.Width, b.Bounds.Height);
                b = n;
            }
            b = brick;
            while (PlayArena.LevelMap.GetBrickToRight(b, out n))
            {
                rx = new Rectangle(rx.X, rx.Y, rx.Width + n.Bounds.Width, b.Bounds.Height);
                b = n;
            }
            b = brick;
            while (PlayArena.LevelMap.GetBrickAbove(b, out n))
            {
                ry = new Rectangle(n.Bounds.X, n.Bounds.Y, ry.Width, ry.Height + n.Bounds.Height);
                b = n;
            }
            b = brick;
            while (PlayArena.LevelMap.GetBrickBelow(b, out n))
            {
                ry = new Rectangle(ry.X, ry.Y, ry.Width, ry.Height + n.Bounds.Height);
                b = n;
            }
            var rect = new Rectangle(rx.X, ry.Y, rx.Width, ry.Height);
            Direction d;
            CollisionPoint c;
            Collisions.IsCollision(this, rect, out d, out c);
            return c;
        }

        public void DeflectBrick(CollisionPoint c, Direction d, Brick brick)
        {
            switch (c)
            {
                case CollisionPoint.Right:
                case CollisionPoint.Left:
                    Motion = new Vector2(Motion.X * -1, Motion.Y);
                    break;
                case CollisionPoint.Top:
                case CollisionPoint.Bottom:
                    Motion = new Vector2(Motion.X, Motion.Y   * -1);
                    break;
                case CollisionPoint.TopRight:
                    if (d==Direction.DownRight)
                        Motion = new Vector2(Motion.X, Motion.Y   * -1);
                    else if(d==Direction.Up || d==Direction.UpLeft || d==Direction.UpRight)
                        Motion = new Vector2(Motion.X   * -1, Motion.Y);                    
                    else
                        Motion = new Vector2(Motion.X   * -1, Motion.Y   * -1);
                    break;
                case CollisionPoint.TopLeft:
                    if (d==Direction.DownLeft)
                        Motion = new Vector2(Motion.X, Motion.Y   * -1);
                    else if(d==Direction.Up || d==Direction.UpLeft || d==Direction.UpRight)
                        Motion = new Vector2(Motion.X   * -1, Motion.Y);                    
                    else
                        Motion = new Vector2(Motion.X   * -1, Motion.Y   * -1);
                    break;
                case CollisionPoint.BottomRight:  
                    if (d == Direction.UpRight)
                        Motion = new Vector2(Motion.X, Motion.Y   * -1);
                    else if(d==Direction.Down || d==Direction.DownLeft || d==Direction.DownRight)
                        Motion = new Vector2(Motion.X   * -1, Motion.Y);                    
                    else
                        Motion = new Vector2(Motion.X   * -1, Motion.Y   * -1);
                    break;
                case CollisionPoint.BottomLeft:
                    if (d == Direction.UpLeft)
                        Motion = new Vector2(Motion.X, Motion.Y   * -1);
                    else if(d==Direction.Down || d==Direction.DownLeft || d==Direction.DownRight)
                        Motion = new Vector2(Motion.X   * -1, Motion.Y);                    
                    else
                        Motion = new Vector2(Motion.X   * -1, Motion.Y   * -1);
                    break;

            }
            
           Motion =
                new Vector2(
                    Motion.X < 0
                        ? Motion.X - (float) (ArkanoidDX.Random.NextDouble()/20)
                        : Motion.X + (float) (ArkanoidDX.Random.NextDouble()/20),
                    Motion.Y < 0
                        ? Motion.Y - (float) (ArkanoidDX.Random.NextDouble()/20)
                        : Motion.Y + (float) (ArkanoidDX.Random.NextDouble()/20));
            Motion.Normalize();
        }

        public void Deflect(CollisionPoint c, Direction d)
        {
            switch (c)
            {
                case CollisionPoint.Right:
                case CollisionPoint.Left:
                    Motion = new Vector2(Motion.X * -1, Motion.Y);
                    break;
                case CollisionPoint.Top:
                case CollisionPoint.Bottom:
                    Motion = new Vector2(Motion.X, Motion.Y * -1);
                    break;
                case CollisionPoint.BottomRight:
                case CollisionPoint.BottomLeft:
                case CollisionPoint.TopRight:
                case CollisionPoint.TopLeft:
                    Motion = new Vector2(Motion.X * -1, Motion.Y * -1);
                    break;
            }
        }
    }
}