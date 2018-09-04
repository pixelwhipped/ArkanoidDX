using System.Collections.Generic;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = ArkanoidDXUniverse.Graphics.IDrawable;

namespace ArkanoidDXUniverse.Objects
{
    public class SideEntry : ISpawner, IDrawable
    {
        public bool Flip;
        public Arkanoid Game;
        public Vector2 Location;

        public SideEntry(Arkanoid game, Vector2 location, bool flip)
        {
            Game = game;
            Texture = Sprites.FrmSideOpen;
            Location = location;
            Flip = flip;
        }

        public Sprite Texture { get; }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Location, Color.White, 1, Flip);
        }

        #region IEnimyEntrycs Members

        public void Spawn(EnemyTypes type, PlayArena playArena, List<Enemy> enimies)
        {
            Texture.SetAnimation(AnimationState.Play);
            Texture.OnFinish = () =>
            {
                var s = Types.GetEnemySprite(type);
                enimies.Add(Types.GetEnemy(type, Game, playArena,
                    Location +
                    new Vector2(
                        Flip
                            ? -s.Width
                            : Game.ArenaArea.X,
                        (Sprites.FrmSideOpen.Height -
                         s.Height)/2),
                    Flip
                        ? Direction.Left
                        : Direction.Right));
                Texture.SetAnimation(AnimationState.Rewind);
                Texture.OnFinish = () => { };
            };
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            Texture.Update(gameTime);
        }
    }
}