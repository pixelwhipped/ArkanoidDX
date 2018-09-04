using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Arena
{
    public class PauseArena : BaseArena
    {
        public List<string> Credits;
        public TimeSpan LastTouch;
        public TimeSpan Show;
        public Starfield Starfield;

        public PauseArena(Arkanoid game)
            : base(game)
        {
            Starfield = new Starfield(100, new Rectangle(0, 0, (int)Game.Width, (int)Game.Height));
        }

        public override void Update(GameTime gameTime)
        {
            if (Starfield.Bounds != Game.Bounds) Starfield = new Starfield(100, Game.Bounds);
            Starfield.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            Starfield.Draw(batch);
            base.Draw(batch);
        }
    }
}