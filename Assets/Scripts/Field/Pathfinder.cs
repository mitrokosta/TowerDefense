using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Field
{
    public class Pathfinder
    {
        private Grid m_Grid;
        private Vector2Int m_Start;
        private Vector2Int m_Target;
        private const float SQRT_2 = 1.41f;

        public Pathfinder(Grid grid, Vector2Int start, Vector2Int target)
        {
            m_Grid = grid;
            m_Start = start;
            m_Target = target;
        }

        public void UpdateField()
        {
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.ResetWeight();
                node.m_OccupationAvailability = OccupationAvailability.CanOccupy;
            }
            
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);
            
            Node start = m_Grid.GetNode(m_Start);
            Node target = m_Grid.GetNode(m_Target);
            
            target.PathWeight = 0f;

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
            
            start.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
            target.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;

            while (start != target && start != null) // rider ругается, но почему?
            {
                start = start.NextNode;
                start.m_OccupationAvailability = OccupationAvailability.Undefined;
            }
        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {
            float sideStep = 1.0f;
            float diagonalStep = SQRT_2;
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

        public bool CanOccupy(Vector2Int coordinate)
        {
            Node node = m_Grid.GetNode(coordinate);

            // проверка кэша
            if (node.m_OccupationAvailability != OccupationAvailability.Undefined)
            {
                return node.m_OccupationAvailability == OccupationAvailability.CanOccupy;
            }
            
            // --> обход в ширину
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            bool[] visited = new bool[m_Grid.Width * m_Grid.Height];
            for (int i = 0; i < visited.Length; i++)
            {
                visited[i] = false;
            }

            queue.Enqueue(m_Target);
            visited[m_Target.x * m_Grid.Width + m_Target.y] = true;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                foreach (Connection neighbour in GetNeighbours(current))
                {
                    Vector2Int nearCoordinate = neighbour.Coordinate;
                    if (nearCoordinate != coordinate && !visited[nearCoordinate.x * m_Grid.Width + nearCoordinate.y])
                    {
                        queue.Enqueue(nearCoordinate);
                        visited[nearCoordinate.x * m_Grid.Width + nearCoordinate.y] = true;
                    }
                }
            }
            // <--

            if (visited[m_Start.x * m_Grid.Width + m_Start.y])
            {
                node.m_OccupationAvailability = OccupationAvailability.CanOccupy;
                return true;
            }

            node.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
            return false;
        }
    }
}