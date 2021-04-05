using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Bullet
{
    [CreateAssetMenu(menuName = "Assets/Bullet Projectile Asset", fileName = "Bullet Projectile Asset")]
    public class BulletProjectileAsset : ProjectileAssetBase
    {
        [SerializeField]
        private BulletProjectile m_BulletPrefab;

        [SerializeField]
        private float m_Speed;
        
        [SerializeField]
        private float m_Damage;
        
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            BulletProjectile projectile = Instantiate(m_BulletPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            projectile.SetBulletProperties(m_Speed, m_Damage);
            return projectile;
        }
    }
}