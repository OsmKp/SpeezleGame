using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.Core;
using SpeezleGame.Graphics;
using SpeezleGame.Renderers;
using SpeezleGame.UI;
using SpeezleGame.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.States
{
    public class DeathState : GameState
    {
        private string levelNamePlayed;

        List<Component> _components;

        private Background _background;

        SpriteFont font;
        Texture2D buttonTexture;
        Texture2D backFrameTexture;
        Texture2D transparentTexture;

        Label LevelMessage;

        public DeathState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game, SaveLoadManager saveLoadManager) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager)
        {
        }

        public override void Initialize()
        {
            
        }

        public override void LoadContent(ContentManager content)
        {
            HandleBackgroundInitialization(content);
            backgroundRenderer.SetBackground(_background);

            HandleUIInitialization(content);
            guiRenderer.SetComponent(_components);
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

        public override void InitializeForDeath(string _levelNameFinished)
        {
            levelNamePlayed = _levelNameFinished;
            LevelMessage.Text = "You died, level " + levelNamePlayed + " failed.";
        }

        private void HandleBackgroundInitialization(ContentManager contentManager)
        {
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Textures/EndBackground");
            _background = new Background(backgroundTexture);
        }

        private void HandleUIInitialization(ContentManager contentManager)
        {

            font = contentManager.Load<SpriteFont>("Test/generalFont");
            buttonTexture = contentManager.Load<Texture2D>("Test/EndButton");
            backFrameTexture = contentManager.Load<Texture2D>("Test/EndBackFrame");
            transparentTexture = contentManager.Load<Texture2D>("Test/trans");

            Label BackFrame = new Label(backFrameTexture, font)
            {
                Position = new Vector2(176, 64),
                Text = "",
                Layer = 0.3f,
                horizontalStretch = 3,
                verticalStretch = 3,

            };
            //Backframe Label End
            LevelMessage = new Label(transparentTexture, font)
            {
                Position = new Vector2(288, 130),
                Text = "",
                Layer = 0.2f,
                horizontalStretch = 5,
                verticalStretch = 4,
            };

            Button Replay = new Button(buttonTexture, font)
            {
                Position = new Vector2(156, 232),
                Text = "Replay",
                Layer = 0.2f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            Replay.Click += Replay_Click;

            //Quit Button
            Button Menu = new Button(buttonTexture, font)
            {
                Position = new Vector2(288, 232),
                Text = "Levels",
                Layer = 0.2f,

                horizontalStretch = 2,
                verticalStretch = 2,
            };

            Menu.Click += Menu_Click;
            //Quit Button End

            _components = new List<Component>()
            {
                BackFrame,
                LevelMessage,
                Replay,
                Menu,
            };

        }

        private void Replay_Click(object sender, EventArgs e)
        {
            switch (levelNamePlayed)
            {
                case "One":
                    GameStateManager.Instance.ChangeScreen(new LevelOneState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
                    break;
                case "Two":
                    GameStateManager.Instance.ChangeScreen(new LevelTwoState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
                    break;
                case "Three":

                    break;
            }

        }

        private void Menu_Click(object sender, EventArgs e)
        {

            GameStateManager.Instance.ChangeScreen(new LevelSelectionState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }
    }
}
