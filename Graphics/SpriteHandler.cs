using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpeezleGame.Graphics
{
    public class SpriteHandler
    {
        public int TexturePosX { get; set; }
        public int TexturePosY { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Texture2D Texture { get; private set; }

        public Rectangle TextureBounds => new Rectangle(TexturePosX, TexturePosY, Width, Height);

        public SpriteHandler(int texturePosX, int texturePosY, int width, int height, Texture2D texture)
        {
            TexturePosX = texturePosX;
            TexturePosY = texturePosY;
            Width = width;
            Height = height;
            Texture = texture ?? throw new ArgumentNullException(nameof(texture));
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, TextureBounds, Color.White);
        }



    }
}
