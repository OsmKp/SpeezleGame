using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Xna.Framework;

namespace SpeezleGame.MapComponents
{
    public abstract class MapObject
    {
        public int objectId;

        public List<Vector2> tileCoordinates;
        public Rectangle Bounds
        {
            get { return _bounds; }
        }
        private Rectangle _bounds;
        public MapObject(int objectId, Rectangle bounds)
        {
            this.objectId = objectId;
            _bounds = bounds;


        }


    }
}
