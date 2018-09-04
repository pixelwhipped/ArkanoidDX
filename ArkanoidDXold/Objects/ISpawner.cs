using System.Collections.Generic;
using ArkanoidDX.Arena;

namespace ArkanoidDX.Objects
{
    public interface ISpawner
    {
        void Spawn(EnemyTypes type, PlayArena playArena, List<Enemy> enimies);
    }
}