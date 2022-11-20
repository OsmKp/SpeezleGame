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


        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public float Layer { get; set; }
        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        private Rectangle ButtonCollider
        {
            get { return new Rectangle((int)Position.X, (int) Position.Y, _texture.Width, _texture.Height); }
        }

        public string Text { get; set; }

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            
            PenColour = Color.BlueViolet;
        }


        public override void Draw(SpriteBatch spriteBatch, SpriteHandler spriteHandler)
        {
            var colour = Color.Red;
            if (isHovering)
            {
                colour = Color.Aqua;
                
            }
            spriteHandler.Draw(_texture, null, ButtonCollider, 0f, SpriteEffects.None, colour, Layer);
            //spriteBatch.Draw(_texture, ButtonCollider, null, colour, 0, new Vector2(0, 0), SpriteEffects.None, Layer);
            
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (ButtonCollider.X + (ButtonCollider.Width / 2) - (_font.MeasureString(Text).X / 2));
                var y = (ButtonCollider.Y + (ButtonCollider.Height / 2) - (_font.MeasureString(Text).Y / 2));

                spriteHandler.DrawString(_font, Text, new Vector2(x, y), PenColour);
                //spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
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
