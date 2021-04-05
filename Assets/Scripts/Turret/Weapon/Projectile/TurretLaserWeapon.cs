using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile
{
    public class TurretLaserWeapon : ITurretWeapon
    {
        private TurretLaserWeaponAsset m_Asset;
        private LineRenderer m_LineRenderer;
        private TurretView m_View;
        private float m_MaxDistance;
        private float m_Damage;
        private List<Node> m_AvailableNodes;
        
        [CanBeNull]
        private EnemyData m_ClosestEnemyData;
        
        private float m_LastShotTime;

        public TurretLaserWeapon(TurretLaserWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_MaxDistance = asset.MaxDistance;
            m_Damage = asset.Damage;
            m_AvailableNodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
            m_LineRenderer = Object.Instantiate(asset.LineRendererPrefab, m_View.transform);
        }
        
        public void TickShoot()
        {
            TickWeapon();
            TickTower();
            TickLaser();
        }

        private void TickLaser()
        {
            if (m_ClosestEnemyData == null)
            {
                m_LineRenderer.gameObject.SetActive(false);
            }
            else
            {
                m_LineRenderer.gameObject.SetActive(true);
                m_LineRenderer.SetPositions(new[] {m_View.ProjectileOrigin.transform.position, m_ClosestEnemyData.View.transform.position});
                m_ClosestEnemyData.ApplyDamage(m_Damage * Time.deltaTime);
            }
        }

        private void TickWeapon()
        {
            m_ClosestEnemyData = EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_AvailableNodes);
        }

        private void TickTower()
        {
            if (m_ClosestEnemyData != null)
            {
                m_View.TowerLookAt(m_ClosestEnemyData.View.transform.position);
            }
        }
    }
}