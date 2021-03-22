using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/Spawn Waves Asset", fileName = "Spawn Waves Asset")]
    public class SpawnWavesAsset : ScriptableObject
    {
        public EnemyAsset EnemyAsset;
        public int Count;
        public float TimeBetweenSpawns;
    }
}