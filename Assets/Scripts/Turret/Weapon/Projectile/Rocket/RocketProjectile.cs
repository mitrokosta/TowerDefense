using System;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    public class RocketProjectile : MonoBehaviour, IProjectile
    {
        private float m_Speed;
        private float m_Radius;
        private float m_Damage;
        private bool m_DidHit = false;
        private EnemyData m_Target = null;

        public void SetRocketProperties(EnemyData target, float speed, float aoe, float damage)
        {
            m_Target = target;
            m_Speed = speed;
            m_Radius = aoe;
            m_Damage = damage;
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
                    float sqrDistance = (enemyData.View.transform.position - transform.position).sqrMagnitude;
                    
                    if (sqrDistance > m_Radius * m_Radius)
                    {
                        continue;
                    }
                    
                    enemyData.ApplyDamage(m_Damage);
                }
            }
            
            Destroy(gameObject);
        }
    }
}