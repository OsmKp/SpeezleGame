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
using System.Security.Cryptography.X509Certificates;
using SpeezleGame.Core;
using SpeezleGame.Entities;
using SpeezleGame.Renderers;
using System.ComponentModel;
using Component = SpeezleGame.UI.Component;
using SpeezleGame.Graphics;

namespace SpeezleGame.States
{
    public class EndOfLevelState : GameState
    {
        private List<Component> _components;
        private string levelNameFinished;
        private int timeLevelTook;

        private int coinsCollectedInPreviousLevel;

        private Background _background;

        Texture2D buttonTexture;
        Texture2D labelTexture;
        Texture2D backFrameTexture;
        Texture2D frontFrameTexture;
        Texture2D coinTexture;
        Texture2D coinCountTexture;

        Label CoinCount;

        SpriteFont generalFont;
        public EndOfLevelState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer,game)
        {
        }

        

        public override void DrawGUI(GameTime gameTime)
        {
            if (_components == null) { return; }
            foreach (Component comp in _components)
            {
                guiRenderer.Draw(gameTime);
            }

        }

        public override void DrawBackground(GameTime gameTime)
        {
            backgroundRenderer.Draw(gameTime);
        }

        public override void Initialize()
        {
            
        }

        public override void InitializeForEnd(int _timeLevelTook, string _levelNameFinished, int _coinsCollected)
        {
            levelNameFinished = _levelNameFinished;
            timeLevelTook = _timeLevelTook;
            coinsCollectedInPreviousLevel = _coinsCollected;
            CoinCount.Text = coinsCollectedInPreviousLevel.ToString();
        }

        public override void LoadContent(ContentManager content)
        {
            

            HandleBackgroundInitialization(content);
            backgroundRenderer.SetBackground(_background);

            HandleUIInitialization(content);
            guiRenderer.SetComponent(_components);
        }

        public override void UnloadContent(ContentManager content)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (_components == null) { return; }
            
            foreach (var component in _components)
            {
                
                component.Update(gameTime);
            }


        }
        private void HandleBackgroundInitialization(ContentManager contentManager)
        {
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Textures/EndBackground");
            _background = new Background(backgroundTexture);
        }

        private void HandleUIInitialization(ContentManager contentManager)
        {
            labelTexture = contentManager.Load<Texture2D>("Test/EndLabel");
            buttonTexture = contentManager.Load<Texture2D>("Test/EndButton");
            backFrameTexture = contentManager.Load<Texture2D>("Test/EndBackFrame");
            frontFrameTexture = contentManager.Load<Texture2D>("Test/EndFrontFrame");
            coinTexture = contentManager.Load<Texture2D>("Test/coinpng");
            coinCountTexture = contentManager.Load<Texture2D>("Test/trans");
            generalFont = contentManager.Load<SpriteFont>("Test/generalFont");
            

            //Backframe Label
            Label BackFrame = new Label(backFrameTexture, generalFont)
            {
                Position = new Vector2(176, 64),
                Text = "",
                Layer = 0.3f,
                horizontalStretch = 3,
                verticalStretch = 3,

            };
            //Backframe Label End

            Label FrontFrame = new Label(frontFrameTexture, generalFont)
            {
                Position = new Vector2(200, 108),
                Text = "",
                Layer = 0.2f,
                horizontalStretch = 5,
                verticalStretch = 4,
            };

            Label CoinImage = new Label(coinTexture, generalFont)
            {
                Position = new Vector2(220, 124),
                Text = "",
                Layer = 0.2f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            CoinCount = new Label(coinCountTexture, generalFont)
            {
                Position = new Vector2(248, 124),
                Text = "",
                Layer = 0.2f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            Button Next = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(416, 232),
                Text = "Next",
                Layer = 0.2f,
                
                horizontalStretch = 2,
                verticalStretch = 2,
            };
            Button Replay = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(156, 232),
                Text = "Replay",
                Layer = 0.2f,
                
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            //Quit Button
            Button Menu = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(288, 232),
                Text = "Menu",
                Layer = 0.2f,
                
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            Menu.Click += Menu_Click;
            //Quit Button End

            _components = new List<Component>()
            {
                BackFrame,
                FrontFrame,
                Next,
                Replay,
                CoinImage,
                CoinCount,
                Menu,
            };


        }

        private void Menu_Click(object sender, EventArgs e)
        {
            //_speezleGame.Exit();
            game.Exit();
        }
    }
}
