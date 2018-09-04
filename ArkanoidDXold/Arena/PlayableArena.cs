using System.Collections.Generic;
using ArkanoidDX.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Arena
{
    public class PlayableArena : BaseArena
    {
        public List<Capsule> Capsules;
        public List<Enemy> Enimies;
        public List<Ball> Balls;
        public List<Laser> Lasers;
        public List<ISpawner> EnimyEntries;
        public Vaus Vaus;
        

        public PlayableArena(ArkanoidDX game)
            : base(game)
        {

            EnimyEntries = new List<ISpawner>();
            Capsules = new List<Capsule>();
            Enimies = new List<Enemy>();
            Balls = new List<Ball>();
            Lasers = new List<Laser>();
        }

       
        public void UpdatePlayRemoveDeadObjects()
        {
            Balls.RemoveAll(p => !p.IsAlive);
            Enimies.RemoveAll(p => !p.IsAlive);
            Lasers.RemoveAll(p => !p.IsAlive);
            Capsules.RemoveAll(e => !e.IsAlive);
        }
        public void UpdateCapsules(GameTime gameTime)
        {
            foreach (var c in Capsules)
                c.Update(gameTime);
        }

        public void UpdateVause(GameTime gameTime)
        {
            Vaus.Update(gameTime);
        }
        public void UpdateLasers(GameTime gameTime)
        {
            foreach (var l in Lasers)
            {
                l.Update(gameTime);
            }
        }
        public void UpdateBalls(GameTime gameTime)
        {
            foreach (var b in Balls)
            {
                b.Update(gameTime);
            }
        }

        public void DrawBalls(SpriteBatch batch)
        {
            foreach (var b in Balls)
                b.Draw(batch);
        }
        public void DrawLasers(SpriteBatch batch)
        {
            foreach (var l in Lasers)
                l.Draw(batch);
        }
        public void DrawVaus(SpriteBatch batch)
        {
            Vaus.Draw(batch);
        }
        public void DrawCapsules(SpriteBatch batch)
        {
            foreach (var c in Capsules)
                c.Draw(batch);
        }
        public void DrawEmimies(SpriteBatch batch)
        {
            foreach (var e in Enimies)
                e.Draw(batch);
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePlayRemoveDeadObjects();            
            UpdateVause(gameTime);
            UpdateLasers(gameTime);
            UpdateBalls(gameTime);
            UpdateCapsules(gameTime);
            base.Update(gameTime);
        }
    }
}
