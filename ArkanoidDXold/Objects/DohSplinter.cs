using ArkanoidDX.Arena;
using ArkanoidDX.Audio;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Objects
{
    public class DohSplinter : GameObject
    {

        private readonly Sprite _texture;
        public bool IsExploding;
        public override bool IsAlive { get { return Location.Y < Game.Height; } }
        public Sprite DieTexture;

        public override Sprite Texture
        {
            get { return _texture; }
        }

        public PlayArena PlayArena;

        public DohSplinter(ArkanoidDX game, PlayArena playArena, Vector2 location)
            : base(game)
        {
            PlayArena = playArena;
            _texture = Sprites.EnmDohSplinter;
            DieTexture = Sprites.EnmDieGeomUfo;
            Location = location;

        }

        public void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                CheckSplinterVausCollision();
                CheckEnemyVausTwinCollision();
                CheckSplinterWallCollision();
                CheckSplinterBallCollision();
                Texture.Update(gameTime);
                Location = new Vector2(Location.X + ((PlayArena.Vaus.Center.X < Location.X) ? (-.5f) : (.5f)),
                                       Location.Y + 1f);
            } if (IsExploding)
            {
                DieTexture.Update(gameTime);
            }
        }

        public void CheckSplinterWallCollision()
        {
            if (!IsAlive) return;
            if (X <= PlayArena.X)
            {
                X = PlayArena.X;
            }
            else if (X >= (PlayArena.X + PlayArena.Width) - Width)
            {
                X = (PlayArena.X + PlayArena.Width - Width);
            }
            else if (Y <= PlayArena.Y)
            {
                Y = PlayArena.Y;
            }

        }

        public void CheckSplinterBallCollision()
        {
            foreach (var b in PlayArena.Balls)
            {
                if (!IsAlive || IsExploding) return;
                Direction d;
                CollisionPoint c;
                if (!Collisions.IsCollision(this, b, out d, out c)) return;
                b.DeflectEnemy(c, d);
                Die();
            }
        }
        public void CheckSplinterVausCollision()
        {
            if (!IsAlive || IsExploding) return;
            Direction d;
            CollisionPoint c;
            if (!Collisions.IsCollision(this, PlayArena.Vaus, out d, out c)) return;
            PlayArena.Vaus.Die();
            Die();
        }

        public void CheckEnemyVausTwinCollision()
        {
            if (!PlayArena.Vaus.IsTwin) return;
            if (!IsAlive || IsExploding) return;
            Direction d;
            CollisionPoint c;
            if (!Collisions.IsCollision(this, PlayArena.Vaus.Twin, out d, out c)) return;
            PlayArena.Vaus.Twin.Die();
            Die();
        }
        public void Die()
        {
            if (IsExploding) return;
            IsExploding = true;
            DieTexture.ToStart();
            Game.Audio.Play(Sounds.BallBounce);
            PlayArena.Vaus.AddScore(Scoring.Alien);
            DieTexture.SetAnimation(AnimationState.Play);
            PlayArena.Vaus.AddScore(Scoring.Alien);
            DieTexture.OnFinish = () =>
            {
                Location = new Vector2(Location.X, Game.Height);
                DieTexture.SetAnimation(AnimationState.Stop);
                IsExploding = false;
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsExploding && IsAlive)
            {
                spriteBatch.Draw(DieTexture, Location,Color.White * PlayArena.Fade.Fade);
            }
            else if (IsAlive)
            {
                spriteBatch.Draw(Texture, Location, Color.White * PlayArena.Fade.Fade);
            }

        }
    }
}
