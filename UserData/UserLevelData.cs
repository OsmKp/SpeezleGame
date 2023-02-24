using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.UserData
{
    [Serializable]
    public class UserLevelData
    {
        public bool Completed;
        public bool Unlocked;
        public string LevelName;
        public int StarsAchieved;
        public int BestTimeSeconds;

        public UserLevelData(bool completed, bool unlocked, string levelName, int starsAchieved, int bestTimeSeconds)
        {
            Completed = completed;
            Unlocked = unlocked;
            LevelName = levelName;
            StarsAchieved = starsAchieved;
            BestTimeSeconds = bestTimeSeconds;
        }
    }
}