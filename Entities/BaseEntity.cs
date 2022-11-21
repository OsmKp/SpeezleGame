using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Entities
{
    public abstract class BaseEntity
    {
        public abstract void Draw(SpriteBatch spriteBatch,GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}
