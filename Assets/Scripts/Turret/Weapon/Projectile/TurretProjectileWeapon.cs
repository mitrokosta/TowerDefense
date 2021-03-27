﻿using System.Collections.Generic;
using Enemy;
using Field;
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
        private List<Node> availableNodes;

        private float m_LastShotTime;

        public TurretProjectileWeapon(TurretProjectileWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_TimeBetweenShots = 1f / m_Asset.RateOfFire;
            m_MaxDistance = asset.MaxDistance;
            availableNodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
        }

        public void TickShoot()
        {
            float elapsedTime = Time.time - m_LastShotTime;
            if (elapsedTime < m_TimeBetweenShots)
            {
                return;
            }

            EnemyData closestEnemyData = EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, availableNodes);

            if (closestEnemyData == null)
            {
                return;
            }
            
            Shoot(closestEnemyData);
            m_LastShotTime = Time.time;
        }

        private void Shoot(EnemyData enemyData)
        {
            m_Asset.ProjectileAsset.CreateProjectile(m_View.ProjectileOrigin.position, m_View.ProjectileOrigin.forward, enemyData);
        }
    }
}