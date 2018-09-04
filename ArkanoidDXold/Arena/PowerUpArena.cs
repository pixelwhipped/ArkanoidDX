using System;
using System.Collections.Generic;
using System.Linq;
using ArkanoidDX.Graphics;
using ArkanoidDX.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDX.Arena
{
    public class PowerUpArena : BaseArena
    {
        public Starfield Starfield;
        public TimeSpan Show;
        public Dictionary<CapsuleTypes, Sprite> Capsules;
        public TimeSpan LastTouch;
        public PowerUpArena(ArkanoidDX game) : base(game)
        {
            Show = new TimeSpan(0, 0, 0, 15);
            Starfield = new Starfield(100, new Rectangle(0,0,Game.Width,Game.Height));
            Capsules = new Dictionary<CapsuleTypes, Sprite>();
            foreach (var c in Enum.GetValues(typeof(CapsuleTypes)).Cast<CapsuleTypes>())
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
            if (Game.KeyboardInput.TypedKey(Keys.Escape) || Show < TimeSpan.Zero || (Game.TouchInput.TouchLocations.Count>0&&LastTouch<TimeSpan.Zero))
            {
                Game.Arena = new MenuArena(Game);
            }
            foreach(var c in Capsules.Values)
            {
                c.Update(gameTime);
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch batch)
        {

            Starfield.Draw(batch);
            var l = new Vector2(50, 100);
            foreach (var c in Capsules.Keys)
            {
                batch.Draw(Capsules[c], l, Color.White);
                batch.DrawString(Fonts.SmallFont, Types.CapsuleDescriptions[c], l + new Vector2(Capsules[CapsuleTypes.Slow].Width + 10,0), Color.White);
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
