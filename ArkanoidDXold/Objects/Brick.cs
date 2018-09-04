using System;
using ArkanoidDX.Arena;
using ArkanoidDX.Audio;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Objects
{
    public class Brick : GameObject
    {
        public Sprite FlashTexture;
        public bool IsInvincible;
        public bool IsRegen;
        public bool IsTeleport;
        public int Life;
        public int Score;
        public int Chance;
        public CapsuleTypes CapsuleType;
        private readonly Sprite _texture;

        public override Sprite Texture
        {
            get { return _texture; }
        }

        public PlayArena PlayArena;
        public TimeSpan Regen;
        public TimeSpan Swap;
        public bool SwapLeft;
        public bool IsSwap;
        public bool IsTransmit;
        public bool IsHorizontalMoving;
        public bool IsActivated;
        

        public Brick(ArkanoidDX game, PlayArena playArena, Sprite texture, Sprite flash, Vector2 location, int score,
                     int life, int chance, CapsuleTypes capsuleType) : base(game)
        {
            PlayArena = playArena;
            _texture = texture;
            FlashTexture = flash;
            Location = location;
            IsInvincible = life == -1 || life == -3 || life == -5 || life == -9;
            IsRegen = life == -2 || life == -7;
            IsTeleport = life == -3;
            IsTransmit = life == -8;
            IsHorizontalMoving = life == -9;
            IsSwap = life == -4 || life == -5 || life == -6;

            Regen = TimeSpan.Zero;
            Swap = IsTransmit ? new TimeSpan(0, 0, 0, ArkanoidDX.Random.Next(2, 6)) : new TimeSpan(0,0,0,0,500);
            Score = score;
            Life = (life == -2 || life == -4) ? 2 : (life == -6 || life == -8) ? 1 : (life == -7) ? 4 : life;
            Chance = chance;
            CapsuleType = capsuleType;
        }

        public override bool IsAlive
        {
            get { return Life > 0 || IsInvincible; }
        }
      
        public void Die()
        {
            Life--;
            Game.Audio.Play(Sounds.BallBounce);            
            if (IsAlive) 
                Flash();
            else
            {
                PlayArena.Vaus.AddScore(Score == -1 ? (PlayArena.LevelSelector.Level + 1) * 50 : Score == -2 ? (PlayArena.LevelSelector.Level + 1) * 75 : Score);
                if (Chance != 0 && Utilities.ChanceIn(Chance))
                {
                    var cap = Types.GetCapsule(CapsuleType, PlayArena, Game, Location);
                    PlayArena.Capsules.Add(cap);

                }
            }
            if (Life == 0 && IsRegen)
                Regen = new TimeSpan(0, 0, 0,15);
        }

        public void Flash()
        {
            FlashTexture.ToStart();
            FlashTexture.SetAnimation(AnimationState.Play);
            FlashTexture.OnFinish = () => FlashTexture.SetAnimation(AnimationState.Stop);
        }

        public void Update(GameTime gameTime)
        {
            if (Life == 0 && IsRegen)
                Regen -= gameTime.ElapsedGameTime;
            if (Life == 0 && IsRegen && Regen < TimeSpan.Zero)
            {
                Life = 2;
                Flash();
            }

            if(IsActivated && IsHorizontalMoving)
            {
                Brick ob;
                if (SwapLeft)
                {
                    if (Location.X <= PlayArena.X)
                    {
                        SwapLeft = false;
                    }
                    else if (PlayArena.LevelMap.GetBrickCollision(this, out ob))
                    {
                            SwapLeft = false;
                    }
                }else
                {
                    if (Location.X + Width >= PlayArena.X + PlayArena.Width)
                    {
                        SwapLeft = true;
                    }
                    else if (PlayArena.LevelMap.GetBrickCollision(this, out ob))
                    {

                            SwapLeft = true;
                    }
                }
                if (SwapLeft )
                {
                    Location = Location - new Vector2(1, 0);
                }else
                {
                    Location = Location + new Vector2(1, 0);
                }

            }

            if (IsAlive && (IsSwap||IsTransmit))
                Swap -= gameTime.ElapsedGameTime;

            if (IsAlive && IsTransmit && Swap < TimeSpan.Zero)
            {
                Swap = new TimeSpan(0, 0, 0, ArkanoidDX.Random.Next(2, 6));
                while(true)
                {
                    float x = ArkanoidDX.Random.Next(PlayArena.LevelMap.BricksWide);
                    float y = ArkanoidDX.Random.Next(PlayArena.LevelMap.BricksHigh);
                    var ob = PlayArena.LevelMap.BrickMap.Find(b => b.Location == Map.GetBrickLocation(Game,(int)x,(int)y));
                    if(ob==null)
                    {
                        Location = Map.GetBrickLocation(Game,(int)x,(int)y) ;
                        break;
                    }
                    if (ob.IsAlive) continue;
                    var l = ob.Location;
                    ob.Location = Location;
                    Location = l;
                    break;
                }
                

            }
            if (IsAlive && IsSwap && Swap < TimeSpan.Zero)
            {
                Brick ob;
                Swap = new TimeSpan(0, 0, 0, 0, 500);
                if (SwapLeft)
                {
                    if( PlayArena.LevelMap.GetBrickToLeft(this, out ob))
                    {
                        if (ob.IsAlive && !ob.IsSwap)
                        {
                            var l = Location;
                            Location = ob.Location;
                            ob.Location = l;
                            ob.Flash();
                        }else
                        {
                            SwapLeft = false;
                        }
                    }else
                    {
                        SwapLeft = false;
                    }
                }else
                {
                    if (PlayArena.LevelMap.GetBrickToRight(this, out ob))
                    {
                        if (ob.IsAlive && !ob.IsSwap)
                        {
                            var l = Location;
                            Location = ob.Location;
                            ob.Location = l;
                            ob.Flash();
                        }
                        else
                        {
                            SwapLeft = true;
                        }
                    }
                    else
                    {
                        SwapLeft = true;
                    }
                }
                Flash();
            }
            Texture.Update(gameTime);
            FlashTexture.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsAlive && !IsInvincible) return;
            spriteBatch.Draw(Texture, Location + Game.Shadow, new Color(0f, 0f, 0f, .5f));
            spriteBatch.Draw(Texture, Location, Color.Lerp(Color.Black, Color.White, Game.Arena.Fade.Fade));
            spriteBatch.Draw(FlashTexture, Location);
        }

        public static Sprite GetBrickTexture(BrickTypes type)
        {
            switch (type)
            {
                case BrickTypes.White:
                    {
                        return Sprites.BrkWhite;
                    }
                case BrickTypes.Yellow:
                    {
                        return Sprites.BrkYellow;
                    }
                case BrickTypes.Pink:
                    {
                        return Sprites.BrkPink;
                    }
                case BrickTypes.Blue:
                    {
                        return Sprites.BrkBlue;
                    }
                case BrickTypes.Red:
                    {
                        return Sprites.BrkRed;
                    }
                case BrickTypes.Green:
                    {
                        return Sprites.BrkGreen;
                    }
                case BrickTypes.SkyBlue:
                    {
                        return Sprites.BrkSkyBlue;
                    }
                case BrickTypes.Orange:
                    {
                        return Sprites.BrkOrange;
                    }
                case BrickTypes.Silver:
                    {
                        return Sprites.BrkSilver;
                    }
                case BrickTypes.Gold:
                    {
                        return Sprites.BrkGold;
                    }
                case BrickTypes.Regen:
                    {
                        return Sprites.BrkRegen;
                    }
                case BrickTypes.Teleport:
                    {
                        return Sprites.BrkTeleport;
                    }
                case BrickTypes.SilverSwap:
                    {
                        return Sprites.BrkSilverSwap;
                    }
                case BrickTypes.GoldSwap:
                    {
                        return Sprites.BrkGoldSwap;
                    }
                case BrickTypes.BlueSwap:
                    {
                        return Sprites.BrkBlueSwap;
                    }
                case BrickTypes.Black:
                    {
                        return Sprites.BrkBlack;
                    }
                case BrickTypes.BlackRegen:
                    {
                        return Sprites.BrkBlackRegen;
                    }
                case BrickTypes.Transmit:
                    {
                        return Sprites.BrkTransmit;
                    }

                default:
                    {
                        return Sprites.BrkWhite;
                    }
            }
        }
    }
}