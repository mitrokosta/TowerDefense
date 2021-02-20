using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Assertions;
using Vector3 = UnityEngine.Vector3;

namespace Field
{
    public class MovementCursor : MonoBehaviour
    {
        [SerializeField]
        private int m_GridWidth;
        
        [SerializeField]
        private int m_GridHeight;

        [SerializeField]
        private float m_NodeSize;
        
        [SerializeField]
        private MovementAgent m_MovementAgent;
        
        [SerializeField]
        private GameObject m_Cursor;
        
        private Camera m_Camera;

        private Collider m_Collider;

        private Vector3 m_Offset;
        
        private void Awake()
        {
            m_Camera = Camera.main;
            m_Collider = GetComponent<Collider>();
        }

        private void OnValidate()
        {
            // default plane size is 10x10
            var width = m_GridWidth * m_NodeSize;
            var height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);
            m_Offset = transform.position - (new Vector3(width, 0f, height)) * 0.5f;
        }

        private void Update()
        {
            if (m_Camera == null)
            {
                m_Cursor.SetActive(false);
                return;
            }

            var mousePosition = Input.mousePosition;
            
            var ray = m_Camera.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform != transform)
                {
                    m_Cursor.SetActive(false);
                    return;
                }

                var nodeCenter = GetNodeCenter(GetNode(hit.point));
                m_Cursor.transform.position = nodeCenter;
                m_Cursor.SetActive(true);
                
                if (Input.GetMouseButtonDown(1))
                {
                    m_MovementAgent.SetTarget(nodeCenter);
                }
            }
            else
            {
                m_Cursor.SetActive(false);
            }
        }

        private bool IsPositionInCollider(Vector3 position)
        {
            Assert.IsNotNull(m_Collider);
            return m_Collider.bounds.Contains(position);
        }

        // получить индексы ноды по координатам точки
        private Vector2Int GetNode(Vector3 position)
        {
            Assert.IsTrue(IsPositionInCollider(position));
            var normDiff = (position - m_Offset) / m_NodeSize;
            return new Vector2Int((int)normDiff.x, (int)normDiff.z);
        }

        private bool IsNodeInGrid(Vector2Int node)
        {
            return ((node.x >= 0 && node.x < m_GridWidth) && (node.y >= 0 && node.y < m_GridHeight));
        }

        // получить координаты центра ноды
        private Vector3 GetNodeCenter(Vector2Int node)
        {
            Assert.IsTrue(IsNodeInGrid(node));
            return m_Offset + new Vector3((node.x + 0.5f) * m_NodeSize, 0f, (node.y + 0.5f) * m_NodeSize);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            // рисовалка линий параллельных OZ
            for (var i = 0; i <= m_GridWidth; i++)
            {
                var offset = m_Offset.x + i * m_NodeSize;
                Gizmos.DrawLine(new Vector3(offset, 0f, m_Offset.z), new Vector3(offset, 0f, m_Offset.z + m_GridHeight * m_NodeSize));
            }
            
            // рисовалка линий параллельных OX
            for (var i = 0; i <= m_GridHeight; i++)
            {
                var offset = m_Offset.z + i * m_NodeSize;
                Gizmos.DrawLine(new Vector3(m_Offset.x, 0f, offset), new Vector3(m_Offset.x + m_GridWidth * m_NodeSize, 0f, offset));
            }
        }
    }
}