using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component = SpeezleGame.UI.Component;


namespace SpeezleGame.Renderers
{
    public class GUIRenderer : BaseRenderer
    {
        List<Component> uiComponents;

        public GUIRenderer(Core.SpeezleGame game) : base(game) { }
        public override void Initialize()
        {

        }

        public void SetComponent(List<Component> components)
        {
            uiComponents = components;
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (Component component in uiComponents)
            {
                component.Draw(SpriteBatch);
            }
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
