using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Pathfinder
    {
        private Grid m_Grid;
        private Vector2Int m_Target;

        public Pathfinder(Grid grid, Vector2Int target)
        {
            m_Grid = grid;
            m_Target = target;
        }

        public void UpdateField()
        {
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.ResetWeight();
            }
            
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);
            m_Grid.GetNode(m_Target).PathWeight = 0f;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current);
                float weightToTarget = currentNode.PathWeight + 1f;

                foreach (Vector2Int neighbour in GetNeighbours(current))
                {
                    Node neighbourNode = m_Grid.GetNode(neighbour);
                    if (weightToTarget < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = weightToTarget;
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }

        private IEnumerable<Vector2Int> GetNeighbours(Vector2Int coordinate)
        {
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;
            
            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).IsOccupied;
            bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).IsOccupied;
            bool hasUpNode = upCoordinate.y < m_Grid.Height && !m_Grid.GetNode(upCoordinate).IsOccupied;
            bool hasDownNode = downCoordinate.y >= 0 && !m_Grid.GetNode(downCoordinate).IsOccupied;
            
            if (hasRightNode)
            {
                yield return rightCoordinate;
            }
            
            if (hasLeftNode)
            {
                yield return leftCoordinate;
            }
            
            if (hasUpNode)
            {
                yield return upCoordinate;
            }
            
            if (hasDownNode)
            {
                yield return downCoordinate;
            }
        }
    }
}