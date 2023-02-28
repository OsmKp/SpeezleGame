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
using SpeezleGame.UserData;

namespace SpeezleGame.States
{
    public class EndOfLevelState : GameState
    {
        private int ThreeStarTime = 30;
        private int TwoStarTime = 50;
        private int OneStarTime = 70;

        private Dictionary<string, int[]> levelStarTimes;
        private int StarsAchieved;

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
        Texture2D starTexture;
        Texture2D clockTexture;
        Texture2D clockCountTexture;

        Label ClockCount;
        Label CoinCount;
        Label Star1;
        Label Star2;
        Label Star3;
        Label Celebration;
        Label LevelMessage;

        SpriteFont generalFont;
        public EndOfLevelState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game, SaveLoadManager saveLoadManager) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer,game,  saveLoadManager)
        {
            levelStarTimes = new Dictionary<string, int[]>()
            {
                { "one", new [] {30,50,70} },
                {"Two", new [] { 15, 30, 45} },
                {"Three", new[]{20,30,40} },
                {"Four", new[]{20,30,40} },
                {"Five",new[] {20,30,40} },
            };

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
            ClockCount.Text = _timeLevelTook.ToString() + " Sec";
            CalculateStar(_timeLevelTook);
            Celebration.Text = "You Achieved " + StarsAchieved + " Stars!";
            LevelMessage.Text = "Level " + _levelNameFinished + " Cleared!";

            UpdateUserStats();
            
        }
        private void UpdateUserStats()
        {
            string nextLevelName = "Two";
            if (levelNameFinished == "One")
                nextLevelName = "Two";
            if (levelNameFinished == "Two")
                nextLevelName = "Three";
            if (levelNameFinished == "Three")
                nextLevelName = "Four";
            if (levelNameFinished == "Four")
                nextLevelName = "Five";

            Debug.WriteLine("user name is: " + saveLoadManager.currentUser.Name);
            Debug.WriteLine("coins: " + saveLoadManager.currentUser.GetCurrency());
            saveLoadManager.currentUser.AddCurrency(coinsCollectedInPreviousLevel);

            foreach(UserLevelData uld in saveLoadManager.currentUser.userLevelDatas)
            {
                if(uld.LevelName == levelNameFinished)
                {
                    if (StarsAchieved > uld.StarsAchieved)
                        uld.StarsAchieved = StarsAchieved;

                    if (timeLevelTook < uld.BestTimeSeconds)
                        uld.BestTimeSeconds = timeLevelTook;

                    uld.Completed = true;


                }

                if (levelNameFinished != "Five" && uld.LevelName == nextLevelName)
                {
                    uld.Unlocked = true;
                }
            }
            

        }

        private void CalculateStar(int time)
        {
            foreach(var kvp in levelStarTimes)
            {
                if(kvp.Key == levelNameFinished)
                {
                    ThreeStarTime = kvp.Value[0];
                    TwoStarTime = kvp.Value[1];
                    OneStarTime = kvp.Value[2];
                }
            }

            if (time < ThreeStarTime)
            {
                StarsAchieved = 3;
                _components.Add(Star1);
                _components.Add(Star2);
                _components.Add(Star3);
            }
            else if(time < TwoStarTime)
            {
                StarsAchieved = 2;
                _components.Add(Star1);
                _components.Add(Star2);
            }
            else if(time < OneStarTime)
            {
                StarsAchieved = 1;
                _components.Add(Star1);
                

            }
            else { StarsAchieved = 0; }
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
            clockCountTexture = contentManager.Load<Texture2D>("Test/trans");
            starTexture = contentManager.Load<Texture2D>("Test/Star");
            clockTexture = contentManager.Load<Texture2D>("Test/clock");

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
            LevelMessage = new Label(coinCountTexture, generalFont)
            {
                Position = new Vector2(288, 180),
                Text = "",
                Layer = 0.2f,
                horizontalStretch = 5,
                verticalStretch = 4,
            };

            Celebration = new Label(coinCountTexture, generalFont)
            {
                Position = new Vector2(288, 72),
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

            Label ClockImage = new Label(clockTexture, generalFont)
            {
                Position = new Vector2(220, 164),
                Text = "",
                Layer = 0.2f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            ClockCount = new Label(clockCountTexture, generalFont)
            {
                Position = new Vector2(258, 164),
                Text = "",
                Layer = 0.2f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };


            Star1 = new Label(starTexture, generalFont)
            {
                Position = new Vector2(284, 55),
                Text = "",
                Layer = 0.1f,
                horizontalStretch = 2,
                verticalStretch = 2,

            };

            Star2 = new Label(starTexture, generalFont)
            {
                Position = new Vector2(304, 50),
                Text = "",
                Layer = 0.1f,
                horizontalStretch = 2,
                verticalStretch = 2,

            };

            Star3 = new Label(starTexture, generalFont)
            {
                Position = new Vector2(324, 55),
                Text = "",
                Layer = 0.1f,
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

            Next.Click += Next_Click;

            Button Replay = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(156, 232),
                Text = "Replay",
                Layer = 0.2f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            Replay.Click += Replay_Click;

            //Quit Button
            Button Menu = new Button(buttonTexture, generalFont)
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
                Next,
                Replay,
                CoinImage,
                CoinCount,
                ClockImage,
                ClockCount,
                Menu,
                Celebration,
                LevelMessage,
            };


        }

        private void Replay_Click(object sender, EventArgs e)
        {
            switch (levelNameFinished)
            {
                case "One":
                    GameStateManager.Instance.ChangeScreen(new LevelOneState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
                    break;
                case "Two":
                    GameStateManager.Instance.ChangeScreen(new LevelTwoState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
                    break;
                case "Three":
                    GameStateManager.Instance.ChangeScreen(new LevelThreeState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
                    break;
            }
            
        }

        private void Next_Click(object sender, EventArgs e)
        {
            switch (levelNameFinished)
            {
                case "One":
                    GameStateManager.Instance.ChangeScreen(new LevelTwoState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
                    break;
                case "Two":
                    GameStateManager.Instance.ChangeScreen(new LevelThreeState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
                    break;
                case "Three":
                    GameStateManager.Instance.ChangeScreen(new LevelThreeState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
                    break;
            }
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            
            GameStateManager.Instance.ChangeScreen(new LevelSelectionState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }
    }
}
