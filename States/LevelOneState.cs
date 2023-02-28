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
            waypoints1.Add(new Vector2(1340, 750));
            waypoints1.Add(new Vector2(1500, 750));

            List<Vector2> waypoints2 = new List<Vector2>();
            waypoints2.Add(new Vector2(1600, 750));
            waypoints2.Add(new Vector2(1800 , 750));

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






        private void InitializeNodes()
        {
            foreach(Rectangle collisionObj in RectangleMapObjects)
            {
                Vector2 NodePositionStart = new Vector2(collisionObj.X - 1, collisionObj.Y - 1);
                Vector2 NodePositionEnd = new Vector2(collisionObj.X + collisionObj.Width , collisionObj.Y + collisionObj.Height );
            }
        }
    }
}
