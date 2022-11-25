using Microsoft.Xna.Framework;
using SpeezleGame.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace SpeezleGame.Renderers
{
    public class BackgroundRenderer : BaseRenderer
    {
        TileMapHandler tileMapHandler;

        public BackgroundRenderer(Core.SpeezleGame game) : base(game) { }

        public override void Draw(GameTime gameTime)
        {
            tileMapHandler.Draw(SpriteBatch);
        }

        public void SetMapHandler(TileMapHandler handler)
        {
            tileMapHandler = handler;
        }

        public override void Initialize()
        {

        }
    }
}
