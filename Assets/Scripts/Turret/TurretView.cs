using UnityEngine;

namespace Turret
{
    public class TurretView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_ProjectileOrigin;

        [SerializeField]
        private Animator m_Animator;

        [SerializeField]
        private Transform m_Tower;
        
        private TurretData m_Data;
        private static readonly int Shot = Animator.StringToHash("Shot");

        public TurretData Data => m_Data;

        public Transform ProjectileOrigin => m_ProjectileOrigin;

        public void AttachData(TurretData turretData)
        {
            m_Data = turretData;
            transform.position = m_Data.Node.Position;
        }

        public void TowerLookAt(Vector3 point)
        {
            point.y = m_Tower.position.y;
            m_Tower.LookAt(point);
        }

        public void AnimateShot()
        {
            m_Animator.SetTrigger(Shot);
        }
    }
}