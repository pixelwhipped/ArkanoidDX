using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Levels
{
    public class LevelWad
    {
        public Texture2D Box;
        public Arkanoid Game;
        public bool IsCustom;
        public List<KeyValuePair<Level, Level>> Levels;
        public string Name;
        public Texture2D Title;

        public LevelWad(Arkanoid game, string name, Texture2D box, Texture2D title,
            List<KeyValuePair<Level, Level>> levels)
        {
            Game = game;
            Name = name;
            Box = box;
            Title = title;
            Levels = levels;
            IsCustom = false;
        }
    }
}