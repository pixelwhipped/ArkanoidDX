using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using ArkanoidDXUniverse;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDXUniverse.Arena
{
    public class PlayerSelectArena: BaseArena
    {
        public int MenuIndex;
        public Texture2D[] MenuItems;
        public int MenuHeight;
        public BackGroundTypes BackGround;
        public PlayerSelectArena(Arkanoid game, BackGroundTypes backGround) : base(game)
        {
            BackGround = backGround;
            MenuItems = new[] { Textures.Cmn1Player, Textures.Cmn2Player, Textures.CmnBack };
            foreach (var texture in MenuItems)
            {
                MenuHeight += texture.Height;
            }
        }

        private void UpdateKeyboard()
        {
            if (Game.KeyboardInput.TypedKey(Keys.Escape))
            {
                Game.Arena = new MenuArena(Game, BackGround);
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Up))
            {
                MenuIndex--;
                if (MenuIndex < 0) MenuIndex = 0;
                Game.Sounds.Menu.Play();
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Down))
            {
                MenuIndex++;
                if (MenuIndex >= MenuItems.Length - 1) MenuIndex = MenuItems.Length - 1;
                Game.Sounds.Menu.Play();
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Space) || Game.KeyboardInput.TypedKey(Keys.Enter))
            {
                if (MenuItems[MenuIndex] == Textures.Cmn1Player)
                {
                    Game.Arena = new MenuArenaSelector(Game, false, BackGround);
                    Game.Sounds.Menu.Play();
                }
                else if (MenuItems[MenuIndex] == Textures.Cmn2Player)
                {
                    Game.Arena = new MenuArenaSelector(Game, true, BackGround);
                    Game.Sounds.Menu.Play();
                }
                else if (MenuItems[MenuIndex] == Textures.CmnBack)
                {
                    Game.Arena = new MenuArena(Game, BackGround);
                    Game.Sounds.Menu.Play();
                }
                Game.Sounds.Menu.Play();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.KeyboardInput.Any())
                UpdateKeyboard();
            var t = Game.UnifiedInput.VolatileTap;
            if (t != Vector2.Zero) UpdateUnifiedInput(t);            
            base.Update(gameTime);
        }

        private void UpdateUnifiedInput(Vector2 tap)
        {
            var off = new Vector2(0, Game.ArenaArea.Center.Y - MenuHeight / 2);
            for (var i = 0; i < MenuItems.Length; i++)
            {
                off = new Vector2(Game.ArenaArea.Center.X - MenuItems[i].Width / 2, off.Y);
                var r = new Rectangle((int)off.X, (int)off.Y,
                            MenuItems[i].Width,
                            MenuItems[i].Height);
                if (r.Contains(new Point((int)tap.X, (int)tap.Y)))
                {
                    if (MenuItems[i] == Textures.Cmn1Player)
                    {
                        Game.Arena = new MenuArenaSelector(Game, false, BackGround);
                        Game.Sounds.Menu.Play();
                    }
                    else if (MenuItems[i] == Textures.Cmn1Player)
                    {
                        Game.Arena = new MenuArenaSelector(Game, true, BackGround);
                        Game.Sounds.Menu.Play();
                    }
                    else if (MenuItems[i] == Textures.CmnBack)
                    {
                        Game.Arena = new MenuArena(Game, BackGround);
                        Game.Sounds.Menu.Play();
                    }
                }
                off = new Vector2(off.X, off.Y + MenuItems[i].Height);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            DrawBackground(batch, BackGround);
            Arkanoid.Starfield.Draw(batch);

            var off = new Vector2(0, Game.ArenaArea.Center.Y - MenuHeight / 2);
            for (var i = 0; i < MenuItems.Length; i++)
            {
                off = new Vector2(Game.ArenaArea.Center.X - MenuItems[i].Width / 2, off.Y);
                batch.Draw(MenuItems[i], off,
                        i == MenuIndex ? Color.Lerp(Color.White, Color.Blue, FadeRotator) : Color.DarkGray * .5f * Fade.Fade);
                off = new Vector2(off.X, off.Y + MenuItems[i].Height);
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
