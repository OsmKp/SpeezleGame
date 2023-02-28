using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.Core;
using SpeezleGame.Graphics;
using SpeezleGame.Renderers;
using SpeezleGame.UI;
using SpeezleGame.UserData;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.States
{
    public class ShopState : GameState
    {
        private List<Component> _components;

        private Background _background;

        Texture2D displayButtonTexture;
        Texture2D buyButtonTexture;
        Texture2D backButtonTexture;
        Texture2D skin1Texture;
        Texture2D skin2Texture;
        SpriteFont font;

        Button BuyButton1;
        Button BuyButton2;
        Label CoinDisplay;

        private int coinAmount;

        private int cactusPrice = 10;

        bool isCactusBought;
        string button1Text = "Buy Cactus";
        public ShopState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game, SaveLoadManager saveLoadManager) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager)
        {
        }

        public override void Initialize()
        {
            
        }
        public override void DrawBackground(GameTime gameTime)
        {
            backgroundRenderer.Draw(gameTime);
        }
        public override void DrawGUI(GameTime gameTime)
        {
            foreach (Component comp in _components)
            {
                guiRenderer.Draw(gameTime);
            }
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
            foreach (var component in _components)
                component.Update(gameTime);
        }
        private void HandleBackgroundInitialization(ContentManager contentManager)
        {
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Textures/PurpleBackground");
            _background = new Background(backgroundTexture);
        }

        private void UpdateText()
        {
            coinAmount = saveLoadManager.currentUser.GetCurrency();
            CoinDisplay.Text = coinAmount.ToString();

            isCactusBought = saveLoadManager.currentUser.IsSkinOwned("Cactus");
            if (isCactusBought) { button1Text = "Cactus Already Owned"; }
            BuyButton1.Text = button1Text;


        }
        private void HandleUIInitialization(ContentManager contentManager)
        {
            isCactusBought = saveLoadManager.currentUser.IsSkinOwned("Cactus"); 
            if (isCactusBought) { button1Text = "Cactus Already Owned"; }

            font = contentManager.Load<SpriteFont>("Test/generalFont");
            displayButtonTexture = contentManager.Load<Texture2D>("Test/CoinDisplay");
            buyButtonTexture = contentManager.Load<Texture2D>("Test/DisplayGuiTexture");
            skin1Texture = contentManager.Load<Texture2D>("Test/TestSkinFrame");
            skin2Texture = contentManager.Load<Texture2D>("Test/TestSkinFrame2");
            backButtonTexture = contentManager.Load<Texture2D>("Test/GreyButton");
            coinAmount = saveLoadManager.currentUser.GetCurrency();

            

            BuyButton1 = new Button(buyButtonTexture, font)
            {
                Position = new Vector2(116, 260),
                
                Text = button1Text,
                Layer = 0.5f,
            };

            BuyButton1.Click += BuyButton1_Click;

            Label PriceLabel1 = new Label(displayButtonTexture, font)
            {
                Position = new Vector2(120, 50),
                Text = cactusPrice.ToString(),
                Layer = 0.2f,
            };

            Label SkinFrame1 = new Label(skin1Texture, font)
            {
                Position = new Vector2(100, 84),
                Text = "",
                Layer = 0.4f,
                horizontalStretch = 5,
                verticalStretch = 4,
            };

            Label SkinFrame2 = new Label(skin2Texture, font)
            {
                Position = new Vector2(380, 84),
                Text = "",
                Layer = 0.4f,
                horizontalStretch = 5,
                verticalStretch = 4,
            };

            BuyButton2 = new Button(buyButtonTexture, font)
            {
                Position = new Vector2(396, 260),
                Text = "Buy",
                Layer = 0.5f,
            };

            BuyButton2.Click += BuyButton2_Click;

            Label PriceLabel2 = new Label(displayButtonTexture, font)
            {
                Position = new Vector2(400, 50),
                Text = "10",
                Layer = 0.2f,
            };

            CoinDisplay = new Label(displayButtonTexture, font)
            {
                Position = new Vector2(264, 5),
                Text = saveLoadManager.currentUser.GetCurrency().ToString(),
                Layer = 0.2f,
                horizontalStretch = 3,
                verticalStretch = 2,
            };

            Button BackButton = new Button(backButtonTexture, font)
            {
                Position = new Vector2(5, 5),
                Text = "Back",
                Layer = 0.5f,
                horizontalStretch = 3,
                verticalStretch = 2,
            };

            BackButton.Click += BackButton_Click;

            _components = new List<Component>()
            {
                
                PriceLabel1,
                SkinFrame1,
                BuyButton1,
                SkinFrame2,
                PriceLabel2,
                BuyButton2,
                BackButton,
                CoinDisplay,
            };
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }

        private void BuyButton1_Click(object sender, EventArgs e)
        {
            if(isCactusBought == false && saveLoadManager.currentUser.GetCurrency() >= cactusPrice)
            {
                saveLoadManager.currentUser.AddCurrency(-cactusPrice);
                saveLoadManager.currentUser.AddNewSkin("Cactus");
                UpdateText();
            }
        }

        private void BuyButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
