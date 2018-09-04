using System;
using System.Collections.Generic;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Graphics;
using ArkanoidDXUniverse.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Objects
{
    public enum DohState
    {
        OpenMouth,
        CloseMouth
    }

    public class Doh : GameObject
    {
        public const int MaxLife = 40;

        public float DeadFade = 1f;

        public Rectangle DohBdrRect;
        public Rectangle DohRect;


        public DohState DohState;

        public float Hit;
        public int Life;


        public TimeSpan NextRelease;

        public PlayArena PlayArena;
        public List<DohSplinter> Splinters;

        public Doh(Arkanoid game, PlayArena playArena) : base(game)
        {
            PlayArena = playArena;
            DohBdrRect =
                new Rectangle((int) (PlayArena.Bounds.Right - (Sprites.CmnDohBoarder.Width + Sprites.BgDoh.Width)),
                    PlayArena.Bounds.Top,
                    (int) Sprites.CmnDohBoarder.Width, (int) Sprites.CmnDohBoarder.Height);
            DohRect = new Rectangle((int) (DohBdrRect.Center.X - Sprites.EnmDoh.Center.X),
                (int) (Sprites.BgDoh.Height + PlayArena.Bounds.Top),
                (int) Sprites.EnmDoh.Width, (int) Sprites.EnmDoh.Height);
            Splinters = new List<DohSplinter>();
            Life = MaxLife;
            DohState = DohState.CloseMouth;
            Texture.ToStart();
            Texture.SetAnimation(AnimationState.Pause);
            Web.ToStart();
            Web.SetAnimation(AnimationState.Pause);
            // Texture.OnFinish = AddSplinter;
            SetNextRelease();
            Hit = 0f;
        }

        public override bool IsAlive => Life > 0;

        public sealed override Sprite Texture => Sprites.EnmDoh;

        public Sprite Border => Sprites.CmnDohBoarder;

        public Sprite Web => Sprites.EnmDohWeb;


        public void SetNextRelease()
        {
            if (Life < MaxLife/4)
            {
                NextRelease = new TimeSpan(0, 0, 0, 1);
            }
            else if (Life < MaxLife/3)
            {
                NextRelease = new TimeSpan(0, 0, 0, 1, 500);
            }
            else if (Life < MaxLife/2)
            {
                NextRelease = new TimeSpan(0, 0, 0, 2);
            }
            else if (Life <= MaxLife)
            {
                NextRelease = new TimeSpan(0, 0, 0, 3);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!IsAlive)
            {
                DeadFade = MathHelper.Clamp(DeadFade - .008f, 0f, 1f);
            }
            Splinters.RemoveAll(p => !p.IsAlive);
            foreach (var s in Splinters)
                s.Update(gameTime);
            Hit = MathHelper.Clamp(Hit - .1f, 0f, 1f);
            if (IsAlive)
            {
                NextRelease -= gameTime.ElapsedGameTime;
                foreach (var b in PlayArena.Balls)
                {
                    Direction d;
                    CollisionPoint c;
                    if (Collisions.IsCollision(b, DohRect, out d, out c))
                    {
                        Life--;
                        b.Deflect(c, d);
                        Game.Sounds.VausEnter.Play();
                        Hit = 1f;
                        if (Life == 0)
                            PlayArena.Vaus.AddScore(Scoring.Doh);
                    }
                }

                if (NextRelease < TimeSpan.Zero)
                {
                    Texture.SetAnimation(AnimationState.Play);
                    Web.SetAnimation(AnimationState.Play);
                    Texture.OnFinish = () =>
                    {
                        Splinters.Add(new DohSplinter(Game, PlayArena,
                            new Vector2(
                                DohRect.Center.X -
                                Sprites.EnmDohSplinter.Center.X,
                                DohRect.Height*.55f + DohRect.Top)));
                        Texture.SetAnimation(AnimationState.Rewind);
                        Web.SetAnimation(AnimationState.Rewind);
                        Texture.OnFinish = () =>
                        {
                            Texture.SetAnimation(AnimationState.Pause);
                            Web.SetAnimation(AnimationState.Pause);
                        };
                        SetNextRelease();
                    };
                }
            }
        }


        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Sprites.CmnDohBoarder, DohBdrRect, Color.White*PlayArena.Fade.Fade);

            batch.Draw(Sprites.EnmDoh, DohRect,
                Color.Lerp(Color.Purple, Color.Red, (float) Life/MaxLife)*PlayArena.Fade.Fade*DeadFade);
            if (Hit > 0f)
            {
                batch.Draw(Sprites.EnmDohWeb, DohRect,
                    Color.White*PlayArena.Fade.Fade);
            }
            if (Math.Abs(DeadFade - 1f) > float.Epsilon)
            {
                batch.Draw(Sprites.EnmDohWeb, DohRect,
                    Color.White*PlayArena.Fade.Fade);
                var v = Fonts.TextFont.MeasureString("Noo00!!!");
                batch.DrawString(Fonts.TextFont, "Noo00!!!",
                    new Vector2(DohBdrRect.Center.X - v.X/2f, DohBdrRect.Bottom), Color.White*DeadFade);
            }
            foreach (var s in Splinters)
                s.Draw(batch);
        }
    }
}