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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpeezleGame.UserData.SaveLoadManager;

namespace SpeezleGame.States
{
    public class SaveSlotsState : GameState
    {

        private List<Component> _components;


        private Background _background;

        SpriteFont generalFont;
        Texture2D buttonTexture;
        Texture2D backFrame;
        Texture2D invisibleTexture;
        Texture2D displayTexture;

        private List<SaveFile> SaveFiles;
        

        SaveFile saveFile1;
        SaveFile saveFile2;
        SaveFile saveFile3;


        public SaveSlotsState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game,SaveLoadManager saveLoadManager) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game,  saveLoadManager)
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
            if(_background == null) { return; }
            backgroundRenderer.Draw(gameTime);
        }
        public override void Initialize()
        {
            
            InitializeSlotData();

        }

        public override void LoadContent(ContentManager content)
        {
            HandleBackgroundInitialization(content);
            backgroundRenderer.SetBackground(_background);

            HandleUIInitialization(content);
            guiRenderer.SetComponent(_components);

            DisplaySlots();

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
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Textures/MenuBackground");
            _background = new Background(backgroundTexture);
        }

        private void HandleUIInitialization(ContentManager contentManager)
        {
            buttonTexture = contentManager.Load<Texture2D>("Test/GreyButton");
            generalFont = contentManager.Load<SpriteFont>("Test/generalFont");
            backFrame = contentManager.Load<Texture2D>("Test/SlotBackFrame");
            invisibleTexture = contentManager.Load<Texture2D>("Test/trans");
            displayTexture = contentManager.Load<Texture2D>("Test/DisplayLabel2");

            int slot1Currency = saveFile1.Currency;
            int slot2Currency = saveFile2.Currency;
            int slot3Currency = saveFile3.Currency;

            int slot1CompletedLevels = saveFile1.NumberOfCompletedLevels;
            int slot2CompletedLevels = saveFile2.NumberOfCompletedLevels;
            int slot3CompletedLevels = saveFile3.NumberOfCompletedLevels;

            int slot1SkinsOwned = saveFile1.NumberOfSkinsOwned;
            int slot2SkinsOwned = saveFile2.NumberOfSkinsOwned;
            int slot3SkinsOwned = saveFile3.NumberOfSkinsOwned;

            Label Slot1Back = new Label(backFrame, generalFont)
            {
                Position = new Vector2(20, 60),
                Text = "",
                Layer = 0.4f,
                
                verticalStretch = 5,
            };

            Label Slot2Back = new Label(backFrame, generalFont)
            {
                Position = new Vector2(220, 60),
                Text = "",
                Layer = 0.4f,
                
                verticalStretch = 5,
            };

            Label Slot3Back = new Label(backFrame, generalFont)
            {
                Position = new Vector2(420, 60),
                Text = "",
                Layer = 0.4f,
                
                verticalStretch = 5,
            };

            Label Slot1Label = new Label(invisibleTexture, generalFont)
            {
                Position = new Vector2(84, 80),
                Text = "Slot 1",
                Layer = 0.1f,

                
            };

            Label Slot2Label = new Label(invisibleTexture, generalFont)
            {
                Position = new Vector2(284, 80),
                Text = "Slot 2",
                Layer = 0.1f,


            };

            Label Slot3Label = new Label(invisibleTexture, generalFont)
            {
                Position = new Vector2(484, 80),
                Text = "Slot 3",
                Layer = 0.1f,


            };

            Label Slot1Currency = new Label(displayTexture, generalFont)
            {
                Position = new Vector2(52, 110),
                Text = "Currency: " + slot1Currency.ToString(),
                Layer = 0.1f,


            };
            Label Slot2Currency = new Label(displayTexture, generalFont)
            {
                Position = new Vector2(252, 110),
                Text = "Currency: " + slot2Currency.ToString(),
                Layer = 0.1f,


            };
            Label Slot3Currency = new Label(displayTexture, generalFont)
            {
                Position = new Vector2(452, 110),
                Text = "Currency: " + slot3Currency.ToString(),
                Layer = 0.1f,


            };
            Label Slot1Levels = new Label(displayTexture, generalFont)
            {
                Position = new Vector2(52, 150),
                Text = "Completed Levels: " + slot1CompletedLevels.ToString(),
                Layer = 0.1f,


            };
            Label Slot2Levels = new Label(displayTexture, generalFont)
            {
                Position = new Vector2(252, 150),
                Text = "Completed Levels: " + slot2CompletedLevels.ToString(),
                Layer = 0.1f,


            };
            Label Slot3Levels = new Label(displayTexture, generalFont)
            {
                Position = new Vector2(452, 150),
                Text = "Completed Levels: " + slot3CompletedLevels.ToString(),
                Layer = 0.1f,


            };

            Label Slot1Skins = new Label(displayTexture, generalFont)
            {
                Position = new Vector2(52, 190),
                Text = "Skins Owned: " + slot1SkinsOwned.ToString(),
                Layer = 0.1f,


            };

            Label Slot2Skins = new Label(displayTexture, generalFont)
            {
                Position = new Vector2(252, 190),
                Text = "Skins Owned: " + slot2SkinsOwned.ToString(),
                Layer = 0.1f,


            };

            Label Slot3Skins = new Label(displayTexture, generalFont)
            {
                Position = new Vector2(452, 190),
                Text = "Skins Owned: " + slot3SkinsOwned.ToString(),
                Layer = 0.1f,


            };

            Button Slot1 = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(68, 232),
                Text = "Select",
                Layer = 0.2f,
                horizontalStretch = 3,
                verticalStretch = 2,
            };

            Slot1.Click += Save1_Click;
            

            
            Button Slot2 = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(268, 232),
                Text = "Select",
                Layer = 0.2f,
                horizontalStretch = 3,
                verticalStretch = 2,
            };

            Slot2.Click += Save2_Click;
            

            Button Slot3 = new Button(buttonTexture, generalFont)
            {
                Position = new Vector2(468, 232),
                Text = "Select",
                Layer = 0.2f,
                horizontalStretch = 3,
                verticalStretch = 2,
            };

            Slot3.Click += Save3_Click;

            Label Message = new Label(invisibleTexture, generalFont)
            {
                Position = new Vector2(290, 10),
                Text = "Welcome, Please Select A Slot!",
                Layer = 0.2f,
                horizontalStretch = 3,
                verticalStretch = 2,
            };

            _components = new List<Component>()
            {
                Slot1Back,
                Slot2Back,
                Slot3Back,
                Slot1Label,
                Slot2Label,
                Slot3Label,
                Slot1Currency,
                Slot2Currency,
                Slot3Currency,
                Slot1Levels,
                Slot2Levels,
                Slot3Levels,
                Slot1Skins,
                Slot2Skins,
                Slot3Skins,
                Slot1,
                Slot2,
                Slot3,
                Message,
            };

        }
        private void InitializeSlotData()
        {
            SaveFiles = new List<SaveFile>();

            saveFile1 = new SaveFile("slot1.json");
            
            saveFile2 = new SaveFile("slot2.json");
            
            saveFile3 = new SaveFile("slot3.json");


            SaveData saveData1;

            if (saveFile1.empty == false)
            {
                saveData1 = saveLoadManager.LoadSlotData(saveFile1);
                saveFile1.PrepareSlotPreview(saveData1);
            }

            SaveData saveData2;

            if (saveFile2.empty == false)
            {
                saveData2 = saveLoadManager.LoadSlotData(saveFile2);
                saveFile2.PrepareSlotPreview(saveData2);
            }

            SaveData saveData3;

            if (saveFile3.empty == false)
            {
                saveData3 = saveLoadManager.LoadSlotData(saveFile3);
                saveFile3.PrepareSlotPreview(saveData3);
            }

            SaveFiles.Add(saveFile1);
            SaveFiles.Add(saveFile2);
            SaveFiles.Add(saveFile3);
            


        }
        private void DisplaySlots()
        {
            foreach (SaveFile sf in SaveFiles)
            {
                if (sf.empty == false)
                {

                }
                else
                {

                }
            }
        }

        private void Save1_Click(object sender, EventArgs e)
        {
            saveLoadManager.selectedSaveFile = saveFile1;
            saveLoadManager.LoadSlotData();

            GameStateManager.Instance.AddScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
            //load menu screen

            
           
        }
        private void Save2_Click(object sender, EventArgs e)
        {
            saveLoadManager.selectedSaveFile = saveFile2;
            saveLoadManager.LoadSlotData();

            GameStateManager.Instance.AddScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
            //load menu screen

        }
        private void Save3_Click(object sender, EventArgs e)
        {
            saveLoadManager.selectedSaveFile = saveFile3;
            saveLoadManager.LoadSlotData();

            GameStateManager.Instance.AddScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
            //load menu screen

        }

        
    }
}
