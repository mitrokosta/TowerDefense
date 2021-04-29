using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        private Pathfinder m_Pathfinding;
        private GridHolder m_GridHolder;
        
        private Vector2Int m_StartCoordinate;
        private Vector2Int m_TargetCoordinate;
        private Node m_SelectedNode = null;

        public int Width => m_Width;
        public int Height => m_Height;

        public Grid(int width, int height, GridHolder gridHolder, Vector2Int start, Vector2Int target)
        {
            m_Width = width;
            m_Height = height;
            m_GridHolder = gridHolder;
            m_StartCoordinate = start;
            m_TargetCoordinate = target;
            
            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i, j] = new Node(gridHolder.GetGridPosition(new Vector2Int(i, j)));
                }
            }
            
            m_Pathfinding = new Pathfinder(this, start, target);
            
            m_Pathfinding.UpdateField();
        }

        public bool HasNode(Vector2Int coordinate)
        {
            return HasNode(coordinate.x, coordinate.y);
        }
        
        public bool HasNode(int i, int j)
        {
            if (i < 0 || i >= m_Width)
            {
                return false;
            }
            
            if (j < 0 || j >= m_Height)
            {
                return false;
            }

            return true;
        }

        public Node GetNode(Vector2Int coordinate)
        {
            return GetNode(coordinate.x, coordinate.y);
        }

        public Node GetNode(int i, int j)
        {
            Assert.IsTrue(HasNode(i, j));
            return m_Nodes[i, j];
        }
        
        public IEnumerable<Node> EnumerateAllNodes()
        {
            for (int i = 0; i < m_Width; i++)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    yield return GetNode(i, j);
                }
            }
        }

        public void UpdatePathfinding()
        {
            m_Pathfinding.UpdateField();
        }
        
        public void SelectCoordinate(Vector2Int coordinate)
        {
            m_SelectedNode = GetNode(coordinate);
        }

        public void UnselectNode()
        {
            m_SelectedNode = null;
        }

        public bool HasSelectedNode()
        {
            return m_SelectedNode != null;
        }

        public Node GetSelectedNode()
        {
            return m_SelectedNode;
        }
        
        public Node GetStartNode()
        {
            return GetNode(m_StartCoordinate);
        }

        public Node GetTargetNode()
        {
            return GetNode(m_TargetCoordinate);
        }

        public bool CanOccupy(Node node)
        {
            return m_Pathfinding.CanOccupy(m_GridHolder.GetGridCoordinate(node.Position));
        }

        public Node GetNodeAtPoint(Vector3 point)
        {
            return GetNode(m_GridHolder.GetGridCoordinate(point));
        }

        // метод вызывается не часто, поэтому можно просто по всем ходить
        public List<Node> GetNodesInCircle(Vector3 point, float radius)
        {
            List<Node> nodes = new List<Node>();
            
            float sqrRadius = radius * radius;
            
            foreach (Node node in EnumerateAllNodes())
            {
                if ((point - node.Position).sqrMagnitude <= sqrRadius)
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        // вернет true, если что-то поменялось
        public bool ChangeNodeOccupationStatus(Node node, bool isOccupy)
        {
            Vector2Int coordinate = m_GridHolder.GetGridCoordinate(node.Position);
            
            if (isOccupy)
            {
                if (m_Pathfinding.CanOccupy(coordinate))
                {
                    node.IsOccupied = true;
                    return true;
                }
            }
            else
            {
                node.IsOccupied = false;
                return true;
            }

            return false;
        }
    }
}