using System;
using System.Collections;
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

                foreach (Connection neighbour in GetNeighbours(current))
                {
                    float weightToTarget = currentNode.PathWeight + neighbour.Weight;
                    Node neighbourNode = m_Grid.GetNode(neighbour.Coordinate);
                    if (weightToTarget < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = weightToTarget;
                        queue.Enqueue(neighbour.Coordinate);
                    }
                }
            }
        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {
            float sideStep = 1.0f;
            float diagonalStep = (float)Math.Sqrt(2.0f);
            var possibleNeighbours = new List<Connection>()
            {
                new Connection(coordinate + Vector2Int.right, sideStep),
                new Connection(coordinate + Vector2Int.left, sideStep),
                new Connection(coordinate + Vector2Int.up, sideStep),
                new Connection(coordinate + Vector2Int.down, sideStep),
                
                new Connection(coordinate + Vector2Int.right + Vector2Int.up, diagonalStep),
                new Connection(coordinate + Vector2Int.right + Vector2Int.down, diagonalStep),
                
                new Connection(coordinate + Vector2Int.left + Vector2Int.up, diagonalStep),
                new Connection(coordinate + Vector2Int.left + Vector2Int.down, diagonalStep),
            };
            

            foreach (var neighbour in possibleNeighbours)
            {
                if (CanPass(coordinate, neighbour.Coordinate))
                {
                    yield return neighbour;
                }
            }
        }

        // проверка на возможность прохода из from в to
        private bool CanPass(Vector2Int from, Vector2Int to)
        {
            if (!m_Grid.HasNode(from))
            {
                return false;
            }

            if (!m_Grid.HasNode(to) || m_Grid.GetNode(to).IsOccupied)
            {
                return false;
            }
            
            Vector2Int diff = to - from;
            Vector2Int neighbour;

            if (diff.x != 0)
            {
                neighbour = from + new Vector2Int(diff.x, 0);
                if (m_Grid.HasNode(neighbour) && m_Grid.GetNode(neighbour).IsOccupied)
                {
                    return false;
                }
            }
            
            if (diff.y != 0)
            {
                neighbour = from + new Vector2Int(0, diff.y);
                if (m_Grid.HasNode(neighbour) && m_Grid.GetNode(neighbour).IsOccupied)
                {
                    return false;
                }
            }

            return true;
        }
    }
}