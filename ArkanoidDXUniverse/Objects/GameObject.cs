using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = ArkanoidDXUniverse.Graphics.IDrawable;

namespace ArkanoidDXUniverse.Objects
{
    public abstract class GameObject : ILocatable, IDrawable
    {
        public Arkanoid Game;

        protected GameObject(Arkanoid game)
        {
            Game = game;
            Motion = Vector2.Zero;
        }

        public abstract bool IsAlive { get; }
        public abstract Sprite Texture { get; }
        public BaseArena PlayArenaArena => Game.Arena;


        public abstract void Draw(SpriteBatch batch);
        public Vector2 Motion { get; set; }

        #region ILocatable Members

        public virtual Rectangle Bounds => new Rectangle(
            (int) Location.X,
            (int) Location.Y,
            (int) Texture.Width,
            (int) Texture.Height);

        public Vector2 Location { get; set; }

        public Vector2 Center => new Vector2(Bounds.Center.X, Bounds.Center.Y);

        public float X
        {
            get { return Location.X; }
            set { Location = new Vector2(value, Location.Y); }
        }

        public float Y
        {
            get { return Location.Y; }
            set { Location = new Vector2(Location.X, value); }
        }

        public virtual float Width => Bounds.Width;

        public virtual float Height => Bounds.Height;

        #endregion
    }
}