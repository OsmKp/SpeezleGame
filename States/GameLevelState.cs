using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.AI;
using SpeezleGame.Core;
using SpeezleGame.Entities.Players;
using SpeezleGame.Entities;
using SpeezleGame.Graphics;
using SpeezleGame.MapComponents;
using SpeezleGame.Renderers;
using SpeezleGame.UI;
using SpeezleGame.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace SpeezleGame.States
{
    public abstract class GameLevelState : GameState
    {
        protected Player _player;
        protected NodeMap nodeMap;
        protected int LevelTimeInt
        {
            get { return (int)Math.Round(levelTime); }
        }
        protected float levelTime;

        protected Background _background;

        protected TileMapHandler _tileMapHandler;
        protected TiledMap map; //
        protected Dictionary<int, TiledTileset> tilesets; //
        protected Texture2D tilesetTexture; //
        protected string tilemapPathName;

        protected TiledLayer collisionLayer; //
        protected TiledLayer teleportLayer;
        protected TiledLayer leverLayer;
        protected TiledLayer doorLayer;
        protected TiledLayer coinLayer;
        protected TiledLayer endLayer;
        protected TiledLayer nodeLayer;
        protected TiledLayer spawnLayer;
        protected Vector2 PlayerStartCoord;

        protected List<Rectangle> RectangleMapObjects;
        protected List<MapObject> MapObjects;

        protected List<Node> NodeList;


        protected List<TiledPolygon> PolygonCollisionObjects;



        protected Button PauseMenuButton;
        protected Label DashCooldownLabel;
        protected Label SlideCooldownLabel;
        protected Label LevelTimeLabel;
        protected Label CoinDisplayLabel;
        protected Label PlayerHealthLabel;
        protected Label PlayerHealthBackLabel;

        protected List<Component> _components;
        protected List<BaseEntity> _entities = new List<BaseEntity>();
        protected List<BaseEntity> _entitiesWoPlayer = new List<BaseEntity>();

        protected Texture2D mainMenuTexture;
        protected SpriteFont mainMenuFont;
        protected Texture2D displayTexture;
        protected Texture2D pauseMenuTexture;
        protected Texture2D coinDisplayTexture;

        protected Texture2D healthBarTexture;
        protected Texture2D healthBackTexture;

        protected string backgroundTexturePathName;

        protected Camera camera;
        protected GameLevelState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game, SaveLoadManager saveLoadManager) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager)
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
            Debug.WriteLine("uinitititi?");
            guiRenderer.SetComponent(_components);



            camera = new Camera(_player, _graphicsDevice.Viewport, map.Width * 16, map.Height * 16);
        }
        public override void Initialize()
        {
            
        }
        public override void UnloadContent(ContentManager content)
        {
            
        }
        public override void ReInitialize()
        {
            backgroundRenderer.SetBackground(_background);
            tileRenderer.SetMapHandler(_tileMapHandler);
            entityRenderer.SetEntity(_entities);
            guiRenderer.SetComponent(_components);
        }

        public override void Update(GameTime gameTime)
        {
            if (_components == null) { return; }

            foreach (var component in _components)

                component.Update(gameTime);


            HandlePlayerUpdate(gameTime);
            HandleLevelTime(gameTime);


            foreach (var entity in _entitiesWoPlayer)
                entity.Update(gameTime, _player.Position, RectangleMapObjects, PolygonCollisionObjects, MapObjects, _player);

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
        public override void DrawGUI(GameTime gameTime)
        {
            guiRenderer.Draw(gameTime);
            
        }


        public override void DrawTile(GameTime gameTime)
        {
            List<Vector2> tilesToNotRender = new List<Vector2>();
            List<Vector2> tilesToChange = new List<Vector2>();
            if (_player != null)
            {
                tilesToNotRender = _player.GetUnrenderedTileList();
                tilesToChange = _player.GetChangedTileList();
            }

            if (tileRenderer != null)
                tileRenderer.Draw(gameTime, tilesToNotRender, tilesToChange);

        }
        public virtual void HandleLevelTime(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            levelTime += elapsed;
            LevelTimeLabel.Text = "Time: " + LevelTimeInt;

        }
        public virtual void HandlePlayerUpdate(GameTime gameTime)
        {
            _player.Update(gameTime, KeyboardState, MouseState,/*PreviousMouseState,*/RectangleMapObjects, PolygonCollisionObjects, MapObjects); //update player every frame //note to myseld: fix polygon collision objects
            DashCooldownLabel.Text = "Dash: " + _player.DashCooldownString;
            SlideCooldownLabel.Text = "Slide: " + _player.SlideCooldownString;
            CoinDisplayLabel.Text = " " + _player.CoinsCollected;
            _player.timeInLevel = LevelTimeInt;




            float textureMulti = ((float)_player.Health / (float)_player.MaxHealth);
            int emptyCells = 6;
            int initialCells = 16;


            int newPixels = (int)Math.Round(textureMulti * initialCells);
            int amountToShift = initialCells - newPixels;

            Debug.WriteLine("AMOUNT TO SHIFT: " + amountToShift);
            Debug.WriteLine("NEW PIXELS: " + newPixels);

            PlayerHealthLabel.sourceRect = new Rectangle(amountToShift, 0, 32 - amountToShift, 16);

        }

        public virtual void MainMenuButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new MainMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }

        public virtual void PauseMenuButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.AddScreen(new PauseMenuState(_graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager));
        }

        public virtual void HandleUIInitialization(ContentManager contentManager)
        {
            //main menu
            mainMenuTexture = contentManager.Load<Texture2D>("Test/GreyButton");
            mainMenuFont = contentManager.Load<SpriteFont>("Test/generalFont");
            //

            pauseMenuTexture = contentManager.Load<Texture2D>("Test/PauseButton2");
            displayTexture = contentManager.Load<Texture2D>("Test/DisplayLabel2");

            //MainMenu Button
            coinDisplayTexture = contentManager.Load<Texture2D>("Test/CoinDisplay");

            healthBarTexture = contentManager.Load<Texture2D>("Test/HealthBar");
            healthBackTexture = contentManager.Load<Texture2D>("Test/HealthBack");

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

            PlayerHealthLabel = new Label(healthBarTexture, mainMenuFont)
            {
                Position = new Vector2(362, 10),
                Text = " ",
                Layer = 0.1f,
                horizontalStretch = 7,
                verticalStretch = 1,
            };

            PlayerHealthBackLabel = new Label(healthBackTexture, mainMenuFont)
            {
                Position = new Vector2(350, -5),
                Text = " ",
                Layer = 0.2f,
                horizontalStretch = 4,
                verticalStretch = 3,
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
                PlayerHealthBackLabel,
                PlayerHealthLabel,

            };
        }

        public virtual void HandleEnemyInitialization(ContentManager contentManager)
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
            waypoints2.Add(new Vector2(1800, 750));

            List<Vector2> waypoints3 = new List<Vector2>();
            waypoints3.Add(new Vector2(1050, 750));
            waypoints3.Add(new Vector2(1250, 750));

            List<JumpTrigger> jumpObjects = new List<JumpTrigger>();

            PatrollingEnemy enemy1 = new PatrollingEnemy(enemyContainer1, 0.1f, waypoints1, jumpObjects);
            PatrollingEnemy enemy2 = new PatrollingEnemy(enemyContainer1, 0.1f, waypoints2, jumpObjects);
            PatrollingEnemy enemy3 = new PatrollingEnemy(enemyContainer1, 0.1f, waypoints3, jumpObjects);

            _entities.Add(enemy1);
            _entities.Add(enemy2);
            _entities.Add(enemy3);
            _entitiesWoPlayer.Add(enemy1);
            _entitiesWoPlayer.Add(enemy2);
            _entitiesWoPlayer.Add(enemy3);
        }
        private void HandleLevelTimeInitialization()
        {
            levelTime = 0f;
        }


        public virtual void HandlePlayerInitialization(ContentManager contentManager)
        {

            Texture2D idleTexture = contentManager.Load<Texture2D>("Textures/CharacterIdleAnimation");
            Texture2D walkTexture = contentManager.Load<Texture2D>("Textures/CharacterWalkingAnim");
            Texture2D dashTexture = contentManager.Load<Texture2D>("Textures/CharacterWalkingAnim");
            Texture2D slideTexture = contentManager.Load<Texture2D>("Textures/CharacterWalkingAnim");

            PlayerTextureContainer playerContainer = new PlayerTextureContainer()
            {
                Idle = idleTexture,
                Walk = walkTexture,
                Dash = dashTexture,
                Slide = slideTexture
            };


            _player = new Player(playerContainer, _graphicsDevice, PlayerStartCoord);
            _entities.Add(_player);
        }
        public virtual void HandleBackgroundInitialization(ContentManager contentManager)
        {
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("Textures" + backgroundTexturePathName);
            _background = new Background(backgroundTexture);
        }
        public virtual void HandleTileMap(ContentManager contentManager)
        {


            map = new TiledMap(contentManager.RootDirectory + "\\Test" + tilemapPathName);
            tilesets = map.GetTiledTilesets("Content/Test/");
            tilesetTexture = contentManager.Load<Texture2D>("Test/SpeezleTileSetPng");
            collisionLayer = map.Layers.First(l => l.name == "Collidable");
            teleportLayer = map.Layers.First(l => l.name == "Teleport");
            leverLayer = map.Layers.First(l => l.name == "Lever");
            doorLayer = map.Layers.First(l => l.name == "Door");
            coinLayer = map.Layers.First(l => l.name == "Coin");
            endLayer = map.Layers.First(l => l.name == "End");
            nodeLayer = map.Layers.First(l => l.name == "Node");
            spawnLayer = map.Layers.First(l => l.name == "Spawn");

            _tileMapHandler = new TileMapHandler(_graphicsDevice, map, tilesets, tilesetTexture);

            RectangleMapObjects = new List<Rectangle>();
            MapObjects = new List<MapObject>();
            NodeList = new List<Node>();



            PolygonCollisionObjects = new List<TiledPolygon>();
            foreach (var obj in collisionLayer.objects) //get all the collidable objects on the map
            {
                RectangleMapObjects.Add(new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height));
            }

            foreach(var obj in spawnLayer.objects) //get the player start position coords from the object
            {
                PlayerStartCoord = new Vector2(obj.x, obj.y);
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

            foreach (var obj in coinLayer.objects)
            {
                MapObjects.Add(new CoinObject(obj.id, new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height)));
            }

            foreach (var obj in endLayer.objects)
            {
                MapObjects.Add(new EndObject(obj.id, new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height), obj.name));
            }
            foreach (var obj in nodeLayer.objects)
            {
                //NodeList.Add
            }


        }

    }
}
