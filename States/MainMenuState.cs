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

namespace SpeezleGame.States
{
    public class MainMenuState : GameState
    {
        private List<Component> _components;
        private SpriteHandler spritehandler;

        Texture2D playSoloTexture;
        SpriteFont playSoloFont;


        public MainMenuState(GraphicsDevice graphics) : base(graphics)
        {

        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawUI(spriteBatch);
        }

        public override void Initialize(SpriteHandler spriteHandler)
        {
            this.spritehandler = spriteHandler;
        }

        public override void LoadContent(ContentManager content)
        {
            HandleUIInitialization(content);
            
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
        private void DrawUI(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.LinearClamp);

            foreach (var component in _components)
                component.Draw(spriteBatch, spritehandler);

            //spriteBatch.End();
        }

        private void HandleUIInitialization(ContentManager contentManager)
        {
            playSoloTexture = contentManager.Load<Texture2D>("Test/TestButton");
            playSoloFont = contentManager.Load<SpriteFont>("Test/FontTest");


            //PlaySolo Button
            Button PlaySolo = new Button(playSoloTexture, playSoloFont)
            {
                Position = new Vector2(0, 0),
                Text = "Play Solo",
                Layer = 0.1f,
            };

            PlaySolo.Click += PlaySolo_Click;
            //PlaySolo Button End

            //Quit Button
            Button Quit = new Button(playSoloTexture, playSoloFont)
            {
                Position = new Vector2(0, 150),
                Text = "Quit",
                Layer = 0.2f,
            };

            Quit.Click += Quit_Click;
            //Quit Button End

            _components = new List<Component>()
            {
                PlaySolo,
                Quit,
            };
            
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            //_speezleGame.Exit();
        }
        private void PlaySolo_Click(object sender, EventArgs e)
        {
            //load the next state
            
            GameStateManager.Instance.ChangeScreen(new LevelOneState(_graphicsDevice));
        }

    }
}
