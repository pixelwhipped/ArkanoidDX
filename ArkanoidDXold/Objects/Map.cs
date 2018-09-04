using System;
using System.Collections.Generic;
using System.Linq;
using ArkanoidDX.Arena;
using ArkanoidDX.Graphics;
using ArkanoidDX.Levels;
using Microsoft.Xna.Framework;

namespace ArkanoidDX.Objects
{
    public class Map
    {
        public int BricksWide;
        public int BricksHigh;
        public BackGroundTypes BackGround;
        public List<Brick> BrickMap;
        public EnemyTypes EnemyType;
        public int MaxEnimies;
        public int MaxEnimyRealeaseTime;
        public int MinEnimyRealeaseTime;
        public bool SideLeftMidEntryEnable;
        public bool SideLeftTopEntryEnable;
        public bool SideRightMidEntryEnable;
        public bool SideRightTopEntryEnable;
        public bool TopLeftEntryEnable;
        public bool TopRightEntryEnable;
        public bool Leavable;
        public bool IsOver
        {
            get { return BrickMap.All(p => !p.IsAlive || p.IsRegen  || p.IsInvincible) || Leavable; }
        }

        public PlayArena PlayArena;
        public static Map GetLevel(ArkanoidDX game, PlayArena playArena,Level level)
        {
            BackGroundTypes bg;
            if(!Enum.TryParse(Enum.GetNames(typeof(BackGroundTypes))[level.Background],out bg))
                bg = BackGroundTypes.BlueCircuit;
            EnemyTypes et;
            if (!Enum.TryParse(Enum.GetNames(typeof(EnemyTypes))[level.EnemyType], out et))
                et = EnemyTypes.Tri;
            game.BlocksWide = level.BricksWide;
            return new Map
                       {
                           BricksWide = level.BricksWide,
                           BricksHigh = level.BricksHigh,
                           BackGround = bg,
                           TopLeftEntryEnable = level.TopLeftEntryEnable == 1,
                           TopRightEntryEnable = level.TopRightEntryEnable == 1,
                           SideRightTopEntryEnable = level.SideRightTopEntryEnable == 1,
                           SideRightMidEntryEnable = level.SideRightMidEntryEnable == 1,
                           SideLeftTopEntryEnable = level.SideLeftTopEntryEnable == 1,
                           SideLeftMidEntryEnable = level.SideLeftMidEntryEnable == 1,
                           MinEnimyRealeaseTime = level.MaxEnimyRealeaseTime,
                           MaxEnimyRealeaseTime = level.MaxEnimyRealeaseTime,
                           MaxEnimies = level.MaxEnimies,
                           EnemyType = et,
                           PlayArena = playArena,
                           BrickMap = GetLevelBrickMap(game, playArena,level)
                       };
        }

        public static List<Brick> GetLevelBrickMap(ArkanoidDX game, PlayArena playArena, Level map)
        {
            var by = map.BricksHigh;
            var bx = map.BricksWide;
            var bricks = new BrickTypes[by][];
            var chances = new int[by][];
            var powers = new CapsuleTypes[by][];
            for (var y = 0; y < by; y++)
            {
                bricks[y] = new BrickTypes[bx];
                chances[y] = new int[bx];
                powers[y] = new CapsuleTypes[bx];
                for (var x = 0; x < bx; x++)
                {
                    BrickTypes bt; 
                    if (!Enum.TryParse(Enum.GetNames(typeof(BrickTypes))[map.GetBrickValue(y, x)], out bt))
                        bt = BrickTypes.Empty;
                    bricks[y][x] = bt;
                    CapsuleTypes p;
                    if (!Enum.TryParse(Enum.GetNames(typeof(CapsuleTypes))[map.GetPowerValue(y, x)], out p))
                        p = CapsuleTypes.Slow; //todo make random
                    bricks[y][x] = bt;
                    chances[y][x] = map.GetChanceValue(y, x);
                    powers[y][x] = p;  
                    
                }
            }
            var bm = new List<Brick>();
            for (var y = 0; y < bricks.Length; y++)
            {
                for (var x = 0; x < bricks[y].Length; x++)
                {
                    var b = bricks[y][x];
                    if (b != BrickTypes.Empty)
                        bm.Add(Types.GetBrick(b, game, playArena, GetBrickLocation(game, x, y), chances[y][x], powers[y][x]));
                }
            }
            return bm;
        }

        public static Vector2 GetBrickLocation(ArkanoidDX game, int x, int y)
        {
            
            return new Vector2(
                game.ArenaArea.X + (x*Sprites.BrkWhite.Width),
                game.ArenaArea.Y +
                (y*Sprites.BrkWhite.Height));
        }

        public bool GetBrickToLeft(Brick brick , out Brick neighbour)
        {
            var tl = new Vector2(brick.X - brick.Width, brick.Y);
            neighbour = BrickMap.Find(b => b.Location == tl);
            if (neighbour == null) return false;
            if (!neighbour.IsAlive)
                neighbour = null;
            return neighbour != null;
        }
        public bool GetBrickToRight(Brick brick, out Brick neighbour)
        {
            var tl = new Vector2(brick.X + brick.Width, brick.Y);
            neighbour = BrickMap.Find(b => b.Location == tl);
            if (neighbour == null) return false;
            if (!neighbour.IsAlive)
                neighbour = null;
            return neighbour != null;
        }

        public bool GetBrickCollision(Brick brick, out Brick neighbour)
        {
            foreach (var b in PlayArena.LevelMap.BrickMap)
            {
                if (!brick.IsAlive) continue;

                Direction dx;
                CollisionPoint cx;
                if (!Collisions.IsCollision(brick, b, out dx, out cx) || b == brick) continue;
                neighbour = b;
                return true;
            }
            neighbour = null;
            return false;
        }


        public bool GetBrickAbove(Brick brick, out Brick neighbour)
        {
            var tl = new Vector2(brick.X, brick.Y-brick.Height);
            neighbour = BrickMap.Find(b => b.Location == tl);
            if (neighbour == null) return false;
            if (!neighbour.IsAlive)
                neighbour = null;
            return neighbour != null;
        }

        public bool GetBrickBelow(Brick brick, out Brick neighbour)
        {
            var tl = new Vector2(brick.X, brick.Y + brick.Height);
            neighbour = BrickMap.Find(b => b.Location == tl);
            if (neighbour == null) return false;
            if(!neighbour.IsAlive)
                neighbour = null;

            return neighbour != null;
        }

        public bool GetBrickToBelowLeft(Brick brick, out Brick neighbour)
        {
            var tl = new Vector2(brick.X - brick.Width, brick.Y + brick.Height);
            neighbour = BrickMap.Find(b => b.Location == tl);
            if (neighbour == null) return false;
            if (!neighbour.IsAlive)
                neighbour = null;
            return neighbour != null;
        }

        public bool GetBrickToBelowRight(Brick brick, out Brick neighbour)
        {
            var tl = new Vector2(brick.X + brick.Width, brick.Y + brick.Height);
            neighbour = BrickMap.Find(b => b.Location == tl);
            if (neighbour == null) return false;
            if (!neighbour.IsAlive)
                neighbour = null;
            return neighbour != null;
        }

        public bool GetBrickToUpperLeft(Brick brick, out Brick neighbour)
        {
            var tl = new Vector2(brick.X - brick.Width, brick.Y - brick.Height);
            neighbour = BrickMap.Find(b => b.Location == tl);
            if (neighbour == null) return false;
            if (!neighbour.IsAlive)
                neighbour = null;
            return neighbour != null;
        }

        public bool GetBrickToUpperRight(Brick brick, out Brick neighbour)
        {
            var tl = new Vector2(brick.X + brick.Width, brick.Y - brick.Height);
            neighbour = BrickMap.Find(b => b.Location == tl);
            if (neighbour == null) return false;
            if (!neighbour.IsAlive)
                neighbour = null;
            return neighbour != null;
        }

    }
}