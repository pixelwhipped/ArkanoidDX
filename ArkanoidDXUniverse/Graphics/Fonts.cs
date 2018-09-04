using System;
using System.Diagnostics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Graphics
{
    public static class Fonts
    {
        public static GameFont GameFont;
        public static GameFont TextFont;
        public static GameFont SmallFont;
        public static GameFont KeysFont;
        public static GameFont KeysEFont;
        public static GameFont ArtFontGrey;
        public static GameFont ArtFontBlue;

        public static void LoadContent(ContentManager content)
        {
            try
            {
                //var x = content.Load<Texture2D>("Fonts\\font001.xnb");

                var fontFilePath = "Content\\Fonts\\font001.fnt";
                var fontFile = FontLoader.Load(AsyncIO.GetContentStream(fontFilePath));
                var fontTexture = content.Load<Texture2D>("Fonts/font001_0");

                GameFont = new GameFont(fontFile, fontTexture); //content.Load<Texture2D>("Fonts/font001.png"));
                TextFont = GameFont; //new GameFont(content.Load<Texture2D>("Fonts/font002.png"));
                SmallFont = GameFont; // new GameFont(content.Load<Texture2D>("Fonts/font003.png"));
                KeysFont = GameFont; //new GameFont(content.Load<Texture2D>("Fonts/font004.png"));
                KeysEFont = GameFont; //new GameFont(content.Load<Texture2D>("Fonts/font005.png"));
                ArtFontGrey = GameFont; //new GameFont(content.Load<Texture2D>("Fonts/font006.png"));
                ArtFontBlue = GameFont; //new GameFont(content.Load<Texture2D>("Fonts/font007.png"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }
    }
}