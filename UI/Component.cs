using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.UI
{
    public abstract class Component
    {
        public virtual void Draw(SpriteBatch spriteBatch) { }


        public abstract void Update(GameTime gameTime);

    }
}
    