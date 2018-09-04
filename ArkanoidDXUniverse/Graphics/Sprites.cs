using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ArkanoidDXUniverse.Graphics
{
    public static class Sprites
    {
        public static Sprite BgDoh;
        public static Sprite BgPurpleCircuit;
        public static Sprite BgRedCircuit;
        public static Sprite BgDarkRedCircuit;
        public static Sprite BgBlueCircuit;
        public static Sprite BgDarkBlueCircuit;
        public static Sprite BgCellularGreen;
        public static Sprite BgDarkCellularGreen;
        public static Sprite BgBlueHex;
        public static Sprite BgDarkBlueHex;
        public static Sprite BgDarkTech;
        public static Sprite BgGreySkin;
        public static Sprite BgGoldHive;
        public static Sprite BgRedInnards;
        public static Sprite BgGreenWeave;
        public static Sprite BgGreyBevelHex;

        public static Sprite BrkEmpty;
        public static Sprite BrkWhite;
        public static Sprite BrkYellow;
        public static Sprite BrkPink;
        public static Sprite BrkBlue;
        public static Sprite BrkRed;
        public static Sprite BrkGreen;
        public static Sprite BrkSkyBlue;
        public static Sprite BrkOrange;
        public static Sprite BrkSilver;
        public static Sprite BrkGold;
        public static Sprite BrkRegen;
        public static Sprite BrkGoldSwap;
        public static Sprite BrkSilverSwap;
        public static Sprite BrkBlueSwap;
        public static Sprite BrkBlack;
        public static Sprite BrkBlackRegen;
        public static Sprite BrkDarkRed;
        public static Sprite BrkDarkBlue;
        public static Sprite EnmDoh;
        public static Sprite EnmDohWeb;

        public static Sprite BallNormal;
        public static Sprite BallMega;
        public static Sprite BallMagna;
        public static Sprite BallMegaMagna;

        public static Sprite VaEndNormal;
        public static Sprite VaEndNormalP2;
        public static Sprite VaCenterNormal;
        public static Sprite VaSideNormal;
        public static Sprite VaExtensionNormal;
        public static Sprite VaExtensionPlusNormal;
        public static Sprite VaEndLaser;
        public static Sprite VaCenterMegaLaser;
        public static Sprite VaCenterLaser;
        public static Sprite VaSideLaser;
        public static Sprite VaExtensionLaser;
        public static Sprite VaExtensionPlusLaser;
        public static Sprite VaCenterCatch;
        public static Sprite VaSideCatch;
        public static Sprite VaExtensionCatch;
        public static Sprite VaExtensionPlusCatch;
        public static Sprite VaExplode;
        public static Sprite VaExplodeP2;
        public static Sprite VaRevive;
        public static Sprite VaLaser;
        public static Sprite VaMegaLaser;

        public static Sprite CmnDohBoarder;
        public static Sprite CmnLife;
        public static Sprite CmnLife2;
        public static Sprite CmnArkanoidShip;
        public static Sprite CmnArkanoidLogo;
        public static Sprite CmnArkanoidDxLogoA;
        public static Sprite CmnArkanoidDxLogoB;

        public static Sprite CmnTitle;
        public static Sprite CmnTaito;
        public static Sprite CmnEditInfo;
        public static Sprite CmnTournament;

        public static Sprite CmnStart;
        public static Sprite CmnEdit;
        public static Sprite CmnExit;
        public static Sprite CmnDemo;
        public static Sprite CmnDelete;
        public static Sprite CmnMoveLeft;
        public static Sprite CmnMoveRight;
        public static Sprite CmnVerArkanoid;
        public static Sprite CmnVerArkanoidDx;
        public static Sprite CmnVerCustom;
        public static Sprite CmnLoad;
        public static Sprite CmnSmallFrame;
        public static Sprite CmnSmallFrameEmpty;
        public static Sprite CmnSmallFrameCustom;

        public static Sprite CmnElectricFence;

        public static Sprite FrmTop;
        public static Sprite FrmSide;
        public static Sprite FrmCorner;
        public static Sprite FrmSideWarp;

        public static Arkanoid Game;

        

        public static Sprite BrkFlash => new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.Stop, Textures.BrkFlash);

        public static Sprite BrkTeleport => new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.BrkTeleport);

        public static Sprite BrkTransmit => new Sprite(Game, 10, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.BrkTransmit);

        public static Sprite EnmOrb => new Sprite(Game, 24, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmOrb);
        public static Sprite EnmCubeOrb => new Sprite(Game, 24, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmCubeOrb);
        public static Sprite EnmRubix => new Sprite(Game, 24, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmRubix);
        public static Sprite EnmRubixSpawn => new Sprite(Game, 7, new TimeSpan(900000), AnimationState.Play, Textures.EnmRubixSpawn);

        public static Sprite EnmGeom => new Sprite(Game, 10, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmGeom);

        public static Sprite EnmTri => new Sprite(Game, 11, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmTri);

        public static Sprite EnmUfo => new Sprite(Game, 8, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmUfo);
        public static Sprite EnmUfoGreen => new Sprite(Game, 8, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmUfoGreen);

        public static Sprite EnmBlob => new Sprite(Game, 6, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmBlob, 0.75f);

        public static Sprite EnmOrbRed => new Sprite(Game, 24, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmOrbRed);

        public static Sprite EnmOrbGreen => new Sprite(Game, 24, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmOrbGreen);

        public static Sprite EnmOrbBlue => new Sprite(Game, 24, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmOrbBlue);

        public static Sprite EnmDohSplinter => new Sprite(Game, 12, new TimeSpan(450000), AnimationState.LoopPlay, Textures.EnmDohSplinter);

        public static Sprite EnmDieOrbTri => new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.Play, Textures.EnmDieOrbTri);

        public static Sprite EnmDieUfo => new Sprite(Game, 12, new TimeSpan(0, 0, 0, 0, 100), AnimationState.Play, Textures.EnmDieUfo);

        public static Sprite EnmDieGeomUfo => new Sprite(Game, 7, new TimeSpan(0, 0, 0, 0, 100), AnimationState.Play, Textures.EnmDieGeomUfo);

        public static Sprite PwrSlowS => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrSlowS);

        public static Sprite PwrCatchC => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrCatchC);

        public static Sprite PwrExpandE => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrExpandE);

        public static Sprite PwrDisruptD => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrDisruptD);

        public static Sprite PwrLaserL => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrLaserL);

        public static Sprite PwrExitB => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrExitB);

        public static Sprite PwrLifeP => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrLifeP);

        public static Sprite PwrReduceR => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrReduceR);

        public static Sprite PwrMegaBallH => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrMegaBallH);

        public static Sprite PwrMagnaBallG => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrMagnaBallG);

        public static Sprite PwrFastF => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrFastF);

        public static Sprite PwrIllusionI => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrIllusionI);

        public static Sprite PwrOrbitO => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrOrbitO);

        public static Sprite PwrTwinT => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrTwinT);

        public static Sprite PwrRandom => new Sprite(Game, 17, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.PwrRandom);

        public static Sprite PwrMegaLaserM => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
            Textures.PwrMegaLaserM);

        public static Sprite PwrElectricFence => new Sprite(Game, 8, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
            Textures.PwrElectricFence);

        public static Sprite FrmTopOpen => new Sprite(Game, 6, new TimeSpan(900000), AnimationState.Pause, Textures.FrmTopOpen);

        public static Sprite FrmSideOpen => new Sprite(Game, 6, new TimeSpan(900000), AnimationState.Pause, Textures.FrmSideOpen);

        public static Sprite FrmSideWarpOpen => new Sprite(Game, 6, new TimeSpan(900000), AnimationState.Pause, Textures.FrmSideWarpOpen);

        public static void LoadContent(Arkanoid game, ContentManager content)
        {
            Game = game;
            BgDoh = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgDoh);
            BgPurpleCircuit = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgPurpleCircuit);
            BgRedCircuit = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgRedCircuit);
            BgDarkRedCircuit = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgDarkRedCircuit);
            BgBlueCircuit = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgBlueCircuit);
            BgDarkBlueCircuit = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgDarkBlueCircuit);
            BgCellularGreen = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgCellularGreen);
            BgDarkCellularGreen = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgDarkCellularGreen);
            BgBlueHex = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgBlueHex);
            BgDarkBlueHex = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgDarkBlueHex);
            BgDarkTech = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgDarkTech);
            BgGreySkin = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgGreySkin);
            BgGoldHive = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgGoldHive);
            BgRedInnards = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgRedInnards);
            BgGreenWeave = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgGreenWeave);
            BgGreyBevelHex = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BgGreyBevelHex);

            BrkEmpty = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkEmpty);
            BrkWhite = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkWhite);
            BrkYellow = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkYellow);
            BrkPink = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkPink);
            BrkBlue = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkBlue);
            BrkRed = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkRed);
            BrkGreen = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkGreen);
            BrkSkyBlue = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkSkyBlue);
            BrkOrange = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkOrange);
            BrkSilver = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkSilver);
            BrkGold = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkGold);
            BrkRegen = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkRegen);
            BrkGoldSwap = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkGoldSwap);
            BrkSilverSwap = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkSilverSwap);
            BrkBlueSwap = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkBlueSwap);
            BrkBlack = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkBlack);
            BrkBlackRegen = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkBlackRegen);
            BrkDarkRed = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkDarkRed);
            BrkDarkBlue = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.BrkDarkBlue);

            EnmDoh = new Sprite(Game, 4, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmDoh);
            EnmDohWeb = new Sprite(Game, 4, new TimeSpan(900000), AnimationState.LoopPlay, Textures.EnmDohWeb);

            BallNormal = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.LoopPlay, Textures.BallNormal, 0.75f);
            BallMega = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.LoopPlay, Textures.BallMega, 0.75f);
            BallMagna = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.LoopPlay, Textures.BallMagna, 0.75f);
            BallMegaMagna = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.LoopPlay, Textures.BallMegaMagna, 0.75f);


            VaEndNormal = new Sprite(Game, 4, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaEndNormal);

            VaEndNormalP2 = new Sprite(Game, 4, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaEndNormalP2);

            VaCenterNormal = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.VaCenterNormal);
            VaSideNormal = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.VaSideNormal);
            VaExtensionNormal = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.VaExtensionNormal);
            VaExtensionPlusNormal = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause,
                Textures.VaExtensionPlusNormal);
            VaEndLaser = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.VaEndLaser);
            VaCenterMegaLaser = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaCenterMegaLaser);
            VaCenterLaser = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaCenterLaser);
            VaSideLaser = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaSideLaser);
            VaExtensionLaser = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaExtensionLaser);
            VaExtensionPlusLaser = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaExtensionPlusLaser);
            VaExplode = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.Play, Textures.VaExplode);
            VaExplodeP2 = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.Play, Textures.VaExplodeP2);
            VaRevive = new Sprite(Game, 3, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay, Textures.VaRevive);
            VaLaser = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.VaLaser);
            VaMegaLaser = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.VaMegaLaser);


            VaCenterCatch = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaCenterCatch);
            VaSideCatch = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaSideCatch);
            VaExtensionCatch = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaExtensionCatch);
            VaExtensionPlusCatch = new Sprite(Game, 6, new TimeSpan(0, 0, 0, 0, 100), AnimationState.LoopPlay,
                Textures.VaExtensionPlusCatch);


            CmnDohBoarder = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnDohBoarder);
            CmnLife = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnLife);
            CmnLife2 = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnLife2);
            CmnArkanoidShip = new Sprite(Game, 2, new TimeSpan(900000), AnimationState.LoopPlay,
                Textures.CmnArkanoidShip);
            CmnArkanoidLogo = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnArkanoidLogo);
            CmnArkanoidDxLogoA = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnArkanoidDxLogoA);
            CmnArkanoidDxLogoB = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnArkanoidDxLogoB);
            CmnStart = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnStart);
            CmnEdit = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnEdit);
            CmnExit = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnExit);
            CmnTitle = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnTitle);
            CmnTournament = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnTournament);
            CmnTaito = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnTaito);
            CmnEditInfo = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnEditInfo);
            CmnDemo = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnDemo);
            CmnDelete = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnDelete);
            CmnMoveLeft = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnMoveLeft);
            CmnMoveRight = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnMoveRight);
            CmnVerArkanoid = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnVerArkanoid);
            CmnVerArkanoidDx = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnVerArkanoidDx);
            CmnVerCustom = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnVerCustom);
            CmnLoad = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnLoad);
            CmnSmallFrame = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnSmallFrame);
            CmnSmallFrameEmpty = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnSmallFrameEmpty);
            CmnSmallFrameCustom = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.CmnSmallFrameCustom);

            CmnElectricFence = new Sprite(Game, 4, new TimeSpan(500000), AnimationState.LoopPlay,
                Textures.CmnElectricFence);

            FrmTop = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.FrmTop);
            FrmSide = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.FrmSide);
            FrmCorner = new Sprite(Game, 1, TimeSpan.Zero, AnimationState.Pause, Textures.FrmCorner);
            FrmSideWarp = new Sprite(Game, 4, new TimeSpan(900000), AnimationState.LoopPlay, Textures.FrmSideWarp);
        }

        public static void Update(GameTime gameTime)
        {
            EnmDoh.Update(gameTime);
            EnmDohWeb.Update(gameTime);

            VaEndNormal.Update(gameTime);
            VaEndNormalP2.Update(gameTime);
            VaEndLaser.Update(gameTime);
            VaCenterLaser.Update(gameTime);
            VaSideLaser.Update(gameTime);
            VaExtensionLaser.Update(gameTime);
            VaExtensionPlusLaser.Update(gameTime);
            VaExplode.Update(gameTime);
            VaExplodeP2.Update(gameTime);
            VaRevive.Update(gameTime);
            VaCenterCatch.Update(gameTime);
            VaCenterMegaLaser.Update(gameTime);
            VaSideCatch.Update(gameTime);
            VaExtensionCatch.Update(gameTime);
            VaExtensionPlusCatch.Update(gameTime);

            CmnArkanoidShip.Update(gameTime);

            CmnElectricFence.Update(gameTime);

            FrmTopOpen.Update(gameTime);
            FrmSideOpen.Update(gameTime);
            FrmSideWarpOpen.Update(gameTime);
            FrmSideWarp.Update(gameTime);
        }
    }
}