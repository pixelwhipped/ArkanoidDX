using System;
using System.Collections.Generic;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDX.Arena
{

    public class PauseArena : BaseArena
    {
        public Starfield Starfield;
        public TimeSpan Show;
        public List<String> Credits;
        public TimeSpan LastTouch;

        public PauseArena(ArkanoidDX game)
            : base(game)
        {            
            Starfield = new Starfield(100, new Rectangle(0, 0, Game.Width, Game.Height));
            
        }

        public override void Update(GameTime gameTime)
        {
            if (Starfield._bounds != Game.Window) Starfield = new Starfield(100, Game.Window);
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
