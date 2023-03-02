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

        public static int ScreenWidth = 1920; 
        public static int ScreenHeight = 1080; 
        
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

        public Rectangle CalculateDestinationRectangle() //Calculate the dimensions depending on the device
        {
            Rectangle backbufferBounds = this.game.GraphicsDevice.PresentationParameters.Bounds; //monitor screen size
            float backbufferAspectRatio = (float)backbufferBounds.Width / backbufferBounds.Height;
            float screenAspectRatio = (float)Width / Height; //target aspect ratio

            float rectX = 0f;
            float rectY = 0f;
            float rectWidth = backbufferBounds.Width;
            float rectHeight = backbufferBounds.Height; 
            
            if(backbufferAspectRatio > screenAspectRatio) //if the aspect ratio of the monitor is larger than target aspect ratio
            {
                rectWidth = rectHeight * screenAspectRatio; //calculate the largest target rectangle width while maintaining the height
                rectX = (backbufferBounds.Width - rectWidth) / 2f; //calculate top left X-coordinate
            }
            else if(screenAspectRatio > backbufferAspectRatio) //if the aspect ratio of the monitor is smaller than target aspect ratio
            {
                rectHeight = rectWidth / screenAspectRatio; //calculate largest rectangle height while maintaining the aspect ratio
                rectY = (backbufferBounds.Height - rectHeight) / 2f; //calculate top left y-coordinate
            }

            Rectangle result = new Rectangle((int)rectX, (int)rectY, (int)rectWidth, (int)rectHeight); //create the target rectangle
            stretchRatio = rectWidth / target.Width; //calculate stretch ratio
            shiftAmountVector = new Vector2(rectX, rectY);

            return result;

        }
         
        public void Display(Matrix transform, bool textureFiltering = true)
        {
            game.GraphicsDevice.Clear(Color.Black);

            Rectangle destinationRectangle = this.CalculateDestinationRectangle();

            game.tileRenderer.Begin(Matrix.Identity, textureFiltering);
            game.tileRenderer.SpriteBatch.Draw(target, destinationRectangle, null, Color.White); //draw tiles
            game.tileRenderer.End();

            game.guiRenderer.Begin(Matrix.Identity, textureFiltering);
            game.guiRenderer.SpriteBatch.Draw(target2, destinationRectangle, null, Color.White); //draw gui
            game.guiRenderer.End();

        }
        
    }
}
