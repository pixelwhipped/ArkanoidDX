using System.Collections.Generic;
using ArkanoidDX.Arena;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = ArkanoidDX.Graphics.IDrawable;

namespace ArkanoidDX.Objects
{
    public class TopEntry : ISpawner, IDrawable
    {
        private readonly Sprite _texture;

        public Sprite Texture
        {
            get { return _texture; }
        }
    
        public ArkanoidDX Game;
        public BaseArena Arena { get { return Game.Arena; } }

        public Vector2 Location;

        public TopEntry(ArkanoidDX game, Vector2 location)
        {
            Game = game;
            _texture = Sprites.FrmTopOpen;
            Location = location;
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
                                                                                           ((Sprites.FrmTopOpen.Width -
                                                                                             s.Width)/2),
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

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Location, Color.White);
        }
    }
}