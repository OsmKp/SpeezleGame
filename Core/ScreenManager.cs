using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        
        public Texture2D LastFrame
        {
            get { return _lastFrame; }
        }

        private Texture2D _lastFrame;
        public int Width
        {
            get { return target.Width; }
        }

        public int Height 
        { 
            get { return target.Height; } 
        }

        public static float stretchRatio;
        public static Vector2 shiftAmountVector;

        private SpeezleGame game;
        private RenderTarget2D target;
        private RenderTarget2D target2;

        private bool isSet;
        private bool isSet2;

        public ScreenManager(SpeezleGame game, int width, int height)
        {
            this.game = game;
            

            width = MathHelper.Clamp(width, MinDimensions, MaxDimensions);

            target = new RenderTarget2D(game.GraphicsDevice, width, height);
            target2 = new RenderTarget2D(game.GraphicsDevice, width, height);
            isSet = false;
        }

        public void Set()
        {
            game.GraphicsDevice.SetRenderTarget(target);
            isSet = true;
            game.GraphicsDevice.Clear(Color.LightSkyBlue);
        }
        public void SetGUI()
        {
            game.GraphicsDevice.SetRenderTarget(target2);
            isSet2 = true;
            game.GraphicsDevice.Clear(Color.Transparent);
        }

        public void UnSetGUI()
        {
            game.GraphicsDevice.SetRenderTarget(null);
            isSet2 = false;
        }

        public void UnSet()
        {
            _lastFrame = (Texture2D)target;
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
            stretchRatio = rectWidth / target.Width;
            shiftAmountVector = new Vector2(rectX, rectY);

            return result;

        }

        public void Display(/*SpriteHandler spriteHandler, */Matrix transform, bool textureFiltering = true)
        {

           game.GraphicsDevice.Clear(Color.Black);

            Rectangle destinationRectangle = this.CalculateDestinationRectangle();
            Debug.WriteLine("width " + destinationRectangle.Width + "height " + destinationRectangle.Height);

            

            game.tileRenderer.Begin(Matrix.Identity, textureFiltering);
            game.tileRenderer.SpriteBatch.Draw(target, destinationRectangle, null, Color.White);
            game.tileRenderer.End();

            

            game.guiRenderer.Begin(Matrix.Identity, textureFiltering);
            game.guiRenderer.SpriteBatch.Draw(target2, destinationRectangle, null, Color.White);
            game.guiRenderer.End();


            /*spriteHandler.Begin(textureFiltering, transform);
            spriteHandler.Draw(target, null, destinationRectangle, Color.White);
            spriteHandler.End();*/
        }
        //public void Display()
    }
}
