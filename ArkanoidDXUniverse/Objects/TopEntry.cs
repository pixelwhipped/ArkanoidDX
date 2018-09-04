using System.Collections.Generic;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = ArkanoidDXUniverse.Graphics.IDrawable;

namespace ArkanoidDXUniverse.Objects
{
    public class TopEntry : ISpawner, IDrawable
    {
        public Arkanoid Game;

        public Vector2 Location;

        public TopEntry(Arkanoid game, Vector2 location)
        {
            Game = game;
            Texture = Sprites.FrmTopOpen;
            Location = location;
        }

        public Sprite Texture { get; }
        public BaseArena Arena => Game.Arena;

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Location, Color.White);
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
                        (Sprites.FrmTopOpen.Width -
                         s.Width)/2,
                        Game.ArenaArea.Y),
                    Direction.Down));
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