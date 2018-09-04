using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Graphics
{    
    public enum AnimationState
    {
        Play,
        Pause,
        Stop,
        Rewind,
        LoopPlay,
        LoopRewind
    }

    public class Sprite
    {
        public AnimationState Animation;
        public int Frame;
        public int Frames;
        public ArkanoidDX Game;
        public Texture2D Map;
        public Routine OnFinish;
        public TimeSpan Rate;
        public TimeSpan Time;
        public float SubScale;

        public Sprite(ArkanoidDX game, int frames, TimeSpan rate, AnimationState anim, Texture2D map,float subScale =1)
        {
            Game = game;
            Map = map;
            Animation = anim;
            SubScale = subScale;
            Rate = rate;
            Frames = frames;
            OnFinish += () => { };
        }

        
        public float Width
        {
            get { return GetSource().Width * (Game.Scale.X * SubScale); }
        }

        public float Height
        {
            get { return GetSource().Height * (Game.Scale.Y * SubScale); }
        }

        public Vector2 Center
        {
            get { return new Vector2(Width/2, Height/2); }
        }

        public void SetAnimation(AnimationState anim)
        {
            Animation = anim;
        }

        public void ToStart()
        {
            Frame = 0;
        }

        public void ToEnd()
        {
            Frame = Frames - 1;
        }

        public void Update(GameTime gameTime)
        {
            Time += gameTime.ElapsedGameTime;
            if (Time < Rate) return;
            Time = new TimeSpan(0);
            switch (Animation)
            {
                case AnimationState.LoopPlay:
                case AnimationState.Play:
                    Frame++;
                    if (Frame >= Frames)
                    {
                        if (Animation == AnimationState.LoopPlay)
                        {
                            Frame = 0;
                        }
                        else
                        {
                            Frame = Frames - 1;
                            OnFinish();
                        }
                    }
                    break;
                case AnimationState.LoopRewind:
                case AnimationState.Rewind:
                    Frame--;
                    if (Frame <= 0)
                    {
                        if (Animation == AnimationState.LoopRewind)
                        {
                            Frame = Frames - 1;
                        }
                        else
                        {
                            Frame = 0;
                            OnFinish();
                        }
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch batch, Vector2 location, Color? color = null, float scale = 1f)
        {
            batch.Draw(this, location, color, scale);
        }

        public Rectangle GetSource()
        {
            var frameWidth = Map.Width/Frames;
            return new Rectangle(frameWidth*Frame, 0, frameWidth, Map.Height);
        }
    }
}