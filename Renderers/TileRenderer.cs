using Microsoft.Xna.Framework;
using SpeezleGame.MapComponents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace SpeezleGame.Renderers
{
    public class TileRenderer : BaseRenderer
    {
        TileMapHandler tileMapHandler;
        
        

        public TileRenderer(Core.SpeezleGame game) : base(game) { }

        public override void Draw(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, List<Vector2> tilesToNotRender, List<Vector2> tilesToChange)
        {
            if (tileMapHandler != null)
            {
                
                tileMapHandler.Draw(SpriteBatch, tilesToNotRender, tilesToChange);
            }
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
