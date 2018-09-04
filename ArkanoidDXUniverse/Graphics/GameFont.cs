using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Graphics
{
    public static class Extension
    {
        public static void DrawString(this SpriteBatch spritebatch, GameFont font, string text, Vector2 textPos,
            Color color)
        {
            font.DrawText(spritebatch, (int) textPos.X, (int) textPos.Y, text, color);
                // (spritebatch, text, font, textPos, color);
        }
    }

    [XmlRoot("font")]
    public class FontFile
    {
        [XmlElement("info")]
        public FontInfo Info { get; set; }

        [XmlElement("common")]
        public FontCommon Common { get; set; }

        [XmlArray("pages")]
        [XmlArrayItem("page")]
        public List<FontPage> Pages { get; set; }

        [XmlArray("chars")]
        [XmlArrayItem("char")]
        public List<FontChar> Chars { get; set; }

        [XmlArray("kernings")]
        [XmlArrayItem("kerning")]
        public List<FontKerning> Kernings { get; set; }
    }

    public class FontInfo
    {
        private Rectangle _padding;

        private Point _spacing;

        [XmlAttribute("face")]
        public string Face { get; set; }

        [XmlAttribute("size")]
        public int Size { get; set; }

        [XmlAttribute("bold")]
        public int Bold { get; set; }

        [XmlAttribute("italic")]
        public int Italic { get; set; }

        [XmlAttribute("charset")]
        public string CharSet { get; set; }

        [XmlAttribute("unicode")]
        public int Unicode { get; set; }

        [XmlAttribute("stretchH")]
        public int StretchHeight { get; set; }

        [XmlAttribute("smooth")]
        public int Smooth { get; set; }

        [XmlAttribute("aa")]
        public int SuperSampling { get; set; }

        [XmlAttribute("padding")]
        public string Padding
        {
            get { return _padding.X + "," + _padding.Y + "," + _padding.Width + "," + _padding.Height; }
            set
            {
                var padding = value.Split(',');
                _padding = new Rectangle(Convert.ToInt32(padding[0]), Convert.ToInt32(padding[1]),
                    Convert.ToInt32(padding[2]), Convert.ToInt32(padding[3]));
            }
        }

        [XmlAttribute("spacing")]
        public string Spacing
        {
            get { return _spacing.X + "," + _spacing.Y; }
            set
            {
                var spacing = value.Split(',');
                _spacing = new Point(Convert.ToInt32(spacing[0]), Convert.ToInt32(spacing[1]));
            }
        }

        [XmlAttribute("outline")]
        public int OutLine { get; set; }
    }

    public class FontCommon
    {
        [XmlAttribute("lineHeight")]
        public int LineHeight { get; set; }

        [XmlAttribute("base")]
        public int Base { get; set; }

        [XmlAttribute("scaleW")]
        public int ScaleW { get; set; }

        [XmlAttribute("scaleH")]
        public int ScaleH { get; set; }

        [XmlAttribute("pages")]
        public int Pages { get; set; }

        [XmlAttribute("packed")]
        public int Packed { get; set; }

        [XmlAttribute("alphaChnl")]
        public int AlphaChannel { get; set; }

        [XmlAttribute("redChnl")]
        public int RedChannel { get; set; }

        [XmlAttribute("greenChnl")]
        public int GreenChannel { get; set; }

        [XmlAttribute("blueChnl")]
        public int BlueChannel { get; set; }
    }

    public class FontPage
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("file")]
        public string File { get; set; }
    }

    public class FontChar
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("x")]
        public int X { get; set; }

        [XmlAttribute("y")]
        public int Y { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlAttribute("xoffset")]
        public int XOffset { get; set; }

        [XmlAttribute("yoffset")]
        public int YOffset { get; set; }

        [XmlAttribute("xadvance")]
        public int XAdvance { get; set; }

        [XmlAttribute("page")]
        public int Page { get; set; }

        [XmlAttribute("chnl")]
        public int Channel { get; set; }
    }

    public class FontKerning
    {
        [XmlAttribute("first")]
        public int First { get; set; }

        [XmlAttribute("second")]
        public int Second { get; set; }

        [XmlAttribute("amount")]
        public int Amount { get; set; }
    }

    public class FontLoader
    {
        public static FontFile Load(Stream stream)
        {
            var deserializer = new XmlSerializer(typeof (FontFile));
            var file = (FontFile) deserializer.Deserialize(stream);
            return file;
        }
    }

    public class GameFont
    {
        private readonly Dictionary<char, FontChar> _characterMap;
        private readonly Texture2D _texture;

        public GameFont(FontFile fontFile, Texture2D fontTexture)
        {
            _texture = fontTexture;
            _characterMap = new Dictionary<char, FontChar>();

            foreach (var fontCharacter in fontFile.Chars)
            {
                var c = (char) fontCharacter.Id;
                _characterMap.Add(c, fontCharacter);
            }
        }

        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text, Color color)
        {
            var dx = x;
            var dy = y;
            foreach (var c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                    var position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);

                    spriteBatch.Draw(_texture, position, sourceRectangle, color);
                    dx += fc.XAdvance;
                }
            }
        }

        public Vector2 MeasureString(string text)
        {
            var dx = 0;
            var dy = 0;
            foreach (var c in text)
            {
                FontChar fc;
                if (!_characterMap.TryGetValue(c, out fc))
                    if (!_characterMap.TryGetValue(char.ToUpper(c), out fc)) continue;
                if (fc.Height > dy)
                    dy = fc.Height;
                dx += fc.XAdvance;
            }
            return new Vector2(dx, dy);
        }
    }
}