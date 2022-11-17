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
        private List<Rectangle> RectangleCollisionObjects;
        private List<TiledPolygon> PolygonCollisionObjects;


        private Matrix transformMatrix; //

        public SpeezleGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 640;
            _graphics.ApplyChanges(); //set temporary window size

            var WindowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            var MapSize = new Vector2(960, 320);

            transformMatrix = Matrix.CreateScale(new Vector3(WindowSize / MapSize, 1)); //a matrix that allows me to scale everything drawn on the screen

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
            

            //load textures
            map = new TiledMap(Content.RootDirectory + "\\Test/tilemaptest.tmx");
            tilesets = map.GetTiledTilesets("Content/Test/");
            tilesetTexture = Content.Load<Texture2D>("Test/SpeezleTileSetPng");
            collisionLayer = map.Layers.First(l => l.name == "Collidable");
            _tileMapHandler = new TileMapHandler(_spriteBatch, map, tilesets, tilesetTexture);

            RectangleCollisionObjects = new List<Rectangle>();
            PolygonCollisionObjects = new List<TiledPolygon>();
            foreach (var obj in collisionLayer.objects) //get all the collidable objects on the map
            {
                RectangleCollisionObjects.Add(new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height));
            }



            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            HandleInput(gameTime); //get keyboard state

            _player.Update(gameTime, keyboardState, RectangleCollisionObjects, PolygonCollisionObjects); //update player every frame //note to myseld: fix polygon collision objects
            base.Update(gameTime);

            var initPos = _player.Position; //note to myself: temp collision

            



        }

        private void HandleInput(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            base.Draw(gameTime);

            _tileMapHandler.Draw(transformMatrix);

            
            _player.Draw(_spriteBatch, gameTime, transformMatrix);
            
        }
    }
}