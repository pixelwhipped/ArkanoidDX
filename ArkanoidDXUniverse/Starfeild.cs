using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse
{
    /// <summary>
    ///     A starfield represented by single-pixel "stars", with parallax-scrolling.
    /// </summary>
    public class Starfield
    {
        private readonly float[] _scales;
        private readonly Sprite[] _sprites;
        private readonly Vector2[] _starMotions;
        private readonly Vector2[] _starPositions;
        public Rectangle Bounds;

        public Starfield(int count, Rectangle bounds)
        {
            Bounds = bounds;
            if (count <= 0)
            {
                count = 1;
            }
            _starPositions = new Vector2[count];
            _starMotions = new Vector2[count];
            _sprites = new Sprite[count];
            _scales = new float[count];
            for (var i = 0; i < count; i++)
            {
                _starPositions[i] = new Vector2(bounds.X + (float) Arkanoid.Random.NextDouble()*bounds.Width,
                    bounds.Y + (float) Arkanoid.Random.NextDouble()*bounds.Height);
                _starMotions[i] = new Vector2(1, 1);
                _starMotions[i] =
                    new Vector2((float) Arkanoid.Random.NextDouble()*(_starPositions[i].X < bounds.Center.X ? -1 : 1),
                        (float) Arkanoid.Random.NextDouble()*(_starPositions[i].Y < bounds.Center.Y ? -1 : 1));
                _sprites[i] = Types.GetEnemySprite(RandomUtils.RandomEnum<EnemyTypes>());
                _scales[i] = 0f;
            }
        }


        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < _starPositions.Length; i++)
            {
                _scales[i] = MathHelper.Clamp(_scales[i] + 0.005f, 0, 1);
                _sprites[i].Update(gameTime);
                _starPositions[i] += _starMotions[i];
                if (_starPositions[i].X >= Bounds.Right - _sprites[i].Width || _starPositions[i].X <= Bounds.X ||
                    _starPositions[i].Y >= Bounds.Bottom || _starPositions[i].Y <= Bounds.Y)
                {
                    _starPositions[i] = new Vector2(Bounds.Center.X, Bounds.Center.Y);
                    _scales[i] = 0f;
                    _sprites[i].ToStart();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _starPositions.Length; i++)
            {
                spriteBatch.Draw(_sprites[i], _starPositions[i], Color.White*_scales[i], _scales[i]);
            }
        }
    }
}