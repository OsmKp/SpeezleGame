using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.AI
{
    public class PathfindingAI
    {
        public Rectangle EntityArea;
        public PathfindingAI(Rectangle entityArea, NodeMap nodeMap)
        {
            EntityArea = entityArea;
            nodes = nodeMap;
        }

        private NodeMap nodes;

        public Stack<Node> DoCalculations(Vector2 currentPosition, Vector2 playerPosition, Rectangle playerBounds)
        {
            Stack<Node> path = new Stack<Node>();
            if(EntityArea.Intersects(playerBounds) == false) { return path ; }

            Vector2 playerFeetPosition = playerPosition + new Vector2(0, 32);
            Vector2 currentFeetPosition = currentPosition + new Vector2(0, 32);

            Stack<Node> stack = NavigateTo(currentFeetPosition, playerFeetPosition);

            return stack;
        }

        public Stack<Node> NavigateTo(Vector2 currentPos, Vector2 destination)
        {

            Stack<Node> path = new Stack<Node>();

            Node currentNode = FindClosestNode(currentPos);
            
            
            Node endNode = FindClosestNode(destination);

            
            if (currentNode == null || endNode == null || currentNode == endNode)
                return path;

            
            SortedList<float, Node> openList = new SortedList<float, Node>();
            
            List<Node> closedList = new List<Node>();

            
            openList.Add(0, currentNode);
            currentNode.Parent = null;
            currentNode.Distance = 0f;

            
            while (openList.Count > 0)
            {

                currentNode = openList.Values[0];
                openList.RemoveAt(0);
                float dist = currentNode.Distance;
                closedList.Add(currentNode);

                
                if (currentNode == endNode)
                    break;

                
                foreach (Node neighbor in currentNode.Neighbours)
                {

                    
                    if (closedList.Contains(neighbor) || openList.ContainsValue(neighbor))
                        continue;

                   
                    neighbor.Parent = currentNode;
                    neighbor.Distance = dist + Vector2.Distance(neighbor.Position, currentNode.Position);
                        
                    float distanceToTarget = Vector2.Distance(neighbor.Position, endNode.Position);
                    openList.Add(neighbor.Distance + distanceToTarget, neighbor);
                }
            }

            if (currentNode == endNode)
            {
                
                while (currentNode.Parent != null)
                {
                    path.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                
            }
            
            return path;
        }
        private Node FindClosestNode(Vector2 targetPosition)
        {
            Node closest = null;
            float minDist = float.MaxValue;

            for (int i = 0; i < nodes.GetMapSize(); i++)
            {
                float dist = Vector2.Distance(nodes.Nodes[i].Position, targetPosition);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = nodes.Nodes[i];
                }
            }
            return closest;
        }
    }
}
