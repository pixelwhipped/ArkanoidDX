using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArkanoidDX.Levels
{
   [DataContract]
    public class WadScore
    {
        [DataMember]
        public int HighScore;
        [DataMember]
        public string Name;
        [DataMember]
        public List<int> LevelScores = new List<int>();
    }
}
