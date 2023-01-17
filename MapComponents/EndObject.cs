using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.MapComponents
{
    public class EndObject : MapObject
    {
        public string CurrentLevel
        {
            get { return currentLevel; }
        }
        private string currentLevel;
        public EndObject(int objectId, Rectangle bounds) : base(objectId, bounds)
        {

        }

        public EndObject(int objectId, Rectangle bounds, string currentLevel) : base(objectId, bounds)
        {
            this.currentLevel = currentLevel;
        }
    }
}
