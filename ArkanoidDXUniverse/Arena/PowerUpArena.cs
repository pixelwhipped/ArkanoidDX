using System;
using System.Collections.Generic;
using System.Linq;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDXUniverse.Arena
{
    public class PowerUpArena : BaseArena
    {
        public Dictionary<CapsuleTypes, Sprite> Capsules;
        public TimeSpan LastTouch;
        public TimeSpan Show;
        public Starfield Starfield;

        public PowerUpArena(Arkanoid game) : base(game)
        {
            Show = new TimeSpan(0, 0, 0, 15);
            Starfield = new Starfield(100, new Rectangle(0, 0, (int) Game.Width, (int) Game.Height));
            Capsules = new Dictionary<CapsuleTypes, Sprite>();
            foreach (var c in Enum.GetValues(typeof (CapsuleTypes)).Cast<CapsuleTypes>())
            {
                Capsules.Add(c, Capsule.GetCapTexture(c));
            }
            LastTouch = new TimeSpan(0, 0, 0, 0, 200);
        }

        public override void Update(GameTime gameTime)
        {
            Starfield.Update(gameTime);
            LastTouch -= gameTime.ElapsedGameTime;
            Show -= gameTime.ElapsedGameTime;
            if (Game.KeyboardInput.TypedKey(Keys.Escape) || Show < TimeSpan.Zero)
            {
                Game.Arena = new MenuArena(Game);
            }
            foreach (var c in Capsules.Values)
            {
                c.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void OnTap(Vector2 a)
        {
            Show = TimeSpan.Zero;
        }

        public override void Draw(SpriteBatch batch)
        {
            Starfield.Draw(batch);
            var l = new Vector2(50, 100);
            foreach (var c in Capsules.Keys)
            {
                batch.Draw(Capsules[c], l, Color.White);
                batch.DrawString(Fonts.SmallFont, Types.CapsuleDescriptions[c],
                    l + new Vector2(Capsules[CapsuleTypes.Slow].Width + 10, 0), Color.White);
                l += new Vector2(0, Capsules[c].Height + 5);
            }
            //  DrawFrameLeft(batch);
            //  DrawFrameRight(batch);
            //  DrawEntries(batch);
            //  DrawWarps(batch);
            //  DrawShip(batch);
            //DrawTitle(batch);
            base.Draw(batch);
        }
    }
}