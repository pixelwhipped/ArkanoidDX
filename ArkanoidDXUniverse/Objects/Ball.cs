using System;
using System.Linq;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Objects
{
    public class Ball : GameObject
    {
        private const float BallStartSpeed = 2.5f;
        public float BallSpeed;
        public float CoughtAt;
        public bool CoughtByTwin;
        public bool IsCaught;
        public bool IsMegaBall;
        public bool IsOrbit;
        public bool IsMagnaBall;
        public float MaxBallSpeed = 6.5f;
        public float MinBallSpeed = 1.7f;
        public bool Orbitplus;
        public float OrbitX;
        public float MagnaModX;

        public PlayArena PlayArena;

        public Ball(Arkanoid game, PlayArena playArena, bool megaBall, bool isCaught, bool isMagna) : base(game)
        {
            PlayArena = playArena;
            IsMegaBall = megaBall;
            IsMagnaBall = isMagna;
            X = Y = 0;
            IsCaught = isCaught;
        }

        public override Sprite Texture
        {
            get
            {
                if (IsMegaBall && IsMagnaBall) return Sprites.BallMegaMagna;
                if (IsMagnaBall) return Sprites.BallMagna;
                if (IsMegaBall) return Sprites.BallMega;
                return Sprites.BallNormal;
            }
        } 
        public override bool IsAlive => Location.Y <= Game.Height;

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
            if (IsMagnaBall && Motion.Y >= 0)
            {
                Vector2 vloc;
                float d;
                if (PlayArena.Vaus.IsTwin)
                {
                    var d1 = Vector2.Distance(this.Location, PlayArena.Vaus.Center);
                    var d2 = Vector2.Distance(this.Location, PlayArena.Vaus.Twin.Center);

                    if (d1 < d2)
                    {
                        d = d1;
                        vloc = PlayArena.Vaus.Center;
                    }
                    else
                    {
                        d = d1;
                        vloc = PlayArena.Vaus.Twin.Center;
                    }
                }
                else
                {
                    d = Vector2.Distance(this.Location, PlayArena.Vaus.Center);
                    vloc = PlayArena.Vaus.Center;
                }
                d = 1f-(MathHelper.Clamp(d, 0, 250)/250);
                if (vloc.X < Location.X) MagnaModX = -.2f * d;
                else if (vloc.X > Location.X) MagnaModX =  .2f * d;
                else
                {
                    MagnaModX = 0;
                }
            }
            else
            {
                MagnaModX = 0;
            }

            Motion = new Vector2(Motion.X+MagnaModX, Motion.Y);
            if (PlayArena.Vaus.Started && !IsCaught)
            {
                var m = (Motion*BallSpeed);
                Location += new Vector2(MathHelper.Clamp(m.X, MaxBallSpeed*-1, MaxBallSpeed), MathHelper.Clamp(m.Y, MaxBallSpeed*-1, MaxBallSpeed)) + new Vector2(OrbitX, 0);
                 BallSpeed = MathHelper.Clamp(BallSpeed + 0.0009f, MinBallSpeed, MaxBallSpeed);
            }
            else if (IsCaught)
            {
                if (CoughtByTwin && PlayArena.Vaus.IsTwin)
                    Location = new Vector2(PlayArena.Vaus.Twin.X + CoughtAt, PlayArena.Vaus.Twin.Y - Height);
                else
                    Location = new Vector2(PlayArena.Vaus.X + CoughtAt, PlayArena.Vaus.Y - Height);
            }
            else
            {
                Location = new Vector2(PlayArena.Vaus.Center.X - Width/2, Location.Y);
            }

        }

        public void SetStartVausLocation(Vector2 location)
        {
            BallSpeed = BallStartSpeed;
            var bounceAngle = location.X/PlayArena.Bounds.Right;
            bounceAngle = MathHelper.Pi + bounceAngle*MathHelper.Pi;
            Motion = new Vector2((float) Math.Cos(bounceAngle), (float) -Math.Abs(-Math.Sin(bounceAngle)));
            Motion.Normalize();
            Location = location;
        }

        public void SetStartBallLocation(Vector2 location, Ball ball)
        {
            Motion = new Vector2(ball.Motion.X + (float) (Arkanoid.Random.NextDouble() - Arkanoid.Random.NextDouble()),
                ball.Motion.Y + (float) (Arkanoid.Random.NextDouble() - Arkanoid.Random.NextDouble()));
            Motion.Normalize();
            BallSpeed = ball.BallSpeed;
            Location = location;
        }

        private Rectangle LastCollision;
        public override void Draw(SpriteBatch spriteBatch)
        {
           // spriteBatch.DrawRectangle(LastCollision,Color.Red,2f);
            spriteBatch.Draw(Texture, Location + Game.Shadow, Game.ShadowColor*PlayArena.Fade.Fade);
            spriteBatch.Draw(Texture, Location, Color.White*PlayArena.Fade.Fade);
          //  spriteBatch.DrawString(Fonts.SmallFont, Math.Round(Motion.X, 2) + " " + Math.Round(Motion.Y,2), Location + new Vector2(5, 0), Color.IndianRed);
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
                if (Collisions.IsCollision(this,
                    new Rectangle(PlayArena.Bounds.X, (int) PlayArena.Height - 35, PlayArena.Bounds.Width,
                        Textures.CmnElectricFence.Height), out d, out c) && IsAlive)
                {
                    Game.Sounds.BallMetalBounce.Play();
                    Motion = new Vector2(Motion.X, Motion.Y*-1f);
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
                    Motion = new Vector2(Motion.X*-1f, Motion.Y);
                    Location = new Vector2(PlayArena.Vaus.X - Width, Location.Y);
                    Game.Sounds.BallMetalBounce.Play();
                    break;
                case CollisionPoint.Right:
                    Motion = new Vector2(Motion.X*-1f, Motion.Y);
                    Location = new Vector2(PlayArena.Vaus.Bounds.Right, Location.Y);
                    Game.Sounds.BallMetalBounce.Play();
                    break;
                case CollisionPoint.Bottom:
                case CollisionPoint.BottomLeft:
                case CollisionPoint.BottomRight:
                    Motion = new Vector2(Motion.X, Motion.Y*-1f);
                    Location = new Vector2(Location.X, PlayArena.Vaus.Bounds.Bottom);
                    Game.Sounds.BallMetalBounce.Play();
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
            if (
                !(Collisions.IsCollision(this, PlayArena.Vaus.Twin.Bounds, out d, out c) ||
                  Collisions.IsCollision(this, PlayArena.Vaus.Twin.IllusionBBounds, out d, out c) ||
                  Collisions.IsCollision(this, PlayArena.Vaus.Twin.IllusionABounds, out d, out c)) || !IsAlive) return;

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
                    Motion = new Vector2(Motion.X*-1f, Motion.Y);
                    Location = new Vector2(PlayArena.Vaus.Twin.X - Width, Location.Y);
                    Game.Sounds.BallMetalBounce.Play();
                    break;
                case CollisionPoint.Right:
                    Motion = new Vector2(Motion.X*-1f, Motion.Y);
                    Location = new Vector2(PlayArena.Vaus.Twin.Bounds.Right, Location.Y);
                    Game.Sounds.BallMetalBounce.Play();
                    break;
                case CollisionPoint.Bottom:
                case CollisionPoint.BottomLeft:
                case CollisionPoint.BottomRight:
                    Motion = new Vector2(Motion.X, Motion.Y*-1f);
                    Location = new Vector2(Location.X, PlayArena.Vaus.Twin.Bounds.Bottom);
                    Game.Sounds.BallMetalBounce.Play();
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
                var normalizedRelativeIntersectionX = relativeIntersectX/v.Width;
                var bounceAngle =
                    MathHelper.Clamp(
                        MathHelper.Pi +
                        (normalizedRelativeIntersectionX +
                         (float) (Arkanoid.Random.NextDouble()/8f*(Arkanoid.Random.NextDouble() > .5 ? 1 : -1)))*
                        MathHelper.Pi, MathHelper.Pi + .2f, MathHelper.TwoPi - .2f);
                Motion = new Vector2((float) Math.Cos(bounceAngle), (float) -Math.Abs(-Math.Sin(bounceAngle)));
                Motion.Normalize();
                Location = new Vector2(Location.X, v.Y - Height);
            }
            else
            {
                Motion = new Vector2(Motion.X, Motion.Y*-1f);
            }
            Game.Sounds.BallMetalBounce.Play();
        }

        public void CheckWallCollision()
        {
            if (!IsAlive) return;
            if (X <= PlayArena.X)
            {
                X = PlayArena.X;
                Motion = new Vector2(Motion.X*-1, Motion.Y);
                if (!(PlayArena.Vaus.ActionExitLeft || PlayArena.Vaus.ActionExitRight))
                    Game.Sounds.BallMetalBounce.Play();
            }
            else if (X >= PlayArena.X + PlayArena.Width - Width)
            {
                X = PlayArena.X + PlayArena.Width - Width;
                Motion = new Vector2(Motion.X*-1, Motion.Y);
                if (!(PlayArena.Vaus.ActionExitLeft || PlayArena.Vaus.ActionExitRight))
                    Game.Sounds.BallMetalBounce.Play();
            }
            else if (Y <= PlayArena.Y)
            {
                Y = PlayArena.Y;
                Motion = new Vector2(Motion.X, Motion.Y*-1);
                if (!(PlayArena.Vaus.ActionExitLeft || PlayArena.Vaus.ActionExitRight))
                    Game.Sounds.BallMetalBounce.Play();
            }
        }

        public void CheckBrickCollisionold()
        {
            if (Game.GameMode == GameMode.Boss) return;
            foreach (var brick in PlayArena.LevelMap.BrickMap)
            {
                if (!brick.IsAlive) continue;
                Direction dx;
                CollisionPoint cx;
                if (!brick.Bounds.Contains(Bounds)) continue;
                Rectangle area;
                GetCollisionArea(brick, out area);
                Collisions.IsCollision(this, area, out dx, out cx);
                LastCollision = area;
                while (area.Contains(Bounds))
                {
                    Location += Motion*-1;
                }
                DeflectBrick(cx, dx, brick);
                break;
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
                        Location += Motion*-1; // * BallSpeed);
                    }
                    Location += Motion; // * BallSpeed);
                }
                if (!Collisions.IsCollision(this, brick, out dx, out cx)) continue;
                if (!brick.IsActivated)
                    brick.IsActivated = true;
                cx = GetCollisionArea(brick,out LastCollision);
                if (brick.IsTeleport)
                {
                    var t = PlayArena.LevelMap.BrickMap.Where(b => b.IsTeleport && b != brick).ToList();
                    var br = t.Count == 0 ? brick : t[Arkanoid.Random.Next(t.Count)];
                    Brick ob;
                    switch (cx)
                    {
                        case CollisionPoint.Left:
                            if (PlayArena.LevelMap.GetBrickToRight(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick, cx, dx);
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
                                DeflectBallFromBrick(brick, cx, dx);
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
                                DeflectBallFromBrick(brick, cx, dx);
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
                                DeflectBallFromBrick(brick, cx, dx);
                            }
                            else
                            {
                                Location = new Vector2(br.Center.X, br.Bounds.Top - Height);
                            }
                            break;
                        case CollisionPoint.TopLeft:
                            if (PlayArena.LevelMap.GetBrickToBelowRight(br, out ob))
                            {
                                ob.Die();
                                DeflectBallFromBrick(brick, cx, dx);
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
                                DeflectBallFromBrick(brick, cx, dx);
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
                                DeflectBallFromBrick(brick, cx, dx);
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
                                DeflectBallFromBrick(brick, cx, dx);
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

                if (IsMegaBall && !brick.IsAlive && !brick.IsInvincible) continue;
                DeflectBallFromBrick(brick, cx, dx);
            }
        }

        public void DeflectBallFromBrick(Brick brick, CollisionPoint cx, Direction dx)
        {
            Direction d;
            CollisionPoint c;
            while (Collisions.IsCollision(this, brick, out d, out c))
            {
                switch (d)
                {
                    case Direction.Up:
                        Location += new Vector2(Motion.X*BallSpeed, Math.Abs(Motion.Y*BallSpeed));
                        break;
                    case Direction.Down:
                        Location += new Vector2(Motion.X*BallSpeed, -1*Math.Abs(Motion.Y*BallSpeed));
                        break;
                    case Direction.Left:
                        Location += new Vector2(Math.Abs(Motion.X*BallSpeed), Motion.Y*BallSpeed);
                        break;
                    case Direction.Right:
                        Location += new Vector2(-1*Math.Abs(Motion.X*BallSpeed), Motion.Y*BallSpeed);
                        break;
                    case Direction.DownLeft:
                        Location += new Vector2(Math.Abs(Motion.X*BallSpeed), -1*Math.Abs(Motion.Y*BallSpeed));
                        break;
                    case Direction.DownRight:
                        Location += new Vector2(-1*Math.Abs(Motion.X*BallSpeed), -1*Math.Abs(Motion.Y*BallSpeed));
                        break;
                    case Direction.UpLeft:
                        Location += new Vector2(Math.Abs(Motion.X*BallSpeed), Math.Abs(Motion.Y*BallSpeed));
                        break;
                    case Direction.UpRight:
                        Location += new Vector2(-1*Math.Abs(Motion.X*BallSpeed), Math.Abs(Motion.Y*BallSpeed));
                        break;
                    case Direction.Stop:
                        break;
                }
            }

            DeflectBrick(cx, dx, brick);
        }

        public CollisionPoint GetCollisionArea(Brick brick, out Rectangle area)
        {
            var rx = brick.Bounds;            
            Brick n;
            var b = brick;
            while (PlayArena.LevelMap.GetBrickToLeft(b, out n))
            {
                rx = new Rectangle(n.Bounds.X, n.Bounds.Y, rx.Right - n.Bounds.X, rx.Height);
                b = n;
            }
            b = brick;
            while (PlayArena.LevelMap.GetBrickToRight(b, out n))
            {
                rx = new Rectangle(rx.X, rx.Y,n.Bounds.Right- rx.X, rx.Height);
                b = n;
            }
            b = brick;
            while (PlayArena.LevelMap.GetBrickAbove(b, out n))
            {
                rx = new Rectangle(rx.X, n.Bounds.Y, rx.Width, rx.Bottom - n.Bounds.Y);
                b = n;
            }
            b = brick;
            while (PlayArena.LevelMap.GetBrickBelow(b, out n))
            {
                rx = new Rectangle(rx.X, rx.Y, rx.Width,  n.Bounds.Bottom - rx.Top);
                b = n;
            }
            var rect = new Rectangle(rx.X, rx.Y, rx.Width, rx.Height);
            Direction d;
            CollisionPoint c;
            Collisions.IsCollision(this, rect, out d, out c);
            area = rect;
            return c;
        }

        public void DeflectBrick(CollisionPoint c, Direction d, Brick brick)
        {
            switch (c)
            {
                case CollisionPoint.Right:
                case CollisionPoint.Left:
                    Motion = new Vector2(Motion.X*-1, Motion.Y);
                    break;
                case CollisionPoint.Top:
                case CollisionPoint.Bottom:
                    Motion = new Vector2(Motion.X, Motion.Y*-1);
                    break;
                case CollisionPoint.TopRight:
                    if (d == Direction.DownRight)
                        Motion = new Vector2(Motion.X, Motion.Y*-1);
                    else if (d == Direction.Up || d == Direction.UpLeft || d == Direction.UpRight)
                        Motion = new Vector2(Motion.X*-1, Motion.Y);
                    else
                        Motion = new Vector2(Motion.X*-1, Motion.Y*-1);
                    break;
                case CollisionPoint.TopLeft:
                    if (d == Direction.DownLeft)
                        Motion = new Vector2(Motion.X, Motion.Y*-1);
                    else if (d == Direction.Up || d == Direction.UpLeft || d == Direction.UpRight)
                        Motion = new Vector2(Motion.X*-1, Motion.Y);
                    else
                        Motion = new Vector2(Motion.X*-1, Motion.Y*-1);
                    break;
                case CollisionPoint.BottomRight:
                    if (d == Direction.UpRight)
                        Motion = new Vector2(Motion.X, Motion.Y*-1);
                    else if (d == Direction.Down || d == Direction.DownLeft || d == Direction.DownRight)
                        Motion = new Vector2(Motion.X*-1, Motion.Y);
                    else
                        Motion = new Vector2(Motion.X*-1, Motion.Y*-1);
                    break;
                case CollisionPoint.BottomLeft:
                    if (d == Direction.UpLeft)
                        Motion = new Vector2(Motion.X, Motion.Y*-1);
                    else if (d == Direction.Down || d == Direction.DownLeft || d == Direction.DownRight)
                        Motion = new Vector2(Motion.X*-1, Motion.Y);
                    else
                        Motion = new Vector2(Motion.X*-1, Motion.Y*-1);
                    break;
            }

            Motion =
                new Vector2(
                    Motion.X < 0
                        ? Motion.X - (float) (Arkanoid.Random.NextDouble()/20)
                        : Motion.X + (float) (Arkanoid.Random.NextDouble()/20),
                    Motion.Y < 0
                        ? Motion.Y - (float) (Arkanoid.Random.NextDouble()/20)
                        : Motion.Y + (float) (Arkanoid.Random.NextDouble()/20));
            Motion.Normalize();
        }

        public void Deflect(CollisionPoint c, Direction d)
        {
            switch (c)
            {
                case CollisionPoint.Right:
                case CollisionPoint.Left:
                    Motion = new Vector2(Motion.X*-1, Motion.Y);
                    break;
                case CollisionPoint.Top:
                case CollisionPoint.Bottom:
                    Motion = new Vector2(Motion.X, Motion.Y*-1);
                    break;
                case CollisionPoint.BottomRight:
                case CollisionPoint.BottomLeft:
                case CollisionPoint.TopRight:
                case CollisionPoint.TopLeft:
                    Motion = new Vector2(Motion.X*-1, Motion.Y*-1);
                    break;
            }
        }
    }
}