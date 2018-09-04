using System;
using System.Collections.Generic;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDX.Arena
{

    public class CreditsArena : BaseArena
    {
        public Starfield Starfield;
        public TimeSpan Show;
        public List<String> Credits;
        public TimeSpan LastTouch;

        public CreditsArena(ArkanoidDX game)
            : base(game)
        {
            Show = new TimeSpan(0, 0, 0, 15);
            Starfield = new Starfield(100, new Rectangle(0, 0, Game.Width, Game.Height));
            Credits = new List<string>
                          {
                              "Lead Developer",
                              "Benjamin Tarrant",
                              "I",
                              "Creative Concepts and Levels",
                              "Chloe Tarrant",
                              "Benjamin Tarrant",
                              "I",
                              "Deticated To",
                              "Jansen, Aurora and Ianeta Tarrant"
                          };
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
            var l = new Vector2(Game.Width/2f, 50f);
            batch.Draw(Textures.CmnCredits, l - new Vector2(Textures.CmnCredits.Width / 2f, 0), Color.White);
            l += new Vector2(0, Textures.CmnCredits.Height);
            foreach(var c in Credits)
            {
                var v = Fonts.ArtFontGrey.MeasureString(c);// *.7f;
                if (c != "I")
                {
                    batch.DrawString(Fonts.ArtFontGrey, c, l - new Vector2(v.X / 2, 0), Color.White);//, 0, Vector2.Zero, .7f, SpriteEffects.None, 1);
                    l += new Vector2(0, v.Y);
                }
                else
                {
                    l += new Vector2(0, v.Y / 2);
                }
            }
         //   DrawTitle(batch);
            base.Draw(batch);
        }
    }
}
