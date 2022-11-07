using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpeezleGame.Graphics
{
    public interface IAnimation
    {
        void Draw(SpriteBatch spriteBatch, Vector2 vector2, SpriteEffects spriteEffects);

        void Update(GameTime gameTime);

        void Stop();

        void Play();

        void Pause();

    }
}
