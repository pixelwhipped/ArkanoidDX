using System;
using System.Collections.Generic;
using ArkanoidDX.Graphics;
using ArkanoidDX.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDX.Arena
{

    public class BrickArena : BaseArena
    {
        public Starfield Starfield;
        public TimeSpan Show;
        public Dictionary<BrickTypes, Sprite> Bricks;

        public BrickArena(ArkanoidDX game)
            : base(game)
        {
            Show = new TimeSpan(0, 0, 0, 15);
            Starfield = new Starfield(100, new Rectangle(0, 0, Game.Width, Game.Height));
            Bricks = new Dictionary<BrickTypes, Sprite>
                         {
                             {BrickTypes.White, Brick.GetBrickTexture(BrickTypes.White)},
                             {BrickTypes.Orange, Brick.GetBrickTexture(BrickTypes.Orange)},
                             {BrickTypes.SkyBlue, Brick.GetBrickTexture(BrickTypes.SkyBlue)},
                             {BrickTypes.Green, Brick.GetBrickTexture(BrickTypes.Green)},
                             {BrickTypes.Red, Brick.GetBrickTexture(BrickTypes.Red)},
                             {BrickTypes.Blue, Brick.GetBrickTexture(BrickTypes.Blue)},
                             {BrickTypes.Pink, Brick.GetBrickTexture(BrickTypes.Pink)},
                             {BrickTypes.Yellow, Brick.GetBrickTexture(BrickTypes.Yellow)},
                             {BrickTypes.Silver, Brick.GetBrickTexture(BrickTypes.Silver)},
                             {BrickTypes.Black, Brick.GetBrickTexture(BrickTypes.Black)},
                             {BrickTypes.Gold, Brick.GetBrickTexture(BrickTypes.Gold)},
                             {BrickTypes.Regen, Brick.GetBrickTexture(BrickTypes.Regen)},
                             {BrickTypes.BlackRegen, Brick.GetBrickTexture(BrickTypes.BlackRegen)},
                             {BrickTypes.SilverSwap, Brick.GetBrickTexture(BrickTypes.SilverSwap)},
                             {BrickTypes.GoldSwap, Brick.GetBrickTexture(BrickTypes.GoldSwap)},
                             {BrickTypes.BlueSwap, Brick.GetBrickTexture(BrickTypes.BlueSwap)},
                             {BrickTypes.Teleport, Brick.GetBrickTexture(BrickTypes.Teleport)},
                             {BrickTypes.Transmit, Brick.GetBrickTexture(BrickTypes.Transmit)}
                         };
        }

        public override void Update(GameTime gameTime)
        {
            Starfield.Update(gameTime);
            Show -= gameTime.ElapsedGameTime;
            if (Game.KeyboardInput.TypedKey(Keys.Escape) || Show < TimeSpan.Zero)
            {
                Game.Arena = new MenuArena(Game);
            }
            foreach (var c in Bricks.Values)
            {
                c.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {

            Starfield.Draw(batch);
            var l = new Vector2(50, 100);
            foreach (var c in Bricks.Keys)
            {
                if (c == BrickTypes.Empty) continue;
                batch.Draw(Bricks[c], l, Color.White);
                batch.DrawString(Fonts.SmallFont, Types.BrickDescriptions[c],
                                 l + new Vector2(Bricks[BrickTypes.White].Width + 10, 0), Color.White);
                l += new Vector2(0, Bricks[c].Height + 5);
            }
            //  DrawFrameLeft(batch);
            //  DrawFrameRight(batch);
            //  DrawEntries(batch);
            //  DrawWarps(batch);
            //  DrawShip(batch);
            DrawTitle(batch);
            base.Draw(batch);
        }
    }
}
