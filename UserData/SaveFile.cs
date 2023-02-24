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
        public int Currency;
        public int NumberOfCompletedLevels;


        public void PrepareSlotPreview(SaveData savedata )
        {
            

            this.userLevelDatas = savedata.LevelData;
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
