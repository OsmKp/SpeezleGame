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
    public class CustomizeState : GameState
    {
        private List<Component> _components;

        private Background _background;

        Texture2D buyButtonTexture;
        Texture2D backButtonTexture;
        Texture2D skin1Texture;
        Texture2D skin2Texture;
        Texture2D skin3Texture;
        SpriteFont font;

        Button Button1;
        Button Button2;
        Button Button3;

        bool isSteveEquipped;
        string button1Text = "Equipped";

        bool isCactusBought;
        bool isCactusEquipped;
        string button2Text = "Not Unlocked";

        bool isFlowerEquipped;
        bool isFlowerBought;
        string button3Text = "Not Unlocked";
        public CustomizeState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game, SaveLoadManager saveLoadManager) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager)
        {
        }

        public override void Initialize()
        {
            
        }

        public override void DrawGUI(GameTime gameTime)
        {
            foreach (Component comp in _components)
            {
                guiRenderer.Draw(gameTime);
            }
        }

        public override void DrawBackground(GameTime gameTime)
        {
            backgroundRenderer.Draw(gameTime);
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
        private void HandleOwnership()
        {
            isCactusBought = saveLoadManager.currentUser.IsSkinOwned("Cactus");
            isFlowerBought = saveLoadManager.currentUser.IsSkinOwned("Flower");

            if (saveLoadManager.currentUser.GetEquippedSkin() == "Steve")
            {
                button1Text = "Equipped";
                button2Text = "Equip";
                button3Text = "Equip";
            }
            else if (saveLoadManager.currentUser.GetEquippedSkin() == "Cactus")
            {
                button1Text = "Equip";
                button2Text = "Equipped";
                button3Text = "Equip";
            }
            else
            {
                button1Text = "Equip";
                button2Text = "Equip";
                button3Text = "Equipped";
            }

            if (!isCactusBought)
            {
                button2Text = "Not Unlocked";
            }
            if (!isFlowerBought)
            {
                button3Text = "Not Unlocked";
            }
        }
        
        private void UpdateText()
        {
            isCactusBought = saveLoadManager.currentUser.IsSkinOwned("Cactus");
            isFlowerBought = saveLoadManager.currentUser.IsSkinOwned("Flower");

            if (saveLoadManager.currentUser.GetEquippedSkin() == "Steve")
            {
                button1Text = "Equipped";
                button2Text = "Equip";
                button3Text = "Equip";
            }
            else if (saveLoadManager.currentUser.GetEquippedSkin() == "Cactus")
            {
                button1Text = "Equip";
                button2Text = "Equipped";
                button3Text = "Equip";
            }
            else
            {
                button1Text = "Equip";
                button2Text = "Equip";
                button3Text = "Equipped";
            }

            if (!isCactusBought)
            {
                button2Text = "Not Unlocked";
            }
            if (!isFlowerBought)
            {
                button3Text = "Not Unlocked";
            }

            Button1.Text = button1Text;
            Button2.Text = button2Text;
            Button3.Text = button3Text;
        }

        private void HandleUIInitialization(ContentManager contentManager)
        {
            HandleOwnership();


            font = contentManager.Load<SpriteFont>("Test/generalFont");
            
            buyButtonTexture = contentManager.Load<Texture2D>("Test/DisplayGuiTexture");
            skin1Texture = contentManager.Load<Texture2D>("Test/SteveSkinFrame");
            skin2Texture = contentManager.Load<Texture2D>("Test/TestSkinFrame");
            skin3Texture = contentManager.Load<Texture2D>("Test/FlowerSkinFrame");
            backButtonTexture = contentManager.Load<Texture2D>("Test/GreyButton");

            Label SkinFrame1 = new Label(skin1Texture, font)
            {
                Position = new Vector2(60, 74),
                Text = "",
                Layer = 0.4f,
                horizontalStretch = 5,
                verticalStretch = 4,
            };

            Label SkinFrame2 = new Label(skin2Texture, font)
            {
                Position = new Vector2(235, 74),
                Text = "",
                Layer = 0.4f,
                horizontalStretch = 5,
                verticalStretch = 4,
            };

            Label SkinFrame3 = new Label(skin3Texture, font)
            {
                Position = new Vector2(410, 74),
                Text = "",
                Layer = 0.4f,
                horizontalStretch = 5,
                verticalStretch = 4,
            };

            Button1 = new Button(buyButtonTexture, font)
            {
                Position = new Vector2(76, 250),
                Text = button1Text,
                Layer = 0.4f,
            };

            Button1.Click += Button1_Click;

            Button2 = new Button(buyButtonTexture, font)
            {
                Position = new Vector2(251, 250),
                Text = button2Text,
                Layer = 0.4f,
            };

            Button2.Click += Button2_Click;

            Button3 = new Button(buyButtonTexture, font)
            {
                Position = new Vector2(416, 250),
                Text = button3Text,
                Layer = 0.4f,
            };

            Button3.Click += Button3_Click;

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
                SkinFrame1,
                SkinFrame2,
                SkinFrame3,
                Button1,
                Button2,
                Button3,
                BackButton,
            };

        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if(saveLoadManager.currentUser.IsSkinOwned("Steve") == true)
            {
                saveLoadManager.currentUser.SetEquippedSkin("Steve");
                UpdateText();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (saveLoadManager.currentUser.IsSkinOwned("Cactus") == true)
            {
                saveLoadManager.currentUser.SetEquippedSkin("Cactus");
                UpdateText();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (saveLoadManager.currentUser.IsSkinOwned("Flower") == true)
            {
                saveLoadManager.currentUser.SetEquippedSkin("Flower");
                UpdateText();
            }
        }
    }
}
