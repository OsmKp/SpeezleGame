using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Renderers
{
    public abstract class BaseRenderer
    {
        private Core.SpeezleGame game;
        public Core.SpeezleGame Game
        {
            get { return game; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return Game.GraphicsDevice; }
        }
        public GraphicsDeviceManager Graphics
        {
            get { return Game.Graphics; }
           
        }
        public BaseRenderer(Core.SpeezleGame game)
        {
            this.game = game;
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public virtual void Begin(Matrix transformMatrix, bool isTextureFilteringEnabled)
        {
            SamplerState sampler = SamplerState.PointClamp;
            if (isTextureFilteringEnabled)
                sampler = SamplerState.PointClamp;
            SpriteBatch.Begin(samplerState: sampler, transformMatrix: transformMatrix);
        }
        public virtual void End()
        {
            SpriteBatch.End();
        }
        public abstract void Initialize();
        public abstract void Draw(GameTime gameTime);

        public virtual void Draw(GameTime gameTime, List<Vector2> tilesToNotRender, List<Vector2>  tilesToChange)
        {

        }



    }
}
