using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.Renderers;
using SpeezleGame.States;
using SpeezleGame.UserData;
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
        GUIRenderer guiRenderer;
        EntityRenderer entityRenderer;
        TileRenderer tileRenderer;
        BackgroundRenderer backgroundRenderer;
        GraphicsDevice graphicsDevice;

        private ContentManager Content;
        private static GameStateManager _instance;
        private Stack<GameState> _screens = new Stack<GameState>();

        private ScreenManager screenManager;
        private SaveLoadManager saveLoadManager;

        private SpeezleGame game;
        private int renderingTargetDimW = 640; //eveyrthing is drawn onto a rendering target to be stretched depending on the device
        private int renderingTargetDimH = 360;
        public static Matrix TransformMatrix = Matrix.Identity;
        
        public static Matrix transformMatrix;

        public User CurrentUser;


        public static GameStateManager Instance //A singleton controlling the game states
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

        public void Initialize(SpeezleGame game, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, GraphicsDevice graphicsDevice)
        {
            this.game = game;
            this.guiRenderer = guiRenderer;
            this.entityRenderer = entityRenderer;
            this.tileRenderer = tileRenderer;
            this.backgroundRenderer = backgroundRenderer;
            this.graphicsDevice = graphicsDevice;

            var MapSize = new Vector2(960, 320);
            screenManager = new ScreenManager(this.game, renderingTargetDimW, renderingTargetDimH);

            CurrentUser = new User();
            CurrentUser.InitializeUser();
            Debug.WriteLine("userleveldata: " + CurrentUser.userLevelDatas.Count);
            Debug.WriteLine("initilaized user");
            saveLoadManager = new SaveLoadManager(CurrentUser);
            

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

        public static void UpdateCamera(Matrix transform)
        {
            TransformMatrix = transform;
        }
        public void AddScreen(GameState screen)
        {
            try
            {
                // Add the screen to the stack
                _screens.Push(screen);
                // Initialize the screen

                _screens.Peek().Initialize();


                

                
                
                // Call the LoadContent on the screen
                if (Content != null)
                {
                    _screens.Peek().LoadContent(Content);
                    
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        // Remove the top screen from the stack
        public void RemoveScreen()
        {
            if (_screens.Count > 0)
            {
                try
                {
                    var screen = _screens.Peek();
                    _screens.Pop();
                    _screens.Peek().ReInitialize();
                }
                catch (Exception ex)
                {
                    
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
                
            }
        }

        public void LoadDeathScreen(string levelName)
        {
            this.ChangeScreen(new DeathState(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
            _screens.Peek().InitializeForDeath(levelName);
        }
        public void LoadEndScreen(int timeInLevel, string levelName, int coinsCollected) //This is for the level ending screen
        {
            
            this.ChangeScreen(new EndOfLevelState(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer ,game,  saveLoadManager));
            _screens.Peek().InitializeForEnd(timeInLevel, levelName, coinsCollected);
        }

        public void Update(GameTime gameTime)
        {
            try
            {
                if (_screens.Count > 0)
                {
                    _screens.Peek().Update(gameTime); //Update the screen at the top of the stack
                }
            }
            catch (Exception ex)
            {

            }
        }

        public SaveLoadManager GetSaveLoadManager()
        {
            return saveLoadManager;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            try
            {
                if (_screens.Count > 0)
                {

                    screenManager.Set();

                    backgroundRenderer.Begin(Matrix.Identity, false);
                    _screens.Peek().DrawBackground(gameTime);
                    backgroundRenderer.End();

                    tileRenderer.Begin(TransformMatrix, false);
                    _screens.Peek().DrawTile(gameTime);
                    tileRenderer.End();

                    entityRenderer.Begin(TransformMatrix, false);
                    _screens.Peek().DrawEntity(gameTime);
                    entityRenderer.End();


                    screenManager.UnSet();

                    screenManager.SetGUI();

                    guiRenderer.Begin(Matrix.Identity, false);
                    _screens.Peek().DrawGUI(gameTime);
                    guiRenderer.End();

                    screenManager.UnSetGUI();


                    screenManager.Display(TransformMatrix);


                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
