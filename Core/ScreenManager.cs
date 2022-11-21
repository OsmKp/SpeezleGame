using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Core
{
    public sealed class ScreenManager
    {
        private readonly static int MinDimensions = 64;
        private readonly static int MaxDimensions = 4096;

        public static int ScreenWidth = 1920; //test
        public static int ScreenHeight = 1080; //test
        

        public int Width
        {
            get { return target.Width; }
        }

        public int Height 
        { 
            get { return target.Height; } 
        }


        private SpeezleGame game;
        private RenderTarget2D target;

        private bool isSet;

        public ScreenManager(SpeezleGame game, int width, int height)
        {
            this.game = game;
            

            width = MathHelper.Clamp(width, MinDimensions, MaxDimensions);

            target = new RenderTarget2D(game.GraphicsDevice, width, height);
            isSet = false;
        }

        public void Set()
        {
            game.GraphicsDevice.SetRenderTarget(target);
            isSet = true;
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        public void UnSet()
        {
            game.GraphicsDevice.SetRenderTarget(null);
            isSet = false;
        }

        public Rectangle CalculateDestinationRectangle()
        {
            Rectangle backbufferBounds = this.game.GraphicsDevice.PresentationParameters.Bounds;
            float backbufferAspectRatio = (float)backbufferBounds.Width / backbufferBounds.Height;
            float screenAspectRatio = (float)Width / Height;

            float rectX = 0f;
            float rectY = 0f;
            float rectWidth = backbufferBounds.Width;
            float rectHeight = backbufferBounds.Height; 
            
            if(backbufferAspectRatio > screenAspectRatio)
            {
                rectWidth = rectHeight * screenAspectRatio;
                rectX = (backbufferBounds.Width - rectWidth) / 2f;
            }
            else if(screenAspectRatio > backbufferAspectRatio)
            {
                rectHeight = rectWidth / screenAspectRatio;
                rectY = (backbufferBounds.Height - rectHeight) / 2f;
            }

            Rectangle result = new Rectangle((int)rectX, (int)rectY, (int)rectWidth, (int)rectHeight);
            return result;

        }

        public void Display(/*SpriteHandler spriteHandler, */Matrix transform, bool textureFiltering = true)
        {

            game.GraphicsDevice.Clear(Color.CornflowerBlue);

            Rectangle destinationRectangle = this.CalculateDestinationRectangle();


            game.backgroundRenderer.Begin(transform, textureFiltering);
            game.backgroundRenderer.SpriteBatch.Draw(target, destinationRectangle, null, Color.White);
            game.backgroundRenderer.End();

            /*spriteHandler.Begin(textureFiltering, transform);
            spriteHandler.Draw(target, null, destinationRectangle, Color.White);
            spriteHandler.End();*/
        }
    }
}
