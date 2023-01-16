using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.MapComponents
{
    public class LeverObject : MapObject
    {
        public int TargetDoorID;
        public LeverObject(int objectId, Rectangle bounds) : base(objectId, bounds)
        {
        }
        public LeverObject(int objectId ,Rectangle bounds, int targetDoorID) : base(objectId, bounds)
        {
            TargetDoorID = targetDoorID;
        }
    }
}
