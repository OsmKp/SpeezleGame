using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledCS;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeezleGame.Entities.Players;
using SpeezleGame.Graphics;
using static System.Net.Mime.MediaTypeNames;
using SpeezleGame.Physics;

namespace SpeezleGame.Core
{
    public class SpeezleGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player _player;
        private TileMapHandler _tileMapHandler;
        private KeyboardState keyboardState;

        private TiledMap map; //
        private Dictionary<int, TiledTileset> tilesets; //
        private Texture2D tilesetTexture; //
        private TiledLayer collisionLayer; //
        private List<Rectangle> collisionObjects;


        private Matrix transformMatrix; //

        public SpeezleGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 640;
            _graphics.ApplyChanges();

            var WindowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            var MapSize = new Vector2(960, 320);

            transformMatrix = Matrix.CreateScale(new Vector3(WindowSize / MapSize, 1));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D idleTexture = Content.Load<Texture2D>("Textures/main-char_idle_unarmed");
            Texture2D walkTexture = Content.Load<Texture2D>("Textures/main-char_walk_unarmed");

            PlayerTextureContainer container = new PlayerTextureContainer
            {
                Idle = idleTexture,
                Walk = walkTexture
            };


            _player = new Player(container);

            map = new TiledMap(Content.RootDirectory + "\\Test/tilemaptest.tmx");
            tilesets = map.GetTiledTilesets("Content/Test/");
            tilesetTexture = Content.Load<Texture2D>("Test/SpeezleTileSetPng");
            collisionLayer = map.Layers.First(l => l.name == "Collidable");
            _tileMapHandler = new TileMapHandler(_spriteBatch, map, tilesets, tilesetTexture);

            collisionObjects = new List<Rectangle>();
            foreach (var obj in collisionLayer.objects)
            {
                collisionObjects.Add(new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height));
            }



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            HandleInput(gameTime);

            _player.Update(gameTime, keyboardState, collisionObjects);
            base.Update(gameTime);

            var initPos = _player.Position; //temp collision

            /*_player.CheckPlatformCollision(collisionObjects, gameTime);*/



        }

        private void HandleInput(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _tileMapHandler.Draw(transformMatrix);

            
            _player.Draw(_spriteBatch, gameTime, transformMatrix);
            
        }
    }
}