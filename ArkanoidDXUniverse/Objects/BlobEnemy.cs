using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Objects
{
    public class BlobEnemy : Enemy
    {
        private Brick _lastBrick;
        private bool _lastColVaus;
        private bool _lastColVausTwin;

        public BlobEnemy(Arkanoid game, PlayArena playArena, Sprite texture, Sprite dieTexture, Vector2 location)
            : base(game, playArena, texture, dieTexture, location)
        {
            if (location.X < playArena.Center.X)
            {
                Motion = location.X < PlayArena.TopLeftEntry.Location.X ? new Vector2(1, 0) : new Vector2(-1, 0);
            }
            else
            {
                Motion = location.X > PlayArena.TopRightEntry.Location.X + PlayArena.TopRightEntry.Texture.Width
                    ? new Vector2(-1, 0)
                    : new Vector2(1, 0);
            }
        }

        public override bool IsAlive => Location.Y < Game.Height + Height && Life > 0;


        public override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                CheckEnemyVausCollision();
                CheckEnemyVausTwinCollision();
                CheckEnemyBrickCollision();
                CheckEnemyWallCollision();
                Texture.Update(gameTime);
                Motion = new Vector2(Motion.X, Motion.Y + 0.1f);
                Location += Motion;
            }
        }

        public override void Die()
        {
        }


        public override void CheckEnemyWallCollision()
        {
            if (!IsAlive) return;
            if (X <= PlayArena.X)
            {
                X = PlayArena.X;
                if (Motion.X < 0)
                    Motion = new Vector2(Motion.X*-1, Motion.Y);
                _lastColVaus = false;
                _lastColVausTwin = false;
            }
            else if (X >= PlayArena.X + PlayArena.Width - Width)
            {
                X = PlayArena.X + PlayArena.Width - Width;
                if (Motion.X > 0)
                    Motion = new Vector2(Motion.X*-1, Motion.Y);
                _lastColVaus = false;
                _lastColVausTwin = false;
            }
            else if (Y <= PlayArena.Y)
            {
                Y = PlayArena.Y;
                if (Motion.Y < 0)
                    Motion = new Vector2(Motion.X, Motion.Y*-1);
                _lastColVaus = false;
                _lastColVausTwin = false;
            }
        }

        public override void CheckEnemyVausCollision()
        {
            if (!IsAlive || IsExploding) return;
            Direction d;
            CollisionPoint c;
            if (!Collisions.IsCollision(this, PlayArena.Vaus, out d, out c)) return;
            if (!_lastColVaus)
                Game.Sounds.BallBounce.Play();
            _lastColVaus = true;
            _lastColVausTwin = false;
            var vtop = new Rectangle(PlayArena.Vaus.Bounds.X, PlayArena.Vaus.Bounds.Y,
                PlayArena.Vaus.Bounds.Width, PlayArena.Vaus.Bounds.Height/2);
            var vbottom = new Rectangle(PlayArena.Vaus.Bounds.X,
                PlayArena.Vaus.Bounds.Y + PlayArena.Vaus.Bounds.Height/2,
                PlayArena.Vaus.Bounds.Width, PlayArena.Vaus.Bounds.Height/2);

            if (Collisions.IsCollision(vtop, Bounds))
            {
                PlayArena.Vaus.RemovePower();
                if (c == CollisionPoint.Left || c == CollisionPoint.Right)
                {
                    Motion = new Vector2(Motion.X*-1f, Motion.Y*-.75f);
                }
                else
                {
                    Motion = new Vector2(Motion.X, Motion.Y*-.75f);
                }
            }
            else if (Collisions.IsCollision(vbottom, Bounds))
            {
                PlayArena.Vaus.Die();
                Motion = new Vector2(Motion.X, Motion.Y*-.75f);
            }
        }

        public override void CheckEnemyVausTwinCollision()
        {
            if (!PlayArena.Vaus.IsTwin) return;
            if (!IsAlive || IsExploding) return;
            Direction d;
            CollisionPoint c;
            if (!Collisions.IsCollision(this, PlayArena.Vaus.Twin, out d, out c)) return;
            if (!_lastColVausTwin)
                Game.Sounds.BallBounce.Play();
            _lastColVaus = false;
            _lastColVausTwin = true;
            var vtop = new Rectangle(PlayArena.Vaus.Twin.Bounds.X, PlayArena.Vaus.Twin.Bounds.Y,
                PlayArena.Vaus.Twin.Bounds.Width, PlayArena.Vaus.Twin.Bounds.Height/2);
            var vbottom = new Rectangle(PlayArena.Vaus.Twin.Bounds.X,
                PlayArena.Vaus.Twin.Bounds.Y + PlayArena.Vaus.Twin.Bounds.Height/2,
                PlayArena.Vaus.Twin.Bounds.Width, PlayArena.Vaus.Twin.Bounds.Height/2);

            if (Collisions.IsCollision(vtop, Bounds))
            {
                PlayArena.Vaus.RemovePower();
                if (c == CollisionPoint.Left || c == CollisionPoint.Right)
                {
                    Motion = new Vector2(Motion.X*-1f, Motion.Y*-.75f);
                }
                else
                {
                    Motion = new Vector2(Motion.X, Motion.Y*-.75f);
                }
            }
            else if (Collisions.IsCollision(vbottom, Bounds))
            {
                PlayArena.Vaus.Twin.Die();
                if (c == CollisionPoint.Left || c == CollisionPoint.Right)
                {
                    Motion = new Vector2(Motion.X*-1f, Motion.Y*-.75f);
                }
                else
                {
                    Motion = new Vector2(Motion.X, Motion.Y*-.75f);
                }
            }
        }

        public CollisionPoint GetCollisionArea(Brick brick)
        {
            var rx = brick.Bounds;
            var ry = brick.Bounds;
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

        public override void CheckEnemyBrickCollision()
        {
            foreach (var b in PlayArena.LevelMap.BrickMap)
            {
                if (!b.IsAlive) continue;
                Direction d;
                CollisionPoint c;

                if (!Collisions.IsCollision(this, b, out d, out c,true)) continue;
                _lastColVausTwin = false;
                _lastColVaus = false;
                if (_lastBrick != b)
                {
                    _lastBrick = b;
                    b.Flash();
                    Game.Sounds.BallBounce.Play();
                }
                c = GetCollisionArea(b);
                switch (c)
                {
                    case CollisionPoint.Left:
                        Motion = new Vector2(Motion.X*-1, Motion.Y);
                        Location = new Vector2(b.Location.X - Width, Location.Y);
                        break;
                    case CollisionPoint.Right:
                        Motion = new Vector2(Motion.X*-1, Motion.Y);
                        Location = new Vector2(b.Location.X + b.Width, Location.Y);
                        break;
                    case CollisionPoint.Bottom:
                        Motion = new Vector2(Motion.X, Motion.Y*1f);
                        Location = new Vector2(Location.X, b.Bounds.Bottom);
                        break;
                    case CollisionPoint.Top:
                        Motion = new Vector2(Motion.X, Motion.Y*-.75f);
                        Location = new Vector2(Location.X, b.Bounds.Top - Height);
                        break;
                    case CollisionPoint.TopLeft:
                        if (d == Direction.Left || d == Direction.DownLeft)
                            Motion = new Vector2(Motion.X, Motion.Y*-.75f);
                        else
                            Motion = new Vector2(Motion.X*-1f, Motion.Y*-.75f);
                        Location = new Vector2(Location.X, b.Bounds.Top - Height);
                        break;
                    case CollisionPoint.TopRight:
                        if (d == Direction.Right || d == Direction.DownRight)
                            Motion = new Vector2(Motion.X, Motion.Y*-.75f);
                        else
                            Motion = new Vector2(Motion.X*-1f, Motion.Y*-.75f);
                        Location = new Vector2(Location.X, b.Bounds.Top - Height);
                        break;
                    case CollisionPoint.BottomLeft:
                        if (d == Direction.Left || d == Direction.UpLeft)
                            Motion = new Vector2(Motion.X * -1f, Motion.Y * -0.75f);
                        else
                            Motion = new Vector2(Motion.X * -1f, Motion.Y);
                        Location = new Vector2(Location.X, b.Bounds.Bottom);
                        break;
                    case CollisionPoint.BottomRight:
                        if (d == Direction.Right || d == Direction.UpRight)
                            Motion = new Vector2(Motion.X*-1f, Motion.Y*-0.75f);
                        else
                            Motion = new Vector2(Motion.X * -1f, Motion.Y);
                        Location = new Vector2(Location.X, b.Bounds.Bottom);
                        break;
                    case CollisionPoint.None:
                        break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsExploding && IsAlive)
            {
                spriteBatch.Draw(DieTexture, Location, Color.White*PlayArena.Fade.Fade);
            }
            else if (IsAlive)
            {
                spriteBatch.Draw(Texture, Location, Color.White*PlayArena.Fade.Fade);
            }
        }
    }
}