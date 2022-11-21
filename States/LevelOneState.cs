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
        List<BaseEntity> _entities = new List<BaseEntity>();
        List<BaseEntity> _entitiesWoPlayer = new List<BaseEntity>();

        Texture2D mainMenuTexture;
        SpriteFont mainMenuFont;

        Camera camera;

        ContentManager contentManager;

        public LevelOneState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, BackgroundRenderer backgroundRenderer) : 
            base(graphicsDevice, guiRenderer, entityRenderer, backgroundRenderer)
        {
            
        }

        public override void Initialize(SpriteHandler sprite)
        {
            spriteHandler = sprite;
        }
        public override void LoadContent(ContentManager contentManager)
        {

            HandleUIInitialization(contentManager);
            guiRenderer.SetComponent(_components);

            HandleTileMap(contentManager);
            backgroundRenderer.SetMapHandler(_tileMapHandler);

            HandlePlayerInitialization(contentManager);
            entityRenderer.SetEntity(_entities);

            camera = new Camera(_player);
        }
        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload();
        }

        public override void Update(GameTime gameTime)
        {

            foreach (var component in _components)
                component.Update(gameTime);

            foreach (var entity in _entitiesWoPlayer)
                entity.Update(gameTime);

            HandlePlayerUpdate(gameTime);
            GameStateManager.UpdateCamera(camera.TransformMatrix);
            camera.Follow();
            //camera follow playr
        }
        public override void DrawGUI(GameTime gameTime)
        {
            foreach (Component comp in _components)
            {
                guiRenderer.Draw(gameTime);
            }

        }
        public override void DrawEntity(GameTime gameTime)
        {
            foreach (BaseEntity entity in _entities)
                entityRenderer.Draw(gameTime);
        }
        public override void DrawBackground(GameTime gameTime)
        {
            backgroundRenderer.Draw(gameTime);
        }
        /*public override void Draw(GameTime gameTime)
        {
            //_graphicsDevice.Clear(Color.CornflowerBlue);


            
            DrawUI(spriteBatch);

            _tileMapHandler.Draw(SpriteBatch);
            _player.Draw(spriteBatch, gameTime, GameStateManager.transformMatrix, spriteHandler);

        }*/

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, backgroundRenderer));
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
            _entities.Add(_player);
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
