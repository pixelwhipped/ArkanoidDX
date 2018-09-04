using System;
using ArkanoidDX.Audio;
using ArkanoidDX.Graphics;
using ArkanoidDX.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDX.Arena
{
    public class MenuArena: BaseArena
    {

        public Starfield Starfield;
        public int MenuIndex;
        public Texture2D[] MenuItems;
        public TimeSpan Demo;
        public bool SelectStart;
        public int Select;
        public TimeSpan LastTouch;
        
        public MenuArena(ArkanoidDX game) : base(game)
        {
            Game.Audio.Play(Sounds.IntroMusic);
            MenuItems = new[] { Textures.CmnStart, Textures.CmnEdit, Textures.CmnCredits };//,Textures.CmnLoad Textures.CmnExit };
            Demo = new TimeSpan(0, 0, 0, 30);
            LastTouch = TimeSpan.Zero;

            Starfield = new Starfield(100, Game.ArenaArea);
            Levels.Levels.LoadWads(Game, Game.GraphicsDevice, Game.Content);
            Levels.Levels.UpdateCustom(Game);
        }

        
        public override void Update(GameTime gameTime)
        {
            Starfield.Update(gameTime);
            Demo -= gameTime.ElapsedGameTime;
            LastTouch -= gameTime.ElapsedGameTime;
            if (Game.KeyboardInput.TypedKey(Keys.Escape))
            {
                if (SelectStart)
                {
                    SelectStart = false;
                }
            }
            if (Game.KeyboardInput.TypedKey(Keys.Left) && SelectStart)
            {
                Select = (int)MathHelper.Clamp(Select - 1, 0, Levels.Levels.Wads.Count-1);
                Game.Audio.Play(Sounds.Menu);
            }
            if (Game.KeyboardInput.TypedKey(Keys.Right) && SelectStart)
            {
                Select = (int)MathHelper.Clamp(Select + 1, 0, Levels.Levels.Wads.Count-1);                
                Game.Audio.Play(Sounds.Menu);
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Space) || Game.KeyboardInput.TypedKey(Keys.Enter))
            {
                Game.Audio.Play(Sounds.Menu);
                Demo = new TimeSpan(0, 0, 0, 25);
                if (MenuItems[MenuIndex] == Textures.CmnStart)
                {
                    if (SelectStart)
                    {
                        Game.TouchInput.TapLocations.Clear();

                        Game.Arena = new LoadArena(Game, Levels.Levels.Wads[Select]); //new PlayArena(Game, new LevelWadSelector(Game, Levels.Levels.Wads[Select], 0, true), null, 0);
                        //Game.Arena = new LoadArena(Game, Levels.Levels.Wads[Select]);// new PlayArena(Game, new LevelWadSelector(Game, Levels.Levels.Wads[Select], -1, true), null, 0);
                        //    Game.Arena.Fade.DoFadeIn(() => { });

                    }else
                    {
                        SelectStart = true;
                    }
                }
                else if (MenuItems[MenuIndex] == Textures.CmnCredits)
                {
                    Game.TouchInput.TapLocations.Clear();
                    Game.Arena.Fade.DoFadeOut(() =>
                    {
                        Game.Arena = new CreditsArena(Game);
                        Game.Arena.Fade.DoFadeIn(() => { });
                    });
                    
                }
                else if (MenuItems[MenuIndex] == Textures.CmnEdit)
                {
                    Game.TouchInput.TapLocations.Clear();
                    Game.Arena.Fade.DoFadeOut(() =>
                    {
                        Game.Arena = new EditArena(Game,1,"Custom");
                        Game.Arena.Fade.DoFadeIn(() => { });
                    });
                }
            }
            if(Demo< TimeSpan.Zero)
            {

                        switch(ArkanoidDX.Random.Next(6))
                        {
                            case 0:
                        
                                Game.Arena = new BrickArena(Game);
                   
                                break;
                            case 1:                        
                                Game.Arena = new PlayArena(Game, null, null, 0, null, null,true);
                                break;
                            case 2:
                                Game.Arena = new PowerUpArena(Game);
                                break;
                            case 3:
                                Game.Arena = new CreditsArena(Game);
                                break;
                            case 4:                                                                
                            case 5:                                
                            case 6:
                                Game.Arena = new Related(Game);                                
                                break;
                            default:
                                Game.Arena = new PowerUpArena(Game);
                                break;

                        }
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Up))
            {
                Demo = new TimeSpan(0, 0, 0, 25);
                MenuIndex--;
                if (MenuIndex < 0) MenuIndex = 0;
                Game.Audio.Play(Sounds.Menu);
            }
            else if (Game.KeyboardInput.TypedKey(Keys.Down))
            {
                SelectStart = false;
                Demo = new TimeSpan(0, 0, 0, 25);
                MenuIndex++;
                if (MenuIndex >= MenuItems.Length - 1) MenuIndex = MenuItems.Length - 1;
                Game.Audio.Play(Sounds.Menu);
            }
            else if (Game.KeyboardInput.TypedKey(Keys.B))
            {
                Game.Arena = new BrickArena(Game);
            }
            else if (Game.KeyboardInput.TypedKey(Keys.R))
            {
                Game.Arena = new Related(Game);
            }
            else if (Game.KeyboardInput.TypedKey(Keys.D))
            {
                Game.Arena = new PlayArena(Game, null, null, 0, null, null, true);
            }
            else if (Game.KeyboardInput.TypedKey(Keys.P))
            {
                Game.Arena = new PowerUpArena(Game);
            }

            if(LastTouch<TimeSpan.Zero)
            foreach (var t in Game.TouchInput.TouchLocations)
            {

                LastTouch = new TimeSpan(0, 0, 0, 0,200);
                Demo = new TimeSpan(0, 0, 0, 25);
                var off = new Vector2(0, Game.ArenaArea.Center.Y-(Sprites.CmnArkanoidDxLogoB.Height/2 ));
                for (int i = 0; i < MenuItems.Length; i++)
                {
                    off = new Vector2(Center.X - (MenuItems[i].Width  / 2f),
                            off.Y + MenuItems[i].Height );
                    Rectangle r;
                    if (SelectStart)
                    {
                        if (Levels.Levels.Wads[Select].Title == null)
                        {
                            off = new Vector2(Center.X - (Fonts.ArtFontBlue.MeasureString(Levels.Levels.Wads[Select].Name).X / 2f), off.Y);
                            r = new Rectangle((int) off.X, (int) off.Y,
                                (int) Fonts.ArtFontBlue.MeasureString(Levels.Levels.Wads[Select].Name).X,
                                (int)Fonts.ArtFontBlue.MeasureString(Levels.Levels.Wads[Select].Name).Y);
                        }
                        else
                        {
                            off = new Vector2(Center.X - (Levels.Levels.Wads[Select].Title.Width / 2f), off.Y);
                            r = new Rectangle((int)off.X, (int)off.Y,
                                
                                Levels.Levels.Wads[Select].Title.Width ,
                                Levels.Levels.Wads[Select].Title.Height);
                        }
                        var lr = new Rectangle((int)(off.X + r.Width), (int)off.Y, (int)(Width-(off.X + r.Width)),
                            Textures.CmnRight.Height);
                        var ll = new Rectangle(0, (int)off.Y, (int)off.X,
                            Textures.CmnLeft.Height);
                        if (ll.Contains(new Point((int) t.X, (int) t.Y)))
                        {
                            Select = (int)MathHelper.Clamp(Select - 1, 0, Levels.Levels.Wads.Count - 1);
                            Game.Audio.Play(Sounds.Menu);
                        }
                        else if (lr.Contains(new Point((int) t.X, (int) t.Y)))
                        {
                            Select = (int) MathHelper.Clamp(Select + 1, 0, Levels.Levels.Wads.Count - 1);
                            Game.Audio.Play(Sounds.Menu);
                        }
                        else if (r.Contains(new Point((int) t.X, (int) t.Y)))
                        {
                            Game.TouchInput.TapLocations.Clear();               
                            Game.Arena = new LoadArena(Game, Levels.Levels.Wads[Select]); //new PlayArena(Game, new LevelWadSelector(Game, Levels.Levels.Wads[Select], 0, true), null, 0);
                        }
                    }

                        r = new Rectangle((int)off.X, (int)off.Y, MenuItems[i].Width, MenuItems[i].Height);
                        if (r.Contains(new Point((int) t.X, (int) t.Y)))
                        {
                            if (MenuItems[i] == Textures.CmnCredits)
                            {
                                SelectStart = false;
                                MenuIndex = i;
                                    Game.Arena = new CreditsArena(Game);
                                    //Game.Arena.Fade.DoFadeIn(() => { });
                            }
                            if (MenuItems[i] == Textures.CmnEdit)
                            {
                                SelectStart = false;
                                MenuIndex = i;
                                //Game.Arena.Fade.DoFadeOut(() => Game.Arena.Fade.DoFadeOut(() =>
                               // {
                                    Game.Arena = new EditArena(Game, 1, "Custom");
                                  //  Game.Arena.Fade.DoFadeIn(() => { });
                                //}));
                            }
                            else if (MenuItems[i] == Textures.CmnStart)
                            {
                                MenuIndex = i;
                                SelectStart = true;
                            }
                        }                    
                    
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {

            Starfield.Draw(batch);
            var s = Game.ArenaArea.Width / Sprites.CmnArkanoidDxLogoA.Width;
            batch.Draw(Sprites.CmnArkanoidDxLogoA, new Vector2((Game.FrameArea.Center.X) - (Sprites.CmnArkanoidDxLogoA.Center.X * s), Game.ArenaArea.Y), Color.White * Fade.Fade, s);
            batch.Draw(Sprites.CmnArkanoidDxLogoB, new Vector2((Game.FrameArea.Center.X) - (Sprites.CmnArkanoidDxLogoA.Center.X * s), Game.ArenaArea.Y), Color.White * FadeRotator * Fade.Fade, s);
            var off = new Vector2(0, Game.ArenaArea.Center.Y-(Sprites.CmnArkanoidDxLogoB.Height/2 ));
            for (int i = 0; i < MenuItems.Length ;i++ )
            {
                off = new Vector2(Center.X - (MenuItems[i].Width  / 2f),
                            off.Y + MenuItems[i].Height );
                if (i == 0 && SelectStart)
                {
                    if (Levels.Levels.Wads[Select].Title == null)
                    {
                        off = new Vector2(Center.X - (Fonts.ArtFontBlue.MeasureString(Levels.Levels.Wads[Select].Name).X / 2f), off.Y);
                        batch.DrawString(Fonts.ArtFontBlue, Levels.Levels.Wads[Select].Name, off, Color.Lerp(Color.White, Color.Blue, FadeRotator));
                        batch.Draw(Textures.CmnLeft, off - new Vector2(Textures.CmnLeft.Width, 0),
                                   Color.Lerp(Color.White, Color.Blue, FadeRotator));
                        batch.Draw(Textures.CmnRight, off + new Vector2((Fonts.ArtFontBlue.MeasureString(Levels.Levels.Wads[Select].Name).X), 0),
                               Color.Lerp(Color.White, Color.Blue, FadeRotator * Fade.Fade));
                    }
                    else
                    {
                        off = new Vector2(Center.X - (Levels.Levels.Wads[Select].Title.Width / 2f), off.Y);
                        batch.Draw(Levels.Levels.Wads[Select].Title, off, Color.Lerp(Color.White, Color.Blue, FadeRotator));
                        batch.Draw(Textures.CmnLeft, off - new Vector2(Textures.CmnLeft.Width, 0),
                                   Color.Lerp(Color.White, Color.Blue, FadeRotator));
                        batch.Draw(Textures.CmnRight, off + new Vector2((Levels.Levels.Wads[Select].Title.Width), 0),
                               Color.Lerp(Color.White, Color.Blue, FadeRotator * Fade.Fade));
                    }                        
                }else
                {
                    batch.Draw(MenuItems[i], off,
                               (i == MenuIndex) ? Color.Lerp(Color.White, Color.Blue, FadeRotator) : Color.DarkGray * .5f * Fade.Fade);
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
