using System.Collections;
using Assets;
using Runtime;
using UnityEngine;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;
        private float m_Health;
        public readonly EnemyAsset Asset;

        public EnemyView View => m_View;

        public EnemyData(EnemyAsset asset)
        {
            m_Health = asset.StartHealth;
            this.Asset = asset;
        }

        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }

        public void ApplyDamage(float damage)
        {
            m_Health -= damage;
            if (m_Health < 0)
            {
                Die();
            }
        }

        private void Die()
        {
            m_View.Die();
            Game.Player.EnemyDied(this);
        }
    }
}