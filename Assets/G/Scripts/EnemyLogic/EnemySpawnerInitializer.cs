using UnityEngine;

namespace G.Scripts.EnemyLogic
{
    public class EnemySpawnerInitializer : MonoBehaviour
    {
        [Header("Основные ссылки")]
        [SerializeField] private EnemyWaveConfig _waveConfig;
        [SerializeField] private Transform _enemyRoot;

        private EnemyWaveSpawner _spawner;

        private void Start()
        {
            if (_waveConfig == null || _enemyRoot == null)
            {
                Debug.LogError("EnemySpawnerInitializer: WaveConfig или EnemyRoot не назначены!");
                return;
            }

            _spawner = new EnemyWaveSpawner(_waveConfig, _enemyRoot);
        }

        private void OnDestroy()
        {
            _spawner?.Dispose();
        }
    }
}