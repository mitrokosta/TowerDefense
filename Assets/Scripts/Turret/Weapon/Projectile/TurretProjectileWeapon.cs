using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile
{
    public class TurretProjectileWeapon : ITurretWeapon
    {
        private TurretProjectileWeaponAsset m_Asset;
        private TurretView m_View;
        private float m_TimeBetweenShots;
        private float m_MaxDistance;
        private List<Node> m_AvailableNodes;
        
        [CanBeNull]
        private EnemyData m_ClosestEnemyData;

        private List<IProjectile> m_Projectiles = new List<IProjectile>();

        private float m_LastShotTime;

        public TurretProjectileWeapon(TurretProjectileWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_TimeBetweenShots = 1f / m_Asset.RateOfFire;
            m_MaxDistance = asset.MaxDistance;
            m_AvailableNodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
        }

        public void TickShoot()
        {
            TickWeapon();
            TickTower();
            TickProjectiles();
        }

        private void TickWeapon()
        {
            float elapsedTime = Time.time - m_LastShotTime;
            if (elapsedTime < m_TimeBetweenShots)
            {
                return;
            }

            m_ClosestEnemyData = EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_AvailableNodes);

            if (m_ClosestEnemyData == null)
            {
                return;
            }
            
            TickTower();
            
            Shoot(m_ClosestEnemyData);
            m_LastShotTime = Time.time;
        }

        private void TickProjectiles()
        {
            for (var i = 0; i < m_Projectiles.Count; i++)
            {
                IProjectile projectile = m_Projectiles[i];
                projectile.TickApproaching();
                if (projectile.DidHit())
                {
                    projectile.DestroyProjectile();
                    m_Projectiles[i] = null;
                }
            }

            m_Projectiles.RemoveAll(projectile => projectile == null);
        }

        private void TickTower()
        {
            if (m_ClosestEnemyData != null)
            {
                m_View.TowerLookAt(m_ClosestEnemyData.View.transform.position);
            }
        }

        private void Shoot(EnemyData enemyData)
        {
            m_Projectiles.Add(m_Asset.ProjectileAsset.CreateProjectile(m_View.ProjectileOrigin.position, m_View.ProjectileOrigin.forward, enemyData));
            m_View.AnimateShot();
        }
    }
}