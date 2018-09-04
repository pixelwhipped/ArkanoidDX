using ArkanoidDXUniverse.Arena;
using ArkanoidDXUniverse.Objects;

namespace ArkanoidDXUniverse.Levels
{
    public class EditLevelWadSelector : LevelWadSelector
    {
        public string EditPath;

        public EditLevelWadSelector(Arkanoid game, LevelWad wad, string editPath, int level, bool left)
            : base(game,false, wad, level, left)
        {
            EditPath = editPath;
        }

        public override void Initialise(PlayArena playArena)
        {
            PlayArena = playArena;
            Map = Map.GetLevel(Game, PlayArena, Wad.Levels[0].Value) ?? Map.GetLevel(Game, PlayArena, Wad.Levels[0].Key);
        }

        public override PlayableArena WarpLeft(Vaus vaus)
        {
            return new EditArena(Game, Level, EditPath);
        }

        public override PlayableArena WarpRight(Vaus vaus)
        {
            return new EditArena(Game, Level, EditPath);
        }
    }
}