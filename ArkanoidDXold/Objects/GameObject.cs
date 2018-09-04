using ArkanoidDX.Arena;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = ArkanoidDX.Graphics.IDrawable;

namespace ArkanoidDX.Objects
{
    public abstract class GameObject : ILocatable, IDrawable
    {

        public abstract bool IsAlive { get; }
        public abstract Sprite Texture { get; }
        public Vector2 Motion { get; set; }
        public ArkanoidDX Game;
        public BaseArena PlayArenaArena { get { return Game.Arena; } }

        protected GameObject(ArkanoidDX game)
        {
            Game = game;
            Motion = Vector2.Zero;
        }
        #region ILocatable Members

        public virtual Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Location.X,
                    (int)Location.Y,
                    (int)Texture.Width,
                    (int)Texture.Height);
            }
        }

        public Vector2 Location { get; set; }

        public Vector2 Center
        {
            get { return new Vector2(Bounds.Center.X, Bounds.Center.Y); }
        }

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

        public virtual float Width
        {
            get { return Bounds.Width; }
        }

        public virtual float Height
        {
            get { return Bounds.Height; }
        }

        #endregion

        

        
        public abstract void Draw(SpriteBatch batch);

    }
}
