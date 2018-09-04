using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArkanoidDXUniverse.Levels
{
    [DataContract]
    public class WadScore
    {
        [DataMember] public int HighScore;

        [DataMember] public List<int> LevelScores = new List<int>();

        [DataMember] public string Name;
    }
}