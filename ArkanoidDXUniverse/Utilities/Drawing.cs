using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Utilities
{
    public static class Drawing
    {
        public static void DrawLine(this SpriteBatch batch, Vector2 point1, Vector2 point2, Color color,
            float width = 1f)
        {
            var angle = (float) Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            var length = Vector2.Distance(point1, point2);

            batch.Draw(Arkanoid.Pixel, point1, null, color,
                angle, Vector2.Zero, new Vector2(length, width),
                SpriteEffects.None, 0);
        }

        public static void DrawRectangle(this SpriteBatch batch, Rectangle rect, Color color, float width = 1f)
        {
            batch.DrawLine(new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y), color, width);
            batch.DrawLine(new Vector2(rect.X + rect.Width, rect.Y),
                new Vector2(rect.X + rect.Width, rect.Y + rect.Height), color, width);
            batch.DrawLine(new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
                new Vector2(rect.X, rect.Y + rect.Height), color, width);
            batch.DrawLine(new Vector2(rect.X, rect.Y + rect.Height), new Vector2(rect.X, rect.Y), color, width);
        }

        public static void FillRectangle(this SpriteBatch batch, Rectangle rect, Color color)
        {
            batch.Draw(Arkanoid.Pixel, rect, color);
        }
    }
}