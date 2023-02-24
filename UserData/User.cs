using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.UserData
{
    public class User
    {
        public string Name { get; set; }
        public List<UserLevelData> userLevelDatas;
        private int Currency;
        
        public User()
        {
            Name = "User";
            Currency = 0;

        }

        public void InitializeUser()
        {
            userLevelDatas = new List<UserLevelData>();
            UserLevelData level1 = new UserLevelData(false, true, "One", 0, 9999);
            UserLevelData level2 = new UserLevelData(false, false, "Two", 0, 9999);
            UserLevelData level3 = new UserLevelData(false, false, "Three", 0, 9999);
            UserLevelData level4 = new UserLevelData(false, false, "Four", 0, 9999);
            UserLevelData level5 = new UserLevelData(false, false, "Five", 0, 9999);
            userLevelDatas.Add(level1);
            userLevelDatas.Add(level2);
            userLevelDatas.Add(level3);
            userLevelDatas.Add(level4);
            userLevelDatas.Add(level5);
        }

        public void AddCurrency(int amount)
        {
            Currency += amount;
        }
        public int GetCurrency()
        {
            return Currency;
        }
        public void SetCurrency(int amount)
        {
            Currency = amount;
        }
    }
}
