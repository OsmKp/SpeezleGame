using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpeezleGame.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.UI
{
    public class Button : Component
    {

        private MouseState _currentMouse;
        private MouseState _previousMouse;

        private SpriteFont _font;

        private bool isHovering;

        private Texture2D _texture;
        private const int horizontalStretch = 4;
        private const int verticalStretch = 3;


        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public float Layer { get; set; }
        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        private Rectangle ButtonCollider
        {
            get { return new Rectangle((int)Math.Round(Position.X * ScreenManager.stretchRatio, 0) + (int)Math.Round(ScreenManager.shiftAmountVector.X, 0), (int)Math.Round(Position.Y * ScreenManager.stretchRatio, 0) + (int)Math.Round(ScreenManager.shiftAmountVector.Y, 0), (int)Math.Round(_texture.Width* horizontalStretch * ScreenManager.stretchRatio, 0), (int)Math.Round(_texture.Height* verticalStretch * ScreenManager.stretchRatio)); }
        }

        private Rectangle ButtonDrawSize
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width * horizontalStretch, _texture.Height * verticalStretch); }
        }

        public string Text { get; set; }

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            
            PenColour = Color.White;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            var colour = Color.White;
            if (isHovering)
            {
                colour = Color.DarkGray;
                
            }
            //spriteBatch.Draw(_texture, null, ButtonCollider, 0f, SpriteEffects.None, colour, Layer);
            spriteBatch.Draw(_texture, ButtonDrawSize, null, colour, 0, new Vector2(0, 0), SpriteEffects.None, Layer);
            
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (ButtonDrawSize.X + (ButtonDrawSize.Width / 2) - (_font.MeasureString(Text).X / 2) );
                var y = (ButtonDrawSize.Y + (ButtonDrawSize.Height / 2) - (_font.MeasureString(Text).Y / 2) - 4);

                //spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
            

        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var MouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            isHovering = false;

            if (MouseRectangle.Intersects(ButtonCollider))
            {
                isHovering = true;

                if(_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }

        }
    }
}
