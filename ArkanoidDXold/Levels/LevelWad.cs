using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Levels
{
    public class LevelWad
    {
        public ArkanoidDX Game;
        public String Name;
        public Texture2D Box;
        public Texture2D Title;
        public List<KeyValuePair<Level, Level>> Levels;
        public bool IsCustom;

        public LevelWad(ArkanoidDX game, String name, Texture2D box, Texture2D title, List<KeyValuePair<Level, Level>> levels)
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
