using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework;

namespace ArkanoidDXUniverse.Objects
{
    public class UfoGreenEnemy: Enemy
    {
        public TimeSpan TimeToMove;
        public UfoGreenEnemy(Arkanoid game, PlayArena playArena, Sprite texture, Sprite dieTexture, Vector2 location, Direction direction = Direction.Down) : base(game, playArena, texture, dieTexture, location, direction)
        {
            ChangeDirection = new TimeSpan(0, 0, 5);
            TimeToMove = TimeSpan.Zero;
            TimeToChangeDirection = TimeSpan.Zero;
        }

        public override Direction GetRandomDirection()
        {
            return RandomUtils.RandomEnum<Direction>();

        }

        public override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                CheckEnemyVausCollision();
                CheckEnemyVausTwinCollision();
                CheckEnemyBrickCollision();
                CheckEnemyWallCollision();
                Texture.Update(gameTime);
                TimeToChangeDirection += gameTime.ElapsedGameTime;
                TimeToMove -= gameTime.ElapsedGameTime;
                if (TimeToChangeDirection >= ChangeDirection)
                {
                    TimeToChangeDirection = TimeSpan.Zero;
                    TimeToMove = new TimeSpan(0,0,1);
                    Direction = GetRandomDirection();
                    ChangeDirection = new TimeSpan(0, 0, 5);
                }
                if (TimeToMove > TimeSpan.Zero)
                {
                    switch (Direction)
                    {
                        case Direction.Down:
                        {
                            Location = new Vector2(Location.X, Location.Y + 2);
                            break;
                        }
                        case Direction.Up:
                        {
                            Location = new Vector2(Location.X, Location.Y - 2);
                            break;
                        }
                        case Direction.Left:
                        {
                            Location = new Vector2(Location.X - 2, Location.Y);
                            break;
                        }
                        case Direction.Right:
                            Location = new Vector2(Location.X + 2, Location.Y);                            
                            break;
                        case Direction.DownLeft:
                            Location = new Vector2(Location.X - 2, Location.Y + 2);
                            break;
                        case Direction.DownRight:
                            Location = new Vector2(Location.X + 2, Location.Y + 2);
                            break;
                        case Direction.UpLeft:
                            Location = new Vector2(Location.X - 2, Location.Y - 2);
                            break;
                        case Direction.UpRight:
                            Location = new Vector2(Location.X + 2, Location.Y - 2);
                            break;
                        case Direction.Stop:
                            break;
                        default:
                        {
                            Location = new Vector2(Location.X + 2, Location.Y);
                            break;
                        }
                    }
                }
            }
            if (IsExploding)
            {
                DieTexture.Update(gameTime);
            }
        }

        public override void CheckEnemyBrickCollision()
        {
            foreach (var b in PlayArena.LevelMap.BrickMap)
            {
                if (!b.IsAlive) continue;
                Direction d;
                CollisionPoint c;
                if (!Collisions.IsCollision(this, b, out d, out c)) continue;
                Direction=Direction.Stop;
                ChangeDirection = new TimeSpan(0, 0, 1);
            }
        }
    }
}
