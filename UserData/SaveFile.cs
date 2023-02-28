using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeezleGame.UserData;
using static SpeezleGame.UserData.SaveLoadManager;

namespace SpeezleGame.UserData
{
    public class SaveFile
    {
        public string FileName { get; set; }
        public bool empty = true;
        public List<UserLevelData> userLevelDatas;
        public List<string> skinsOwned;
        public int Currency;
        public int NumberOfCompletedLevels;
        public int NumberOfSkinsOwned;


        public void PrepareSlotPreview(SaveData savedata )
        {
            

            this.userLevelDatas = savedata.LevelData;
            this.skinsOwned = savedata.OwnedSkins;
            Currency = savedata.Currency;

            int i = 0;
            if(savedata.LevelData != null)
            {
                foreach (var levelData in savedata.LevelData)
                {
                    if (levelData.Completed == true)
                    {
                        i++;
                    }
                }
                
            }
            NumberOfCompletedLevels = i;

            int j = 0;
            if (savedata.OwnedSkins != null)
            {
                foreach (var skin in savedata.OwnedSkins)
                {
                    j++;
                }

            }
            if(j == 0) { j = 1; }
            NumberOfSkinsOwned = j;

        }

        public SaveFile(string fileName)
        {
            FileName = fileName;

            if (File.Exists(FileName))
            {
                empty = false;
            }
            else { empty = true; }
        }
    }
}
