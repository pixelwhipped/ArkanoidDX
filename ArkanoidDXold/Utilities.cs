using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using ArkanoidDX.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX
{
    public static class Utilities
    {
        public static bool DoesFileExistAsync(StorageFolder folder, string fileName)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var f = await folder.GetFileAsync(fileName);
                    return f !=null;
                }
                catch
                {
                    return false;
                }
            }).Result;
        }

        public static bool DoesFolderExistAsync(StorageFolder folder, string folderName)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var f = await folder.GetFolderAsync(folderName);
                    return f != null;
                }
                catch
                {
                    return false;
                }
            }).Result;
        }

        public static IList<StorageFile> GetFilesAsync(StorageFolder folder)
        {
            var files = Task.Run(async () =>
            {
                try
                {
                    return await folder.GetFilesAsync();
                }
                catch (Exception e)
                {
                    return null;
                }
            }).Result;
            var ret = new List<StorageFile>();
            if (files != null) ret.AddRange(files);
            return ret;
        }

        public static StorageFolder CreateFolderAsync(StorageFolder folder, string folderName)
        {
            return Task.Run(async () =>
            {
                try
                {
                    return await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                }
                catch
                {
                    return null;
                }
            }).Result;
        }

        public static StorageFile CreateFileAsync(StorageFolder folder, string fileName)
        {
            return Task.Run(async () =>
            {
                try
                {
                    return await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                }
                catch
                {
                    return null;
                }
            }).Result;
        }
        

        public static string ReadTextFileAsync(StorageFile file)
        {
            return Task.Run(async () =>
            {
                return await FileIO.ReadTextAsync(file);
            }).Result;
        }



        public static T RandomEnum<T>()
        {
            return Enum
                .GetValues(typeof (T))
                .Cast<T>()
                .OrderBy(x => ArkanoidDX.Random.Next())
                .FirstOrDefault();
        }

        public static bool IsCollision(Rectangle a, Rectangle b, bool checkInside = false)
        {
            return a.Intersects(b) || (checkInside && (a.Contains(b) || b.Contains(a)));
        }

        public static bool IsCollision(ILocatable a, ILocatable b, bool checkInside = false)
        {
            return a.Bounds.Intersects(b.Bounds) ||
                   (checkInside && (a.Bounds.Contains(b.Bounds) || b.Bounds.Contains(a.Bounds)));
        }

        public static void DrawLine(this SpriteBatch batch, Vector2 point1, Vector2 point2, Color color,
                                    float width = 1f)
        {
            var angle = (float) Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(ArkanoidDX.Pixel, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        public static void DrawRectangle(this SpriteBatch batch, Rectangle rect, Color color, float width = 1f)
        {
            batch.DrawLine(new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y), color, width);
            batch.DrawLine(new Vector2(rect.X + rect.Width, rect.Y),
                           new Vector2(rect.X + rect.Width, rect.Y + rect.Height), color, width);
            batch.DrawLine(new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
                           new Vector2(rect.X, rect.Y + rect.Height), color, width);
            batch.DrawLine(new Vector2(rect.X, rect.Y + rect.Height), new Vector2(rect.X, rect.Y), color, width);
        }

        public static void FillRectangle(this SpriteBatch batch, Rectangle rect, Color color)
        {
            batch.Draw(ArkanoidDX.Pixel, rect,  color);
        }


        public static bool ChanceIn(int chance)
        {
            var a = ArkanoidDX.Random.Next(chance);
            var b = ArkanoidDX.Random.Next(chance);
            return a==b;
        }

        public static async void WriteLevel(StorageFile f, Level value)
        {
            var output = new List<string>();
            output.Add(value.BricksWide.ToString());
            output.Add(value.BricksHigh.ToString());
            output.Add(value.TopLeftEntryEnable.ToString());  //0
            output.Add(value.TopRightEntryEnable.ToString());
            output.Add(value.SideRightTopEntryEnable.ToString());
            output.Add(value.SideRightMidEntryEnable.ToString());
            output.Add(value.SideLeftTopEntryEnable.ToString());
            output.Add(value.SideLeftMidEntryEnable.ToString()); //5

            output.Add(value.Background.ToString());
            output.Add(value.EnemyType.ToString());
            output.Add(value.MaxEnimies.ToString());
            output.Add(value.MaxEnimyRealeaseTime.ToString());  //9
            output.Add(value.MinEnimyRealeaseTime.ToString());  //10

            string s;
            for (var row = 0; row < value.BricksHigh; row++)
            {
                s = string.Empty;
                for (var column = 0; column < value.BricksWide; column++)
                {
                    s = s + value.GetBrickValue(row,column) + " ";
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

        public static Level ReadLevel(String input)
        {
            try
            {

                var row = 0;
                var lines = input.Split(new[] {'\n'});
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
                    var values = lines[row + 13].Split(new[] {' '});
                    for (var column = 0; column < columns; column++)
                    {
                        brickData[row, column] = Convert.ToInt32(values[column]);
                    }
                }

                for (; row < (rows*2); row++)
                {
                    var values = lines[row + 13].Split(new[] {' '});
                    for (var column = 0; column < columns; column++)
                    {
                        chanceData[(row - rows), column] = Convert.ToInt32(values[column]);
                    }
                }

                for (; row < (rows*3); row++)
                {
                    var values = lines[row + 13].Split(new[] {' '});
                    for (var column = 0; column < columns; column++)
                    {
                        powerData[row - (rows*2), column] = Convert.ToInt32(values[column]);
                    }
                }
                return new Level(columns, rows, tle, tre, slte, srte, slme, srme, background, enemyType, maxEnimies,
                    maxEnimyRealeaseTime, minEnimyRealeaseTime, brickData, chanceData, powerData);
            }
            catch (Exception e)
            {
                return Level.EmptyLevel(Level.ClassicBricksWide, Level.ClassicBricksHigh);
            }

        }

       

    }
}
