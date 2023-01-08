using Microsoft.Xna.Framework;
using SpeezleGame.Entities;
using SpeezleGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Renderers
{
    public class BackgroundRenderer : BaseRenderer
    {
        Background background;
        public BackgroundRenderer(Core.SpeezleGame game) : base(game)
        {

        }
        public void SetBackground(Background bg)
        {
            this.background = bg;
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(SpriteBatch);
        }

        public override void Initialize()
        {
            
        }

        public override void Begin(Matrix transformMatrix, bool isTextureFilteringEnabled)
        {
            base.Begin(transformMatrix, isTextureFilteringEnabled);
        }

        public override void End()
        {
            base.End();
        }
    }
}
