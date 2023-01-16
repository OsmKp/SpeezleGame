using Microsoft.Xna.Framework;
using SpeezleGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Renderers
{
    public class EntityRenderer : BaseRenderer
    {
        List<BaseEntity> entities;

        public EntityRenderer(Core.SpeezleGame game) : base(game) { }
        public override void Begin(Matrix transformMatrix, bool isTextureFilteringEnabled)
        {
            base.Begin(transformMatrix, isTextureFilteringEnabled);
            
        }

        public void SetEntity(List<BaseEntity> entities)
        {
            this.entities = entities;
        }

        public override void Draw(GameTime gameTime)
        {
            if (entities == null) { return; }
            foreach (var entity in entities)
            {
                entity.Draw(SpriteBatch, gameTime);
            }
        }

        public override void End()
        {
            base.End();
        }

        public override void Initialize()
        {
            
        }
    }
}
