using System;
using System.Linq;
using Windows.UI.Xaml;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDXUniverse.Arena
{
    public class MenuArena : BaseArena
    {
        public TimeSpan Demo;        
        public int MenuIndex;
        public Texture2D[] MenuItems;
        public int MenuHeight;
        public int Select;
        public bool SelectStart;
        public BackGroundTypes BackGround;
        public MenuArena(Arkanoid game, BackGroundTypes? background = null) : base(game)
        {
            BackGround = background ?? RandomUtils.RandomEnum<BackGroundTypes>();
            Levels.Levels.LoadWads(Game, Game.GraphicsDevice, Game.Content);
            Levels.Levels.UpdateCustom(Game);
            Demo = new TimeSpan(0, 0, 0, 30);
            Game.Sounds.IntroMusic.Play();
            MenuItems = new[] {Textures.CmnStart, Textures.CmnEdit, Textures.CmnCredits,Textures.CmnExit};            
            foreach (var texture in MenuItems)
            {
                MenuHeight += texture.Height;
            }
        }


        public override void Update(GameTime gameTime)
        {                       
            if (Game.KeyboardInput.Any())
                UpdateKeyboard();
            var t = Game.UnifiedInput.VolatileTap;
            if (t != Vector2.Zero) UpdateUnifiedInput(t);
            UpdateScreenSaver(gameTime);
            base.Update(gameTime);
        }

        private void UpdateScreenSaver(GameTime gameTime)
        {
            Demo -= gameTime.ElapsedGameTime;
            if (Demo >= TimeSpan.Zero) return;
            switch (Arkanoid.Random.Next(2))
            {
                case 0:
                    Game.Arena = new BrickArena(Game);
                    break;
                case 1:
                    Game.Arena = new PlayArena(Game,false, null, null, 0, null, null, true);
                    break;
                default:
                    Game.Arena = new PowerUpArena(Game);
                    break;
            }
        }

        private void UpdateUnifiedInput(Vector2 tap)
        {
            Demo = new TimeSpan(0, 0, 0, 25);
            var off = new Vector2(0, Game.ArenaArea.Center.Y - MenuHeight / 2);
            for (var i = 0; i < MenuItems.Length; i++)
            {
                off = new Vector2(Game.ArenaArea.Center.X - MenuItems[i].Width / 2, off.Y);
                var r = new Rectangle((int)off.X, (int)off.Y,
                            MenuItems[i].Width,
                            MenuItems[i].Height);
                if (r.Contains(new Point((int) tap.X, (int) tap.Y)))
                {
                    if (MenuItems[i] == Textures.CmnStart)
                    {
                        Game.Arena = new PlayerSelectArena(Game, BackGround);
                    }
                    else if (MenuItems[i] == Textures.CmnEdit)
                    {
                        //Game.Arena.Fade.DoFadeOut(() =>
                        //{
                            Game.Arena = new EditArena(Game, 1, "Custom");
                            Game.Arena.Fade.DoFadeIn(() => { });
                        //});
                    }
                    else if (MenuItems[i] == Textures.CmnCredits)
                    {
                        Game.Arena = new CreditsArena(Game,BackGround);                            
                    }
                    else if (MenuItems[i] == Textures.CmnExit)
                    {
                        Application.Current.Exit();
                    }
                }
                off = new Vector2(off.X, off.Y + MenuItems[i].Height);
            }            
        }    

        private void UpdateKeyboard()
        {
            Demo = new TimeSpan(0, 0, 0, 25);            
            if (Game.KeyboardInput.TypedKey(Keys.Up))
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
                if (MenuItems[MenuIndex] == Textures.CmnStart)
                {
                    Game.Arena = new PlayerSelectArena(Game, BackGround);
                }
                else if (MenuItems[MenuIndex] == Textures.CmnEdit)
                {
                  //  Game.Arena.Fade.DoFadeOut(() =>
                  //  {
                        Game.Arena = new EditArena(Game, 1, "Custom");
                        Game.Arena.Fade.DoFadeIn(() => { });
                  //  });
                }
                else if(MenuItems[MenuIndex] == Textures.CmnCredits)
                {
                    Game.Arena = new CreditsArena(Game,BackGround);
                }
                else if (MenuItems[MenuIndex] == Textures.CmnExit)
                {
                    Application.Current.Exit();
                }
                Game.Sounds.Menu.Play();
            }            
            else if (Game.KeyboardInput.TypedKey(Keys.B))
            {
                Game.Arena = new BrickArena(Game);
                Game.Sounds.Menu.Play();
            }
            else if (Game.KeyboardInput.TypedKey(Keys.D))
            {                
                Game.Arena = new PlayArena(Game, false, null, null, 0, null, null, true);
                Game.Sounds.Menu.Play();
            }
            else if (Game.KeyboardInput.TypedKey(Keys.P))
            {
                Game.Arena = new PowerUpArena(Game);
                Game.Sounds.Menu.Play();
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