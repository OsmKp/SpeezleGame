using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace SpeezleGame.UserData
{
    public class SaveLoadManager
    {
        


        public User currentUser;
        public SaveFile selectedSaveFile;
        public SaveLoadManager(User user)
        {
            currentUser = user;
            
        }

        public void SaveUserData()
        {
            Debug.WriteLine("IN save user data");
            Debug.WriteLine("current user name is: " + currentUser.Name);
            Debug.WriteLine("current user curr is: " + currentUser.GetCurrency());
            Debug.WriteLine("current user level data count is " + currentUser.userLevelDatas.Count);

            SaveData saveData = new SaveData()
            {
                LevelData = currentUser.userLevelDatas,
                
                Currency = currentUser.GetCurrency(),

            };
            

            string serializedText = JsonConvert.SerializeObject(saveData, Formatting.Indented);

            Debug.WriteLine("path is: " + selectedSaveFile.FileName);
            
            File.WriteAllText(selectedSaveFile.FileName, serializedText);
            
        }

        public SaveData LoadSlotData(SaveFile saveFile)
        {
            SaveData deserializedData = new SaveData();
            if (File.Exists(saveFile.FileName))
            {
                Debug.WriteLine("It does exist wow");
                var fileContents = File.ReadAllText(saveFile.FileName);
                deserializedData = JsonConvert.DeserializeObject<SaveData>(fileContents);
            }


            return deserializedData;

        }

        public void LoadSlotData()
        {
            
            if (File.Exists(selectedSaveFile.FileName))
            {
                Debug.WriteLine("CURRENT SLOT EXISTS");
                var fileContents = File.ReadAllText(selectedSaveFile.FileName);
                SaveData deserializedData = JsonConvert.DeserializeObject<SaveData>(fileContents);
                if(deserializedData.LevelData != null)
                {
                    currentUser.userLevelDatas = deserializedData.LevelData;
                    currentUser.SetCurrency(deserializedData.Currency);
                }

                
                
            }

            
            


        }




    }
}
