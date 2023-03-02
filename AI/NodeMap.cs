using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.AI
{
    public class NodeMap
    {
        public List<Node> Nodes;
        private int MapSize;
        public NodeMap(List<Node> nodes)
        {
            Nodes = nodes;
            
            MapSize = nodes.Count;
            InitializeNodeNeighbours();
        }
        public int GetMapSize()
        {
            return MapSize;
        }
        private void InitializeNodeNeighbours()
        {
            foreach(var node in Nodes)
            {
                List<Node> tempNodeList = new List<Node>();
                List<int> neigbourIds = node.NeighbourIds;
                foreach(var id in neigbourIds)
                {
                    tempNodeList.Add(GetNodeFromId(id));
                }
                node.SetNeighbours(tempNodeList);
            }
        }
        public Node GetNodeFromId(int id)
        {
            foreach(var node in Nodes)
            {
                if(node.Id == id) {return node; }
                
            }
            
            return null;
        }
    }
}
