using System;
using System.Linq;
using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Objects;
using Lev = ArkanoidDXUniverse.Levels.Level;

namespace ArkanoidDXUniverse.Levels
{
    public class LevelWadSelector
    {
        public Arkanoid Game;
        public bool IsLeft;
        public int Level;
        public Map Map;
        public string Name;
        public PlayArena PlayArena;
        public LevelWad Wad;
        public bool TwoPlayer;
        public LevelWadSelector(Arkanoid game, bool twoPlayer, LevelWad wad, int level, bool left)
        {
            Game = game;
            TwoPlayer = twoPlayer;
            IsLeft = left;
            Wad = wad;
            if (!game.Settings.Unlocks.ContainsKey(wad.Name))
            {
                //exception not set.
                game.Settings.Unlocks.Add(wad.Name, new WadScore
                {
                    Name = wad.Name
                });
            }
            else if (level == -1)
            {
                try
                {
                    level = game.Settings.Unlocks[wad.Name].LevelScores.Count;
                }
                catch
                {
                    level = 0;
                }
            }
            Level = level;
            Name = "Round " + (Level + 1);
        }

        public virtual void Initialise(PlayArena playArena)
        {
            PlayArena = playArena;
            if (Map == null)
            {
                if (IsLeft)
                {
                    if (Wad.Levels[Level].Key != null)
                        Map = Map.GetLevel(Game, PlayArena,
                            Level > Wad.Levels.Count - 1
                                ? Lev.EmptyLevel(Wad.Levels[0].Value.BricksWide, Wad.Levels[0].Value.BricksHigh)
                                : Wad.Levels[Level].Key);
                }


                if (Map == null)
                {
                    if (Wad.Levels[Level].Value != null)
                        Map = Map.GetLevel(Game, PlayArena,
                            Level > Wad.Levels.Count - 1
                                ? Lev.EmptyLevel(Wad.Levels[0].Value.BricksWide, Wad.Levels[0].Value.BricksHigh)
                                : Wad.Levels[Level].Value);
                }
                if (Map == null)
                {
                    Map.GetLevel(Game, PlayArena,
                        Lev.EmptyLevel(Wad.Levels[0].Value.BricksWide, Wad.Levels[0].Value.BricksHigh));
                }
            }
        }

        public virtual PlayableArena WarpLeft(Vaus vaus)
        {
            Level++;

            if (Level >= Wad.Levels.Count)
            {
                Map = Map.GetLevel(Game, PlayArena, Lev.EmptyLevel(Lev.ClassicBricksWide, Lev.ClassicBricksHigh));
                Name = "Final Round";
                return new BossArena(Game, TwoPlayer, this, vaus);
            }
            if (Game.Settings.Unlocks[Wad.Name].LevelScores.Count > Level - 1)
            {
                Game.Settings.Unlocks[Wad.Name].LevelScores[Level - 1] =
                    Math.Max(Game.Settings.Unlocks[Wad.Name].LevelScores[Level - 1], vaus.Score);
            }
            else
            {
                Game.Settings.Unlocks[Wad.Name].LevelScores.Add(vaus.Score);
            }
            Game.Settings.Unlocks[Wad.Name].HighScore = Game.Settings.Unlocks[Wad.Name].LevelScores.Max();
            Game.Settings.Save();
            if (Wad.Levels[Level].Key != null)
            {
                Map = Map.GetLevel(Game, PlayArena, Wad.Levels[Level].Key);
            }
            else
            {
                if (Wad.Levels[Level].Value != null)
                {
                    Map = Map.GetLevel(Game, PlayArena, Wad.Levels[Level].Value);
                }
                else
                {
                    Name = "Final Round";
                    return new BossArena(Game, TwoPlayer, this, vaus);
                }
            }
            Name = "Round " + (Level + 1);
            return new PlayArena(Game, TwoPlayer, this, vaus, 0);
        }

        public virtual PlayableArena WarpRight(Vaus vaus)
        {
            Map = null;
            IsLeft = false;
            Level++;

            if (Level >= Wad.Levels.Count)
            {
                Map = Map.GetLevel(Game, PlayArena, Lev.EmptyLevel(Lev.ClassicBricksWide, Lev.ClassicBricksHigh));
                Name = "Final Round";
                return new BossArena(Game, TwoPlayer, this, vaus);
            }
            if (Game.Settings.Unlocks[Wad.Name].LevelScores.Count > Level - 1)
            {
                Game.Settings.Unlocks[Wad.Name].LevelScores[Level - 1] =
                    Math.Max(Game.Settings.Unlocks[Wad.Name].LevelScores[Level - 1], vaus.Score);
            }
            else
            {
                Game.Settings.Unlocks[Wad.Name].LevelScores.Add(vaus.Score);
            }
            Game.Settings.Unlocks[Wad.Name].HighScore = Game.Settings.Unlocks[Wad.Name].LevelScores.Max();
            Game.Settings.Save();
            if (Wad.Levels[Level].Value != null)
            {
                Map = Map.GetLevel(Game, PlayArena, Wad.Levels[Level].Value);
            }
            else
            {
                if (Wad.Levels[Level].Key != null)
                {
                    Map = Map.GetLevel(Game, PlayArena, Wad.Levels[Level].Key);
                }
                else
                {
                    Name = "Final Round";
                    return new BossArena(Game, TwoPlayer, this, vaus);
                }
            }
            Name = "Round " + (Level + 1);
            return new PlayArena(Game, TwoPlayer, this, vaus, 0);
        }
    }
}