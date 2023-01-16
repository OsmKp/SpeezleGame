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
    public class PauseMenuState : GameState
    {
        private Background _background;

        private List<Component> _components;

        Texture2D buttonTexture;
        Texture2D backFrameTexture;
        SpriteFont generalFont;

        public PauseMenuState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game)
        {
        }

        public override void DrawGUI(GameTime gameTime)
        {
            Debug.WriteLine("PAUSE MENU DRAWWWWWWW");
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
        /*public override void SetBackgroundTexture(Texture2D bg)
        {
            Debug.WriteLine("setbgtextfirst");
            _background = new Background(bg);
        }*/
        public override void LoadContent(ContentManager content)
        {
            Debug.WriteLine("end of level init??");

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
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Textures/LevelBackground");
            _background = new Background(backgroundTexture);
        }

        private void HandleUIInitialization(ContentManager contentManager)
        {
            buttonTexture = contentManager.Load<Texture2D>("Test/GreyButton");
            backFrameTexture = contentManager.Load<Texture2D>("Test/BackFrame");
            generalFont = contentManager.Load<SpriteFont>("Test/generalFont");


            Label BackFrame = new Label(backFrameTexture, generalFont)
            {
                Position = new Vector2(224, 32),
                Text = "",
                Layer = 0.3f,
                horizontalStretch = 4,
                verticalStretch = 4,
            };


            //Quit Button
            Button Quit = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(256, 205),
                Text = "Quit",
                Layer = 0.2f,
            };

            Quit.Click += Quit_Click;
            //Quit Button End

            Button Resume = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(256, 67),
                Text = "Resume",
                Layer = 0.2f,
            };
            Resume.Click += Resume_Click;

            Button Settings = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(256, 136),
                Text = "Settings",
                Layer = 0.2f,
            };

            _components = new List<Component>()
            {

                BackFrame,
                Quit,
                Resume,
                Settings,
            };


        }

        private void Resume_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.RemoveScreen();
        }

        private void Quit_Click(object sender, EventArgs e)
        {

            GameStateManager.Instance.ChangeScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game));
            
            
        }
    }
}
