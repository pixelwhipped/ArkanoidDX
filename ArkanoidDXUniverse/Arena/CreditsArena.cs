using System;
using System.Collections.Generic;
using ArkanoidDXUniverse.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDXUniverse.Arena
{
    public class CreditsArena : BaseArena
    {
        public List<string> Credits;        
        public TimeSpan Show;
        public BackGroundTypes BackGround;

        public CreditsArena(Arkanoid game, BackGroundTypes backGround)
            : base(game)
        {
            Show = new TimeSpan(0, 0, 0, 15);            
            Credits = new List<string>
            {
                "Lead Developer",
                "Benjamin Tarrant",
                "I",
                "Creative Concepts and Levels",
                "Chloe Tarrant",
                "Jansen Tarrant",
                "Benjamin Tarrant",
                "I",
                "Deticated To",
                "Aurora and Ianeta Tarrant"
            };            
        }

        public override void Update(GameTime gameTime)
        {                        
            Show -= gameTime.ElapsedGameTime;
            if (Game.KeyboardInput.TypedKey(Keys.Escape) || Game.UnifiedInput.VolatileTap != Vector2.Zero)
            {
                Game.Arena = new MenuArena(Game,BackGround);
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            DrawBackground(batch, BackGround);
            Arkanoid.Starfield.Draw(batch);
            var l = new Vector2(Bounds.Width/2f, 50f);
            batch.Draw(Textures.CmnCredits, l - new Vector2(Textures.CmnCredits.Width/2f, 0), Color.White);
            l += new Vector2(0, Textures.CmnCredits.Height);
            foreach (var c in Credits)
            {
                var v = Fonts.ArtFontGrey.MeasureString(c); // *.7f;
                if (c != "I")
                {
                    batch.DrawString(Fonts.ArtFontGrey, c, l - new Vector2(v.X/2, 0), Color.White);
                        //, 0, Vector2.Zero, .7f, SpriteEffects.None, 1);
                    l += new Vector2(0, v.Y);
                }
                else
                {
                    l += new Vector2(0, v.Y/2);
                }
            }
            DrawFrameLeft(batch);
            DrawFrameRight(batch);
            DrawEntries(batch);
            DrawWarps(batch);
            DrawShip(batch);
            DrawTitle(batch);

            base.Draw(batch);
        }
    }
}