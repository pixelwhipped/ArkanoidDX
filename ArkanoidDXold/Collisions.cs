using ArkanoidDX.Arena;
using ArkanoidDX.Objects;
using Microsoft.Xna.Framework;

namespace ArkanoidDX
{
    public static class Collisions
    {

        public static bool IsCollision(ILocatable me, ILocatable target, out Direction from, out CollisionPoint where)
        {
            var prjTarget = new Rectangle((int)(target.Bounds.X + target.Motion.X), (int)(target.Bounds.Y + target.Motion.Y),
                                          target.Bounds.Width, target.Bounds.Height);
            return IsCollision(me, prjTarget, out from, out where);

        }


        public static bool IsCollision(ILocatable me, Rectangle target, out Direction from, out CollisionPoint where)
        {
            var prjMe = new Rectangle((int)(me.Bounds.X + me.Motion.X), (int)(me.Bounds.Y + me.Motion.Y), me.Bounds.Width,
                                      me.Bounds.Height);
            var prjTarget = target;

            from = Direction.Stop;
            if (me.Motion.X < 0)
            {
                from = Direction.Left;
            }
            else if (me.Motion.X > 0)
            {
                from = Direction.Right;
            }
            if (me.Motion.Y > 0)
            {
                from = from == Direction.Stop
                            ? Direction.Down
                           : from == Direction.Left ? Direction.DownLeft : Direction.DownRight;
            }
            else
            {
                if (me.Motion.Y < 0)
                {
                    from = from == Direction.Stop
                               ? Direction.Up
                               : from == Direction.Left ? Direction.UpLeft : Direction.UpRight;
                }
            }

            where = CollisionPoint.None;
            if (prjMe.Intersects(prjTarget))
            {
                if (
                    prjMe.Intersects(new Rectangle(prjTarget.Left, prjTarget.Top,
                                                   prjTarget.Width, 0)))
                {
                    where = CollisionPoint.Top;
                }
                else if (
                    prjMe.Intersects(new Rectangle(prjTarget.Left, prjTarget.Bottom,
                                                   prjTarget.Width, 0)))
                {
                    where = CollisionPoint.Bottom;
                }
                if (prjMe.Intersects(new Rectangle(prjTarget.Left, prjTarget.Top, 0,
                                                   prjTarget.Height)))
                {
                    where = where == CollisionPoint.None
                                 ? CollisionPoint.Left
                                 : where == CollisionPoint.Top ? CollisionPoint.TopLeft : CollisionPoint.BottomLeft;
                }
                if (prjMe.Intersects(new Rectangle(prjTarget.Right, prjTarget.Top, 0,
                                                   prjTarget.Height)))
                {
                    where = where == CollisionPoint.None
                               ? CollisionPoint.Right
                                : where == CollisionPoint.Top ? CollisionPoint.TopRight : CollisionPoint.BottomRight;
                }

                return true;
            }
            return false;
        }

        public static void CheckWallAndDeflect<T>(T o, PlayArena arena, Deflection deflect) where T : GameObject
        {
                if (!o.IsAlive) return;
                Direction d;
                CollisionPoint c;
                var l = new Rectangle(arena.Bounds.Left, arena.Bounds.Top, 0, arena.Bounds.Height);
                var r = new Rectangle(arena.Bounds.Right, arena.Bounds.Top, 0, arena.Bounds.Height);
                var t = new Rectangle(arena.Bounds.Left, arena.Bounds.Top, arena.Bounds.Width, 0);

                if (IsCollision(o, l, out d, out c))
                {
                    
                    deflect(c, Direction.Left);
                }
                else if (IsCollision(o, r, out d, out c))
                {

                    deflect(c, Direction.Right);
                }
                else if (IsCollision(o, t, out d, out c))
                {

                    deflect(c, Direction.Up);
                }
        }        

    }
}
