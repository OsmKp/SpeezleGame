using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpeezleGame.Core
{
    public class SpriteHandler
    {
        //640 * 360

        private Game game;
        private SpriteBatch spriteBatch;


        public SpriteHandler(Game game)
        {
            this.game = game;
            spriteBatch = new SpriteBatch(this.game.GraphicsDevice);

        }

        public void Begin(bool isTextureFilteringAvailable)
        {
            SamplerState sampler = SamplerState.PointClamp;
            if (isTextureFilteringAvailable)
            {
                sampler = SamplerState.LinearClamp;
            }

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: sampler, rasterizerState: RasterizerState.CullNone);
        }

        public void End()
        {
            spriteBatch.End();
        }
        public void Draw(Texture2D texture, Vector2 origin, Vector2 position, Color colour)
        {
            spriteBatch.Draw(texture, position, null, colour, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Vector2 origin, Vector2 position, float rotation, Vector2 scale, Color colour, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, colour, rotation, origin, scale, spriteEffects, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Rectangle destinationRectangle, Color colour)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, colour);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Rectangle destinationRectangle, float rotation, SpriteEffects spriteEffects, Color colour, float layer = 0)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, rotation, Vector2.Zero, spriteEffects, 0);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color colour)
        {
            spriteBatch.DrawString(font, text, position, colour);
        }


    }
}
