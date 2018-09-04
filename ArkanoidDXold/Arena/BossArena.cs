using System;
using ArkanoidDX.Levels;
using ArkanoidDX.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Arena
{
    public class BossArena : PlayArena
    {

        public Doh Doh;

        public override bool IsOver { get { return !Doh.IsAlive; } }

        
        public BossArena(ArkanoidDX game, LevelWadSelector selector, Vaus vaus)
            : base(game, selector,vaus,0)
        {
            Doh = new Doh(Game,this);
        }

        public bool Finish ;
        public override void Update(GameTime gameTime)
        {
            if (!Doh.IsAlive && Math.Abs(Doh.DeadFade - 0f) < float.Epsilon && !Finish )
            {
                Finish = true;
                if (Fade.FadeIn)
                {
                    Fade.DoFadeOut(() =>
                                       {
                                           Game.Arena = new MenuArena(Game);
                                       });
                }else
                {
                    Fade.Evnt = () => Game.Arena = new MenuArena(Game);
                }
            }
            base.Update(gameTime);
            Doh.Update(gameTime);
        }
        public override void Draw(SpriteBatch batch)
        {
            DrawBackground(batch, BackGroundTypes.Doh);
            DrawFrameLeft(batch);
            Doh.Draw(batch);
            DrawFrameRight(batch);
            DrawEntries(batch);
            DrawWarps(batch);
            DrawEmimies(batch);
            DrawMap(batch);
            DrawBalls(batch);
            DrawLasers(batch);
            DrawVaus(batch);
            DrawShip(batch);
            DrawTitle(batch);
            DrawScore(batch);
        }

    }
}
