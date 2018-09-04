using System;
using ArkanoidDXUniverse.Levels;
using ArkanoidDXUniverse.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDXUniverse.Arena
{
    public class BossArena : PlayArena
    {
        public Doh Doh;

        public bool Finish;


        public BossArena(Arkanoid game,bool twoPlayer, LevelWadSelector selector, Vaus vaus)
            : base(game, twoPlayer, selector, vaus, 0)
        {
            Doh = new Doh(Game, this);
        }

        public override bool IsOver => !Doh.IsAlive;

        public override void Update(GameTime gameTime)
        {
            if (!Doh.IsAlive && Math.Abs(Doh.DeadFade - 0f) < float.Epsilon && !Finish)
            {
                Finish = true;
                if (Fade.FadeIn)
                {
                    Fade.DoFadeOut(() => { Game.Arena = new MenuArena(Game); });
                }
                else
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