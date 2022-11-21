using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Core
{
    public abstract class BaseRenderer
    {
        private SpeezleGame game;
        public SpeezleGame Game
        {
            get { return game; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return this.Game.GraphicsDevice; }
        }
        public GraphicsDeviceManager Graphics
        {
            get { return this.Game.Graphics; }
            //set { this.Game.GraphicsDevice = value; }
        }
        public BaseRenderer(SpeezleGame game)
        {
            this.game = game;
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
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
                sampler = SamplerState.LinearClamp;
            SpriteBatch.Begin(samplerState: sampler, transformMatrix: transformMatrix);
        }
        public virtual void End()
        {
            SpriteBatch.End();
        }
        public abstract void Initialize();
        public abstract void Draw(GameTime gameTime);



    }
}
