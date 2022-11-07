using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Graphics
{
    public class SpriteAnimation : IAnimation
    {
        public const double DEFAULT_FPS = 8;


        public int SpriteCount { get; set; }
        public double TotalDuration => SpriteCount * (1 / Fps);
        public int CurrentOffset => (int)(PlaybackTime / (1 / Fps));
        public double PlaybackTime { get; set; } = 0;



        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }

        public Texture2D Texture { get; private set; }

        public double Fps { get; set; } = DEFAULT_FPS;

        public AnimationState State { get; set; } = AnimationState.Stopped;

        public SpriteAnimation(Texture2D texture, int spriteCount, int spriteWidth, int spriteHeight)
        {
            SpriteCount = spriteCount;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            if (State == AnimationState.Playing)
            {
                PlaybackTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (PlaybackTime > TotalDuration)
                {
                    PlaybackTime = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            var drawRect = new Rectangle(CurrentOffset * SpriteWidth, 0, SpriteWidth, SpriteHeight);
            spriteBatch.Draw(Texture, position, drawRect, Color.White, 0f, Vector2.Zero, Vector2.One, spriteEffects, 0f);

        }

        public void Play()
        {
            State = AnimationState.Playing;
        }
        public void Stop()
        {
            State = AnimationState.Stopped;
            PlaybackTime = 0;
        }
        public void Pause()
        {
            State = AnimationState.Pause;
        }
    }
}
