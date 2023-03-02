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
using SpeezleGame.UserData;
using SpeezleGame.AI;
using System.Xml.Schema;

namespace SpeezleGame.States
{
    public class LevelOneState : GameLevelState
    {

        public LevelOneState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game, SaveLoadManager saveLoadManager) : 
            base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer,backgroundRenderer ,game,  saveLoadManager)
        {
            tilemapPathName = "\\tilemaptest.tmx";
            backgroundTexturePathName = "\\LevelBackground6";
            currentLevel = "One";
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void DrawBackground(GameTime gameTime)
        {
            base.DrawBackground(gameTime);
        }
        public override void DrawEntity(GameTime gameTime)
        {

            base.DrawEntity(gameTime);
        }

        public override void DrawTile(GameTime gameTime)
        {
            base.DrawTile(gameTime);
        }

        public override void DrawGUI(GameTime gameTime)
        {

            base.DrawGUI(gameTime);

        }
        public override void HandleEnemyInitialization(ContentManager contentManager)
        {
            
            Texture2D idleTexture = contentManager.Load<Texture2D>("Textures/Enemy1IdleAnim");
            Texture2D walkTexture = contentManager.Load<Texture2D>("Textures/Enemy1WalkAnim");

            EnemyTextureContainer enemyContainer1 = new EnemyTextureContainer()
            {
                Idle = idleTexture,
                Walk = walkTexture,
            };

            List<Vector2> waypoints1 = new List<Vector2>();
            waypoints1.Add(new Vector2(865, 735));
            waypoints1.Add(new Vector2(1200, 735));

            List<Vector2> waypoints2 = new List<Vector2>();
            waypoints2.Add(new Vector2(1320, 735));
            waypoints2.Add(new Vector2(1800 , 735));

            PatrollingEnemy enemy1 = new PatrollingEnemy(enemyContainer1, 0.1f, waypoints1);
            PatrollingEnemy enemy2 = new PatrollingEnemy(enemyContainer1, 0.1f, waypoints2);
  
            _entities.Add(enemy1);
            _entities.Add(enemy2);
            
            _entitiesWoPlayer.Add(enemy1);
            _entitiesWoPlayer.Add(enemy2);
            
        }
    }
}
