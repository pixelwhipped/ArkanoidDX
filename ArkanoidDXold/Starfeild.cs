using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX
{
    /// <summary>

    /// A starfield represented by single-pixel "stars", with parallax-scrolling.

    /// </summary>
    public class Starfield
    {
        readonly Vector2[] _starPositions;
        readonly Vector2[] _starMotions;
        readonly Sprite[] _sprites;
        public Rectangle _bounds;
        readonly float[] _scales;
        public Starfield(int count, Rectangle bounds)
        {
            _bounds = bounds;
            if (count <= 0)
            {

                count = 1;
            }
            _starPositions = new Vector2[count];
            _starMotions = new Vector2[count];
            _sprites = new Sprite[count];
            _scales = new float[count];
            for (int i = 0; i < count; i++)
            {
                _starPositions[i] = new Vector2(bounds.X + ((float)ArkanoidDX.Random.NextDouble() * bounds.Width), bounds.Y + ((float)ArkanoidDX.Random.NextDouble() * bounds.Height));
                _starMotions[i] = new Vector2(1, 1);
                _starMotions[i] = new Vector2((float)ArkanoidDX.Random.NextDouble() * ((_starPositions[i].X < bounds.Center.X) ? -1 : 1), (float)ArkanoidDX.Random.NextDouble() * ((_starPositions[i].Y < bounds.Center.Y) ? -1 : 1));
                _sprites[i] = Types.GetEnemySprite(Utilities.RandomEnum<EnemyTypes>());
                _scales[i] = 0f;
            }
        }


        public void Update(GameTime gameTime)
        {
                for (int i = 0; i < _starPositions.Length; i++)
                {
                    _scales[i] = MathHelper.Clamp(_scales[i] + 0.005f, 0, 1);
                    _sprites[i].Update(gameTime);
                    _starPositions[i] += _starMotions[i];
                    if(_starPositions[i].X>=_bounds.Right-_sprites[i].Width||_starPositions[i].X<=_bounds.X||_starPositions[i].Y>=_bounds.Bottom||_starPositions[i].Y<=_bounds.Y)
                    {
                        _starPositions[i] = new Vector2(_bounds.Center.X, _bounds.Center.Y);
                        _scales[i] = 0f;
                        _sprites[i].ToStart();
                    }
                }

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            for (int i = 0; i < _starPositions.Length; i++)
            {

                spriteBatch.Draw(_sprites[i], _starPositions[i], Color.White * _scales[i], _scales[i]);

            }

        }

    } 

}
