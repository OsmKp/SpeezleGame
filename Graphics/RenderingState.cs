using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.Core;

namespace SpeezleGame.Graphics
{
    public class RenderingState
    {   
        public string Name { get; }
        public SpriteAnimation Animation { get; }

        public RenderingState(string name, SpriteAnimation animation)
        {
            Name = name;
            Animation = animation;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            Animation?.Draw(spriteBatch, position, spriteEffects);
        }
    }
}
