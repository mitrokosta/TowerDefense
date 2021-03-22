using Enemy;
using Runtime;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/Enemy Asset", fileName = "Enemy Asset")]
    public class EnemyAsset : ScriptableObject
    {
        public int StartHealth;
        public EnemyView ViewPrefab;
    }
}