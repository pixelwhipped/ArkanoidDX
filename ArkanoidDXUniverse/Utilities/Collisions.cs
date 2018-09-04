using System;
using System.Collections.Generic;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Interfaces;
using ArkanoidDXUniverse.Objects;
using Microsoft.Xna.Framework;

namespace ArkanoidDXUniverse.Utilities
{
    public static class Collisions
    {
        public static bool IsCollision(Rectangle a, Rectangle b, bool checkInside = false)
        {
            return a.Intersects(b) || (checkInside && (a.Contains(b) || b.Contains(a)));
        }

        public static bool IsCollision(ILocatable a, ILocatable b, bool checkInside = false)
        {
            return a.Bounds.Intersects(b.Bounds) ||
                   (checkInside && (a.Bounds.Contains(b.Bounds) || b.Bounds.Contains(a.Bounds)));
        }

        public static bool IsCollision(ILocatable me, ILocatable target, out Direction from, out CollisionPoint where, bool circle = false)
        {
            var prjTarget = new Rectangle((int) (target.Bounds.X + target.Motion.X),
                (int) (target.Bounds.Y + target.Motion.Y),
                target.Bounds.Width, target.Bounds.Height);
            return IsCollision(me, prjTarget, out from, out where, circle);
        }

        public static bool IsCollisionCircle(Rectangle prjMe, Rectangle prjTarget)
        {

                Vector2[] vec;
                var ccol = CircleIntersectsLine(new Vector2(prjTarget.Left, prjTarget.Top),
                    new Vector2(prjTarget.Right, prjTarget.Top), new Vector2(prjMe.Center.X, prjMe.Center.Y),
                    prjMe.Width / 2f, out vec);
                ccol = ccol || CircleIntersectsLine(new Vector2(prjTarget.Left, prjTarget.Bottom),
                    new Vector2(prjTarget.Right, prjTarget.Bottom), new Vector2(prjMe.Center.X, prjMe.Center.Y),
                    prjMe.Width / 2f, out vec);
                ccol = ccol || CircleIntersectsLine(new Vector2(prjTarget.Left, prjTarget.Top),
                    new Vector2(prjTarget.Left, prjTarget.Bottom), new Vector2(prjMe.Center.X, prjMe.Center.Y),
                    prjMe.Width / 2f, out vec);
                ccol = ccol || CircleIntersectsLine(new Vector2(prjTarget.Right, prjTarget.Top),
                    new Vector2(prjTarget.Bottom, prjTarget.Top), new Vector2(prjMe.Center.X, prjMe.Center.Y),
                    prjMe.Width / 2f, out vec);
                if (!ccol) return false;
            return true;
        }
        public static bool IsCollision(ILocatable me, Rectangle target, out Direction from, out CollisionPoint where, bool circle = false)
        {
            var prjMe = new Rectangle((int) (me.Bounds.X + me.Motion.X), (int) (me.Bounds.Y + me.Motion.Y),
                me.Bounds.Width,
                me.Bounds.Height);
            var prjTarget = target;

            from = Direction.Stop;
            if (Math.Abs(me.Motion.X) < 0.00001 && Math.Abs(me.Motion.Y) < 0.00001)
            {
                from = Direction.Stop;
            }else if (me.Motion.X < 0) //Left
            {
                if (Math.Abs(me.Motion.Y) < 0.00001)
                {
                    from = Direction.Left;
                }
                else if (me.Motion.Y < 0)
                {
                    from = Direction.UpLeft;
                }
                else
                {
                    from = Direction.DownLeft;
                } 
            }
            else
            {
                if (Math.Abs(me.Motion.Y) < 0.00001)
                {
                    from = Direction.Right;
                }
                else if (me.Motion.Y < 0)
                {
                    from = Direction.UpRight;
                }
                else
                {
                    from = Direction.DownRight;
                }
            }

            where = CollisionPoint.None;
            if(circle && !IsCollisionCircle(prjMe,prjTarget)) return false;
            if (prjMe.Intersects(prjTarget))
            {
                if (
                    prjMe.Intersects(new Rectangle(prjTarget.Left, prjTarget.Top,
                        prjTarget.Width, 1)))
                {
                    where = CollisionPoint.Top;
                }
                else if (
                    prjMe.Intersects(new Rectangle(prjTarget.Left, prjTarget.Bottom,
                        prjTarget.Width, 1)))
                {
                    where = CollisionPoint.Bottom;
                }
                if (prjMe.Intersects(new Rectangle(prjTarget.Left, prjTarget.Top, 1,
                    prjTarget.Height)))
                {
                    where = where == CollisionPoint.None
                        ? CollisionPoint.Left
                        : where == CollisionPoint.Top ? CollisionPoint.TopLeft : CollisionPoint.BottomLeft;
                }
                if (prjMe.Intersects(new Rectangle(prjTarget.Right, prjTarget.Top, 1,
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

        /// <summary>
        /// Test if circle interesect another circle.
        /// </summary>
        /// <param name="xy1">Center of 1st circle.</param>
        /// <param name="radius1">Radius of 1st circle.</param>
        /// <param name="xy2">Center of 2nd circle.</param>
        /// <param name="radius2">Radius of 2nd circle.</param>
        /// <param name="intersections">Points of intersection.</param>
        /// <returns>True if circle contains point.</returns>

        public static bool CircleIntersectsCircle(Vector2 xy1, float radius1, Vector2 xy2, float radius2,
                                                  out Vector2[] intersections)
        {
            // dx and dy are the vertical and horizontal distances between the circle centers.
            var dx = xy2.X - xy1.X;
            var dy = xy2.Y - xy1.Y;
            // Determine the straight-line distance between the centers. 
            var d = Math.Sqrt((dy * dy) + (dx * dx));
            // Check for solvability. 
            if (d > (radius1 + radius2))
            {
                // no solution. circles do not intersect. 
                intersections = new[] { new Vector2(0, 0), new Vector2(0, 0) };
                return false;
            }
            if (d < Math.Abs(radius1 - radius2))
            {
                // no solution. one circle is contained in the other 
                intersections = new[] { new Vector2(0, 0), new Vector2(0, 0) };
                return true;
            }

            // 'point 2' is the point where the line through the circle
            // intersection points crosses the line between the circle
            // centers.  

            // Determine the distance from point 0 to point 2. 
            var a = ((radius1 * radius1) - (radius2 * radius2) + (d * d)) / (2.0f * d);

            // Determine the coordinates of point 2. 
            var x2 = xy1.X + (dx * a / d);
            var y2 = xy1.Y + (dy * a / d);

            // Determine the distance from point 2 to either of the intersection points.
            var h = Math.Sqrt((radius1 * radius1) - (a * a));

            // Now determine the offsets of the intersection points from point 2.

            var rx = -dy * (h / d);
            var ry = dx * (h / d);

            // Determine the absolute intersection points. 
            intersections = new[] { new Vector2((float)(x2 + rx), (float)(y2 + ry)), new Vector2((float)(x2 - rx), (float)(y2 - ry)) };
            return true;
        }

        public static bool CircleIntersectsLine(Vector2 lStart, Vector2 lEnd, Vector2 center, float radius, out Vector2[] intersections)
        {
            float t;
            var a = (lEnd.X - lStart.X) * (lEnd.X - lStart.X) + (lEnd.Y - lStart.Y) * (lEnd.Y - lStart.Y);
            var b = 2 * ((lEnd.X - lStart.X) * (lStart.X - center.X) + (lEnd.Y - lStart.Y) * (lStart.Y - center.Y));
            var cc = (lStart.X - center.X) * (lStart.X - center.X) + (lStart.Y - center.Y) * (lStart.Y - center.Y) - radius * radius;

            var deter = b * b - 4f * a * cc;

            var vec = new List<Vector2>();

            var v1 = new Vector2((float)lStart.X + (t = ((-b + (float)Math.Sqrt(deter)) / (2f * a))) * (lEnd.X - lStart.X), lStart.Y + t * (lEnd.Y - lStart.Y));
            var v2 = new Vector2(lStart.X + (t = ((-b - (float)Math.Sqrt(deter)) / (2f * a))) * (lEnd.X - lStart.X), lStart.Y + t * (lEnd.Y - lStart.Y));

            if (Math.Abs(DistanceFromLine(v1, lEnd, lStart)) < 0.00001f)
                vec.Add(v1);
            if (Math.Abs(DistanceFromLine(v2, lEnd, lStart)) < 0.00001f)
                vec.Add(v2);
            intersections = vec.ToArray();
            return intersections.Length != 0;
        }

        public static Vector2 ClosestPointOnLine(Vector2 p, Vector2 a, Vector2 b)
        {
            var u = Vector2.Subtract(b, a);
            var t = Vector2.Dot(Vector2.Subtract(p, a), u) / Vector2.Dot(u, u);
            if (t < 0) t = 0;
            if (t > 1) t = 1;
            return Vector2.Add(a, Vector2.Multiply(u, t));
        }

        public static double DistanceFromLine(Vector2 p, Vector2 a, Vector2 b)
        {
            var pt = ClosestPointOnLine(p, a, b);
            return Vector2.Subtract(p, pt).Length();
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