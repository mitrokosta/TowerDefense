using System;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    public class RocketProjectile : MonoBehaviour, IProjectile
    {
        private float m_Speed = 10f;
        private float m_Radius = 20f;
        private int m_Damage = 20;
        private bool m_DidHit = false;
        private EnemyData m_Target = null;

        public void SetTarget(EnemyData enemyData)
        {
            m_Target = enemyData;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            m_DidHit = true;
        }
        
        public void TickApproaching()
        {
            transform.LookAt(m_Target.View.transform);
            transform.Translate(transform.forward * (m_Speed * Time.deltaTime), Space.World);
        }

        public bool DidHit()
        {
            return m_DidHit;
        }

        public void DestroyProjectile()
        {
            foreach (Node node in Game.Player.Grid.GetNodesInCircle(transform.position, m_Radius))
            {
                foreach (EnemyData enemyData in node.EnemyDatas)
                {
                    enemyData.ApplyDamage(m_Damage);
                }
            }
            
            Destroy(gameObject);
        }
    }
}