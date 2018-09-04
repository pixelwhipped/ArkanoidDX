using Microsoft.Xna.Framework;

namespace ArkanoidDXUniverse.Interfaces
{
    public interface ILocatable
    {
        Vector2 Motion { get; set; }
        Rectangle Bounds { get; }
        Vector2 Location { get; set; }
        Vector2 Center { get; }
        float Width { get; }
        float Height { get; }
        float X { get; set; }
        float Y { get; set; }
    }
}