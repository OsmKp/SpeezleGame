using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace SpeezleGame.UserData
{
    [Serializable]
    public struct SaveData
    {
        public List<UserLevelData> LevelData { get; set; }
        public int Currency {get; set; }
        public string EquippedSkin { get; set; }
        public List<string> OwnedSkins { get; set; }
    }
}
