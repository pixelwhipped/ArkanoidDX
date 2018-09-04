using ArkanoidDX.Arena;
using ArkanoidDX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidDX.Objects
{
    public class Laser : GameObject
    {
        public  PlayArena PlayArena;
        public Laser(ArkanoidDX game, PlayArena playArena, Vector2 location, Vector2 motion):base(game)
        {
            PlayArena = playArena;
            Motion = motion;
            Location = location;
        }

        public override Sprite Texture
        {
            get { return Sprites.VaLaser; }
        }

        public override bool IsAlive
        {
            get { return Location.Y > Game.ArenaArea.Y; }
        }

        public virtual void Update(GameTime gameTime)
        {
            CheckEnemyLaserCollision();
            CheckLaserMapCollision();
            Location += Motion;
            Texture.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!IsAlive) return;
            batch.Draw(Texture, Location + Game.Shadow, Game.ShadowColor);
            batch.Draw(Texture, Location, Color.White);
        }

        public void Die()
        {
            Location = new Vector2(0, -Texture.Height);
        }

        public void CheckEnemyLaserCollision()
        {
            foreach (var e in PlayArena.Enimies)
            {
                if (!e.IsAlive || e.IsExploding) continue;
                Direction d;
                CollisionPoint c;
                if (!Collisions.IsCollision(e, this, out d, out c)) continue;
                e.Die();
            }
        }

        public void CheckLaserMapCollision()
        {
            foreach (var brick in PlayArena.LevelMap.BrickMap)
            {
                if (!brick.IsAlive) continue;
                if (!Utilities.IsCollision(this, brick) || !brick.IsAlive) continue;
                brick.Die();
                Die();
            }
        }
    }

    public class MegaLaser : Laser
    {
        public MegaLaser(ArkanoidDX game, PlayArena playArena, Vector2 location, Vector2 motion)
            : base(game,playArena,location,motion)
        {
        }

        public override Sprite Texture
        {
            get { return Sprites.VaMegaLaser; }
        }

        public override bool IsAlive
        {
            get { return Location.Y > Game.ArenaArea.Y; }
        }

        public override void Update(GameTime gameTime)
        {
            CheckEnemyLaserCollision();
            CheckLaserMapCollision();
            Location += Motion;
            Texture.Update(gameTime);
            if(Center.X<PlayArena.X||Center.X>PlayArena.Bounds.Right)
            {
                Motion = new Vector2(Motion.X*-1, Motion.Y);
            }
        }     
    }
}