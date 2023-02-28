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
            SaveData saveData = new SaveData()
            {
                LevelData = currentUser.userLevelDatas,
                
                Currency = currentUser.GetCurrency(),

                EquippedSkin = currentUser.GetEquippedSkin(),

                OwnedSkins = currentUser.skinsOwned,

            };
            
            string serializedText = JsonConvert.SerializeObject(saveData, Formatting.Indented);

            File.WriteAllText(selectedSaveFile.FileName, serializedText);
            
        }

        public SaveData LoadSlotData(SaveFile saveFile)
        {
            SaveData deserializedData = new SaveData();
            if (File.Exists(saveFile.FileName))
            {
                
                var fileContents = File.ReadAllText(saveFile.FileName);
                deserializedData = JsonConvert.DeserializeObject<SaveData>(fileContents);
            }


            return deserializedData;

        }

        public void LoadSlotData()
        {
            
            if (File.Exists(selectedSaveFile.FileName))
            {
                
                var fileContents = File.ReadAllText(selectedSaveFile.FileName);
                SaveData deserializedData = JsonConvert.DeserializeObject<SaveData>(fileContents);
                if(deserializedData.LevelData != null)
                {
                    currentUser.userLevelDatas = deserializedData.LevelData;
                    currentUser.SetCurrency(deserializedData.Currency);


                }
                if(deserializedData.OwnedSkins != null)
                {
                    currentUser.SetEquippedSkin(deserializedData.EquippedSkin);
                    currentUser.skinsOwned = deserializedData.OwnedSkins;
                }

            }

        }
    }
}
