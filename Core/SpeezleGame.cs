using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledCS;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeezleGame.Entities.Players;
using SpeezleGame.Graphics;
using static System.Net.Mime.MediaTypeNames;
using SpeezleGame.Physics;
using SpeezleGame.UI;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using SpeezleGame.States;

namespace SpeezleGame.Core
{
    public class SpeezleGame : Game
    {
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Color _backgroundColour = Color.CornflowerBlue;


        //private Player _player;



        public SpeezleGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {



            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = ScreenManager.ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenManager.ScreenHeight;
            _graphics.ApplyChanges(); //set temporary window size

            GameStateManager.Instance.Initialize(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameStateManager.Instance.SetContent(Content);
            GameStateManager.Instance.AddScreen(new MainMenuState(GraphicsDevice));

            //ScreenManager.Instance.LoadContent(Content, GraphicsDevice);
            
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameStateManager.Instance.Update(gameTime);

            //ScreenManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            GameStateManager.Instance.UnloadContent();
            //ScreenManager.Instance.UnloadContent();
        }
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.CornflowerBlue);

            GameStateManager.Instance.Draw(_spriteBatch, gameTime);

            //ScreenManager.Instance.Draw(_spriteBatch, gameTime);
            base.Draw(gameTime);

           
            

            
        }


    }
}