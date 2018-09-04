using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Objects;

namespace ArkanoidDXUniverse.Levels
{
    public class DemoLevelWadSelector : LevelWadSelector
    {
        public DemoLevelWadSelector(Arkanoid game, LevelWad wad, int level, bool left) : base(game,false, wad, level, left)
        {
        }

        public override void Initialise(PlayArena playArena)
        {
            PlayArena = playArena;
            if (Arkanoid.Random.Next() > .5)
            {
                Level = Arkanoid.Random.Next(0, Wad.Levels.Count - 1);
                Name = "Round " + (Level + 1);
                Map = Map.GetLevel(Game, PlayArena, Wad.Levels[Level].Value ?? Wad.Levels[Level].Key);
            }
            else
            {
                Level = Arkanoid.Random.Next(0, Levels.ArkanoidDxLevels.Count - 1);
                Name = "Round " + (Level + 1);
                Map = Map.GetLevel(Game, PlayArena, Wad.Levels[Level].Key ?? Wad.Levels[Level].Value);
            }
        }
    }
}