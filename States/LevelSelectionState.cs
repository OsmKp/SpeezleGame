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
        Texture2D threeStarTexture;
        Texture2D lockTexture;

        Label LevelTwoLock;
        Label LevelThreeLock;
        Label LevelFourLock;
        Label LevelFiveLock;

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

        private void HandleLocks()
        {
            bool level2unlock = saveLoadManager.currentUser.IsLevelUnlocked("Two");
            bool level3unlock = saveLoadManager.currentUser.IsLevelUnlocked("Three");
            bool level4unlock = saveLoadManager.currentUser.IsLevelUnlocked("Four");
            bool level5unlock = saveLoadManager.currentUser.IsLevelUnlocked("Five");

            if (!level2unlock) { _components.Add(LevelTwoLock); }
            if (!level3unlock) { _components.Add(LevelThreeLock); }
            if (!level4unlock) { _components.Add(LevelFourLock); }
            if (!level5unlock) { _components.Add(LevelFiveLock); }
        }
        private void HandleUIInitialization(ContentManager contentManager)
        {
            labelTexture = contentManager.Load<Texture2D>("Test/EndLabel");
            buttonTexture = contentManager.Load<Texture2D>("Test/GreyButton");
            backFrameTexture = contentManager.Load<Texture2D>("Test/LevelSelectionBackFrame");
            levelTexture = contentManager.Load<Texture2D>("Test/LevelButton");
            threeStarTexture = contentManager.Load<Texture2D>("Test/ThreeStars");
            lockTexture = contentManager.Load<Texture2D>("Test/Lock");

            generalFont = contentManager.Load<SpriteFont>("Test/generalFont");


            int level1stars = saveLoadManager.currentUser.GetStarsAchieved("One");
            int level2stars = saveLoadManager.currentUser.GetStarsAchieved("Two");
            int level3stars = saveLoadManager.currentUser.GetStarsAchieved("Three");
            int level4stars = saveLoadManager.currentUser.GetStarsAchieved("Four");
            int level5stars = saveLoadManager.currentUser.GetStarsAchieved("Five");

            Rectangle rect1 = new Rectangle(0, 0, 16 * level1stars, 16);
            float rect1RatioX = (float)(16 * level1stars) / threeStarTexture.Width;

            Rectangle rect2 = new Rectangle(0, 0, 16 * level2stars, 16);
            float rect2RatioX = (float)(16 * level2stars) / threeStarTexture.Width;

            Rectangle rect3 = new Rectangle(0, 0, 16 * level3stars, 16);
            float rect3RatioX = (float)(16 * level3stars) / threeStarTexture.Width;

            Rectangle rect4 = new Rectangle(0, 0, 16 * level4stars, 16);
            float rect4RatioX = (float)(16 * level4stars) / threeStarTexture.Width;

            Rectangle rect5 = new Rectangle(0, 0, 16 * level5stars, 16);
            float rect5RatioX = (float)(16 * level5stars) / threeStarTexture.Width;

            

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

            Label LevelOneStars = new Label(threeStarTexture, generalFont)
            {
                Position = new Vector2(145, 116),
                Text = "",
                Layer = 0.1f,
                horizontalStretch = 1,
                verticalStretch = 1,
                sourceRect = rect1,
                sourceRectRatioX = rect1RatioX,
            };

            Button LevelTwo = new Button(levelTexture, generalFont)
            {
                Position = new Vector2(213, 126),
                Text = "2",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 4,
                
            };

            LevelTwo.Click += LevelTwo_Click;

            Label LevelTwoStars = new Label(threeStarTexture, generalFont)
            {
                Position = new Vector2(220, 116),
                Text = "",
                Layer = 0.1f,
                horizontalStretch = 1,
                verticalStretch = 1,
                sourceRect = rect2,
                sourceRectRatioX = rect2RatioX,
            };

            LevelTwoLock = new Label(lockTexture, generalFont)
            {
                Position = new Vector2(230, 111),
                Text = "",
                Layer = 0.05f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            Button LevelThree = new Button(levelTexture, generalFont)
            {
                Position = new Vector2(288, 126),
                Text = "3",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 4,
            };

            LevelThree.Click += LevelThree_Click;


            Label LevelThreeStars = new Label(threeStarTexture, generalFont)
            {
                Position = new Vector2(295, 116),
                Text = "",
                Layer = 0.1f,
                horizontalStretch = 1,
                verticalStretch = 1,
                sourceRect = rect3,
                sourceRectRatioX = rect3RatioX,
            };

            LevelThreeLock = new Label(lockTexture, generalFont)
            {
                Position = new Vector2(3055, 111),
                Text = "",
                Layer = 0.05f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            Button LevelFour = new Button(levelTexture, generalFont)
            {
                Position = new Vector2(363, 126),
                Text = "4",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 4,
            };

            LevelFour.Click += LevelFour_Click;

            Label LevelFourStars = new Label(threeStarTexture, generalFont)
            {
                Position = new Vector2(370, 116),
                Text = "",
                Layer = 0.1f,
                horizontalStretch = 1,
                verticalStretch = 1,
                sourceRect = rect4,
                sourceRectRatioX = rect4RatioX,
            };

            LevelFourLock = new Label(lockTexture, generalFont)
            {
                Position = new Vector2(380, 111),
                Text = "",
                Layer = 0.05f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            Button LevelFive = new Button(levelTexture, generalFont)
            {
                Position = new Vector2(436, 126),
                Text = "5",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 4,
            };

            LevelFive.Click += LevelFive_Click;

            Label LevelFiveStars = new Label(threeStarTexture, generalFont)
            {
                Position = new Vector2(443, 116),
                Text = "",
                Layer = 0.1f,
                horizontalStretch = 1,
                verticalStretch = 1,
                sourceRect = rect5,
                sourceRectRatioX = rect5RatioX,
            };

            LevelFiveLock = new Label(lockTexture, generalFont)
            {
                Position = new Vector2(453, 111),
                Text = "",
                Layer = 0.05f,
                horizontalStretch = 2,
                verticalStretch = 2,
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
                LevelOneStars,
                LevelTwo,
                LevelTwoStars,
                LevelThree,
                LevelThreeStars,
                LevelFour,
                LevelFourStars,
                LevelFive,
                LevelFiveStars,

            };

            HandleLocks();


        }

        private void LevelOne_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new LevelOneState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }
        private void LevelTwo_Click(object sender, EventArgs e)
        {
            if (saveLoadManager.currentUser.IsLevelUnlocked("Two"))
            {
                GameStateManager.Instance.ChangeScreen(new LevelTwoState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
            }
            
        }

        private void LevelThree_Click(object sender, EventArgs e)
        {
            if (saveLoadManager.currentUser.IsLevelUnlocked("Three"))
            {
                GameStateManager.Instance.ChangeScreen(new LevelThreeState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
            }

        }

        private void LevelFour_Click(object sender, EventArgs e)
        {
            if (saveLoadManager.currentUser.IsLevelUnlocked("Four"))
            {
                GameStateManager.Instance.ChangeScreen(new LevelFourState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
            }
        }

        private void LevelFive_Click(object sender, EventArgs e)
        {
            if (saveLoadManager.currentUser.IsLevelUnlocked("Five"))
            {
                GameStateManager.Instance.ChangeScreen(new LevelFiveState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
            }
        }

        private void Menu_Click(object sender, EventArgs e)
        {
           
            GameStateManager.Instance.ChangeScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }
    }
}
