﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.Renderers;
using SpeezleGame.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeezleGame.Entities;

namespace SpeezleGame.States
{
    public class LevelThreeState : GameLevelState
    {
        public LevelThreeState(GraphicsDevice graphicsDevice, GUIRenderer guiRenderer, EntityRenderer entityRenderer, TileRenderer tileRenderer, BackgroundRenderer backgroundRenderer, Core.SpeezleGame game, SaveLoadManager saveLoadManager) : base(graphicsDevice, guiRenderer, entityRenderer, tileRenderer, backgroundRenderer, game, saveLoadManager)
        {
            tilemapPathName = "\\levelthreetilemap.tmx";
            backgroundTexturePathName = "\\LevelBackground5";
            currentLevel = "Three";

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

            Rectangle smartEnemy1Rect = new Rectangle();
            foreach(var entityAreaObj in entityAreas)
            {
                if(entityAreaObj.objectId == 78)
                {
                    smartEnemy1Rect = entityAreaObj.Bounds;
                }
            }

            SmartEnemy smartEnemy1 = new SmartEnemy(enemyContainer1, 0.1f, nodeMap, smartEnemy1Rect, new Vector2(464, 400));

            _entities.Add(smartEnemy1);
            _entitiesWoPlayer.Add(smartEnemy1);

        }
    }
}
