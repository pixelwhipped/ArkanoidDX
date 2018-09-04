using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.UI.Xaml.Documents;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Levels;
using ArkanoidDXUniverse.Objects;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDXUniverse.Arena
{
    public class EditArena : PlayArena
    {
        public Dictionary<CapsuleTypes, Sprite> CapsulesD;
        public Dictionary<int,Rectangle> CapsuleRects;
        public Dictionary<BrickTypes, Sprite> Bricks;
        public Dictionary<int,Rectangle> BrickRects;
        public Dictionary<BackGroundTypes, Sprite> BackGrounds;
        public Dictionary<int,Rectangle> BackgroundRects;
        public Dictionary<EnemyTypes, Sprite> EnemyTypesDictionary;
        public Dictionary<int,Rectangle> EnemyRects;
        public Dictionary<int, Rectangle> ChanceRects;

        public int EditArkLevel = 0;
        public Level EditLevel;
        public int EditLevelNumber;
        public Vector2 EditLocation;
        public Vector2 LastBrickLocation = Vector2.Zero;
        public string EditPath;

        public StorageFolder Folder;

        public bool IsLeft;
        public int LastBrickVal;
        public int LastChanceVal;
        public int LastPowerVal;
        public float Savedfade;

        public EditArena(Arkanoid game, int levelnum, string path)
            : base(game, false, null, null, levelnum)
        {
            EditPath = path;
            IsLeft = path.EndsWith("Left");

            Folder = AsyncIO.CreateFolderAsync(ApplicationData.Current.RoamingFolder, EditPath);

            CapsulesD = new Dictionary<CapsuleTypes, Sprite>();
            CapsuleRects = new Dictionary<int, Rectangle>();
            Bricks = new Dictionary<BrickTypes, Sprite>();
            BrickRects = new Dictionary<int, Rectangle>();
            BackGrounds = new Dictionary<BackGroundTypes, Sprite>();
            BackgroundRects = new Dictionary<int, Rectangle>();
            EnemyTypesDictionary = new Dictionary<EnemyTypes, Sprite>();
            EnemyRects = new Dictionary<int,Rectangle>();
            ChanceRects = new Dictionary<int, Rectangle>();
            foreach (var c in Enum.GetValues(typeof (CapsuleTypes)).Cast<CapsuleTypes>())
            {
                CapsulesD.Add(c, Capsule.GetCapTexture(c));
                CapsuleRects.Add((int)c,Rectangle.Empty);
            }

            foreach (var c in Enum.GetValues(typeof(BrickTypes)).Cast<BrickTypes>())
            {
                Bricks.Add(c, Brick.GetBrickTexture(c));
                BrickRects.Add((int)c,Rectangle.Empty);
            }
            foreach (var c in Enum.GetValues(typeof(EnemyTypes)).Cast<EnemyTypes>())
            {                
                EnemyTypesDictionary.Add(c, Types.GetEnemySprite(c));
                EnemyRects.Add((int)c,Rectangle.Empty);
            }
            for (int i = 0; i < 10; i++)
            {
                ChanceRects.Add(i, Rectangle.Empty);
            }
            foreach (var c in Enum.GetValues(typeof(BackGroundTypes)).Cast<BackGroundTypes>())
            {
                BackGrounds.Add(c, Types.GetBackGround(c));
                BackgroundRects.Add((int)c,Rectangle.Empty);
            }

            OpenLevel(levelnum);            
            Fade.DoFadeIn(() => { });
        }        



        public override void Update(GameTime gameTime)
        {
            Savedfade = MathHelper.Clamp(Savedfade - 0.01f, 0, 1);
            Fade.Update();
            foreach (var c in CapsulesD.Values)
            {
                c.Update(gameTime);
            }
            foreach (var c in EnemyTypesDictionary.Values)
            {
                c.Update(gameTime);
            }
            foreach (var c in Bricks.Values)
            {
                c.Update(gameTime);
            }

            if (Game.KeyboardInput.Any())
                UpdateKeyboard();
            var tap = Game.UnifiedInput.VolatileTap;
            if (tap != Vector2.Zero) UpdateUnifiedInput(tap);                       
        }

        public Rectangle ChangeEnemyViewRect
        {
            get
            {
                EnemyTypes et;
                if (!Enum.TryParse(Enum.GetNames(typeof(EnemyTypes))[EditLevel.EnemyType], out et))
                    et = EnemyTypes.Orb;
                var t = Types.GetEnemySprite(et);
                return new Rectangle((int)X, (int)Y, (int)t.Width / 2, (int)t.Height / 2);
            }
        }
        public Rectangle EditRect
        {
            get
            {
                
                var b = (float)(Bounds.Height - (Sprites.BrkWhite.Height*EditLevel.BricksHigh));
                b = b/Textures.CmnEditInfo.Height;
                return new Rectangle((int)(Bounds.Center.X - (Textures.CmnEditInfo.Width*b) / 2), (int)(Bounds.Bottom - (Textures.CmnEditInfo.Height*b)), (int)(Textures.CmnEditInfo.Width * b), (int)(Textures.CmnEditInfo.Height *b));
            }
        } 
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
                return new Rectangle(EditRect.X + (int)(40 * xMul), EditRect.Y, (int)(40 * xMul), (int)(80 * yMul));
            }
        }
        public Rectangle ChangeEnemyRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(90 * xMul), EditRect.Y, (int)(40 * xMul), (int)(80 * yMul));
            }
        }
        public Rectangle EnableTopLeftEnemyRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(147 * xMul), EditRect.Y +(int)(39 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnableSideTopLeftEnemyRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(145 * xMul), EditRect.Y + +(int)(83 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnableSideMidLeftEnemyRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(145 * xMul), EditRect.Y + +(int)(128 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnableTopRightEnemyRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(182 * xMul), EditRect.Y + (int)(39 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnableSideTopRightEnemyRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(182 * xMul), EditRect.Y + +(int)(83 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnableSideMidRightEnemyRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(182 * xMul), EditRect.Y + +(int)(128 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle TopLeftEntryRect
        {
            get
            {
                return
                    new Rectangle((int) TopLeftEntry.Location.X, (int) TopLeftEntry.Location.Y,
                        (int) TopLeftEntry.Texture.Width, (int) TopLeftEntry.Texture.Height);
            }
        }
        public Rectangle TopRightEntryRect
        {
            get
            {
                return new Rectangle((int) TopRightEntry.Location.X, (int) TopRightEntry.Location.Y,
                    (int) TopRightEntry.Texture.Width,
                    (int) TopRightEntry.Texture.Height);
            }
        }
        public Rectangle SideLeftTopEntryRect
        {
            get
            {
                return new Rectangle((int) SideLeftTopEntry.Location.X, (int) SideLeftTopEntry.Location.Y,
                    (int) SideLeftTopEntry.Texture.Width,
                    (int) SideLeftTopEntry.Texture.Height);
            }
        }
        public Rectangle SideRightTopEntryRect
        {
            get
            {
                return new Rectangle((int)SideRightTopEntry.Location.X, (int)SideRightTopEntry.Location.Y,
                    (int)SideRightTopEntry.Texture.Width,
                    (int)SideRightTopEntry.Texture.Height);
            }
        }
        public Rectangle SideLeftMidEntryRect
        {
            get
            {
                return new Rectangle((int)SideLeftMidEntry.Location.X, (int)SideLeftMidEntry.Location.Y,
                    (int)SideLeftMidEntry.Texture.Width,
                    (int)SideLeftMidEntry.Texture.Height);
            }
        }
        public Rectangle SideRightMidEntryRect
        {
            get
            {
                return new Rectangle((int)SideRightMidEntry.Location.X, (int)SideRightMidEntry.Location.Y,
                    (int)SideRightMidEntry.Texture.Width,
                    (int)SideRightMidEntry.Texture.Height);
            }
        }
        public Rectangle MoveLeftRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(12 * xMul), EditRect.Y + +(int)(128 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle MoveRightRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(87 * xMul), EditRect.Y + +(int)(128 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle MoveUpRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(49 * xMul), EditRect.Y +(int)(84 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle MoveDownRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(49 * xMul), EditRect.Y + (int)(128 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle TestRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(13 * xMul), EditRect.Y + (int)(179 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle SpaceButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(54 * xMul), EditRect.Y + (int)(176 * xMul), (int)(125 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle TournamentButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(26 * xMul), EditRect.Y + (int)(233 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnterButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(186 * xMul), EditRect.Y + (int)(51 * xMul), (int)(85 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle AddPowerUpButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(100 * xMul), EditRect.Y + (int)(233 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle RemoveBrickButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(153 * xMul), EditRect.Y + (int)(233 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle ChangeCapsuleButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(214 * xMul), EditRect.Y + (int)(16 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle UseCapsuleButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(275 * xMul), EditRect.Y + (int)(16 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle ChangeChanceButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(214 * xMul), EditRect.Y + (int)(40 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle UseChanceButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(275 * xMul), EditRect.Y + (int)(40 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnemyMinReleaseDecButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(214 * xMul), EditRect.Y + (int)(98 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnemyMinReleaseIncButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(275 * xMul), EditRect.Y + (int)(98 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnemyMaxReleaseDecButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(214 * xMul), EditRect.Y + (int)(139 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnemyMaxReleaseIncButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(275 * xMul), EditRect.Y + (int)(139 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnemyMaxDecButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(214 * xMul), EditRect.Y + (int)(180 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle EnemyMaxIncButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(275 * xMul), EditRect.Y + (int)(180 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle LevelDecButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(214 * xMul), EditRect.Y + (int)(221 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }
        public Rectangle LevelIncButtonRect
        {
            get
            {
                var xMul = EditRect.Width / 432f;
                var yMul = EditRect.Height / 296f;
                return new Rectangle(EditRect.X + (int)(275 * xMul), EditRect.Y + (int)(221 * xMul), (int)(35 * xMul), (int)(43 * yMul));
            }
        }



        public Rectangle EnemyMinReleaseDecTextButtonRect { get; set; }
        public Rectangle EnemyMinReleaseIncTextButtonRect { get; set; }
        public Rectangle EnemyMaxReleaseDecTextButtonRect { get; set; }
        public Rectangle EnemyMaxReleaseIncTextButtonRect { get; set; }
        public Rectangle EnemyMaxDecButtonTextRect { get; set; }
        public Rectangle EnemyMaxIncButtonTextRect { get; set; }
        public Rectangle LevelDecButtonTextRect { get; set; }
        public Rectangle LevelIncButtonTextRect { get; set; }

        public Rectangle SaveTextRect { get; set; }
        public Rectangle NewTextRect { get; set; }
        public Rectangle TestTextRect { get; set; }

        public Rectangle ViewTextRect { get; set; }
        public Rectangle BackTextRect { get; set; }

        public bool viewMode = false;

        private void UpdateUnifiedInput(Vector2 tap)
        {            
            if (TopLeftEntryRect.Contains(tap) || EnableTopLeftEnemyRect.Contains(tap)) ChangeTopLeftEntry();
            if (TopRightEntryRect.Contains(tap) || EnableTopRightEnemyRect.Contains(tap)) ChangeTopRightEntry();
            if (SideLeftTopEntryRect.Contains(tap) || EnableSideTopLeftEnemyRect.Contains(tap)) ChangeSideLeftTopEntry();
            if (SideRightTopEntryRect.Contains(tap) || EnableSideTopRightEnemyRect.Contains(tap)) ChangeSideRightTopEntry();
            if (SideLeftMidEntryRect.Contains(tap) || EnableSideMidLeftEnemyRect.Contains(tap)) ChangeSideLeftMidEntry();
            if (SideRightMidEntryRect.Contains(tap) || EnableSideMidRightEnemyRect.Contains(tap)) ChangeSideRightMidEntry();
            if (SaveRect.Contains(tap) || SaveTextRect.Contains(tap)) SaveLevel();
            if (NewRect.Contains(tap) || NewTextRect.Contains(tap)) NewLevel();
            if (ViewTextRect.Contains(tap)) viewMode = !viewMode;
            if (ChangeEnemyRect.Contains(tap) || ChangeEnemyViewRect.Contains(tap)) ChangeEnemy();
            if (MoveUpRect.Contains(tap))
            {
                EditLocation = new Vector2(EditLocation.X, EditLocation.Y - 1);
                ConstrainEditLocation();
            }
            if (MoveDownRect.Contains(tap))
            {
                EditLocation = new Vector2(EditLocation.X, EditLocation.Y + 1);
                ConstrainEditLocation();
            }
            if (MoveLeftRect.Contains(tap))
            {
                EditLocation = new Vector2(EditLocation.X -1, EditLocation.Y);
                ConstrainEditLocation();
            }
            if (MoveRightRect.Contains(tap))
            {
                EditLocation = new Vector2(EditLocation.X +1, EditLocation.Y);
                ConstrainEditLocation();
            }
            if (TestRect.Contains(tap) || TestTextRect.Contains(tap)) Test();
            if (BackTextRect.Contains(tap)) Exit();
            if (TournamentButtonRect.Contains(tap)) NewTournamentLevel();
            if(SpaceButtonRect.Contains(tap)|| EnterButtonRect.Contains(tap)) SetBrick();
            if(AddPowerUpButtonRect.Contains(tap)) AddRandomPowerUp();
            if(RemoveBrickButtonRect.Contains(tap)) ClearBrick();
            if(ChangeCapsuleButtonRect.Contains(tap)) ChangePower();
            if (UseCapsuleButtonRect.Contains(tap)) SetPower();
            if (ChangeChanceButtonRect.Contains(tap)) ChangeChance();
            if (UseChanceButtonRect.Contains(tap)) SetChance();            
            if(EnemyMinReleaseDecButtonRect.Contains(tap)||EnemyMinReleaseDecTextButtonRect.Contains(tap)) MinEnemyReleaseTimeDec();
            if (EnemyMinReleaseIncButtonRect.Contains(tap)||EnemyMinReleaseIncTextButtonRect.Contains(tap)) MinEnemyReleaseTimeInc();
            if (EnemyMaxReleaseDecButtonRect.Contains(tap)||EnemyMaxReleaseDecTextButtonRect.Contains(tap)) MaxEnemyReleaseTimeDec();
            if (EnemyMaxReleaseIncButtonRect.Contains(tap)||EnemyMaxReleaseIncTextButtonRect.Contains(tap)) MaxEnemyReleaseTimeInc();
            if (EnemyMaxDecButtonRect.Contains(tap)||EnemyMaxDecButtonTextRect.Contains(tap)) MaxEnemiesDec();
            if (EnemyMaxIncButtonRect.Contains(tap)||EnemyMaxIncButtonTextRect.Contains(tap)) MaxEnemiesInc();
            if (LevelIncButtonRect.Contains(tap)||LevelIncButtonTextRect.Contains(tap)) OpenNextLevel();
            if (LevelDecButtonRect.Contains(tap) ||LevelDecButtonTextRect.Contains(tap)) OpenPreviousLevel();

            foreach (var c in CapsuleRects)
            {
                if (c.Value.Contains(tap))
                {
                    if (LastPowerVal == c.Key)
                    {
                        ChangeChance();
                    }
                    LastPowerVal = c.Key;
                    if (LastBrickVal != 10)
                    {
                        if (LastChanceVal == 0) LastChanceVal = 1;
                         SetPower();
                    }
                }
            }
            foreach (var c in BrickRects)
            {
                if (c.Value.Contains(tap))
                {
                    LastBrickVal = c.Key;
                    EditLevel.SetBrickValue((int)EditLocation.Y, (int)EditLocation.X, LastBrickVal);
                }
            }
            foreach (var c in EnemyRects)
            {
                if (c.Value.Contains(tap))
                {
                    EditLevel.EnemyType = c.Key;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                if(ChanceRects[i].Contains(tap))
                {
                    LastChanceVal = i;
                    SetChance();
                }
            }
            foreach (var c in BackgroundRects)
            {
                if (c.Value.Contains(tap))
                {
                    EditLevel.Background = c.Key;
                }
            }
            CheckBrickTaps(tap);
        }

        private void CheckBrickTaps(Vector2 tap)
        {
            for (var r = 0; r < EditLevel.BricksHigh; r++)
            {
                for (var c = 0; c < EditLevel.BricksWide; c++)
                {

                        var rect = new Rectangle((int)(X + c * Sprites.BrkWhite.Width),
                            (int)(Y + r * Sprites.BrkWhite.Height), (int)Sprites.BrkWhite.Width, (int)Sprites.BrkWhite.Height);
                    if (rect.Contains(tap))
                    {
                        EditLocation = new Vector2(c,r);
                        SetBrick();
                    }
                }
            }
        }


        private void UpdateKeyboard()
        {
            
            Game.Sounds.Menu.Play();
            foreach (var k in Game.KeyboardInput.TypedKeys)
            {
                switch (k)
                {
                    case Keys.Escape : 
                        Exit();
                        break;
                    case Keys.Left:
                        EditLocation = new Vector2(EditLocation.X - 1, EditLocation.Y);
                        ConstrainEditLocation();
                        break;
                    case Keys.Right:
                        EditLocation = new Vector2(EditLocation.X + 1, EditLocation.Y);
                        ConstrainEditLocation();
                        break;
                    case Keys.Up:
                        EditLocation = new Vector2(EditLocation.X, EditLocation.Y - 1);
                        ConstrainEditLocation();
                        break;
                    case Keys.Down:
                        EditLocation = new Vector2(EditLocation.X, EditLocation.Y + 1);
                        ConstrainEditLocation();
                        break;
                    case Keys.T:
                        Test();
                        break;
                    case Keys.B:
                        ChangeBackground();
                        break;
                   case Keys.Enter:
                   case Keys.Space:
                        SetBrick();
                        break;
                    case Keys.E:
                        ChangeEnemy();
                        break;
                    case Keys.C:
                        ChangeChance();
                        break;
                    case Keys.V:
                        SetChance();
                        break;
                    case Keys.A:
                        AddRandomPowerUp();
                        break;
                    case Keys.P:
                        ChangePower();
                        break;
                    case Keys.O:
                        SetPower();
                        break;
                    case Keys.J:                    
                        MinEnemyReleaseTimeInc();
                        break;
                    case Keys.H:
                        MinEnemyReleaseTimeDec();
                        break;
                    case Keys.L:
                        MaxEnemyReleaseTimeInc();
                        break;
                    case Keys.K:
                        MaxEnemyReleaseTimeDec();
                        break;
                    case Keys.F:
                        MaxEnemiesDec();
                        break;
                    case Keys.G:
                        MaxEnemiesInc();
                        break;
                    case Keys.N:
                        NewLevel();
                        break;
                    case Keys.X:
                        NewTournamentLevel();
                        break;
                    case Keys.S:
                        SaveLevel();
                        break;
                    case Keys.OemPlus:
                        OpenNextLevel();
                        break;
                    case Keys.OemMinus:
                        OpenPreviousLevel();
                        break;
                    case Keys.NumPad7:
                        ChangeTopLeftEntry();
                        break;
                    case Keys.NumPad9:
                        ChangeTopRightEntry();
                        break;
                    case Keys.NumPad6:
                        ChangeSideRightTopEntry();
                        break;
                    case Keys.NumPad4:
                        ChangeSideLeftTopEntry();
                        break;
                    case Keys.NumPad3:
                        ChangeSideRightMidEntry();
                        break;
                    case Keys.NumPad1:
                        ChangeSideLeftMidEntry();
                        break;
                    case Keys.Delete:
                        ClearBrick();
                        break;
                    default:
                        break;
                }
            }                    
        }

        public void SetSelector()
        {
            var w = new LevelWad(Game, EditPath, null, null,
                new List<KeyValuePair<Level, Level>> { new KeyValuePair<Level, Level>(EditLevel, EditLevel) });
            LevelSelector = new EditLevelWadSelector(Game, w, EditPath, EditLevelNumber, true);
            LevelSelector.Initialise(this);
        }
        public void OpenLevel(int level)
        {
            var name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            var found = false;
            try
            {
                foreach (var f in AsyncIO.GetFilesAsync(Folder))
                {
                    if (!f.Name.EndsWith(".level") || f.Name != name) continue;

                    EditLevel = LevelIO.ReadLevel(AsyncIO.ReadTextFileAsync(f));
                    found = true;
                    break;
                }
            }
            catch (Exception)
            {
                // Game.Exit();
            }
            EditLevel = found ? EditLevel : Level.EmptyLevel(Level.ClassicBricksWide, Level.ClassicBricksHigh);
            EditLevel.KnownEditName = name;
            EditLevelNumber = level;
            SetSelector();
        }

        #region Actions
        public void SaveLevel()
        {
            Savedfade = 1;
            if (EditLevel.KnownEditName == null)
            {
                EditLevel.KnownEditName = (EditLevelNumber < 10 ? "00" : (EditLevelNumber < 100 ? "0" : "")) +
                                          EditLevelNumber + ".level";
            }

            LevelIO.WriteLevel(AsyncIO.CreateFileAsync(Folder, EditLevel.KnownEditName), EditLevel);
            Levels.Levels.UpdateCustom(Game);
        }

        public void OpenNextLevel()
        {
            var level = EditLevelNumber + 1;
            var name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            var found = false;
            foreach (var f in AsyncIO.GetFilesAsync(Folder))
            {
                if (!f.Name.EndsWith(".level") || f.Name != name) continue;
                SaveLevel();
                EditLevel = LevelIO.ReadLevel(AsyncIO.ReadTextFileAsync(f));
                found = true;
                break;
            }
            if (found)
            {
                //EditLevel = Level.EmptyLevel(Level.ClassicBricksWide, Level.ClassicBricksHigh);
                //EditLevel.KnownEditName = name;
                EditLevelNumber = level;
                SetSelector();
            }
        }

        public void OpenPreviousLevel()
        {
            var level = MathHelper.Clamp(EditLevelNumber - 1, 1, EditLevelNumber);
            var name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            var found = false;
            foreach (var f in AsyncIO.GetFilesAsync(Folder))
            {
                if (!f.Name.EndsWith(".level") || f.Name != name) continue;
                SaveLevel();
                EditLevel = LevelIO.ReadLevel(AsyncIO.ReadTextFileAsync(f));
                found = true;
                break;
            }
            if (found)
            {
                //EditLevel = found ? EditLevel : Level.EmptyLevel(Level.ClassicBricksWide, Level.ClassicBricksHigh);
                //EditLevel.KnownEditName = name;
                EditLevelNumber = level;
                SetSelector();
            }
        }

        public void NewLevel(int w = Level.ClassicBricksWide, int h = Level.ClassicBricksHigh)
        {
            var level = EditLevelNumber + (w == Level.ClassicBricksWide ? 1 : 0);
            var name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            while (AsyncIO.DoesFileExistAsync(Folder, name))
            {
                level = level + 1;
                name = (level < 10 ? "00" : (level < 100 ? "0" : "")) + level + ".level";
            }
            EditLevel = Level.EmptyLevel(w, h);
            EditLevel.KnownEditName = name;
            EditLevelNumber = level;
            SetSelector();
        }

        private void NewTournamentLevel()
        {
            NewLevel(Level.TournamentBricksWide, Level.TournamentBricksHigh);
        }
        private void Exit() => Game.Arena = new MenuArena(Game);
        private void Test()
        {
            SaveLevel();
            var a = new PlayArena(Game, false, LevelSelector, null, EditLevelNumber, EditLevel, EditPath);
            a.LevelSelector.Initialise(a);
            Game.Arena = a;
        }
        private void ChangeBackground()
        {
            EditLevel.Background++;
            if (EditLevel.Background > Enum.GetValues(typeof(BackGroundTypes)).Length - 1)
                EditLevel.Background = 0;
        }
        private void SetBrick()
        {
            if (LastBrickLocation == EditLocation)
            {
                var v = EditLevel.GetBrickValue((int)EditLocation.Y, (int)EditLocation.X) + 1;
                if (v >= Enum.GetValues(typeof(BrickTypes)).Length) v = 0;
                LastBrickVal = v;
            }
            LastBrickLocation = EditLocation;
            EditLevel.SetBrickValue((int)EditLocation.Y, (int)EditLocation.X, LastBrickVal);
        }

        private void ClearBrick()
        {
            EditLevel.SetBrickValue((int)EditLocation.Y, (int)EditLocation.X, 10);
        }
        private void ChangeEnemy()
        {
            var v = EditLevel.EnemyType + 1;
            if (v >= Enum.GetValues(typeof(EnemyTypes)).Length) v = 0;
            EditLevel.EnemyType = v;
        }
        private void ChangeChance()
        {
            var v = EditLevel.GetChanceValue((int)EditLocation.Y, (int)EditLocation.X) + 1;
            if (v >= 9) v = 0;
            LastChanceVal = v;
            EditLevel.SetChanceValue((int)EditLocation.Y, (int)EditLocation.X, v);
        }
        private void SetChance()
        {
            EditLevel.SetChanceValue((int)EditLocation.Y, (int)EditLocation.X, LastChanceVal);
        }
        private void ChangePower()
        {
            var v = EditLevel.GetPowerValue((int)EditLocation.Y, (int)EditLocation.X) + 1;
            if (v >= Enum.GetValues(typeof(CapsuleTypes)).Length) v = 0;
            LastPowerVal = v;
            EditLevel.SetPowerValue((int)EditLocation.Y, (int)EditLocation.X, v);
        }

        private void SetPower()
        {
            EditLevel.SetPowerValue((int)EditLocation.Y, (int)EditLocation.X, LastPowerVal);
        }
        private void AddRandomPowerUp()
        {
            int i, j;
            var found = false;
            for (i = 0; i < EditLevel.BricksWide - 1; i++)
            {
                for (j = 0; j < EditLevel.BricksHigh - 1; j++)
                {
                    if (EditLevel.GetBrickValue(j, i) != 10)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }
            if (!found) return;
            i = Arkanoid.Random.Next(EditLevel.BricksWide - 1);
            j = Arkanoid.Random.Next(EditLevel.BricksHigh - 1);
            while (EditLevel.GetBrickValue(j, i) == 10)
            {
                i = Arkanoid.Random.Next(EditLevel.BricksWide - 1);
                j = Arkanoid.Random.Next(EditLevel.BricksHigh - 1);
            }
            var a = (int)RandomUtils.RandomEnum<CapsuleTypes>();
            EditLevel.SetPowerValue(j, i, a);
            EditLevel.SetChanceValue(j, i, Arkanoid.Random.Next(8));
        }

        private void MaxEnemyReleaseTimeInc()
        {
            EditLevel.MaxEnemyReleaseTime++;
            if (EditLevel.MaxEnemyReleaseTime > 60)
                EditLevel.MaxEnemyReleaseTime = 60;
        }
        private void MaxEnemyReleaseTimeDec()
        {
            EditLevel.MaxEnemyReleaseTime--;
            if (EditLevel.MaxEnemyReleaseTime < EditLevel.MinEnemyReleaseTime + 1)
                EditLevel.MaxEnemyReleaseTime = EditLevel.MinEnemyReleaseTime + 1;
        }
        private void MinEnemyReleaseTimeInc()
        {
            EditLevel.MinEnemyReleaseTime++;
            if (EditLevel.MinEnemyReleaseTime > EditLevel.MaxEnemyReleaseTime - 1)
                EditLevel.MinEnemyReleaseTime = EditLevel.MaxEnemyReleaseTime - 1;
        }
        private void MinEnemyReleaseTimeDec()
        {
            EditLevel.MinEnemyReleaseTime--;
            if (EditLevel.MinEnemyReleaseTime < 1)
                EditLevel.MinEnemyReleaseTime = 1;
        }

        private void MaxEnemiesInc()
        {
            EditLevel.MaxEnemies++;
        }
        private void MaxEnemiesDec()
        {
            EditLevel.MaxEnemies--;
            if (EditLevel.MaxEnemies < 0)
                EditLevel.MaxEnemies = 0;
        }

        private void ChangeSideLeftMidEntry()
        {
            EditLevel.SideLeftMidEntryEnable = EditLevel.SideLeftMidEntryEnable == 0 ? 1 : 0;
        }
        private void ChangeSideRightMidEntry()
        {
            EditLevel.SideRightMidEntryEnable = EditLevel.SideRightMidEntryEnable == 0 ? 1 : 0;
        }

        private void ChangeSideLeftTopEntry()
        {
            EditLevel.SideLeftTopEntryEnable = EditLevel.SideLeftTopEntryEnable == 0 ? 1 : 0;
        }
        private void ChangeSideRightTopEntry()
        {
            EditLevel.SideRightTopEntryEnable = EditLevel.SideRightTopEntryEnable == 0 ? 1 : 0;
        }
        private void ChangeTopRightEntry()
        {
            EditLevel.TopRightEntryEnable = EditLevel.TopRightEntryEnable == 0 ? 1 : 0;
        }
        private void ChangeTopLeftEntry()
        {
            EditLevel.TopLeftEntryEnable = EditLevel.TopLeftEntryEnable == 0 ? 1 : 0;
        }
        private void ConstrainEditLocation()
        {
            if (EditLocation.X < 0) EditLocation = new Vector2(0, EditLocation.Y);
            if (EditLocation.X >= EditLevel.BricksWide - 1)
                EditLocation = new Vector2(EditLevel.BricksWide - 1, EditLocation.Y);
            if (EditLocation.Y < 0) EditLocation = new Vector2(EditLocation.X, 0);
            if (EditLocation.Y >= EditLevel.BricksHigh - 1)
                EditLocation = new Vector2(EditLocation.X, EditLevel.BricksHigh - 1);
        }
        #endregion

        public override void Draw(SpriteBatch batch)
        {
            BackGroundTypes bg;
            if (!Enum.TryParse(Enum.GetNames(typeof (BackGroundTypes))[EditLevel.Background], out bg))
                bg = BackGroundTypes.BlueCircuit;


            DrawBackground(batch, bg);
            DrawFrameLeft(batch);
            

            EnemyTypes et;
            if (!Enum.TryParse(Enum.GetNames(typeof (EnemyTypes))[EditLevel.EnemyType], out et))
                et = EnemyTypes.Orb;
            batch.Draw(Types.GetEnemySprite(et), new Vector2(X, Y), Color.White);

            for (var r = 0; r < EditLevel.BricksHigh; r++)
            {
                for (var c = 0; c < EditLevel.BricksWide; c++)
                {
                    BrickTypes b;
                    if (!Enum.TryParse(Enum.GetNames(typeof (BrickTypes))[EditLevel.GetBrickValue(r, c)], out b))
                        b = BrickTypes.Empty;
                    var l = new Vector2(X + c * Sprites.BrkWhite.Width,
                            Y + r * Sprites.BrkWhite.Height);
                    //batch.DrawRectangle(new Rectangle((int)l.X, (int)l.Y, (int)Sprites.BrkWhite.Width, (int)Sprites.BrkWhite.Height),Color.Black);
                    if (b == BrickTypes.Empty) continue;
                    batch.Draw(Sprites.BrkBlack, l + Game.Shadow, new Color(0f, 0f, 0f, .5f));
                    batch.Draw(Types.GetBrick(b, Game, this, Vector2.Zero, 0, CapsuleTypes.Slow).Texture,
                        l, Color.White);

                    if(!viewMode)batch.DrawString(Fonts.SmallFont, EditLevel.GetChanceValue(r, c) + "",
                        new Vector2(l.X, l.Y ), Color.DarkGray);
                    CapsuleTypes p;
                    var cname = Enum.GetNames(typeof (CapsuleTypes))[EditLevel.GetPowerValue(r, c)];
                    if (!Enum.TryParse(cname, out p))
                        p = CapsuleTypes.Fast;
                    if (!viewMode) batch.Draw(CapsulesD[p], l + new Vector2(Sprites.BrkWhite.Width/2, 0), //.GetCapTexture(p)
                        Color.White, .5f);
                }
            }
            DrawFrameRight(batch);
            if (!viewMode) batch.DrawRectangle(
                new Rectangle((int) (X + EditLocation.X*Sprites.BrkWhite.Width - 3),
                    (int) (Y + EditLocation.Y*Sprites.BrkWhite.Height) - 3,
                    (int) Sprites.BrkWhite.Width + 6, (int) Sprites.BrkWhite.Height + 6), Color.Red, 3f);

            var strHeigth = Fonts.TextFont.MeasureString("aZj");
            var strWidth = Fonts.TextFont.MeasureString("0");
            var scale = strHeigth.Y/ Textures.CmnLeft.Height;
            
            var tloc = new Vector2(Game.FrameArea.Width, Y);
            LevelDecButtonTextRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)(Textures.CmnLeft.Width * scale), (int)(Textures.CmnLeft.Height * scale));
            batch.Draw(Textures.CmnLeft, tloc, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            tloc = new Vector2(tloc.X + Textures.CmnLeft.Width * scale + 10, tloc.Y);
            LevelIncButtonTextRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)(Textures.CmnLeft.Width * scale), (int)(Textures.CmnLeft.Height * scale));
            batch.Draw(Textures.CmnRight, tloc, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            tloc = new Vector2(tloc.X + Textures.CmnRight.Width + 10, tloc.Y);
            batch.DrawString(Fonts.TextFont, "Level " + EditLevelNumber, tloc, Color.White);
            tloc = new Vector2(Game.FrameArea.Width, tloc.Y + strHeigth.Y);

            EnemyMaxDecButtonTextRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)(Textures.CmnLeft.Width * scale), (int)(Textures.CmnLeft.Height * scale));
            batch.Draw(Textures.CmnLeft,tloc,null,Color.White,0,Vector2.Zero,scale,SpriteEffects.None,0);
            tloc = new Vector2(tloc.X+ Textures.CmnLeft.Width * scale+10, tloc.Y);
            EnemyMaxIncButtonTextRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)(Textures.CmnLeft.Width * scale), (int)(Textures.CmnLeft.Height * scale));
            batch.Draw(Textures.CmnRight, tloc, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            tloc = new Vector2(tloc.X + Textures.CmnRight.Width + 10, tloc.Y);
            batch.DrawString(Fonts.TextFont, "Max Enimies = " + EditLevel.MaxEnemies, tloc, Color.White);
            tloc = new Vector2(Game.FrameArea.Width, tloc.Y + strHeigth.Y);

            EnemyMinReleaseDecTextButtonRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)(Textures.CmnLeft.Width * scale), (int)(Textures.CmnLeft.Height * scale));
            batch.Draw(Textures.CmnLeft, tloc, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            tloc = new Vector2(tloc.X + Textures.CmnLeft.Width * scale + 10, tloc.Y);
            EnemyMinReleaseIncTextButtonRect = new Rectangle((int)tloc.X, (int)tloc.Y,(int)(Textures.CmnLeft.Width * scale), (int)(Textures.CmnLeft.Height * scale));
            batch.Draw(Textures.CmnRight, tloc, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            tloc = new Vector2(tloc.X + Textures.CmnRight.Width + 10, tloc.Y);            
            batch.DrawString(Fonts.TextFont, "Min Enimey RT = " + EditLevel.MinEnemyReleaseTime, tloc, Color.White);
            tloc = new Vector2(Game.FrameArea.Width, tloc.Y + strHeigth.Y);

            EnemyMaxReleaseDecTextButtonRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)(Textures.CmnLeft.Width * scale), (int)(Textures.CmnLeft.Height * scale));
            batch.Draw(Textures.CmnLeft, tloc, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            tloc = new Vector2(tloc.X + Textures.CmnLeft.Width * scale + 10, tloc.Y);
            EnemyMaxReleaseIncTextButtonRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)(Textures.CmnLeft.Width * scale), (int)(Textures.CmnLeft.Height * scale));
            batch.Draw(Textures.CmnRight, tloc, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            tloc = new Vector2(tloc.X + Textures.CmnRight.Width + 10, tloc.Y);
            batch.DrawString(Fonts.TextFont, "Max Enimey RT = " + EditLevel.MaxEnemyReleaseTime, tloc, Color.White);
            tloc = new Vector2(Game.FrameArea.Width, tloc.Y + strHeigth.Y);

            var mbounds = Fonts.TextFont.MeasureString("TEST");

            NewTextRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)mbounds.X, (int)mbounds.Y);
            batch.DrawString(Fonts.TextFont, "New", tloc, Color.White);            
            tloc = new Vector2(Game.FrameArea.Width, tloc.Y + strHeigth.Y);

            SaveTextRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)mbounds.X, (int)mbounds.Y);
            batch.DrawString(Fonts.TextFont, "Save", tloc, Color.White);
            tloc = new Vector2(Game.FrameArea.Width, tloc.Y + strHeigth.Y);

            TestTextRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)mbounds.X, (int)mbounds.Y);
            batch.DrawString(Fonts.TextFont, "Test", tloc, Color.White);
            tloc = new Vector2(Game.FrameArea.Width, tloc.Y + strHeigth.Y);

            ViewTextRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)mbounds.X, (int)mbounds.Y);
            batch.DrawString(Fonts.TextFont, "View Mode", tloc, Color.White);
            tloc = new Vector2(Game.FrameArea.Width, tloc.Y + strHeigth.Y);
            

            BackTextRect = new Rectangle((int)tloc.X, (int)tloc.Y, (int)mbounds.X, (int)mbounds.Y);
            batch.DrawString(Fonts.TextFont, "Back", tloc, Color.White);
            //tloc = new Vector2(Game.FrameArea.Width, tloc.Y + strHeigth.Y);

            var lx = new Vector2(Game.Width- CapsulesD.First().Value.Width+2,0);
            foreach (var c in CapsulesD.Keys)
            {                
                batch.Draw(CapsulesD[c], lx, Color.White*.5f);
                CapsuleRects[(int) c] = new Rectangle((int)lx.X, (int)lx.Y, (int)CapsulesD[c].Width, (int)CapsulesD[c].Height);
                if (Enum.GetValues(typeof (CapsuleTypes)).Cast<CapsuleTypes>().ToArray()[LastPowerVal] == c)
                {
                    batch.Draw(CapsulesD[c], lx, Color.White);
                    batch.DrawRectangle(new Rectangle((int)lx.X-1, (int)lx.Y-1, (int)CapsulesD[c].Width+2, (int)CapsulesD[c].Height+2),Color.Red,3);  
                }
                lx += new Vector2(0, CapsulesD[c].Height);
            }

            lx = new Vector2(Game.Width - (CapsulesD.First().Value.Width + 2 + Bricks.First().Value.Width+2), 0);
            foreach (var c in Bricks.Keys)
            {
                //if (c == BrickTypes.Empty) continue;
                batch.Draw(Bricks[c], lx, Color.White *.5f);
                BrickRects[(int)c] = new Rectangle((int)lx.X, (int)lx.Y, (int)Bricks[c].Width, (int)Bricks[c].Height);
                if ((int)c == LastBrickVal)
                {
                    batch.Draw(Bricks[c], lx, Color.White);
                    batch.DrawRectangle(new Rectangle((int)lx.X - 1, (int)lx.Y - 1, (int)Bricks[c].Width + 2, (int)Bricks[c].Height + 2), Color.Red, 3);
                }
                lx += new Vector2(0, Bricks[c].Height);
            }

            lx = new Vector2(Game.Width - (CapsulesD.First().Value.Width + 2 + Bricks.First().Value.Width + 2 + EnemyTypesDictionary.First().Value.Width+2), 0);
            foreach (var c in EnemyTypesDictionary.Keys)
            {
                batch.Draw(EnemyTypesDictionary[c], lx, Color.White * .5f);
                EnemyRects[(int)c] = new Rectangle((int)lx.X, (int)lx.Y, (int)EnemyTypesDictionary[c].Width, (int)EnemyTypesDictionary[c].Height);
                if ((int)c == EditLevel.EnemyType)
                {
                    batch.Draw(EnemyTypesDictionary[c], lx, Color.White);
                    batch.DrawRectangle(new Rectangle((int)lx.X - 1, (int)lx.Y - 1, (int)EnemyTypesDictionary[c].Width + 2, (int)EnemyTypesDictionary[c].Height + 2), Color.Red, 3);
                }
                lx += new Vector2(0, EnemyTypesDictionary[c].Height);
            }

            lx = new Vector2(Game.Width - (CapsulesD.First().Value.Width + 2 + Bricks.First().Value.Width + 2 + EnemyTypesDictionary.First().Value.Width + 2 + (BackGrounds.First().Value.Width*.25f) + 2), 0);
            for (int i = 0; i < 10; i++)
            {
                batch.DrawString(Fonts.SmallFont,i.ToString(),lx, Color.White * .5f);
                ChanceRects[i]=  new Rectangle((int)lx.X, (int)lx.Y, (int)strWidth.X, (int)strHeigth.Y);
                if (i == LastChanceVal)
                {
                    batch.DrawString(Fonts.SmallFont, i.ToString(), lx, Color.White);
                    batch.DrawRectangle(new Rectangle((int)lx.X - 1, (int)lx.Y - 1, (int)(strWidth.X) + 2, (int)(strWidth.Y) + 2), Color.Red, 3);
                }
                lx += new Vector2(0, strHeigth.Y);
            }
                       
            lx = new Vector2(Game.Width - (strWidth.X + 6 + CapsulesD.First().Value.Width + 2 + Bricks.First().Value.Width + 2 + EnemyTypesDictionary.First().Value.Width + 2 + (BackGrounds.First().Value.Width * .25f) + 2), 0);
            foreach (var c in BackGrounds.Keys)
            {
                batch.Draw(BackGrounds[c], lx, Color.White * .5f, .25f);
                BackgroundRects[(int)c] = new Rectangle((int)lx.X, (int)lx.Y, (int)(BackGrounds[c].Width * .25f), (int)(BackGrounds[c].Height * .25f));
                if ((int)c == EditLevel.Background)
                {
                    batch.Draw(BackGrounds[c], lx, Color.White, .25f);
                    batch.DrawRectangle(new Rectangle((int)lx.X - 1, (int)lx.Y - 1, (int)(BackGrounds[c].Width * .25f) + 2, (int)(BackGrounds[c].Height * .25f) + 2), Color.Red, 3);
                }
                lx += new Vector2(0, BackGrounds[c].Height * .25f);
            }

            if (!viewMode) batch.Draw(Sprites.CmnEditInfo, EditRect, Color.White);

            DrawEntries(batch);
            DrawWarps(batch);
            if (!viewMode) batch.DrawRectangle(
                new Rectangle((int) TopLeftEntry.Location.X, (int) TopLeftEntry.Location.Y,
                    (int) TopLeftEntry.Texture.Width,
                    (int) TopLeftEntry.Texture.Height), EditLevel.TopLeftEntryEnable == 1 ? Color.Green : Color.Red, 3);
            if (!viewMode) batch.DrawRectangle(
                new Rectangle((int) TopRightEntry.Location.X, (int) TopRightEntry.Location.Y,
                    (int) TopRightEntry.Texture.Width,
                    (int) TopRightEntry.Texture.Height), EditLevel.TopRightEntryEnable == 1 ? Color.Green : Color.Red, 3);
            if (!viewMode) batch.DrawRectangle(
                new Rectangle((int) SideLeftTopEntry.Location.X, (int) SideLeftTopEntry.Location.Y,
                    (int) SideLeftTopEntry.Texture.Width,
                    (int) SideLeftTopEntry.Texture.Height),
                EditLevel.SideLeftTopEntryEnable == 1 ? Color.Green : Color.Red, 3);
            if (!viewMode) batch.DrawRectangle(
                new Rectangle((int) SideRightTopEntry.Location.X, (int) SideRightTopEntry.Location.Y,
                    (int) SideRightTopEntry.Texture.Width,
                    (int) SideRightTopEntry.Texture.Height),
                EditLevel.SideRightTopEntryEnable == 1 ? Color.Green : Color.Red, 3);
            if (!viewMode) batch.DrawRectangle(
                new Rectangle((int) SideLeftMidEntry.Location.X, (int) SideLeftMidEntry.Location.Y,
                    (int) SideLeftMidEntry.Texture.Width,
                    (int) SideLeftMidEntry.Texture.Height),
                EditLevel.SideLeftMidEntryEnable == 1 ? Color.Green : Color.Red, 3);
            if (!viewMode) batch.DrawRectangle(
                new Rectangle((int) SideRightMidEntry.Location.X, (int) SideRightMidEntry.Location.Y,
                    (int) SideRightMidEntry.Texture.Width,
                    (int) SideRightMidEntry.Texture.Height),
                EditLevel.SideRightMidEntryEnable == 1 ? Color.Green : Color.Red, 3);
   
            batch.Draw(Textures.CmnSaved, Center - new Vector2(Textures.CmnSaved.Width/2f, Textures.CmnSaved.Height/2f),
                Color.White*Savedfade);  
        }
    }
}