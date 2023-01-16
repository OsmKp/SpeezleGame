using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.MapComponents
{
    public class TeleportObject : MapObject
    {
        
        public int TargetID;
        public TeleportObject(int objectId, Rectangle bounds) : base(objectId, bounds)
        {
        }

        public TeleportObject(int objectId, Rectangle bounds,int targetID) : base(objectId, bounds)
        {
            TargetID = targetID;
        }
    }
}
