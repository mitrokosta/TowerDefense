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
        
        private Vector2Int m_StartCoordinate;
        private Vector2Int m_TargetCoordinate;
        private Node m_SelectedNode = null;

        public int Width => m_Width;
        public int Height => m_Height;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int start, Vector2Int target)
        {
            m_Width = width;
            m_Height = height;
            m_StartCoordinate = start;
            m_TargetCoordinate = target;
            
            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize);
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

        // вернет true, если что-то поменялось
        public bool ChangeNodeOccupationStatus(Vector2Int coordinate, bool isOccupy)
        {
            Node node = GetNode(coordinate);
            
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