using System;
using System.Collections.Generic;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDX.Arena
{

    public class Related : BaseArena
    {
        public Starfield Starfield;
        public TimeSpan Show;
        public List<String> Credits;
        public TimeSpan LastTouch;

        public Related(ArkanoidDX game)
            : base(game)
        {
            Show = new TimeSpan(0, 0, 0, 15);
            Starfield = new Starfield(100, new Rectangle(0, 0, Game.Width, Game.Height));
            
            LastTouch = new TimeSpan(0, 0, 0, 0, 200);
        }

        public override void Update(GameTime gameTime)
        {
            LastTouch -= gameTime.ElapsedGameTime;
            Starfield.Update(gameTime);
            Show -= gameTime.ElapsedGameTime;
            if (Game.KeyboardInput.TypedKey(Keys.Escape) || Show < TimeSpan.Zero || (Game.TouchInput.TouchLocations.Count > 0 && LastTouch < TimeSpan.Zero))
            {
                    Game.Arena = new MenuArena(Game);         
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {

            Starfield.Draw(batch);
            batch.Draw(Textures.CmnADD, new Vector2(Game.Center.X - (Textures.CmnADD.Width / 2f), Game.Center.Y - (Textures.CmnADD.Height / 2f)), Color.White);
         //   DrawTitle(batch);
            base.Draw(batch);
        }
    }
}
