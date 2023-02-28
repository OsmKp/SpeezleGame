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
        public List<string> skinsOwned;
        private string equippedSkin;
        public User()
        {
            Name = "User";
            Currency = 0;
            equippedSkin = "Steve";
        }

        public void InitializeUser()
        {
            skinsOwned = new List<string>();
            skinsOwned.Add("Steve");
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
        public bool IsLevelUnlocked(string name)
        {
            bool isUnlocked = false;
            foreach(var uld in userLevelDatas)
            {
                if(uld.LevelName == name && uld.Unlocked == true)
                {
                    isUnlocked = true;
                }
            }
            return isUnlocked;
        }

        public int GetStarsAchieved(string name)
        {
            int stars = 0;
            foreach (var uld in userLevelDatas)
            {
                if (uld.LevelName == name)
                {
                    stars = uld.StarsAchieved;
                }
            }
            return stars;
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

        public string GetEquippedSkin()
        {
            return equippedSkin;
        }
        public void SetEquippedSkin(string skin)
        {
            equippedSkin = skin;
        }
        public void AddNewSkin(string skin)
        {
            skinsOwned.Add(skin);
        }

        public bool IsSkinOwned(string skin)
        {
            bool isSkinOwned = false;
            foreach(var name in skinsOwned)
            {
                if(name == skin)
                {
                    isSkinOwned = true;
                }
            }
            return isSkinOwned;
        }
    }
}
