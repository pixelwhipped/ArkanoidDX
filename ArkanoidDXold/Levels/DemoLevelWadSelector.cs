using ArkanoidDX.Arena;
using ArkanoidDX.Objects;

namespace ArkanoidDX.Levels
{
    public class DemoLevelWadSelector : LevelWadSelector
    {
        public DemoLevelWadSelector(ArkanoidDX game,LevelWad wad, int level, bool left) : base(game,wad,level,left)
        {
            
            
        }
        public override void Initialise(PlayArena playArena)
        {
            PlayArena = playArena;
            if (ArkanoidDX.Random.Next() > .5)
            {
                Level = ArkanoidDX.Random.Next(0, Wad.Levels.Count - 1);
                Name = "Round " + (Level + 1);
                Map = Map.GetLevel(Game, PlayArena, Wad.Levels[Level].Value ?? Wad.Levels[Level].Key);
            }
            else
            {
                Level = ArkanoidDX.Random.Next(0, Levels.ArkanoidDXLevels.Count - 1);
                Name = "Round " + (Level + 1);
                Map = Map.GetLevel(Game, PlayArena, Wad.Levels[Level].Key ?? Wad.Levels[Level].Value);
            }
        }
    }
}
