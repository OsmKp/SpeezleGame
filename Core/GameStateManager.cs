using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SpeezleGame.Core
{

    public class GameStateManager
    {
        private ContentManager Content;
        private static GameStateManager _instance;
        private Stack<GameState> _screens = new Stack<GameState>();
        private ScreenManager screenManager;
        private SpriteHandler spriteHandler;
        private Game game;

        
        public static Matrix transformMatrix; //maybe make a new class for resolution handling


        public static GameStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameStateManager();
                }
                return _instance;
            }
        }
        public GameStateManager()
        {

            
        }

        public void Initialize(Game game)
        {
            this.game = game;
            
            var MapSize = new Vector2(960, 320);
            screenManager = new ScreenManager(this.game, 640, 360);
            spriteHandler = new SpriteHandler(this.game);

            //transformMatrix = Matrix.CreateScale(new Vector3(Dimensions / MapSize, 1)); //a matrix that allows me to scale everything drawn on the screen
        }
        public void SetContent(ContentManager content)
        {
            Content = content;
        }

        public void UnloadContent()
        {
            foreach (GameState state in _screens)
            {
                state.UnloadContent(Content);
            }
        }
        public void AddScreen(GameState screen)
        {
            try
            {
                // Add the screen to the stack
                _screens.Push(screen);
                // Initialize the screen
                _screens.Peek().Initialize(spriteHandler);
                // Call the LoadContent on the screen
                if (Content != null)
                {
                    _screens.Peek().LoadContent(Content);

                }
            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }

        // Removes the top screen from the stack
        public void RemoveScreen()
        {
            if (_screens.Count > 0)
            {
                try
                {
                    var screen = _screens.Peek();
                    _screens.Pop();
                }
                catch (Exception ex)
                {
                    // Log the exception
                }
            }
        }

        public void ClearScreens()
        {
            while (_screens.Count > 0)
            {
                _screens.Pop();
            }
        }

        public void ChangeScreen(GameState screen)
        {
            try
            {
                ClearScreens();
                AddScreen(screen);
            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }

        public void Update(GameTime gameTime)
        {
            try
            {
                if (_screens.Count > 0)
                {
                    _screens.Peek().Update(gameTime);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            try
            {
                if (_screens.Count > 0)
                {
                    screenManager.Set();
                    spriteHandler.Begin(false);
                    _screens.Peek().Draw(spriteBatch, gameTime);
                    spriteHandler.End();
                    screenManager.UnSet();

                    screenManager.Display(spriteHandler);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
