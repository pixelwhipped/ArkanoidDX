using System.Collections.Generic;
using ArkanoidDX.Arena;
using ArkanoidDX.Graphics;
using ArkanoidDX.Objects;
using Microsoft.Xna.Framework;

namespace ArkanoidDX
{
    public enum GameMode
    {
        Menu,Play,Edit,Boss

    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        DownLeft,
        DownRight,
        UpLeft,
        UpRight,
        Stop
    }

    public enum CollisionPoint
    {
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        None
    }

    public enum BrickTypes
    {
        White,
        Yellow,
        Pink,
        Blue,
        Red,
        Green,
        SkyBlue,
        Orange,
        Silver,
        Gold,
        Empty,
        Regen,
        Teleport,
        SilverSwap,
        GoldSwap,
        BlueSwap,
        Black,
        BlackRegen,
        Transmit,
        HorizonalMove
    }

    public enum EnemyTypes
    {
        Orb,
        Geom,
        Tri,
        Ufo,
        Blob,
        OrbSpawn
    }

    public enum BackGroundTypes
    {
        Doh,
        PurpleCircuit,
        RedCircuit,
        DarkRedCircuit,
        BlueCircuit,
        DarkBlueCircuit,
        CellularGreen,
        DarkCellularGreen,
        BlueHex,
        DarkBlueHex,
        DarkTech,
        GreySkin,
        GoldHive,
        RedInnards,
        GreenWeave,
        GreyBevelHex
    }

    public enum CapsuleTypes
    {
        Slow,
        Catch,
        Expand,
        Disrupt,
        Laser,
        Exit,
        Life,
        Reduce,
        MegaBall,
        Fast,
        Illusion,
        Orbit,
        Twin,
        Random,
        MegaLaser,
        ElectricFence
    }

    public static class Types
    {
        public static readonly Dictionary<CapsuleTypes, string> CapsuleDescriptions = new Dictionary<CapsuleTypes, string>
                                                                                          {
            {CapsuleTypes.Slow,"Slow slows down the speed of the energy bolt."},
            {CapsuleTypes.Catch,"Catch enables you to glue the bolt to your Vaus."},
            {CapsuleTypes.Expand,"Expand expands the Vaus craft."},
            {CapsuleTypes.Disrupt,"Disrupt splits the energy bolt into three separate energy bolts."},
            {CapsuleTypes.Laser,"Laser arms your Vaus with a laser allowing it to shoot bricks and aliens."},
            {CapsuleTypes.Exit,"Exit opens the warp gates allowing you to pass to the next round."},
            {CapsuleTypes.Life,"Life awards you an extra life."},
            {CapsuleTypes.Reduce,"Reduce reduces the Vause craft."},
            {CapsuleTypes.MegaBall,"Mega Ball enables the bolt to smash though any but indestructible blocks."},
            {CapsuleTypes.Fast,"Accelerates the bolt to high velocity"},
            {CapsuleTypes.Illusion,"Illusion leaves a ghost Trail behind the Vaus which deflects the energy bolt."},
            {CapsuleTypes.Orbit,"Orbit cause all energy bolts to orbit from left to right."},
            {CapsuleTypes.Twin,"Twin created a mirror Vaus above your Vaus ship."},
            {CapsuleTypes.Random,"Random adds any power up type."},
            {CapsuleTypes.MegaLaser,"Mega Laser makes the Vaus rapidly shoot mini energy bolts for a short time."},
            {CapsuleTypes.ElectricFence,"Creates a electric field below the Vaus that will deflect the bolt."},
        };


        public static readonly Dictionary<BrickTypes , string> BrickDescriptions = new Dictionary<BrickTypes, string>
                                                                                       {
            {BrickTypes.White,"White brick = " + Scoring.White},
            {BrickTypes.Yellow,"Yellow brick = " + Scoring.Yellow},
            {BrickTypes.Pink,"Pink brick = " + Scoring.Pink},
            {BrickTypes.Blue,"Blue brick = " + Scoring.Blue},
            {BrickTypes.Red,"Red brick = " + Scoring.Red},
            {BrickTypes.Green,"Green brick = " + Scoring.Green},
            {BrickTypes.SkyBlue,"SkyBlue brick = " + Scoring.SkyBlue},
            {BrickTypes.Orange,"Orange brick = " + Scoring.Orange},
            {BrickTypes.Silver,"Silver brick has 2 lives and is worth 50 x the current round number."},
            {BrickTypes.Gold,"Gold Brick is indestructible."},
            {BrickTypes.Regen,"Regen is like the Silver Brick Except it resurrects after time."},
            {BrickTypes.Teleport,"Teleport teleports the energy bolt to any other Teleport brick."},
            {BrickTypes.SilverSwap,"Silver swapping brick is like the Silver Brick and swap with other bricks."},
            {BrickTypes.GoldSwap,"Gold swapping brick is like the Gold Brick and will swap with other bricks."},
            {BrickTypes.BlueSwap,"Blue swapping brick is like the Blue Brick and will swap  with other bricks."},
            {BrickTypes.Black,"Black brick has 4 lives and is worth 75 x the current round number."},
            {BrickTypes.BlackRegen,"Regen is like the Black Brick Except it resurrects after time."},
            {BrickTypes.Transmit,"This Brick transmits from one location to another and is worth " + Scoring.Transmit +"."},
            {BrickTypes.HorizonalMove,"This Brick is the same as the Gold Brick except will move hen activated."},

        };

        public static Sprite GetBackGround(BackGroundTypes type)
        {
            switch (type)
            {
                case BackGroundTypes.Doh:
                    {
                        return Sprites.BgDoh;
                    }
                case BackGroundTypes.PurpleCircuit:
                    {
                        return Sprites.BgPurpleCircuit;
                    }
                case BackGroundTypes.RedCircuit:
                    {
                        return Sprites.BgRedCircuit;
                    }
                case BackGroundTypes.DarkRedCircuit:
                    {
                        return Sprites.BgDarkRedCircuit;
                    }
                case BackGroundTypes.BlueCircuit:
                    {
                        return Sprites.BgBlueCircuit;
                    }
                case BackGroundTypes.DarkBlueCircuit:
                    {
                        return Sprites.BgDarkBlueCircuit;
                    }
                case BackGroundTypes.CellularGreen:
                    {
                        return Sprites.BgCellularGreen;
                    }
                case BackGroundTypes.DarkCellularGreen:
                    {
                        return Sprites.BgDarkCellularGreen;
                    }
                case BackGroundTypes.BlueHex:
                    {
                        return Sprites.BgBlueHex;
                    }
                case BackGroundTypes.DarkBlueHex:
                    {
                        return Sprites.BgDarkBlueHex;
                    }
                case BackGroundTypes.DarkTech:
                    {
                        return Sprites.BgDarkTech;
                    }
                case BackGroundTypes.GreySkin:
                    {
                        return Sprites.BgGreySkin;
                    }
                case BackGroundTypes.GoldHive:
                    {
                        return Sprites.BgGoldHive;
                    }
                case BackGroundTypes.RedInnards:
                    {
                        return Sprites.BgRedInnards;
                    }
                case BackGroundTypes.GreenWeave:
                    {
                        return Sprites.BgGreenWeave;
                    }
                case BackGroundTypes.GreyBevelHex:
                    {
                        return Sprites.BgGreyBevelHex;
                    }
                default:
                    {
                        return Sprites.BgBlueHex;
                    }
            }
        }

        public static Sprite GetRandomBackGround()
        {
            return GetBackGround(Utilities.RandomEnum<BackGroundTypes>());
        }

        public static Brick GetBrick(BrickTypes type, ArkanoidDX game, PlayArena playArena, Vector2 location, int chance, CapsuleTypes capsuleType)
        {
            switch (type)
            {
                case BrickTypes.White:
                    {
                        return new Brick(game, playArena,Sprites.BrkWhite, Sprites.BrkFlash, location, Scoring.White, 1, chance, capsuleType);
                    }
                case BrickTypes.Yellow:
                    {
                        return new Brick(game, playArena,Sprites.BrkYellow, Sprites.BrkFlash, location, Scoring.Yellow, 1, chance, capsuleType);
                    }
                case BrickTypes.Pink:
                    {
                        return new Brick(game, playArena,Sprites.BrkPink, Sprites.BrkFlash, location, Scoring.Pink, 1, chance, capsuleType);
                    }
                case BrickTypes.Blue:
                    {
                        return new Brick(game, playArena, Sprites.BrkBlue, Sprites.BrkFlash, location, Scoring.Blue, 1, chance, capsuleType);
                    }
                case BrickTypes.Red:
                    {
                        return new Brick(game, playArena, Sprites.BrkRed, Sprites.BrkFlash, location, Scoring.Red, 1, chance, capsuleType);
                    }
                case BrickTypes.Green:
                    {
                        return new Brick(game, playArena, Sprites.BrkGreen, Sprites.BrkFlash, location, Scoring.Green, 1, chance, capsuleType);
                    }
                case BrickTypes.SkyBlue:
                    {
                        return new Brick(game, playArena, Sprites.BrkSkyBlue, Sprites.BrkFlash, location, Scoring.SkyBlue, 1, chance, capsuleType);
                    }
                case BrickTypes.Orange:
                    {
                        return new Brick(game, playArena, Sprites.BrkOrange, Sprites.BrkFlash, location, Scoring.Orange, 1, chance, capsuleType);
                    }
                case BrickTypes.Silver:
                    {
                        return new Brick(game, playArena, Sprites.BrkSilver, Sprites.BrkFlash, location, Scoring.Silver, 2, chance, capsuleType);
                    }
                case BrickTypes.Gold:
                    {
                        return new Brick(game, playArena, Sprites.BrkGold, Sprites.BrkFlash, location, 0, -1, chance, capsuleType);
                    }
                case BrickTypes.Regen:
                    {
                        return new Brick(game, playArena, Sprites.BrkRegen, Sprites.BrkFlash, location, Scoring.Silver, -2, chance, capsuleType);
                    }
                case BrickTypes.Teleport:
                    {
                        return new Brick(game, playArena, Sprites.BrkTeleport, Sprites.BrkFlash, location, 0, -3, chance, capsuleType);
                    }
                case BrickTypes.SilverSwap:
                    {
                        return new Brick(game, playArena, Sprites.BrkSilverSwap, Sprites.BrkFlash, location, Scoring.Silver, -4, chance, capsuleType);
                    }
                case BrickTypes.GoldSwap:
                    {
                        return new Brick(game, playArena, Sprites.BrkGoldSwap , Sprites.BrkFlash, location, 0, -5, chance, capsuleType);
                    }
                case BrickTypes.BlueSwap:
                    {
                        return new Brick(game, playArena, Sprites.BrkBlueSwap, Sprites.BrkFlash, location, Scoring.Blue, -6, chance, capsuleType);
                    }
                case BrickTypes.Black:
                {
                    return new Brick(game, playArena, Sprites.BrkBlack, Sprites.BrkFlash, location, Scoring.Black, 4, chance, capsuleType);
                }

                case BrickTypes.BlackRegen:
                {
                    return new Brick(game, playArena, Sprites.BrkBlackRegen, Sprites.BrkFlash, location, Scoring.Black, -7, chance, capsuleType);
                }

                case BrickTypes.Transmit:
                {
                    return new Brick(game, playArena, Sprites.BrkTransmit, Sprites.BrkFlash, location, Scoring.Transmit, -8, chance, capsuleType);
                }
                case BrickTypes.HorizonalMove :
                {
                    return new Brick(game, playArena, Sprites.BrkGold, Sprites.BrkFlash, location, 0, -9, chance, capsuleType);
                }

                default:
                    {
                        return new Brick(game, playArena, Sprites.BrkWhite, Sprites.BrkFlash, location, 0, -1, chance, capsuleType);
                    }
            }
        }

        public static Brick GetRandomBrick(ArkanoidDX game, PlayArena playArena, Vector2 location)
        {
            return GetBrick(Utilities.RandomEnum<BrickTypes>(), game, playArena, location, ArkanoidDX.Random.Next(10), Utilities.RandomEnum<CapsuleTypes>());
        }

        public static Enemy GetEnemy(EnemyTypes type, ArkanoidDX game, PlayArena playArena, Vector2 location, Direction direction)
        {
            switch (type)
            {
                case EnemyTypes.Orb:
                    {
                        return new Enemy(game, playArena, Sprites.EnmOrb, Sprites.EnmDieOrbTri, location);
                    }
                case EnemyTypes.Geom:
                    {
                        return new Enemy(game, playArena, Sprites.EnmGeom, Sprites.EnmDieGeomUfo, location);
                    }
                case EnemyTypes.Tri:
                    {
                        return new Enemy(game, playArena, Sprites.EnmTri, Sprites.EnmDieOrbTri, location);
                    }
                case EnemyTypes.Blob:
                    {
                        return new BlobEnemy(game, playArena, Sprites.EnmBlob, Sprites.EnmDieOrbTri, location);
                    }
                case EnemyTypes.OrbSpawn:
                    {
                        return new OrbSpawnEnemy(game, playArena, Sprites.EnmOrb, Sprites.EnmDieOrbTri, location);
                    }
                default:
                    {
                        return new Enemy(game, playArena, Sprites.EnmUfo, Sprites.EnmDieGeomUfo, location, direction);
                    }
            }
        }

        public static Sprite GetEnemySprite(EnemyTypes type)
        {
            switch (type)
            {
                case EnemyTypes.Orb:
                    {
                        return Sprites.EnmOrb;
                    }
                case EnemyTypes.Geom:
                    {
                        return Sprites.EnmGeom;
                    }
                case EnemyTypes.Tri:
                    {
                        return Sprites.EnmTri;
                    }
                case EnemyTypes.Blob:
                    {
                        return Sprites.EnmBlob;
                    }
                case EnemyTypes.OrbSpawn:
                    {
                        return Sprites.EnmOrbRed;
                    }
                default:
                    {
                        return Sprites.EnmUfo;
                    }
            }
        }

        public static Enemy GetRandomEnemy(ArkanoidDX game, PlayArena playArena, Vector2 location)
        {
            return GetEnemy(Utilities.RandomEnum<EnemyTypes>(), game, playArena, location, Direction.Down);
        }

        public static Capsule GetCapsule(CapsuleTypes type,PlayArena playArena, ArkanoidDX game, Vector2 location)
        {
           return new Capsule(type, game, playArena, location);
        }

        public static Capsule GetRandomCapsule(ArkanoidDX game, PlayArena playArena, Vector2 location)
        {
            return GetCapsule(Utilities.RandomEnum<CapsuleTypes>(), playArena, game, location);
        }
    }
}