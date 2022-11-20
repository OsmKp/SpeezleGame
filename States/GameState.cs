using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeezleGame.UI;
using SpeezleGame.Entities.Players;
using SpeezleGame.Physics;
using System.Reflection.Metadata;
using TiledCS;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using SpeezleGame.Core;

namespace SpeezleGame.States
{
    public abstract class GameState
    {
        protected GraphicsDevice _graphicsDevice;
        public KeyboardState KeyboardState
        {
            get
            {
                return Keyboard.GetState();
            }
        }
        
        public MouseState MouseState
        {
            get
            {
                
                return Mouse.GetState();
            }
        }

        public GameState(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }
        public abstract void Initialize(SpriteHandler sprite);
        public abstract void LoadContent(ContentManager content);
        public abstract void UnloadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
