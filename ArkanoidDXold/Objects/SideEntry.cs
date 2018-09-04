using System.Collections.Generic;
using ArkanoidDX.Arena;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = ArkanoidDX.Graphics.IDrawable;

namespace ArkanoidDX.Objects
{
    public class SideEntry : ISpawner, IDrawable
    {
        private readonly Sprite _texture;

        public Sprite Texture
        {
            get { return _texture; }
        }
    
        public bool Flip;
        public ArkanoidDX Game;
        public Vector2 Location;

        public SideEntry(ArkanoidDX game, Vector2 location, bool flip)
        {
            Game = game;
            _texture = Sprites.FrmSideOpen;
            Location = location;
            Flip = flip;
        }

        #region IEnimyEntrycs Members

        public void Spawn(EnemyTypes type, PlayArena playArena, List<Enemy> enimies)
        {
            Texture.SetAnimation(AnimationState.Play);
            Texture.OnFinish = () =>
                                 {
                                     Sprite s = Types.GetEnemySprite(type);
                                     enimies.Add(Types.GetEnemy(type, Game,playArena,
                                                                                       Location +
                                                                                       new Vector2(
                                                                                           Flip
                                                                                               ? -s.Width
                                                                                               : Game.ArenaArea.X,
                                                                                           ((Sprites.FrmSideOpen.Height -
                                                                                             s.Height)/2)),
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

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Location, Color.White, 1, Flip);
        }
    }
}