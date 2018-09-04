using System;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Objects
{
    enum RublixSpawnState
    {
        Normal, Positioning, Spawning
    }
    public class Rubix : Enemy
    {
        

        private RublixSpawnState SpawnState;
        public Brick TargetBrick { get; set; }
        public Sprite SpawnTexture { get;}
        public Rubix(Arkanoid game, PlayArena playArena, Sprite texture, Sprite dieTexture, Vector2 location,
            Direction direction = Direction.Down) : base(game, playArena, texture, dieTexture, location, direction)
        {
            SpawnState = RublixSpawnState.Normal;
            SpawnTexture = Sprites.EnmRubixSpawn;
        }


        public override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                if (SpawnState == RublixSpawnState.Spawning)
                {
                    SpawnTexture.Update(gameTime);
                }
                else if (SpawnState == RublixSpawnState.Positioning)
                {
                    if (MathHelper.Distance(Location.X, TargetBrick.X) > 2 &&
                        MathHelper.Distance(Location.Y, TargetBrick.Y) > 2)
                    {
                        Location = new Vector2(TargetBrick.X, TargetBrick.Y);
                        SpawnState = RublixSpawnState.Spawning;
                        SpawnTexture.Animation = AnimationState.Play;
                        SpawnTexture.OnFinish = () =>
                        {
                            TargetBrick.Life = TargetBrick.InitialLife;
                            Life = 0;
                        };

                    }
                    else
                    {
                        if(TargetBrick.X>Location.X)Location = new Vector2(Location.X-0.1f,Location.Y);
                        else Location = new Vector2(Location.X + 0.1f, Location.Y);
                        if (TargetBrick.Y > Location.Y) Location = new Vector2(Location.X, Location.Y-0.1f);
                        else Location = new Vector2(Location.X, Location.Y +0.1f);
                    }
                }
                else if (SpawnState == RublixSpawnState.Normal)
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

                        var max = 5;
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
                        ChangeDirection = new TimeSpan(0, 0, Arkanoid.Random.Next(1, max));
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
        }

        public virtual void Die()
        {
            if (IsExploding) return;
            IsExploding = true;
            DieTexture.ToStart();
            Game.Sounds.BallBounce.Play();
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
            ChangeDirection = new TimeSpan(0, 0, Arkanoid.Random.Next(1, max));
            var nd = GetRandomDirection();
            while (nd == Direction)
            {
                nd = GetRandomDirection();
            }
            Direction = nd;
        }


        public virtual void CheckEnemyBrickCollision()
        {
            const int max = 5;
            foreach (var b in PlayArena.LevelMap.BrickMap)
            {
                
                Direction d;
                CollisionPoint c;
                if (!Collisions.IsCollision(this, b, out d, out c)) continue;
                if (!b.IsAlive)
                {
                    TargetBrick = b;
                    SpawnState = RublixSpawnState.Positioning;
                    continue;
                };
                if (Direction == Direction.Left)
                {
                    Direction = Direction.Right;
                    X = b.Bounds.Right;
                }
                else if (Direction == Direction.Right)
                {
                    Direction = Direction.Left;
                    X = b.X - Width;
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
                ChangeDirection = new TimeSpan(0, 0, Arkanoid.Random.Next(1, max));
            }
        }        

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (SpawnState != RublixSpawnState.Spawning && IsAlive)
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
            else
            {
                spriteBatch.Draw(SpawnTexture, Location, Color.White * PlayArena.Fade.Fade);
            }
        }
    }
}
