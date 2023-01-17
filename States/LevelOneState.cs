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
using System.Reflection.Metadata;
using TiledCS;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using SpeezleGame.Core;
using SpeezleGame.Entities;
using SpeezleGame.Renderers;
using SpeezleGame.Graphics;
using SpeezleGame.MapComponents;

namespace SpeezleGame.States
{
    public class LevelOneState : GameState
    {

        Player _player;
        
        public int LevelTimeInt
        {
            get { return (int)Math.Round(levelTime); }
        }
        private float levelTime;
        
        private Background _background;

        private TileMapHandler _tileMapHandler;
        private TiledMap map; //
        private Dictionary<int, TiledTileset> tilesets; //
        private Texture2D tilesetTexture; //

        private TiledLayer collisionLayer; //
        private TiledLayer teleportLayer;
        private TiledLayer leverLayer;
        private TiledLayer doorLayer;
        private TiledLayer coinLayer;
        private TiledLayer endLayer;

        private List<Rectangle> RectangleMapObjects;
        private List<MapObject> MapObjects;



        private List<TiledPolygon> PolygonCollisionObjects;


        Button MainMenuButton;
        Button PauseMenuButton;
        Label DashCooldownLabel;
        Label SlideCooldownLabel;
        Label LevelTimeLabel;
        Label CoinDisplayLabel;

        List<Component> _components;
        List<BaseEntity> _entities = new List<BaseEntity>();
        List<BaseEntity> _entitiesWoPlayer = new List<BaseEntity>();

        Texture2D mainMenuTexture;
        SpriteFont mainMenuFont;
        Texture2D displayTexture;
        Texture2D pauseMenuTexture;
        Texture2D coinDisplayTexture;

        Camera camera;

        

        public LevelOneState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game) : 
            base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer,backgroundRenderer ,game)
        {
            
        }

        public override void Initialize()
        {
            
        }
        public override void LoadContent(ContentManager contentManager)
        {
            

            HandleBackgroundInitialization(contentManager);
            backgroundRenderer.SetBackground(_background);

            HandleTileMap(contentManager);
            tileRenderer.SetMapHandler(_tileMapHandler);

            HandlePlayerInitialization(contentManager);
            HandleLevelTimeInitialization();
            HandleEnemyInitialization(contentManager);

            entityRenderer.SetEntity(_entities);

            HandleUIInitialization(contentManager);
            guiRenderer.SetComponent(_components);

            

            

            camera = new Camera(_player, _graphicsDevice.Viewport);
        }

        public override void ReInitialize()
        {
            backgroundRenderer.SetBackground(_background);
            tileRenderer.SetMapHandler(_tileMapHandler);
            entityRenderer.SetEntity(_entities);
            guiRenderer.SetComponent(_components);
        }
        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload();

        }

        public override void Update(GameTime gameTime)
        {
            if(_components == null) { return; }
            
            foreach (var component in _components)
                
                component.Update(gameTime);


            HandlePlayerUpdate(gameTime);
            HandleLevelTime(gameTime);


            foreach (var entity in _entitiesWoPlayer)
                entity.Update(gameTime, _player.Position, RectangleMapObjects, PolygonCollisionObjects, MapObjects);

            GameStateManager.UpdateCamera(camera.TransformMatrix);
            camera.Follow();
            //camera follow playr
        }

        public override void DrawBackground(GameTime gameTime)
        {
            backgroundRenderer.Draw(gameTime);
        }
        public override void DrawEntity(GameTime gameTime)
        {
            
            foreach (BaseEntity entity in _entities)
                entityRenderer.Draw(gameTime);
        }


        public override void DrawTile(GameTime gameTime)
        {
            List<Vector2> tilesToNotRender = new List<Vector2>();
            if (_player != null)
            {
                tilesToNotRender = _player.GetUnrenderedTileList();
            }
                
            if (tileRenderer != null)
                tileRenderer.Draw(gameTime, tilesToNotRender);

        }

        public override void DrawGUI(GameTime gameTime)
        {
            
            if (_components == null) { return; }
            foreach (Component comp in _components)
            {
                guiRenderer.Draw(gameTime);
            }   

        }
        private void HandleLevelTimeInitialization()
        {
            levelTime = 0f;
        }
        private void HandleLevelTime(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            levelTime += elapsed;
            LevelTimeLabel.Text = "Time: " + LevelTimeInt;
            
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer,game));
        }

        private void PauseMenuButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.AddScreen(new PauseMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game));
        }
        private void HandleUIInitialization(ContentManager contentManager)
        {
            //main menu
            mainMenuTexture = contentManager.Load<Texture2D>("Test/GreyButton");
            mainMenuFont = contentManager.Load<SpriteFont>("Test/generalFont");
            //

            pauseMenuTexture = contentManager.Load<Texture2D>("Test/PauseButton2");
            displayTexture = contentManager.Load<Texture2D>("Test/DisplayLabel2");

            //MainMenu Button
            coinDisplayTexture = contentManager.Load<Texture2D>("Test/CoinDisplay");


            PauseMenuButton = new Button(pauseMenuTexture, mainMenuFont)
            {
                Position = new Vector2(304, 0),
                Text = "",
                Layer = 0.1f,
                horizontalStretch = 2,
                verticalStretch = 2,
            };

            CoinDisplayLabel = new Label(coinDisplayTexture, mainMenuFont)
            {
                Position = new Vector2(500, 0),
                Text = "  ",
                Layer = 0.1f,
                horizontalStretch = 3,
                verticalStretch = 2,
            };

            DashCooldownLabel = new Label(displayTexture, mainMenuFont)
            {
                Position = new Vector2(0, 50),
                Text = "Dash Cooldown",
                Layer = 0.1f,
                horizontalStretch = 3,
                verticalStretch = 2,
            };

            SlideCooldownLabel = new Label(displayTexture, mainMenuFont)
            {
                Position = new Vector2(0, 75),
                Text = "Slide Cooldown",
                Layer = 0.1f,
                horizontalStretch = 3,
                verticalStretch = 2,

            };

            LevelTimeLabel = new Label(displayTexture, mainMenuFont)
            {
                Position = new Vector2(0, 0),
                Text = "Time: ",
                Layer = 0.1f,
                horizontalStretch = 4,
                verticalStretch = 3,
            };

            
            PauseMenuButton.Click += PauseMenuButton_Click;
            _components = new List<Component>()
            {
                
                PauseMenuButton,
                DashCooldownLabel,
                SlideCooldownLabel,
                LevelTimeLabel,
                CoinDisplayLabel,
            };
        }

        private void HandleBackgroundInitialization(ContentManager contentManager)
        {
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Textures/LevelBackground6");
            _background = new Background(backgroundTexture);
        }

        private void HandleEnemyInitialization(ContentManager contentManager)
        {
            
            Texture2D idleTexture = contentManager.Load<Texture2D>("Textures/Enemy1IdleAnim");
            Texture2D walkTexture = contentManager.Load<Texture2D>("Textures/Enemy1WalkAnim");


            EnemyTextureContainer enemyContainer1 = new EnemyTextureContainer()
            {
                Idle = idleTexture,
                Walk = walkTexture,

            };


            List<Vector2> waypoints1 = new List<Vector2>();
            waypoints1.Add(new Vector2(1340, 750));
            waypoints1.Add(new Vector2(1500, 750));

            List<Vector2> waypoints2 = new List<Vector2>();
            waypoints2.Add(new Vector2(1600, 750));
            waypoints2.Add(new Vector2(1800 , 750));

            List<Vector2> waypoints3 = new List<Vector2>();
            waypoints3.Add(new Vector2(1050, 750));
            waypoints3.Add(new Vector2(1250, 750));


            PatrollingEnemy enemy1 = new PatrollingEnemy(enemyContainer1, 0.1f, waypoints1);
            PatrollingEnemy enemy2 = new PatrollingEnemy(enemyContainer1, 0.1f, waypoints2);
            PatrollingEnemy enemy3 = new PatrollingEnemy(enemyContainer1, 0.1f, waypoints3);

            _entities.Add(enemy1);
            _entities.Add(enemy2);
            _entities.Add(enemy3);
            _entitiesWoPlayer.Add(enemy1);
            _entitiesWoPlayer.Add(enemy2);
            _entitiesWoPlayer.Add(enemy3);
        }

        private void HandlePlayerInitialization(ContentManager contentManager)
        {
            
            Texture2D idleTexture = contentManager.Load<Texture2D>("Textures/HogRiderIdleAnimPngBetter");
            Texture2D walkTexture = contentManager.Load<Texture2D>("Textures/HogRiderWalkAnimPng-Sheet");
            Texture2D dashTexture = contentManager.Load<Texture2D>("Textures/HogRiderDashAnim");
            Texture2D slideTexture = contentManager.Load<Texture2D>("Textures/HogRiderSlideAnim");

            PlayerTextureContainer playerContainer = new PlayerTextureContainer()
            {
                Idle = idleTexture,
                Walk = walkTexture,
                Dash = dashTexture,
                Slide = slideTexture
            };


            _player = new Player(playerContainer, _graphicsDevice);
            _entities.Add(_player);
        }

        private void HandlePlayerUpdate(GameTime gameTime)
        {
            _player.Update(gameTime, KeyboardState, MouseState,/*PreviousMouseState,*/RectangleMapObjects, PolygonCollisionObjects, MapObjects); //update player every frame //note to myseld: fix polygon collision objects
            DashCooldownLabel.Text = "Dash: " + _player.DashCooldownString;
            SlideCooldownLabel.Text = "Slide: " + _player.SlideCooldownString;
            CoinDisplayLabel.Text = " " + _player.CoinsCollected;
            _player.timeInLevel = LevelTimeInt;
        }
        private void HandleTileMap(ContentManager contentManager)
        {
            

            map = new TiledMap(contentManager.RootDirectory + "\\Test/tilemaptest.tmx");
            tilesets = map.GetTiledTilesets("Content/Test/");
            tilesetTexture = contentManager.Load<Texture2D>("Test/SpeezleTileSetPng");
            collisionLayer = map.Layers.First(l => l.name == "Collidable");
            teleportLayer = map.Layers.First(l => l.name == "Teleport");
            leverLayer = map.Layers.First(l => l.name == "Lever");
            doorLayer = map.Layers.First(l => l.name == "Door");
            coinLayer = map.Layers.First(l => l.name == "Coin");
            endLayer = map.Layers.First(l => l.name == "End");

            _tileMapHandler = new TileMapHandler(_graphicsDevice, map, tilesets, tilesetTexture);

            RectangleMapObjects = new List<Rectangle>();
            MapObjects = new List<MapObject>();


            

            PolygonCollisionObjects = new List<TiledPolygon>();
            foreach (var obj in collisionLayer.objects) //get all the collidable objects on the map
            {
                RectangleMapObjects.Add(new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height));
            }

            

            foreach (var obj in teleportLayer.objects) //get all the teleport objects on the map
            {
                MapObjects.Add(new TeleportObject(obj.id, new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height), int.Parse(obj.name)));
                
            }

            

            foreach (var obj in leverLayer.objects)
            {
                MapObjects.Add(new LeverObject(obj.id, new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height), int.Parse(obj.name)));
            }

            

            foreach (var obj in doorLayer.objects)
            {
                MapObjects.Add(new DoorObject(obj.id, new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height)));
            }

            foreach(var obj in coinLayer.objects)
            {
                MapObjects.Add(new CoinObject(obj.id, new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height)));
            }

            foreach(var obj in endLayer.objects)
            {
                MapObjects.Add(new EndObject(obj.id, new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height), obj.name));
            }
            
        }
    }
}
