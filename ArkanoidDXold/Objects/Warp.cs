using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = ArkanoidDX.Graphics.IDrawable;

namespace ArkanoidDX.Objects
{ 
    public class Warp : IDrawable
    {
        public Sprite Texture { get { return (IsOpening || IsClosing) ? Entry: IsOpen ? WarpEntry:Entry ; } }
        public Sprite Entry;
        public bool Flip;
        public bool IsClosing;
        public bool IsOpen;
        public bool IsOpening;
        public Vector2 Location;
        public Sprite WarpEntry;

        public Warp(Vector2 location, bool flip)
        {
            Entry = Sprites.FrmSideWarpOpen;
            WarpEntry = Sprites.FrmSideWarp;
            Location = location;
            Flip = flip;
        }

        public void Update(GameTime gameTime)
        {
            Entry.Update(gameTime);
            WarpEntry.Update(gameTime);
        }

        public void Open()
        {
            Entry.SetAnimation(AnimationState.Play);
            IsOpening = true;            
            IsClosing = false;
            Entry.OnFinish = () =>
                                 {
                                     Entry.SetAnimation(AnimationState.Pause);
                                     IsOpening = false;
                                     IsOpen = true;
                                 };
        }

        public void Close(Routine e)
        {
            IsClosing = true;
            IsOpening = false;
            Entry.ToEnd();
            Entry.SetAnimation(AnimationState.Rewind);
            Entry.OnFinish = () =>
                                 {
                                     Entry.SetAnimation(AnimationState.Pause);
                                     IsOpen = false;
                                     IsClosing = false;
                                     e();
                                 };
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Location, Color.White, 1, Flip);
        }
    }
}