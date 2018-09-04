using System;
using ArkanoidDX.Arena;
using ArkanoidDX.Audio;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Objects
{


    public class Enemy : GameObject
    {
        
        public TimeSpan ChangeDirection;
        public Sprite DieTexture;
        public Direction Direction;
        public bool IsExploding;
        public int Life;
        private readonly Sprite _texture;
        public override Sprite Texture { get { return _texture; } }
        public TimeSpan TimeToChangeDirection;
        public PlayArena PlayArena;

        public Enemy(ArkanoidDX game, PlayArena playArena, Sprite texture, Sprite dieTexture, Vector2 location,
                     Direction direction = Direction.Down):base(game)
        {
            PlayArena = playArena;
            Location = location;
            _texture = texture;
            DieTexture = dieTexture;
            Life = 1;
            Direction = direction;
            ChangeDirection = new TimeSpan(0, 0, 2);
            TimeToChangeDirection = TimeSpan.Zero;
        }

        public override bool IsAlive
        {
            get { return Location.Y < (Game.Height + Height) && Life > 0; }
        }


        public Direction GetRandomDirection()
        {
            switch(Utilities.RandomEnum<Direction>())
            {
                case Direction.Up:
                    return Direction.Up;
                case Direction.Down:
                    return Direction.Down;
                case Direction.Left:
                    return Direction.Left;
                case Direction.Right:
                    return Direction.Right;
                case Direction.DownLeft:
                    return Direction.Down;
                case Direction.DownRight:
                    return Direction.Down;
                case Direction.UpLeft:
                    return Direction.Left;
                case Direction.UpRight:
                    return Direction.Right;
                default:
                    return Direction.Down;
            }
        }

        public virtual void Update(GameTime gameTime)
        {

            if (IsAlive)
            {
                CheckEnemyVausCollision();
                CheckEnemyVausTwinCollision();
                CheckEnemyBrickCollision();
                CheckEnemyWallCollision();
                Texture.Update(gameTime);
                TimeToChangeDirection += gameTime.ElapsedGameTime;                
                if (TimeToChangeDirection >= ChangeDirection)
                {
                    TimeToChangeDirection = TimeSpan.Zero;
                    Direction = GetRandomDirection();

                    int max = 5;
                    switch (Direction)
                    {
                        case Direction.Down:
                            {
                                max = 8;
                                break;
                            }
                        case Direction.Up:
                            {
                                max = 3;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    ChangeDirection = new TimeSpan(0, 0, ArkanoidDX.Random.Next(1, max));
                }
                switch (Direction)
                {
                    case Direction.Down:
                        {
                            Location = new Vector2(Location.X, Location.Y + 1);
                            break;
                        }
                    case Direction.Up:
                        {
                            Location = new Vector2(Location.X, Location.Y - 1);
                            break;
                        }
                    case Direction.Left:
                        {
                            Location = new Vector2(Location.X - 1, Location.Y);
                            break;
                        }
                    default:
                        {
                            Location = new Vector2(Location.X + 1, Location.Y);
                            break;
                        }
                }
            }
            if (IsExploding)
            {
                DieTexture.Update(gameTime);
            }
        }

        public virtual void Die()
        {
            if (IsExploding) return;
            IsExploding = true;
            DieTexture.ToStart();
            Game.Audio.Play(Sounds.BallBounce);
            PlayArena.Vaus.AddScore(Scoring.Alien);
            DieTexture.SetAnimation(AnimationState.Play);
            DieTexture.OnFinish = () =>
                                      {
                                          DieTexture.SetAnimation(AnimationState.Stop);
                                          Location = new Vector2(Location.X, Game.Height);
                                          IsExploding = false;
                                          Life = 0;
                                      };
        }

        public void Deflect(CollisionPoint c, Direction d)
        {
            const int max = 5;
            ChangeDirection = new TimeSpan(0, 0, ArkanoidDX.Random.Next(1, max));
            var nd = GetRandomDirection();
            while(nd ==Direction)
            {
                nd = GetRandomDirection();
            }
            Direction = nd;
        }

        public virtual void CheckEnemyWallCollision()
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
                Collisions.CheckWallAndDeflect(this, PlayArena, Deflect);

        }

        public virtual void CheckEnemyVausCollision()
        {
            if (!IsAlive || IsExploding) return;
            Direction d;
            CollisionPoint c;
            if (!Collisions.IsCollision(this, PlayArena.Vaus, out d, out c)) return;
            var vtop = new Rectangle(PlayArena.Vaus.Bounds.X, PlayArena.Vaus.Bounds.Y,
                                     PlayArena.Vaus.Bounds.Width, PlayArena.Vaus.Bounds.Height / 2);
            var vbottom = new Rectangle(PlayArena.Vaus.Bounds.X,
                                        PlayArena.Vaus.Bounds.Y + (PlayArena.Vaus.Bounds.Height / 2),
                                        PlayArena.Vaus.Bounds.Width, PlayArena.Vaus.Bounds.Height / 2);

            if (Utilities.IsCollision(vtop, Bounds))
            {
                PlayArena.Vaus.RemovePower();
                Die();
            }
            else if (Utilities.IsCollision(vbottom, Bounds))
            {
                PlayArena.Vaus.Die();
                Die();
            }
        }

        public virtual void CheckEnemyVausTwinCollision()
        {
            if (!PlayArena.Vaus.IsTwin) return;
            if (!IsAlive || IsExploding) return;
            Direction d;
            CollisionPoint c;
            if (!Collisions.IsCollision(this, PlayArena.Vaus.Twin, out d, out c)) return;
            var vtop = new Rectangle(PlayArena.Vaus.Twin.Bounds.X, PlayArena.Vaus.Twin.Bounds.Y,
                                     PlayArena.Vaus.Twin.Bounds.Width, PlayArena.Vaus.Twin.Bounds.Height / 2);
            var vbottom = new Rectangle(PlayArena.Vaus.Twin.Bounds.X,
                                        PlayArena.Vaus.Twin.Bounds.Y + (PlayArena.Vaus.Twin.Bounds.Height / 2),
                                        PlayArena.Vaus.Twin.Bounds.Width, PlayArena.Vaus.Twin.Bounds.Height / 2);

            if (Utilities.IsCollision(vtop, Bounds))
            {
                PlayArena.Vaus.RemovePower();
                Die();
            }
            else if (Utilities.IsCollision(vbottom, Bounds))
            {
                PlayArena.Vaus.Twin.Die();
                Die();
            }
        }

        public virtual void CheckEnemyBrickCollision()
        {
            
            const int max = 5;
            foreach (var b in PlayArena.LevelMap.BrickMap)
            {

                if (!b.IsAlive) continue;
                Direction d;
                CollisionPoint c;
                if (!Collisions.IsCollision(this,b, out d, out c)) continue;
                if(Direction == Direction.Left)
                {
                    Direction = Direction.Right;
                    X = b.Bounds.Right;
                }
                else if (Direction == Direction.Right)
                {
                    Direction = Direction.Left;
                    X = b.X-Width;
                }
                else if (Direction == Direction.Up)
                {
                    Direction = Direction.Down;
                    Y = b.Bounds.Bottom;
                }
                else
                {
                    Direction = Direction.Up;
                    Y = b.Y - Height;
                }
                ChangeDirection = new TimeSpan(0, 0, ArkanoidDX.Random.Next(1, max));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsExploding && IsAlive)
            {
                spriteBatch.Draw(DieTexture, Location, Color.White * PlayArena.Fade.Fade);
            }
            else if (IsAlive)
            {
                spriteBatch.Draw(Texture, Location,Color.White * PlayArena.Fade.Fade);
            }
            
        }

        
    }
}