namespace ArkanoidDX.Levels
{

    public class Level
    {
        public const int ClassicBricksWide = 11;
        public const int ClassicBricksHigh = 17;
        public const int TournamentBricksWide = 13;
        public const int TournamentBricksHigh = 19;
        public int BricksWide { get; set; } //11
        public int BricksHigh { get; set; } //17

        public int TopLeftEntryEnable { get; set; }
        public int TopRightEntryEnable { get; set; }
        public int SideRightTopEntryEnable { get; set; }
        public int SideRightMidEntryEnable { get; set; }
        public int SideLeftTopEntryEnable { get; set; }
        public int SideLeftMidEntryEnable  { get; set; }

        public int Background { get; set; }
        public int EnemyType { get; set; }
        public int MaxEnimies { get; set; }
        public int MaxEnimyRealeaseTime { get; set; }
        public int MinEnimyRealeaseTime { get; set; }

        public int[,] Bricks { get; set; }
        public int[,] Chance { get; set; }
        public int[,] Power  { get; set; }
        public string KnownEditName;

        public Level(int w, int h,int tle, int tre, int slte, int srte, int slme, int srme, int background, int enimyType, int maxEnimies, int minEnimyRt, int maxEnimyRt, int[,] bricks, int[,] chance, int[,] power)
        {
            BricksWide = w;
            BricksHigh = h;
            TopLeftEntryEnable = tle;
            TopRightEntryEnable = tre;
            SideRightTopEntryEnable = srte;
            SideRightMidEntryEnable = srme;
            SideLeftTopEntryEnable = slte;
            SideLeftMidEntryEnable = slme;

            Background = background;
            EnemyType = enimyType;
            MaxEnimies = maxEnimies;
            MaxEnimyRealeaseTime = maxEnimyRt;
            MinEnimyRealeaseTime = minEnimyRt;
            Bricks = bricks;
            Chance = chance;
            Power = power;
        }

        public void SetBrickValue(int row, int column, int value)
        {
            Bricks[row, column] = value;
        }

        public void SetChanceValue(int row, int column, int value)
        {
            Chance[row, column] = value;
        }

        public void SetPowerValue(int row, int column, int value)
        {
            Power[row, column] = value;
        }

        public int GetBrickValue(int row, int column)
        {
            return Bricks[row, column];
        }

        public int GetChanceValue(int row, int column)
        {
            return Chance[row, column];
        }

        public int GetPowerValue(int row, int column)
        {
            return Power[row, column];
        }

        public static Level EmptyLevel(int w, int h)
        {
            var rows = h;
            var columns = w;

            var brickData = new int[rows, columns];
            var chanceData = new int[rows, columns];
            var powerData = new int[rows, columns];

            for (var row = 0; row < h; row++)
            {
                for (var column = 0; column < w; column++)
                {
                    brickData[row, column] = 10;
                    chanceData[row, column] = 0;
                    powerData[row, column] = 0;
                }
            }
            return new Level(w,h, 1, 1, 1, 1, 1, 1, 0, 0, 5, 15, 30, brickData, chanceData, powerData);
        }
    }    
}
