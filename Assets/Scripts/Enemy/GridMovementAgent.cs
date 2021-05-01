using Field;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class GridMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;
        private EnemyData m_Data;
        private Node m_CurrentNode;
        private Grid m_Grid;

        public GridMovementAgent(float speed, Transform transform, Grid grid, EnemyData data)
        {
            m_Speed = speed;
            m_Transform = transform;
            m_Data = data;
            m_Grid = grid;
            m_CurrentNode = grid.GetNodeAtPoint(transform.position);
            SetTargetNode(grid.GetStartNode());
        }

        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 targetNodePosition = m_TargetNode.Position;
            Vector3 currentPosition = m_Transform.position;
            
            Vector3 target = new Vector3(targetNodePosition.x, currentPosition.y, targetNodePosition.z) ;
            float distance = (target - currentPosition).magnitude;
            
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }
        
            Vector3 dir = (target - m_Transform.position).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);

            Node nextNode = m_Grid.GetNodeAtPoint(m_Transform.position);
            
            if (nextNode != m_CurrentNode)
            {
                nextNode?.EnemyDatas.Add(m_Data);
                m_CurrentNode?.EnemyDatas.Remove(m_Data);
                
                m_CurrentNode = nextNode;
            }
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }

        public void Die()
        {
            m_CurrentNode?.EnemyDatas.Remove(m_Data);
        }
    }
}