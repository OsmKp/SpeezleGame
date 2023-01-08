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
        Texture2D celebrationTexture;
        SpriteFont celebrationFont;
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
            Debug.WriteLine("correct thing run i think");
        }

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
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Textures/CloudBackground");
            _background = new Background(backgroundTexture);
        }

        private void HandleUIInitialization(ContentManager contentManager)
        {
            buttonTexture = contentManager.Load<Texture2D>("Test/GreyButton");
            celebrationTexture = contentManager.Load<Texture2D>("Test/WhiteLabel");
            celebrationFont = contentManager.Load<SpriteFont>("Test/generalFont");

            //Congrats Label
            Label Celebration = new Label(celebrationTexture, celebrationFont)
            {
                Position = new Vector2(300, 150),
                Text = "Congrats",
                Layer = 0.1f,
            };
            //Congrats Button End

            //Quit Button
            Button Quit = new Button(buttonTexture, celebrationFont)
            {
                Position = new Vector2(0, 150),
                Text = "Quit",
                Layer = 0.2f,
            };

            Quit.Click += Quit_Click;
            //Quit Button End

            _components = new List<Component>()
            {
                
                Celebration,
                Quit,
            };


        }

        private void Quit_Click(object sender, EventArgs e)
        {
            //_speezleGame.Exit();
            game.Exit();
        }
    }
}
