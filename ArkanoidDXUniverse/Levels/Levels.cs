using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Levels
{
    public static class Levels
    {
        public static List<KeyValuePair<Level, Level>> ArkanoidLevels;
        public static List<KeyValuePair<Level, Level>> ArkanoidDxLevels;
        public static List<KeyValuePair<Level, Level>> RevengeOfTheDohLevels;
        public static List<KeyValuePair<Level, Level>> ArkanoidTournamentLevels;

        public static LevelWad ArkanoidWad;
        public static LevelWad ArkanoidDxWad;
        public static LevelWad RevengeOfTheDohWad;
        public static LevelWad ArkanoidTournamentWad;

        public static List<LevelWad> Wads;

        public static void LoadContent(ContentManager content)
        {
            #region Arkanoid 

            ArkanoidLevels = new List<KeyValuePair<Level, Level>>
            {
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\001.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\002.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\003.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\004.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\005.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\006.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\007.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\008.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\009.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\010.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\011.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\012.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\013.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\014.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\015.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\016.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\017.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\018.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\019.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\020.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\021.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\022.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\023.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\024.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\025.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\026.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\027.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\028.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\029.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\030.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\031.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\032.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\033.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\034.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid\\Right\\035.level"), null)
            };

            #endregion

            #region Arkanoind DX

            ArkanoidDxLevels = new List<KeyValuePair<Level, Level>>
            {
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\001.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\002.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\003.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\004.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\005.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\006.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\007.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\008.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\009.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\010.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\011.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\012.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\013.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\014.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\015.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\016.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\017.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\018.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\019.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\020.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\021.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\022.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\023.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\024.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\025.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\026.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\027.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\028.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\029.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Arkanoid DX\\Right\\030.level"), null)
            };

            #endregion

            #region Revenge of the Doh

            RevengeOfTheDohLevels = new List<KeyValuePair<Level, Level>>
            {
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\001.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\001.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\002.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\002.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\003.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\003.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\004.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\004.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\005.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\005.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\006.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\006.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\007.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\007.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\008.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\008.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\009.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\009.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\010.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\010.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\011.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\011.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\012.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\012.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\013.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\013.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\014.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\014.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\015.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\015.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\016.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\016.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\017.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\017.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\018.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\018.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\019.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\019.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\020.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\020.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\021.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\021.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\022.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\022.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\023.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\023.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\024.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\024.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\025.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\025.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\026.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\026.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\027.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\027.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\028.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\028.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\029.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\029.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\030.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\030.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\031.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\031.level")),
                new KeyValuePair<Level, Level>(
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Right\\032.level"),
                    LevelReader.Read("Content\\Level\\Revenge Of The Doh\\Left\\032.level"))
            };

            #endregion

            #region Tournament

            ArkanoidTournamentLevels = new List<KeyValuePair<Level, Level>>
            {
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\001.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\002.level"),
                    LevelReader.Read("Content\\Level\\Tournament\\Left\\001.level")),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\003.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\004.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\005.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\006.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\007.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\008.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\009.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\010.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\011.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\012.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\013.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\014.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\015.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\016.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\018.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\019.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\020.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\021.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\022.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\023.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\024.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\025.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\026.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\027.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\028.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\029.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\030.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\031.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\032.level"), null),
                new KeyValuePair<Level, Level>(LevelReader.Read("Content\\Level\\Tournament\\Right\\033.level"), null)
            };

            #endregion
        }


        public static void LoadWads(Arkanoid game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            ArkanoidWad = new LevelWad(game, "Arkanoid", content.Load<Texture2D>("Level/Arkanoid/Box.png"),
                content.Load<Texture2D>("Level/Arkanoid/Title.png"), ArkanoidLevels);
            ArkanoidDxWad = new LevelWad(game, "Arkanoid DX", content.Load<Texture2D>("Level/Arkanoid DX/Box.png"),
                content.Load<Texture2D>("Level/Arkanoid DX/Title.png"), ArkanoidDxLevels);
            RevengeOfTheDohWad = new LevelWad(game, "Revenge Of The Doh",
                content.Load<Texture2D>("Level/Revenge Of The Doh/Box.png"),
                content.Load<Texture2D>("Level/Revenge Of The Doh/Title.png"), RevengeOfTheDohLevels);
            ArkanoidTournamentWad = new LevelWad(game, "Arkanoid Tournament",
                content.Load<Texture2D>("Level/Tournament/Box.png"),
                content.Load<Texture2D>("Level/Tournament/Title.png"), ArkanoidTournamentLevels);
            Wads = new List<LevelWad> {ArkanoidWad, ArkanoidDxWad, RevengeOfTheDohWad, ArkanoidTournamentWad};
        }

        public static void UpdateCustom(Arkanoid game)
        {
            var folder = AsyncIO.CreateFolderAsync(ApplicationData.Current.RoamingFolder, "Custom");
            var files = AsyncIO.GetFilesAsync(folder).Where(f => f.Name.EndsWith(".level"));
            var levels = files.ToList();
            if (levels.Count != 0)
            {
                var levelList =
                    levels.Select(
                        f => new KeyValuePair<Level, Level>(LevelIO.ReadLevel(AsyncIO.ReadTextFileAsync(f)), null))
                        .ToList();
                var customWad = new LevelWad(game, "Custom", Textures.CmnTitle, Textures.CmnVerCustom, levelList);
                var ow = Wads.Find(p => p.Name == "Custom");
                if (ow != null)
                    Wads.Remove(ow);
                Wads.Add(customWad);
            }
        }
    }
}