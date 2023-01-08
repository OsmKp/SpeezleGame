using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Graphics
{
    public class Background
    {
        private Texture2D backgroundTexture;
        
        
        public Background(Texture2D textures)
        {
            backgroundTexture = textures;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRect = new Rectangle(0, 0, 640, 360);
            spriteBatch.Draw(backgroundTexture, destinationRect, Color.White);
        }
    }
}
