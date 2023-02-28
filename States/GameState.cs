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
using SpeezleGame.Renderers;
using SpeezleGame.Core;
using SpeezleGame.UserData;

namespace SpeezleGame.States
{
    public abstract class GameState
    {
        protected GraphicsDevice _graphicsDevice;
        protected SpeezleGame.Core.SpeezleGame game;

        protected GUIRenderer guiRenderer;
        protected EntityRenderer entityRenderer;
        protected TileRenderer tileRenderer;
        protected BackgroundRenderer backgroundRenderer;

        protected SaveLoadManager saveLoadManager;
        

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

        public GameState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer,Core.SpeezleGame game, SaveLoadManager saveLoadManager)
        {
            _graphicsDevice = graphicsDevice;
            this.guiRenderer = guiRenderer;
            this.entityRenderer = entityRenderer;
            this.tileRenderer = tileRenderer;
            this.backgroundRenderer = backgroundRenderer;
            this.game = game;
            this.saveLoadManager = saveLoadManager;
        }





        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);

        public virtual void InitializeForEnd(int _timeLevelTook, string _levelNameFinished, int _coinsCollected) { }
        public virtual void InitializeForDeath(string _levelNameFinished) { }
        
        public abstract void UnloadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);

        public virtual void SetBackgroundTexture(Texture2D backgroundTexture) { }
        public virtual void ReInitialize() { }
        public virtual void DrawEntity(GameTime gameTime) { }
        public virtual void DrawGUI(GameTime gameTime) { }
        public virtual void DrawTile(GameTime gameTime) { }
        public virtual void DrawBackground(GameTime gameTime) { }


    }
}
