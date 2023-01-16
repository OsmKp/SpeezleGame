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

        public override void Draw(GameTime gameTime, List<Vector2> tilesToNotRender)
        {
            if (tileMapHandler != null)
            {
                Debug.WriteLine("Called draw first");
                tileMapHandler.Draw(SpriteBatch, tilesToNotRender);
            }
        }

        public void SetMapHandler(TileMapHandler handler)
        {
            Debug.WriteLine("Called set first");
            tileMapHandler = handler;
            
        }

        public override void Initialize()
        {

        }
    }
}
