using System;
using System.Collections.Generic;
using Windows.Storage;
using ArkanoidDXUniverse.Levels;

namespace ArkanoidDXUniverse.Utilities
{
    public static class LevelIO
    {
        public static async void WriteLevel(StorageFile f, Level value)
        {
            var output = new List<string>
            {
                value.BricksWide.ToString(),
                value.BricksHigh.ToString(),
                value.TopLeftEntryEnable.ToString(),
                value.TopRightEntryEnable.ToString(),
                value.SideRightTopEntryEnable.ToString(),
                value.SideRightMidEntryEnable.ToString(),
                value.SideLeftTopEntryEnable.ToString(),
                value.SideLeftMidEntryEnable.ToString(),
                value.Background.ToString(),
                value.EnemyType.ToString(),
                value.MaxEnemies.ToString(),
                value.MaxEnemyReleaseTime.ToString(),
                value.MinEnemyReleaseTime.ToString()
            };

            string s;
            for (var row = 0; row < value.BricksHigh; row++)
            {
                s = string.Empty;
                for (var column = 0; column < value.BricksWide; column++)
                {
                    s = s + value.GetBrickValue(row, column) + " ";
                }
                output.Add(s.Trim());
            }
            for (var row = 0; row < value.BricksHigh; row++)
            {
                s = string.Empty;
                for (var column = 0; column < value.BricksWide; column++)
                {
                    s = s + value.GetChanceValue(row, column) + " ";
                }
                output.Add(s.Trim());
            }
            for (var row = 0; row < value.BricksHigh; row++)
            {
                s = string.Empty;
                for (var column = 0; column < value.BricksWide; column++)
                {
                    s = s + value.GetPowerValue(row, column) + " ";
                }
                output.Add(s.Trim());
            }
            await FileIO.WriteLinesAsync(f, output);
        }

        public static Level ReadLevel(string input)
        {
            try
            {
                var row = 0;
                var lines = input.Split('\n');
                var columns = Convert.ToInt32(lines[0]);
                var rows = Convert.ToInt32(lines[1]);


                var brickData = new int[rows, columns];
                var chanceData = new int[rows, columns];
                var powerData = new int[rows, columns];

                var tle = Convert.ToInt32(lines[2]);
                var tre = Convert.ToInt32(lines[3]);
                var srte = Convert.ToInt32(lines[4]);
                var srme = Convert.ToInt32(lines[5]);
                var slte = Convert.ToInt32(lines[6]);
                var slme = Convert.ToInt32(lines[7]);

                var background = Convert.ToInt32(lines[8]);

                var enemyType = Convert.ToInt32(lines[9]);

                var maxEnimies = Convert.ToInt32(lines[10]);

                var maxEnimyRealeaseTime = Convert.ToInt32(lines[11]);
                var minEnimyRealeaseTime = Convert.ToInt32(lines[12]);

                for (; row < rows; row++)
                {
                    var values = lines[row + 13].Split(' ');
                    for (var column = 0; column < columns; column++)
                    {
                        brickData[row, column] = Convert.ToInt32(values[column]);
                    }
                }

                for (; row < rows*2; row++)
                {
                    var values = lines[row + 13].Split(' ');
                    for (var column = 0; column < columns; column++)
                    {
                        chanceData[row - rows, column] = Convert.ToInt32(values[column]);
                    }
                }

                for (; row < rows*3; row++)
                {
                    var values = lines[row + 13].Split(' ');
                    for (var column = 0; column < columns; column++)
                    {
                        powerData[row - rows*2, column] = Convert.ToInt32(values[column]);
                    }
                }
                return new Level(columns, rows, tle, tre, slte, srte, slme, srme, background, enemyType, maxEnimies,
                    minEnimyRealeaseTime, maxEnimyRealeaseTime, brickData, chanceData, powerData);
            }
            catch (Exception)
            {
                return Level.EmptyLevel(Level.ClassicBricksWide, Level.ClassicBricksHigh);
            }
        }
    }
}