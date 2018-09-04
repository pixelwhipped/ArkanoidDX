using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ArkanoidDXUniverse
{
    public class ResourceCompoment
    {
        public readonly Game Game;

        public ResourceCompoment(Game game)
        {
            Game = game;
        }

        public ContentManager Content => Game.Content;

        public virtual void LoadContent()
        {
        }
    }
}