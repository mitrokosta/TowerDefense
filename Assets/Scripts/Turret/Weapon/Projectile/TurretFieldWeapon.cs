using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile
{
    public class TurretFieldWeapon : ITurretWeapon
    {
        private TurretFieldWeaponAsset m_Asset;
        private TurretView m_View;
        private float m_MaxDistance;
        private float m_Damage;
        private List<Node> m_AvailableNodes;
        
        private float m_LastShotTime;

        public TurretFieldWeapon(TurretFieldWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_MaxDistance = asset.MaxDistance;
            m_Damage = asset.Damage;
            m_AvailableNodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
        }
        
        public void TickShoot()
        {
            foreach (Node node in m_AvailableNodes)
            {
                foreach (EnemyData enemyData in node.EnemyDatas)
                {
                    float sqrDistance = (enemyData.View.transform.position - m_View.ProjectileOrigin.position).sqrMagnitude;
                    
                    if (sqrDistance > m_MaxDistance * m_MaxDistance)
                    {
                        continue;
                    }
                    
                    enemyData.ApplyDamage(m_Damage * Time.deltaTime);
                }
            }
        }
    }
}