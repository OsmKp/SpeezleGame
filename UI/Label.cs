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
    public class Label : Component
    {
        private SpriteFont _font;

        private Texture2D _texture;

        public Rectangle? sourceRect;

        public float sourceRectRatioX = 1;

        public float Layer { get; set; }
        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }
        public int horizontalStretch = 4;
        public int verticalStretch = 3;

        public string Text { get; set; }

        public Label(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;

            PenColour = Color.White;
        }
        public Rectangle ButtonDrawSize
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)( _texture.Width * horizontalStretch * sourceRectRatioX), _texture.Height * verticalStretch); }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, ButtonDrawSize, sourceRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, Layer);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (ButtonDrawSize.X + (ButtonDrawSize.Width  / 2) - (_font.MeasureString(Text).X / 2));
                var y = (ButtonDrawSize.Y + (ButtonDrawSize.Height / 2) - (_font.MeasureString(Text).Y / 2) - 2);

                
                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0.1f);
            }
        }


        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
