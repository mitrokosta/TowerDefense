using Field;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class FlyingMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;

        public FlyingMovementAgent(float speed, Transform transform, Grid grid)
        {
            m_Speed = speed;
            m_Transform = transform;
            
            SetTargetNode(grid.GetTargetNode());
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
                m_TargetNode = null;
                return;
            }
        
            Vector3 dir = (target - m_Transform.position).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}