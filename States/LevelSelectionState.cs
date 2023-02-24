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
using SpeezleGame.Renderers;
using SpeezleGame.Graphics;
using System.Collections;
using SpeezleGame.UserData;

namespace SpeezleGame.States
{
    public class LevelSelectionState : GameState
    {

        private List<Component> _components;

        private Background _background;

        Texture2D buttonTexture;
        Texture2D labelTexture;
        Texture2D backFrameTexture;
        Texture2D levelTexture;
        SpriteFont generalFont;




        public LevelSelectionState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game, SaveLoadManager saveLoadManager) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game,  saveLoadManager)
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
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Test/PauseBackground");
            _background = new Background(backgroundTexture);
        }

        private void HandleUIInitialization(ContentManager contentManager)
        {
            labelTexture = contentManager.Load<Texture2D>("Test/EndLabel");
            buttonTexture = contentManager.Load<Texture2D>("Test/GreyButton");
            backFrameTexture = contentManager.Load<Texture2D>("Test/LevelSelectionBackFrame");
            levelTexture = contentManager.Load<Texture2D>("Test/LevelButton");

            generalFont = contentManager.Load<SpriteFont>("Test/generalFont");



            //Backframe Label
            Label BackFrame = new Label(backFrameTexture, generalFont)
            {
                Position = new Vector2(96, 48),
                Text = "",
                Layer = 0.3f,
                horizontalStretch = 7,
                verticalStretch = 7,

            };
            //Backframe Label End

            Button LevelOne = new Button(levelTexture, generalFont)
            {
                Position = new Vector2(138, 126),
                Text = "1",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 4,
            };

            LevelOne.Click += LevelOne_Click;

            Button LevelTwo = new Button(levelTexture, generalFont)
            {
                Position = new Vector2(213, 126),
                Text = "2",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 4,
            };

            LevelTwo.Click += LevelTwo_Click;

            Button LevelThree = new Button(levelTexture, generalFont)
            {
                Position = new Vector2(288, 126),
                Text = "3",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 4,
            };

            Button LevelFour = new Button(levelTexture, generalFont)
            {
                Position = new Vector2(363, 126),
                Text = "4",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 4,
            };

            Button LevelFive = new Button(levelTexture, generalFont)
            {
                Position = new Vector2(436, 126),
                Text = "5",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 4,
            };

            //Quit Button
            Button Back = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(4, 4),
                Text = "Back",
                Layer = 0.2f,

                horizontalStretch = 3,
                verticalStretch = 3,
            };

            Back.Click += Menu_Click;
            //Quit Button End

            _components = new List<Component>()
            {
                BackFrame,
                Back,
                LevelOne,
                LevelTwo,
                LevelThree,
                LevelFour,
                LevelFive,

            };




        }

        private void LevelOne_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new LevelOneState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }
        private void LevelTwo_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new LevelTwoState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }

        private void Menu_Click(object sender, EventArgs e)
        {
           
            GameStateManager.Instance.ChangeScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }
    }
}
