using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;
using ArkanoidDX;
using ArkanoidDX.Arena;
using ArkanoidDX.Graphics;
using ArkanoidDX.Levels;
using ArkanoidDX.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Graphics;
using Fonts = ArkanoidDX.Graphics.Fonts;
using Sprite = ArkanoidDX.Graphics.Sprite;
using Sprites = ArkanoidDX.Graphics.Sprites;

namespace ArkanoidDX.Arena
{
    public class EditArena : PlayArena
    {
        public Level EditLevel;
        public Vector2 EditLocation;
        public int LastBrickVal;
        public int LastChanceVal;
        public int LastPowerVal;
        public int EditArkLevel = 0;
        public int EditLevelNumber = 0;

        public bool IsLeft;
        public string EditPath;

        public float Savedfade;

        public Dictionary<CapsuleTypes, Sprite> CapsulesD;

        public StorageFolder Folder;

        private TimeSpan NextTouch = TimeSpan.Zero;
        public EditArena(ArkanoidDX game, int levelnum, string path)
            : base(game, null, null, levelnum)
        {
            EditPath = path;
            IsLeft = path.EndsWith("Left");

            Folder = Utilities.CreateFolderAsync(ApplicationData.Current.RoamingFolder, EditPath);

            CapsulesD = new Dictionary<CapsuleTypes, Sprite>();
            foreach (var c in Enum.GetValues(typeof(CapsuleTypes)).Cast<CapsuleTypes>())
            {
                CapsulesD.Add(c, Capsule.GetCapTexture(c));
            }
            OpenLevel(levelnum);
            Fade.DoFadeIn(()=> { });
            
        }
        
        public void OpenLevel(int level)
        {
            var name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            var found = false;
            try
            {
                foreach (var f in Utilities.GetFilesAsync(Folder))
                {
                    if (!f.Name.EndsWith(".level") || f.Name != name) continue;

                    EditLevel = Utilities.ReadLevel(Utilities.ReadTextFileAsync(f));
                    found = true;
                    break;
                }
            }
            catch (Exception e)
            {
               // Game.Exit();
            }
            EditLevel = (found) ? EditLevel : Level.EmptyLevel(Level.ClassicBricksWide,Level.ClassicBricksHigh);
            EditLevel.KnownEditName = name;
            EditLevelNumber = level;
            SetSelector();
        }

        public void OpenNextLevel()
        {
            var level = EditLevelNumber + 1;        
            var name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            var found = false;
            foreach (var f in Utilities.GetFilesAsync(Folder))
            {
                if (!f.Name.EndsWith(".level") || f.Name != name) continue;

                EditLevel = Utilities.ReadLevel(Utilities.ReadTextFileAsync(f));
                found = true;
                break;
            }
            EditLevel = (found) ? EditLevel : Level.EmptyLevel(Level.ClassicBricksWide,Level.ClassicBricksHigh);
            EditLevel.KnownEditName = name;
            EditLevelNumber = level;
            SetSelector();
        }

        public void OpenPreviousLevel()
        {
            var level = (int)MathHelper.Clamp(EditLevelNumber - 1, 1, EditLevelNumber);
            var name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            var found = false;
            foreach (var f in Utilities.GetFilesAsync(Folder))
            {
                if (!f.Name.EndsWith(".level") || f.Name != name) continue;

                EditLevel = Utilities.ReadLevel(Utilities.ReadTextFileAsync(f));
                found = true;
                break;
            }
            EditLevel = (found) ? EditLevel : Level.EmptyLevel(Level.ClassicBricksWide, Level.ClassicBricksHigh);
            EditLevel.KnownEditName = name;
            EditLevelNumber = level;
            SetSelector();
        }

        public void NewLevel(int w = Level.ClassicBricksWide, int h = Level.ClassicBricksHigh)
        {
            var level = EditLevelNumber + ((w == Level.ClassicBricksWide)?1:0);
            var name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            while (Utilities.DoesFileExistAsync(Folder,name))
            {
                level = level + 1;
                name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            }
            EditLevel = Level.EmptyLevel(w, h);
            EditLevel.KnownEditName = name;
            EditLevelNumber = level;
            SetSelector();
        }
        public void SetSelector()
        {
            var w = new LevelWad(Game, EditPath, null, null,
                     new List<KeyValuePair<Level, Level>> { new KeyValuePair<Level, Level>(EditLevel, EditLevel) });
            LevelSelector = new EditLevelWadSelector(Game, w,EditPath, EditLevelNumber, true);
            LevelSelector.Initialise(this);
        }
        public void SaveLevel()
        {
            Savedfade = 1;
            if (EditLevel.KnownEditName == null)
            {
                EditLevel.KnownEditName = (EditLevelNumber < 10 ? "00" : (EditLevelNumber < 100 ? "0" : "")) + EditLevelNumber + ".level";
            }

            Utilities.WriteLevel(Utilities.CreateFileAsync(Folder, EditLevel.KnownEditName), EditLevel);
            Levels.Levels.UpdateCustom(Game);
        }
        public override void Update(GameTime gameTime)
        {
            Savedfade = MathHelper.Clamp(Savedfade - 0.01f, 0, 1);
            Fade.Update();
            foreach(var c in CapsulesD.Values)
            {
                c.Update(gameTime);
            }
            if (Game.KeyboardInput.TypedKey(Keys.Escape))
                Game.Arena = new MenuArena(Game);
            if (Game.KeyboardInput.TypedKey(Keys.B))
            {
                EditLevel.Background++;
                if (EditLevel.Background > Enum.GetValues(typeof(BackGroundTypes)).Length - 1) EditLevel.Background = 0;
            }
            if (Game.KeyboardInput.TypedKey(Keys.Left))
            {
                EditLocation = new Vector2(EditLocation.X - 1, EditLocation.Y);
            }
            if (Game.KeyboardInput.TypedKey(Keys.Right))
            {
                EditLocation = new Vector2(EditLocation.X + 1, EditLocation.Y);
            }
            if (Game.KeyboardInput.TypedKey(Keys.Down))
            {
                EditLocation = new Vector2(EditLocation.X, EditLocation.Y + 1);
            }
            if (Game.KeyboardInput.TypedKey(Keys.Up))
            {
                EditLocation = new Vector2(EditLocation.X, EditLocation.Y - 1);
            }

            if (Game.KeyboardInput.TypedKey(Keys.T))
            {
                SaveLevel();

                var a = new PlayArena(Game, LevelSelector,null, EditLevelNumber, EditLevel,EditPath);
                a.LevelSelector.Initialise(a);
                Game.Arena = a;
            }
            if (EditLocation.X < 0) EditLocation = new Vector2(0, EditLocation.Y);
            if (EditLocation.X >= EditLevel.BricksWide - 1) EditLocation = new Vector2(EditLevel.BricksWide - 1, EditLocation.Y);
            if (EditLocation.Y < 0) EditLocation = new Vector2(EditLocation.X, 0);
            if (EditLocation.Y >= EditLevel.BricksHigh - 1) EditLocation = new Vector2(EditLocation.X, EditLevel.BricksHigh - 1);

            if (Game.KeyboardInput.TypedKey(Keys.Space))
            {
                var v = EditLevel.GetBrickValue((int)EditLocation.Y, (int)EditLocation.X) + 1;
                if (v >= Enum.GetValues(typeof(BrickTypes)).Length) v = 0;
                LastBrickVal = v;
                EditLevel.SetBrickValue((int)EditLocation.Y, (int)EditLocation.X, v);
            }
            if (Game.KeyboardInput.TypedKey(Keys.Enter))
            {
                EditLevel.SetBrickValue((int)EditLocation.Y, (int)EditLocation.X, LastBrickVal);
            }
            if (Game.KeyboardInput.TypedKey(Keys.E))
            {
                var v = EditLevel.EnemyType + 1;
                if (v >= Enum.GetValues(typeof(EnemyTypes)).Length) v = 0;
                EditLevel.EnemyType = v;
            }
            if (Game.KeyboardInput.TypedKey(Keys.C))
            {
                var v = EditLevel.GetChanceValue((int)EditLocation.Y, (int)EditLocation.X) + 1;
                if (v >= 9) v = 0;
                LastChanceVal = v;
                EditLevel.SetChanceValue((int)EditLocation.Y, (int)EditLocation.X, v);
            }

            if (Game.KeyboardInput.TypedKey(Keys.A))
            {
                int i, j;
                bool found = false;
                for (i = 0;i<EditLevel.BricksWide-1;i++)
                {
                    for(j=0;j<EditLevel.BricksHigh-1;j++)
                    {
                        if(EditLevel.GetBrickValue(j,i)!=10)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                        break;
                }
                if(found)
                {
                    i = ArkanoidDX.Random.Next(EditLevel.BricksWide-1);
                    j = ArkanoidDX.Random.Next(EditLevel.BricksHigh - 1);
                    while(EditLevel.GetBrickValue(j,i)==10)
                    {
                        i = ArkanoidDX.Random.Next(EditLevel.BricksWide - 1);
                        j = ArkanoidDX.Random.Next(EditLevel.BricksHigh - 1);
                    }
                    var a = (int) Utilities.RandomEnum<CapsuleTypes>();
                    EditLevel.SetPowerValue( j,i,a);
                    EditLevel.SetChanceValue(j, i, ArkanoidDX.Random.Next(8));
                }
            }

            if (Game.KeyboardInput.TypedKey(Keys.V))
            {
                EditLevel.SetChanceValue((int)EditLocation.Y, (int)EditLocation.X, LastChanceVal);
            }
            if (Game.KeyboardInput.TypedKey(Keys.P))
            {
                var v = EditLevel.GetPowerValue((int)EditLocation.Y, (int)EditLocation.X) + 1;
                if (v >= Enum.GetValues(typeof(CapsuleTypes)).Length) v = 0;
                LastPowerVal = v;
                EditLevel.SetPowerValue((int)EditLocation.Y, (int)EditLocation.X, v);
            }
            if (Game.KeyboardInput.TypedKey(Keys.O))
            {
                EditLevel.SetPowerValue((int)EditLocation.Y, (int)EditLocation.X, LastPowerVal);
            }

            if (Game.KeyboardInput.TypedKey(Keys.J))
            {
                EditLevel.MinEnimyRealeaseTime++;
                if (EditLevel.MinEnimyRealeaseTime > EditLevel.MaxEnimyRealeaseTime - 1)
                    EditLevel.MinEnimyRealeaseTime = EditLevel.MaxEnimyRealeaseTime - 1;
            }
            if (Game.KeyboardInput.TypedKey(Keys.H))
            {
                EditLevel.MinEnimyRealeaseTime--;
                if (EditLevel.MinEnimyRealeaseTime < 1)
                    EditLevel.MinEnimyRealeaseTime = 1;
            }

            if (Game.KeyboardInput.TypedKey(Keys.K))
            {
                EditLevel.MaxEnimyRealeaseTime--;
                if (EditLevel.MaxEnimyRealeaseTime < EditLevel.MinEnimyRealeaseTime + 1)
                    EditLevel.MaxEnimyRealeaseTime = EditLevel.MinEnimyRealeaseTime + 1;
            }
            if (Game.KeyboardInput.TypedKey(Keys.L))
            {
                EditLevel.MaxEnimyRealeaseTime++;
                if (EditLevel.MaxEnimyRealeaseTime > 60)
                    EditLevel.MaxEnimyRealeaseTime = 60;
            }

            if (Game.KeyboardInput.TypedKey(Keys.F))
            {
                EditLevel.MaxEnimies--;
                if (EditLevel.MaxEnimies < 0)
                    EditLevel.MaxEnimies = 0;
            }
            if (Game.KeyboardInput.TypedKey(Keys.G))
            {
                EditLevel.MaxEnimies++;
            }

            if (Game.KeyboardInput.TypedKey(Keys.S))
            {
                SaveLevel();
            }
            if (Game.KeyboardInput.TypedKey(Keys.N))
            {
                NewLevel();
            }
            if (Game.KeyboardInput.TypedKey(Keys.X))
            {
                NewLevel(Level.TournamentBricksWide,Level.TournamentBricksHigh);
            }
            if (Game.KeyboardInput.TypedKey(Keys.OemPlus))
            {
                OpenNextLevel();
            }
            if (Game.KeyboardInput.TypedKey(Keys.OemMinus))
            {
                OpenPreviousLevel();
            }
            if (Game.KeyboardInput.TypedKey(Keys.NumPad1))
            {
                EditLevel.SideLeftMidEntryEnable = EditLevel.SideLeftMidEntryEnable == 0 ? 1 : 0;
            }
            if (Game.KeyboardInput.TypedKey(Keys.NumPad4))
            {
                EditLevel.SideLeftTopEntryEnable = EditLevel.SideLeftTopEntryEnable == 0 ? 1 : 0;
            }
            if (Game.KeyboardInput.TypedKey(Keys.NumPad7))
            {
                EditLevel.TopLeftEntryEnable = EditLevel.TopLeftEntryEnable == 0 ? 1 : 0;
            }


            if (Game.KeyboardInput.TypedKey(Keys.NumPad3))
            {
                EditLevel.SideRightMidEntryEnable = EditLevel.SideRightMidEntryEnable == 0 ? 1 : 0;
            }
            if (Game.KeyboardInput.TypedKey(Keys.NumPad6))
            {
                EditLevel.SideRightTopEntryEnable = EditLevel.SideRightTopEntryEnable == 0 ? 1 : 0;
            }
            if (Game.KeyboardInput.TypedKey(Keys.NumPad9))
            {
                EditLevel.TopRightEntryEnable = EditLevel.TopRightEntryEnable == 0 ? 1 : 0;
            }

            NextTouch -= gameTime.ElapsedGameTime;
            if (NextTouch < TimeSpan.Zero) NextTouch = TimeSpan.Zero;
            if (NextTouch == TimeSpan.Zero)
            {
                foreach (var t in Game.TouchInput.TouchLocations)
                {
                    NextTouch = new TimeSpan(0, 0, 0, 0, 200);
                    #region Entrances

                    if (new Rectangle((int) TopLeftEntry.Location.X, (int) TopLeftEntry.Location.Y,
                        (int) TopLeftEntry.Texture.Width,
                        (int) TopLeftEntry.Texture.Height).Contains(new Rectangle((int) t.X, (int) t.Y, 1, 1)))
                    {
                        EditLevel.TopLeftEntryEnable = EditLevel.TopLeftEntryEnable == 0 ? 1 : 0;
                    }

                    if (new Rectangle((int) TopRightEntry.Location.X, (int) TopRightEntry.Location.Y,
                        (int) TopRightEntry.Texture.Width,
                        (int) TopRightEntry.Texture.Height).Contains(new Rectangle((int) t.X, (int) t.Y, 1, 1)))
                    {
                        EditLevel.TopRightEntryEnable = EditLevel.TopRightEntryEnable == 0 ? 1 : 0;
                    }

                    if (new Rectangle((int) SideLeftTopEntry.Location.X, (int) SideLeftTopEntry.Location.Y,
                        (int) SideLeftTopEntry.Texture.Width,
                        (int) SideLeftTopEntry.Texture.Height).Contains(new Rectangle((int) t.X, (int) t.Y, 1, 1)))
                    {
                        EditLevel.SideLeftTopEntryEnable = EditLevel.SideLeftTopEntryEnable == 0 ? 1 : 0;
                    }

                    if (new Rectangle((int) SideRightTopEntry.Location.X, (int) SideRightTopEntry.Location.Y,
                        (int) SideRightTopEntry.Texture.Width,
                        (int) SideRightTopEntry.Texture.Height).Contains(new Rectangle((int) t.X, (int) t.Y, 1, 1)))
                    {
                        EditLevel.SideRightTopEntryEnable = EditLevel.SideRightTopEntryEnable == 0 ? 1 : 0;
                    }

                    if (new Rectangle((int) SideLeftMidEntry.Location.X, (int) SideLeftMidEntry.Location.Y,
                        (int) SideLeftMidEntry.Texture.Width,
                        (int) SideLeftMidEntry.Texture.Height).Contains(new Rectangle((int) t.X, (int) t.Y, 1, 1)))
                    {
                        EditLevel.SideLeftMidEntryEnable = EditLevel.SideLeftMidEntryEnable == 0 ? 1 : 0;
                    }

                    if (new Rectangle((int) SideRightMidEntry.Location.X, (int) SideRightMidEntry.Location.Y,
                        (int) SideRightMidEntry.Texture.Width,
                        (int) SideRightMidEntry.Texture.Height).Contains(new Rectangle((int) t.X, (int) t.Y, 1, 1)))
                    {
                        EditLevel.SideRightMidEntryEnable = EditLevel.SideRightMidEntryEnable == 0 ? 1 : 0;
                    }

                    if (EditRect != null)
                    {
                        
                        if (SaveRect.Contains(new Rectangle((int) t.X, (int) t.Y, 1, 1)))
                        {
                            SaveLevel();
                        }
                        if (NewRect.Contains(new Rectangle((int)t.X, (int)t.Y, 1, 1)))
                        {
                            NewLevel();
                        }
                        if (ChangeEnemyRectangle.Contains(new Rectangle((int)t.X, (int)t.Y, 1, 1)))
                        {
                            var v = EditLevel.EnemyType + 1;
                            if (v >= Enum.GetValues(typeof(EnemyTypes)).Length) v = 0;
                            EditLevel.EnemyType = v;
                        }
                     
                    }
                    #endregion

                    #region Enemey

                    EnemyTypes et;
                    if (!Enum.TryParse(Enum.GetNames(typeof (EnemyTypes))[EditLevel.EnemyType], out et))
                        et = EnemyTypes.Orb;
                    if (new Rectangle((int) X, (int) Y,
                        (int) Types.GetEnemySprite(et).Width,
                        (int) Types.GetEnemySprite(et).Height).Contains(new Rectangle((int) t.X, (int) t.Y, 1, 1)))
                    {
                        var v = EditLevel.EnemyType + 1;
                        if (v >= Enum.GetValues(typeof (EnemyTypes)).Length) v = 0;
                        EditLevel.EnemyType = v;
                    }

                    #endregion


                }
            }
        }

        public Rectangle EditRect { get; set; }

        public Rectangle SaveRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X, EditRect.Y, (int)(40 * xMul), (int)(80 * yMul));
            }
        }
        public Rectangle NewRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X+(int)(40*xMul), EditRect.Y, (int)(40 * xMul), (int)(80 * yMul));
            }
        }
        public Rectangle ChangeEnemyRectangle
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(90 * xMul), EditRect.Y, (int)(40 * xMul), (int)(80 * yMul));
            }
        }
        public override void Draw(SpriteBatch batch)
        {
            BackGroundTypes bg;
            if (!Enum.TryParse(Enum.GetNames(typeof(BackGroundTypes))[EditLevel.Background], out bg))
                bg = BackGroundTypes.BlueCircuit;


            DrawBackground(batch, bg);
            DrawFrameLeft(batch);
            DrawFrameRight(batch);

            EnemyTypes et;
            if (!Enum.TryParse(Enum.GetNames(typeof(EnemyTypes))[EditLevel.EnemyType], out et))
                et = EnemyTypes.Orb;
            batch.Draw(Types.GetEnemySprite(et), new Vector2(X, Y), Color.White);

            for (int r = 0; r < EditLevel.BricksHigh; r++)
            {
                for (int c = 0; c < EditLevel.BricksWide; c++)
                {
                    BrickTypes b;
                    if (!Enum.TryParse(Enum.GetNames(typeof(BrickTypes))[EditLevel.GetBrickValue(r, c)], out b))
                        b = BrickTypes.Empty;
                    if (b != BrickTypes.Empty)
                    {
                        var l = new Vector2(X + (c * Sprites.BrkWhite.Width),
                                            Y + (r * Sprites.BrkWhite.Height));
                        batch.Draw(Types.GetBrick(b, Game, this, Vector2.Zero, 0, CapsuleTypes.Slow).Texture,
                                   l, Color.White);
                        batch.DrawString(Fonts.SmallFont, EditLevel.GetChanceValue(r, c) + "", new Vector2(l.X,l.Y-10), Color.Black);
                        CapsuleTypes p;
                        var cname = Enum.GetNames(typeof(CapsuleTypes))[EditLevel.GetPowerValue(r, c)];
                        if (!Enum.TryParse(cname, out p))
                            p = CapsuleTypes.Fast;
                        batch.Draw(CapsulesD[p], l + new Vector2(Sprites.BrkWhite.Width / 2, 0), //.GetCapTexture(p)
                                       Color.White, .5f);
                    }

                }
            }



            batch.DrawRectangle(
                new Rectangle((int)(X + (EditLocation.X * Sprites.BrkWhite.Width) - 3), (int)(Y + (EditLocation.Y * Sprites.BrkWhite.Height)) - 3,
                              (int)Sprites.BrkWhite.Width + 6, (int)Sprites.BrkWhite.Height + 6), Color.Red, 3f);




            var tloc = new Vector2(Game.FrameArea.Width, Y + (EditLevel.BricksHigh * Sprites.BrkWhite.Height));
            batch.DrawString(Fonts.TextFont, "Max Enimies = " + EditLevel.MaxEnimies, tloc, Color.White);
            var strHeigth = Fonts.TextFont.MeasureString("aZj");
            tloc = new Vector2(tloc.X, tloc.Y + strHeigth.Y);
            batch.DrawString(Fonts.TextFont, "Min Enimey RT = " + EditLevel.MinEnimyRealeaseTime, tloc, Color.White);
            tloc = new Vector2(tloc.X, tloc.Y + strHeigth.Y);
            batch.DrawString(Fonts.TextFont, "Max Enimey RT = " + EditLevel.MaxEnimyRealeaseTime, tloc, Color.White);
            tloc = new Vector2(X, Y + (EditLevel.BricksHigh * Sprites.BrkWhite.Height));
            EditRect = new Rectangle((int) tloc.X, (int) tloc.Y, (int) Width, (int) (Height - tloc.Y));

            batch.Draw(Sprites.CmnEditInfo, EditRect, Color.White);

            DrawEntries(batch);
            DrawWarps(batch);
            batch.DrawRectangle(
                new Rectangle((int)TopLeftEntry.Location.X, (int)TopLeftEntry.Location.Y, (int)TopLeftEntry.Texture.Width,
                              (int)TopLeftEntry.Texture.Height), EditLevel.TopLeftEntryEnable == 1 ? Color.Green : Color.Red, 3);
            batch.DrawRectangle(
               new Rectangle((int)TopRightEntry.Location.X, (int)TopRightEntry.Location.Y, (int)TopRightEntry.Texture.Width,
                             (int)TopRightEntry.Texture.Height), EditLevel.TopRightEntryEnable == 1 ? Color.Green : Color.Red, 3);
            batch.DrawRectangle(
               new Rectangle((int)SideLeftTopEntry.Location.X, (int)SideLeftTopEntry.Location.Y, (int)SideLeftTopEntry.Texture.Width,
                             (int)SideLeftTopEntry.Texture.Height), EditLevel.SideLeftTopEntryEnable == 1 ? Color.Green : Color.Red, 3);
            batch.DrawRectangle(
               new Rectangle((int)SideRightTopEntry.Location.X, (int)SideRightTopEntry.Location.Y, (int)SideRightTopEntry.Texture.Width,
                             (int)SideRightTopEntry.Texture.Height), EditLevel.SideRightTopEntryEnable == 1 ? Color.Green : Color.Red, 3);
            batch.DrawRectangle(
               new Rectangle((int)SideLeftMidEntry.Location.X, (int)SideLeftMidEntry.Location.Y, (int)SideLeftMidEntry.Texture.Width,
                             (int)SideLeftMidEntry.Texture.Height), EditLevel.SideLeftMidEntryEnable == 1 ? Color.Green : Color.Red, 3);
            batch.DrawRectangle(
               new Rectangle((int)SideRightMidEntry.Location.X, (int)SideRightMidEntry.Location.Y, (int)SideRightMidEntry.Texture.Width,
                             (int)SideRightMidEntry.Texture.Height), EditLevel.SideRightMidEntryEnable == 1 ? Color.Green : Color.Red, 3);
            DrawTitle(batch);
            batch.Draw(Textures.CmnSaved, Center - new Vector2(Textures.CmnSaved.Width / 2f, Textures.CmnSaved.Height / 2f), Color.White * Savedfade);
           // base.Draw(batch);
        }

        
    }
}
