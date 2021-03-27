using Assets;
using Enemy;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace EnemySpawn
{
    public class EnemySpawnController : IController
    {
        private SpawnWavesAsset m_SpawnWaves;
        private Grid m_Grid;

        private int[] m_SpawnCounters;
        private float[] m_SpawnTimers;
        
        public EnemySpawnController(SpawnWavesAsset spawnWaves, Grid grid)
        {
            m_SpawnWaves = spawnWaves;
            m_Grid = grid;
            
            m_SpawnCounters = new int[spawnWaves.SpawnWaves.Length];
            m_SpawnTimers = new float[spawnWaves.SpawnWaves.Length];
        }

        public void OnStart()
        {
            for (int i = 0; i < m_SpawnWaves.SpawnWaves.Length; ++i)
            {
                var wave = m_SpawnWaves.SpawnWaves[i];
                m_SpawnTimers[i] = Time.time + wave.TimeBeforeStartWave - wave.TimeBetweenSpawns; // минус чтобы спавнились сразу
            }
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            for (int i = 0; i < m_SpawnWaves.SpawnWaves.Length; ++i)
            {
                var wave = m_SpawnWaves.SpawnWaves[i];
                if (m_SpawnCounters[i] >= wave.Count)
                {
                    continue; // всех выпустили
                }

                if (m_SpawnTimers[i] + wave.TimeBetweenSpawns < Time.time)
                {
                    SpawnEnemy(wave.EnemyAsset);
                    m_SpawnTimers[i] = Time.time;
                    ++m_SpawnCounters[i];
                }
            }
        }

        private void SpawnEnemy(EnemyAsset asset)
        {
            EnemyView view = Object.Instantiate(asset.ViewPrefab);
            
            Vector3 newPos = m_Grid.GetStartNode().Position;
            var transform = view.transform;
            Vector3 oldPos = transform.position;
            transform.position = new Vector3(newPos.x, oldPos.y, newPos.z);
            
            EnemyData data = new EnemyData(asset);

            data.AttachView(view);
            view.CreateMovementAgent(m_Grid);

            Game.Player.EnemySpawned(data);
        }
    }
}