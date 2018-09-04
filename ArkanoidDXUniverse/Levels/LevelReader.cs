using System;
using System.IO;
using ArkanoidDXUniverse.Utilities;

namespace ArkanoidDXUniverse.Levels
{
    public class LevelReader // : ContentTypeReader<Level>
    {
        
        public static Level Read(string path)
        {
            var input = new StreamReader(AsyncIO.GetContentStream(path)).ReadToEnd();

            var err = "";
            try
            {
                var row = 0;
                var lines = input.Split('\r');
                var rows = Convert.ToInt32(lines[1]);
                var columns = Convert.ToInt32(lines[0]);
                var brickData = new int[rows, columns];
                var chanceData = new int[rows, columns];
                var powerData = new int[rows, columns];

                var tle = Convert.ToInt32(lines[2]);
                var tre = Convert.ToInt32(lines[3]);
                var srte = Convert.ToInt32(lines[4]);
                var srme = Convert.ToInt32(lines[5]);
                var slte = Convert.ToInt32(lines[6]);
                var slme = Convert.ToInt32(lines[7]);
                err = "background";
                var background = Convert.ToInt32(lines[8]);
                err = "enemyType";
                var enemyType = Convert.ToInt32(lines[9]);
                err = "maxEnimies";
                var maxEnimies = Convert.ToInt32(lines[10]);
                err = "maxEnimyRealeaseTime " + lines[11];
                var maxEnimyRealeaseTime = Convert.ToInt32(lines[11]);
                err = "minEnimyRealeaseTime " + lines[12];
                var minEnimyRealeaseTime = Convert.ToInt32(lines[12]);

                for (; row < rows; row++)
                {
                    err = "b row " + row;
                    var values = lines[row + 13].Split(' ');
                    for (var column = 0; column < columns; column++)
                    {
                        err = lines[row + 13] + " " + column;
                        brickData[row, column] = Convert.ToInt32(values[column]);
                    }
                }
                err = "b";
                for (; row < rows*2; row++)
                {
                    var values = lines[row + 13].Split(' ');
                    for (var column = 0; column < columns; column++)
                    {
                        chanceData[row - rows, column] = Convert.ToInt32(values[column]);
                    }
                }
                err = "c";
                for (; row < rows*3; row++)
                {
                    var values = lines[row + 13].Split(' ');
                    for (var column = 0; column < columns; column++)
                    {
                        powerData[row - rows*2, column] = Convert.ToInt32(values[column]);
                    }
                }
                return new Level(columns, rows, tle, tre, slte, srte, slme, srme, background, enemyType, maxEnimies,
                    maxEnimyRealeaseTime, minEnimyRealeaseTime, brickData, chanceData, powerData);
            }
            catch
            {
                throw new Exception(err);
            }
        }
    }
}