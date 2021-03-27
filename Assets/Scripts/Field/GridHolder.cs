using UnityEngine;

namespace Field
{
    public class GridHolder : MonoBehaviour
    {
        [SerializeField]
        private int m_GridWidth;
        [SerializeField]
        private int m_GridHeight;

        [SerializeField]
        private Vector2Int m_TargetCoordinate;
        [SerializeField]
        private Vector2Int m_StartCoordinate;

        [SerializeField]
        private float m_NodeSize;

        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;

        public Vector2Int StartCoordinate => m_StartCoordinate;

        public Grid Grid => m_Grid;

        public void CreateGrid()
        {
            m_Camera = Camera.main;

            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            
            // Default plane size is 10 by 10
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f,
                height * 0.1f);

            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, this, m_StartCoordinate, m_TargetCoordinate);
        }

        public Vector2Int GetGridCoordinate(Vector3 position)
        {
            Vector3 difference = position - m_Offset;
            return new Vector2Int((int) (difference.x / m_NodeSize), (int) (difference.z / m_NodeSize));
        }

        public Vector3 GetGridPosition(Vector2Int coordinate)
        {
            return m_Offset + new Vector3(coordinate.x + .5f, 0, coordinate.y + .5f) * m_NodeSize;
        }

        private void OnValidate()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            
            // Default plane size is 10 by 10
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f,
                height * 0.1f);

            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
        }

        public void RaycastInGrid()
        {
            if (m_Grid == null || m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform)
                {
                    m_Grid.UnselectNode();
                    return;
                }

                m_Grid.SelectCoordinate(GetGridCoordinate(hit.point));
                /*if (Input.GetMouseButtonDown(0))
                {
                    Node node = m_Grid.GetNode(x, y);
                    if (m_Grid.ChangeNodeOccupationStatus(new Vector2Int(x, y), !node.IsOccupied))
                    {
                        m_Grid.UpdatePathfinding();
                    }
                }*/
            }
        }

        private void OnDrawGizmos()
        {
            if (m_Grid == null)
            {
                return;
            }
            
            Gizmos.color = Color.red;

            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                if (node.NextNode == null)
                {
                    continue;
                }
                if (node.IsOccupied)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(node.Position, new Vector3(m_NodeSize, m_NodeSize, m_NodeSize));
                    continue;
                }
                Gizmos.color = Color.red;
                Vector3 start = node.Position;
                Vector3 end = node.NextNode.Position;

                Vector3 dir = end - start;

                start -= dir * 0.25f;
                end -= dir * 0.75f;

                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(end, 0.1f);
            }
        }
    }
}