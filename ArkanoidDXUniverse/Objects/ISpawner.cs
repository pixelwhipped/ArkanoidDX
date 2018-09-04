using System.Collections.Generic;
using ArkanoidDXUniverse.Arena;

namespace ArkanoidDXUniverse.Objects
{
    public interface ISpawner
    {
        void Spawn(EnemyTypes type, PlayArena playArena, List<Enemy> enimies);
    }
}