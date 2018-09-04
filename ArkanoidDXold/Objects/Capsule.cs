using ArkanoidDX.Arena;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Objects
{
    public class Capsule : GameObject
    {
        public CapsuleTypes CapsuleType;

        private readonly Sprite _texture;
        public override Sprite Texture { get { return _texture; } }
        public override bool IsAlive
        {
            get { return Location.Y < Game.Height; }
        }

        public PlayArena PlayArena;

        public Capsule(CapsuleTypes type, ArkanoidDX game, PlayArena playArena, Vector2 location):base(game)
        {
            PlayArena = playArena;
            Motion = new Vector2(0, 2);
            Location = location;
            CapsuleType = type;
            _texture = GetCapTexture(type);
        }        

        public static Sprite GetCapTexture(CapsuleTypes type)
        {
            switch (type)
            {
                case CapsuleTypes.Catch:
                    {
                        return Sprites.PwrCatchC;
                    }
                case CapsuleTypes.Disrupt:
                    {
                        return Sprites.PwrDisruptD;
                    }
                case CapsuleTypes.Exit:
                    {
                        return Sprites.PwrExitB;
                    }
                case CapsuleTypes.Expand:
                    {
                        return Sprites.PwrExpandE;
                    }
                case CapsuleTypes.Fast:
                    {
                        return Sprites.PwrFastF;
                    }
                case CapsuleTypes.Laser:
                    {
                        return Sprites.PwrLaserL;
                    }
                case CapsuleTypes.Life:
                    {
                        return Sprites.PwrLifeP;
                    }
                case CapsuleTypes.MegaBall:
                    {
                        return Sprites.PwrMegaBallH;
                    }
                case CapsuleTypes.Reduce:
                    {
                        return Sprites.PwrReduceR;
                    }
                case CapsuleTypes.Slow:
                    {
                        return Sprites.PwrSlowS;
                    }
                case CapsuleTypes.Illusion:
                    {
                        return Sprites.PwrIllusionI;
                    }
                case CapsuleTypes.Orbit:
                    {
                        return Sprites.PwrOrbitO;
                    }
                case CapsuleTypes.Twin:
                    {
                        return Sprites.PwrTwinT;
                    }
                case CapsuleTypes.Random:
                    {
                        return Sprites.PwrRandom;
                    }
                case CapsuleTypes.MegaLaser:
                    {
                        return Sprites.PwrMegaLaserM;
                    }
                case CapsuleTypes.ElectricFence:
                {
                    return Sprites.PwrElectricFence;
                }
                default:
                    {
                        return null;
                    }
            }
        }

        public void Update(GameTime gameTime)
        {
            Location += Motion;
            Texture.Update(gameTime);            
        }

        public void Die()
        {
            PlayArena.Vaus.AddPowerUp(CapsuleType);
            Location = new Vector2(0, PlayArena.Height + Texture.Height);
        }
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Location, Color.White);
            
        }
    }
}