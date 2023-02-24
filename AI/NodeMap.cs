using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.AI
{
    public class NodeMap
    {
        List<Node> Nodes;

        public NodeMap()
        {
            Nodes = new List<Node>();
        }
        
        public Node GetNodeFromId(int id)
        {
            foreach(var node in Nodes)
            {
                if(node.Id == id) { return node; }
                else { return null; }
            }
            return null;
        }
        
    }
}
