using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Graphics
{
    public static class Textures
    {
        public static Texture2D BgDoh;
        public static Texture2D BgPurpleCircuit;
        public static Texture2D BgRedCircuit;
        public static Texture2D BgDarkRedCircuit;
        public static Texture2D BgBlueCircuit;
        public static Texture2D BgDarkBlueCircuit;
        public static Texture2D BgCellularGreen;
        public static Texture2D BgDarkCellularGreen;
        public static Texture2D BgBlueHex;
        public static Texture2D BgDarkBlueHex;
        public static Texture2D BgDarkTech;
        public static Texture2D BgGreySkin;
        public static Texture2D BgGoldHive;
        public static Texture2D BgRedInnards;
        public static Texture2D BgGreenWeave;
        public static Texture2D BgGreyBevelHex;

        public static Texture2D BrkWhite;
        public static Texture2D BrkYellow;
        public static Texture2D BrkPink;
        public static Texture2D BrkBlue;
        public static Texture2D BrkRed;
        public static Texture2D BrkGreen;
        public static Texture2D BrkSkyBlue;
        public static Texture2D BrkOrange;
        public static Texture2D BrkSilver;
        public static Texture2D BrkGold;
        public static Texture2D BrkFlash;
        public static Texture2D BrkRegen;
        public static Texture2D BrkTeleport;
        public static Texture2D BrkSilverSwap;
        public static Texture2D BrkGoldSwap;
        public static Texture2D BrkBlueSwap;
        public static Texture2D BrkBlack;
        public static Texture2D BrkBlackRegen;
        public static Texture2D BrkTransmit;

        public static Texture2D EnmOrb;
        public static Texture2D EnmBlob;
        public static Texture2D EnmOrbRed;
        public static Texture2D EnmOrbGreen;
        public static Texture2D EnmOrbBlue;
        public static Texture2D EnmGeom;
        public static Texture2D EnmTri;
        public static Texture2D EnmUfo;
        public static Texture2D EnmDohSplinter;
        public static Texture2D EnmDoh;
        public static Texture2D EnmDohWeb;
        public static Texture2D EnmDieOrbTri;
        public static Texture2D EnmDieGeomUfo;

        public static Texture2D PwrSlowS;
        public static Texture2D PwrCatchC;
        public static Texture2D PwrExpandE;
        public static Texture2D PwrDisruptD;
        public static Texture2D PwrLaserL;
        public static Texture2D PwrExitB;
        public static Texture2D PwrLifeP;
        public static Texture2D PwrReduceR;
        public static Texture2D PwrMegaBallH;
        public static Texture2D PwrFastF;
        public static Texture2D PwrIllusionI;
        public static Texture2D PwrOrbitO;
        public static Texture2D PwrTwinT;
        public static Texture2D PwrRandom;
        public static Texture2D PwrMegaLaserM;
        public static Texture2D PwrElectricFence;


        public static Texture2D BallNormal;
        public static Texture2D BallMega;

        public static Texture2D VaEndNormal;
        public static Texture2D VaCenterNormal;
        public static Texture2D VaSideNormal;
        public static Texture2D VaExtensionNormal;
        public static Texture2D VaExtensionPlusNormal;
        public static Texture2D VaEndLaser;
        public static Texture2D VaCenterMegaLaser;
        public static Texture2D VaCenterLaser;
        public static Texture2D VaSideLaser;
        public static Texture2D VaExtensionLaser;
        public static Texture2D VaExtensionPlusLaser;
        public static Texture2D VaExplode;
        public static Texture2D VaRevive;
        public static Texture2D VaLaser;
        public static Texture2D VaMegaLaser;
        public static Texture2D VaCenterCatch;
        public static Texture2D VaSideCatch;
        public static Texture2D VaExtensionCatch;
        public static Texture2D VaExtensionPlusCatch;


        public static Texture2D CmnDohBoarder;
        public static Texture2D CmnLife;
        public static Texture2D CmnArkanoidShip;
        public static Texture2D CmnArkanoidLogo;
        public static Texture2D CmnArkanoidDxLogoA;
        public static Texture2D CmnArkanoidDxLogoB;

        public static Texture2D CmnElectricFence;

        public static Texture2D CmnTitle;
        public static Texture2D CmnTaito;
        public static Texture2D CmnEditInfo;
        public static Texture2D CmnTournament;

        public static Texture2D CmnStart;
        public static Texture2D CmnEdit;
        public static Texture2D CmnExit;
        public static Texture2D CmnDemo;
        public static Texture2D CmnDelete;
        public static Texture2D CmnMoveLeft;
        public static Texture2D CmnMoveRight;
        public static Texture2D CmnVerArkanoid;
        public static Texture2D CmnVerArkanoidDx;
        public static Texture2D CmnVerCustom;
        public static Texture2D CmnLoad;
        public static Texture2D CmnSmallFrame;
        public static Texture2D CmnSmallFrameEmpty;
        public static Texture2D CmnSmallFrameCustom;
        public static Texture2D CmnLeft;
        public static Texture2D CmnRight;
        public static Texture2D CmnSaved;
        public static Texture2D CmnCredits;
        public static Texture2D CmnADD;
        public static Texture2D FrmTop;
        public static Texture2D FrmSide;
        public static Texture2D FrmCorner;
        public static Texture2D FrmTopOpen;
        public static Texture2D FrmSideOpen;
        public static Texture2D FrmSideWarpOpen;
        public static Texture2D FrmSideWarp;

        public static void LoadContent(ContentManager content)
        {
            BgDoh = content.Load<Texture2D>("Background/bg01.png");
            BgPurpleCircuit = content.Load<Texture2D>("Background/bg02.png");
            BgRedCircuit = content.Load<Texture2D>("Background/bg03.png");
            BgDarkRedCircuit = content.Load<Texture2D>("Background/bg04.png");
            BgBlueCircuit = content.Load<Texture2D>("Background/bg05.png");
            BgDarkBlueCircuit = content.Load<Texture2D>("Background/bg06.png");
            BgCellularGreen = content.Load<Texture2D>("Background/bg07.png");
            BgDarkCellularGreen = content.Load<Texture2D>("Background/bg08.png");
            BgBlueHex = content.Load<Texture2D>("Background/bg09.png");
            BgDarkBlueHex = content.Load<Texture2D>("Background/bg10.png");
            BgDarkTech = content.Load<Texture2D>("Background/bg11.png");
            BgGreySkin = content.Load<Texture2D>("Background/bg12.png");
            BgGoldHive = content.Load<Texture2D>("Background/bg13.png");
            BgRedInnards = content.Load<Texture2D>("Background/bg14.png");
            BgGreenWeave = content.Load<Texture2D>("Background/bg15.png");
            BgGreyBevelHex = content.Load<Texture2D>("Background/bg16.png");

            BrkWhite = content.Load<Texture2D>("Bricks/n001.png");
            BrkYellow = content.Load<Texture2D>("Bricks/n002.png");
            BrkPink = content.Load<Texture2D>("Bricks/n003.png");
            BrkBlue = content.Load<Texture2D>("Bricks/n004.png");
            BrkRed = content.Load<Texture2D>("Bricks/n005.png");
            BrkGreen = content.Load<Texture2D>("Bricks/n006.png");
            BrkSkyBlue = content.Load<Texture2D>("Bricks/n007.png");
            BrkOrange = content.Load<Texture2D>("Bricks/n008.png");
            BrkSilver = content.Load<Texture2D>("Bricks/n009.png");
            BrkGold = content.Load<Texture2D>("Bricks/n010.png");
            BrkFlash = content.Load<Texture2D>("Bricks/n011.png");
            BrkRegen = content.Load<Texture2D>("Bricks/n012.png");
            BrkTeleport = content.Load<Texture2D>("Bricks/n013.png");
            BrkGoldSwap = content.Load<Texture2D>("Bricks/n014.png");
            BrkSilverSwap = content.Load<Texture2D>("Bricks/n015.png");
            BrkBlueSwap = content.Load<Texture2D>("Bricks/n016.png");
            BrkBlack = content.Load<Texture2D>("Bricks/n017.png");
            BrkBlackRegen = content.Load<Texture2D>("Bricks/n018.png");
            BrkTransmit = content.Load<Texture2D>("Bricks/n019.png");

             

            EnmOrb = content.Load<Texture2D>("Enemies/e001.png");
            EnmGeom = content.Load<Texture2D>("Enemies/e002.png");
            EnmTri = content.Load<Texture2D>("Enemies/e003.png");
            EnmUfo = content.Load<Texture2D>("Enemies/e004.png");
            EnmBlob = content.Load<Texture2D>("Enemies/e006.png");
            EnmOrbRed = content.Load<Texture2D>("Enemies/e007.png");
            EnmOrbGreen = content.Load<Texture2D>("Enemies/e008.png");
            EnmOrbBlue = content.Load<Texture2D>("Enemies/e009.png");
            EnmDohSplinter = content.Load<Texture2D>("Enemies/e005.png");
            EnmDoh = content.Load<Texture2D>("Common/d002.png");
            CmnADD = content.Load<Texture2D>("Common/IngameAdd.png");
            EnmDohWeb = content.Load<Texture2D>("Common/d003.png");
            EnmDieOrbTri = content.Load<Texture2D>("Enemies/ee01.png");
            EnmDieGeomUfo = content.Load<Texture2D>("Enemies/ee02.png");

            PwrSlowS = content.Load<Texture2D>("PowerUps/pu01.png");
            PwrCatchC = content.Load<Texture2D>("PowerUps/pu02.png");
            PwrExpandE = content.Load<Texture2D>("PowerUps/pu04.png");
            PwrDisruptD = content.Load<Texture2D>("PowerUps/pu05.png");
            PwrLaserL = content.Load<Texture2D>("PowerUps/pu03.png");
            PwrExitB = content.Load<Texture2D>("PowerUps/pu06.png");
            PwrLifeP = content.Load<Texture2D>("PowerUps/pu07.png");
            PwrReduceR = content.Load<Texture2D>("PowerUps/pu08.png");
            PwrMegaBallH = content.Load<Texture2D>("PowerUps/pu09.png");
            PwrFastF = content.Load<Texture2D>("PowerUps/pu10.png");
            PwrIllusionI = content.Load<Texture2D>("PowerUps/pu11.png");
            PwrOrbitO = content.Load<Texture2D>("PowerUps/pu12.png");
            PwrTwinT = content.Load<Texture2D>("PowerUps/pu13.png");
            PwrRandom = content.Load<Texture2D>("PowerUps/pu14.png");
            PwrMegaLaserM = content.Load<Texture2D>("PowerUps/pu15.png");
            PwrElectricFence =content.Load<Texture2D>("PowerUps/pu16.png");

            BallNormal = content.Load<Texture2D>("Vaus/b001.png");
            BallMega = content.Load<Texture2D>("Vaus/b002.png");

            VaEndNormal = content.Load<Texture2D>("Vaus/ve00.png");
            VaCenterNormal = content.Load<Texture2D>("Vaus/ve01.png");
            VaSideNormal = content.Load<Texture2D>("Vaus/ve02.png");
            VaExtensionNormal = content.Load<Texture2D>("Vaus/ve03.png");
            VaExtensionPlusNormal = content.Load<Texture2D>("Vaus/ve04.png");
            VaEndLaser = content.Load<Texture2D>("Vaus/vx00.png");
            VaCenterLaser = content.Load<Texture2D>("Vaus/vx01.png");
            VaCenterMegaLaser = content.Load<Texture2D>("Vaus/vm01.png");
            VaSideLaser = content.Load<Texture2D>("Vaus/vx02.png");
            VaExtensionLaser = content.Load<Texture2D>("Vaus/vx03.png");
            VaExtensionPlusLaser = content.Load<Texture2D>("Vaus/vx04.png");
            VaExplode = content.Load<Texture2D>("Vaus/p001.png");
            VaRevive = content.Load<Texture2D>("Vaus/p002.png");
            VaLaser = content.Load<Texture2D>("Common/l001.png");
            VaMegaLaser = content.Load<Texture2D>("Common/l002.png");
            VaCenterCatch = content.Load<Texture2D>("Vaus/vc01.png");
            VaSideCatch = content.Load<Texture2D>("Vaus/vc02.png");
            VaExtensionCatch = content.Load<Texture2D>("Vaus/vc03.png");
            VaExtensionPlusCatch = content.Load<Texture2D>("Vaus/vc04.png");


            CmnDohBoarder = content.Load<Texture2D>("Common/d001.png");
            CmnLife = content.Load<Texture2D>("Common/g001.png");
            CmnArkanoidShip = content.Load<Texture2D>("Common/g003.png");
            CmnArkanoidLogo = content.Load<Texture2D>("Common/t001.png");
            CmnArkanoidDxLogoA = content.Load<Texture2D>("Common/t004.png");
            CmnArkanoidDxLogoB = content.Load<Texture2D>("Common/t005.png");
            CmnStart = content.Load<Texture2D>("Common/t006.png");
            CmnEdit = content.Load<Texture2D>("Common/t007.png");
            CmnExit = content.Load<Texture2D>("Common/t008.png");
            CmnTitle = content.Load<Texture2D>("Common/t003.png");
            CmnTaito = content.Load<Texture2D>("Common/t002.png");
            CmnTournament = content.Load<Texture2D>("Common/t021.png");
            CmnEditInfo = content.Load<Texture2D>("Common/i002.png");

            CmnDemo  = content.Load<Texture2D>("Common/t009.png");
            CmnDelete = content.Load<Texture2D>("Common/t010.png");
            CmnMoveLeft = content.Load<Texture2D>("Common/t012.png");
            CmnMoveRight = content.Load<Texture2D>("Common/t011.png");
            CmnVerArkanoid = content.Load<Texture2D>("Common/t013.png");
            CmnVerArkanoidDx = content.Load<Texture2D>("Common/t014.png");
            CmnVerCustom = content.Load<Texture2D>("Common/t015.png");
            CmnLoad = content.Load<Texture2D>("Common/t016.png");
            CmnSmallFrame = content.Load<Texture2D>("Common/f001.png");
            CmnSmallFrameEmpty = content.Load<Texture2D>("Common/f002.png");
            CmnSmallFrameCustom = content.Load<Texture2D>("Common/f003.png");
            CmnLeft = content.Load<Texture2D>("Common/t018.png");
            CmnRight = content.Load<Texture2D>("Common/t017.png");
            CmnSaved = content.Load<Texture2D>("Common/t019.png");
            CmnCredits = content.Load<Texture2D>("Common/t020.png");


            CmnElectricFence = content.Load<Texture2D>("Common/l003.png");
                 
            FrmTop = content.Load<Texture2D>("Common/m007.png");
            FrmSide = content.Load<Texture2D>("Common/m006.png");
            FrmCorner = content.Load<Texture2D>("Common/m003.png");
            FrmTopOpen = content.Load<Texture2D>("Common/m002.png");
            FrmSideOpen = content.Load<Texture2D>("Common/m005.png");
            FrmSideWarpOpen = content.Load<Texture2D>("Common/m008.png");
            FrmSideWarp = content.Load<Texture2D>("Common/m001.png");
        }
    }
}