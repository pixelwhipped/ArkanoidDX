using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using Microsoft.Xna.Framework;

namespace ArkanoidDXUniverse.Objects
{
    public class OrbSpawnEnemy : Enemy
    {
        public OrbSpawnEnemy(Arkanoid game, PlayArena playArena, Sprite texture, Sprite dieTexture, Vector2 location)
            : base(game, playArena, texture, dieTexture, location)
        {
        }


        public override void Die()
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
                PlayArena.AddNewEnimies(new[]
                {
                    new Enemy(Game, PlayArena, Sprites.EnmOrbRed, Sprites.EnmDieOrbTri, Location, Direction.Left),
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