using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.Entities.Players;
using SpeezleGame.MapComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace SpeezleGame.Entities
{
    public abstract class BaseEntity
    {
        public int Health;
        public abstract void Draw(SpriteBatch spriteBatch,GameTime gameTime);
        public abstract void Update(GameTime gameTime);

        public virtual void Update(GameTime gameTime, Vector2 playerPos, List<Rectangle> RectangleMapObjects, List<TiledPolygon> PolygonCollisionObjects, List<MapObject> mapObjects) { }
        public virtual void Update(GameTime gameTime, Vector2 playerPos, List<Rectangle> RectangleMapObjects, List<TiledPolygon> PolygonCollisionObjects, List<MapObject> mapObjects, Player player) { }
    }
}
