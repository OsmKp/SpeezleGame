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
    public class LevelOneState : GameState
    {

        Player _player;

        private TileMapHandler _tileMapHandler;
        private TiledMap map; //
        private Dictionary<int, TiledTileset> tilesets; //
        private Texture2D tilesetTexture; //
        private TiledLayer collisionLayer; //
        private List<Rectangle> RectangleCollisionObjects;
        private List<TiledPolygon> PolygonCollisionObjects;
        SpriteHandler spriteHandler;

        List<Component> _components;

        Texture2D mainMenuTexture;
        SpriteFont mainMenuFont;

        ContentManager contentManager;

        public LevelOneState(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            
        }

        public override void Initialize(SpriteHandler sprite)
        {
            spriteHandler = sprite;
        }
        public override void LoadContent(ContentManager contentManager)
        {

            HandleUIInitialization(contentManager);

            HandleTileMap(contentManager);

            HandlePlayerInitialization(contentManager);
        }
        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            

            foreach (var component in _components)
                component.Update(gameTime);

            HandlePlayerUpdate(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //_graphicsDevice.Clear(Color.CornflowerBlue);

            DrawUI(spriteBatch);

            _tileMapHandler.Draw(GameStateManager.transformMatrix, spriteHandler);
            _player.Draw(spriteBatch, gameTime, GameStateManager.transformMatrix, spriteHandler);

        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new MainMenuState(_graphicsDevice));
        }

        private void HandleUIInitialization(ContentManager contentManager)
        {
            //main menu
            mainMenuTexture = contentManager.Load<Texture2D>("Test/TestButton");
            mainMenuFont = contentManager.Load<SpriteFont>("Test/FontTest");
            //

            //MainMenu Button
            Button MainMenuButton = new Button(mainMenuTexture, mainMenuFont)
            {
                Position = new Vector2(0, 0),
                Text = "Main Menu",
                Layer = 0.1f
            };

            MainMenuButton.Click += MainMenuButton_Click;

            _components = new List<Component>()
            {
                MainMenuButton,
            };
        }

        private void DrawUI(SpriteBatch spriteBatch)
        {


            //spriteBatch.Begin(samplerState: SamplerState.LinearClamp);

            foreach (var component in _components)
            {
                component.Draw(spriteBatch, spriteHandler);

            }


            //spriteBatch.End();
        }
        private void HandlePlayerInitialization(ContentManager contentManager)
        {
            Texture2D idleTexture = contentManager.Load<Texture2D>("Textures/main-char_idle_unarmed");
            Texture2D walkTexture = contentManager.Load<Texture2D>("Textures/main-char_walk_unarmed");

            PlayerTextureContainer playerContainer = new PlayerTextureContainer()
            {
                Idle = idleTexture,
                Walk = walkTexture
            };


            _player = new Player(playerContainer);
        }

        private void HandlePlayerUpdate(GameTime gameTime)
        {
            _player.Update(gameTime, KeyboardState, MouseState,/*PreviousMouseState,*/RectangleCollisionObjects, PolygonCollisionObjects); //update player every frame //note to myseld: fix polygon collision objects
        }
        private void HandleTileMap(ContentManager contentManager)
        {
            map = new TiledMap(contentManager.RootDirectory + "\\Test/tilemaptest.tmx");
            tilesets = map.GetTiledTilesets("Content/Test/");
            tilesetTexture = contentManager.Load<Texture2D>("Test/SpeezleTileSetPng");
            collisionLayer = map.Layers.First(l => l.name == "Collidable");
            _tileMapHandler = new TileMapHandler(_graphicsDevice, map, tilesets, tilesetTexture);

            RectangleCollisionObjects = new List<Rectangle>();
            PolygonCollisionObjects = new List<TiledPolygon>();
            foreach (var obj in collisionLayer.objects) //get all the collidable objects on the map
            {
                RectangleCollisionObjects.Add(new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height));
            }
        }
    }
}
