using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    [CreateAssetMenu(menuName = "Assets/Rocket Projectile Asset", fileName = "Rocket Projectile Asset")]
    public class RocketProjectileAsset : ProjectileAssetBase
    {
        [SerializeField]
        private RocketProjectile m_RocketPrefab;
        
        [SerializeField]
        private float m_Speed;
        
        [SerializeField]
        private float m_Radius;
        
        [SerializeField]
        private float m_Damage;
        
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            RocketProjectile projectile = Instantiate(m_RocketPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            projectile.SetRocketProperties(enemyData, m_Speed, m_Radius, m_Damage);
            return projectile;
        }
    }
}