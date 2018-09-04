using ArkanoidDX.Arena;
using ArkanoidDX.Audio;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;

namespace ArkanoidDX.Objects
{


    public class OrbSpawnEnemy : Enemy
    {

        public OrbSpawnEnemy(ArkanoidDX game, PlayArena playArena, Sprite texture, Sprite dieTexture, Vector2 location ):base(game,playArena,texture,dieTexture,location)
        {
        }


        public override void Die()
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
                                          PlayArena.AddNewEnimies(new[]{new Enemy(Game, PlayArena, Sprites.EnmOrbRed, Sprites.EnmDieOrbTri, Location, Direction.Left),
                                                new Enemy(Game, PlayArena, Sprites.EnmOrbGreen, Sprites.EnmDieOrbTri, Location, Direction.Right),
                                                new Enemy(Game, PlayArena, Sprites.EnmOrbBlue, Sprites.EnmDieOrbTri, Location)
                                            });
                                          Location = new Vector2(Location.X, Game.Height);
                                          IsExploding = false;
                                          Life = 0;
                                      };
        }        
    }
}