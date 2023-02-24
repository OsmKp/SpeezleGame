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
using SpeezleGame.UserData;

namespace SpeezleGame.States
{
    public class MainMenuState : GameState
    {
        private List<Component> _components;

        private Background _background;

        Texture2D playSoloTexture;
        SpriteFont playSoloFont;

        private int coinAmount;


        public MainMenuState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game,SaveLoadManager saveLoadManager) : 
            base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer,backgroundRenderer ,game, saveLoadManager)
        {

        }
        public override void DrawGUI(GameTime gameTime)
        {
            foreach(Component comp in _components)
            {
                guiRenderer.Draw(gameTime);
            }
            
        }
        public override void DrawEntity(GameTime gameTime)
        {
            base.DrawEntity(gameTime);
        }
        public override void DrawTile(GameTime gameTime)
        {
            base.DrawTile(gameTime);
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
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            
            foreach (var component in _components)
                component.Update(gameTime);
        }

        private void HandleBackgroundInitialization(ContentManager contentManager)
        {
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Textures/MenuBackground");
            _background = new Background(backgroundTexture);
        }
        private void HandleUIInitialization(ContentManager contentManager)
        {
            playSoloTexture = contentManager.Load<Texture2D>("Test/GreyButton");
            playSoloFont = contentManager.Load<SpriteFont>("Test/generalFont");

            
            coinAmount = saveLoadManager.currentUser.GetCurrency();
            
            //PlaySolo Button
            Button PlaySolo = new Button(playSoloTexture, playSoloFont)
            {
                Position = new Vector2(260, 120),
                Text = "Play Solo",
                Layer = 0.1f,
            };

            PlaySolo.Click += PlaySolo_Click;
            //PlaySolo Button End

            //Quit Button
            Button Quit = new Button(playSoloTexture, playSoloFont)
            {
                Position = new Vector2(260, 170),
                Text = "Quit",
                Layer = 0.2f,
            };

            Quit.Click += Quit_Click;
            //Quit Button End

            Label CoinDisplay = new Label(playSoloTexture, playSoloFont)
            {
                Position = new Vector2(5, 5),
                Text = coinAmount.ToString(),
                Layer = 0.1f,
            };

            _components = new List<Component>()
            {
                PlaySolo,
                Quit,
                CoinDisplay,
            };
            
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            saveLoadManager.SaveUserData();
            game.Exit();

        }
        private void PlaySolo_Click(object sender, EventArgs e)
        {
            //load the next state
            
            GameStateManager.Instance.ChangeScreen(new LevelSelectionState(_graphicsDevice, guiRenderer, entityRenderer,tileRenderer, backgroundRenderer,game, saveLoadManager));
        }

    }
}
