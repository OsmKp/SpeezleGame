using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpeezleGame.AI
{
    public class Node
    {
        public int Id { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle NodeAsSquare { get; set; }
        public int Cost { get; set; }
        public Node Parent { get; set; }

        public List<Node> Neighbours { get; set; }

        public List<int> NeighbourIds { get; set; }
        public Node(Vector2 pos, List<int> neighbourIds)
        {
            Position = pos;
            NodeAsSquare = new Rectangle((int)Position.X, (int)Position.Y, 1, 1);
            NeighbourIds = neighbourIds;
        }

        public void SetNeighbours(List<Node> neighbours)
        {
            Neighbours = neighbours;
        }
    }
}
