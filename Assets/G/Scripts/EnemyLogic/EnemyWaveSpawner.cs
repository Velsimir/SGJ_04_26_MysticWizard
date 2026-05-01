using System;
using System.Collections.Generic;
using G.Scripts.Services.Spawner;
using G.Scripts.Services.Update;
using UnityEngine;
using Random = UnityEngine.Random;

namespace G.Scripts.EnemyLogic
{
    public class EnemyWaveSpawner : IUpdatable, IDisposable
    {
        private readonly ISpawnerService _spawnerService;
        private readonly IUpdateService _updateService;

        private readonly Transform _enemyRoot;
        private readonly EnemyWaveConfig _config;

        private float _timer;
        private int _currentWave = 0;
        private bool _isSpawning = true;

        public EnemyWaveSpawner(EnemyWaveConfig config, Transform enemyRoot)
        {
            _config = config;
            _enemyRoot = enemyRoot;

            _spawnerService = G.Instance.Services.GetService<ISpawnerService>();
            _updateService = G.Instance.Services.GetService<IUpdateService>();

            _updateService.AddNew(this);
            PrewarmAll();
        }

        private void PrewarmAll()
        {
            PrewarmTier(_config.tier1Prefabs);
            PrewarmTier(_config.tier2Prefabs);
            PrewarmTier(_config.tier3Prefabs);
            PrewarmTier(_config.tier4Prefabs);
            if (_config.bossPrefab) _spawnerService.Prewarm(_config.bossPrefab, 1);
        }

        private void PrewarmTier(Enemy[] prefabs)
        {
            if (prefabs == null) return;
            foreach (var p in prefabs)
                if (p)
                    _spawnerService.Prewarm(p, 12);
        }

        public void FixedUpdate(float deltaTime)
        { }

        public void Update(float deltaTime)
        {
            if (!_isSpawning) return;

            _timer += deltaTime;

            if (_timer >= _config.timeBetweenWaves)
            {
                bool isLastWave = _config.totalWaves > 0 && _currentWave >= _config.totalWaves - 1;

                if (isLastWave && _config.bossPrefab != null)
                {
                    SpawnBoss();
                }
                else
                {
                    SpawnVerticalColumn();
                }

                _currentWave++;
                _timer = 0f;

                if (_config.totalWaves > 0 && _currentWave >= _config.totalWaves)
                    _isSpawning = false;
            }
        }

        public void LateUpdate(float deltaTime)
        { }

        private void SpawnVerticalColumn()
        {
            // Экспоненциальный рост количества врагов
            float calculated = _config.baseEnemies * Mathf.Pow(_config.enemyGrowthRate, _currentWave);
            int enemyCount = Mathf.Clamp(Mathf.RoundToInt(calculated),
                _config.minEnemiesPerColumn,
                _config.maxEnemiesPerColumn);

            // Определяем максимальный доступный tier на текущей волне
            int maxTier = 1;
            if (_currentWave >= _config.tier4UnlockWave) maxTier = 4;
            else if (_currentWave >= _config.tier3UnlockWave) maxTier = 3;
            else if (_currentWave >= _config.tier2UnlockWave) maxTier = 2;

            // Вычисляем безопасный центр колонны
            float totalHeight = (enemyCount - 1) * _config.verticalSpacing;
            float halfHeight = totalHeight * 0.5f;

            float minCenterY = _config.minY + halfHeight;
            float maxCenterY = _config.maxY - halfHeight;
            if (minCenterY > maxCenterY) minCenterY = maxCenterY = (_config.minY + _config.maxY) / 2f;

            float spawnY = Random.Range(minCenterY, maxCenterY);

            // Создаём колонну
            GameObject columnObj = new GameObject($"Column_{_currentWave}");
            columnObj.transform.SetParent(_enemyRoot, false);
            columnObj.transform.position = new Vector3(_config.spawnX, spawnY, 0);

            var column = columnObj.AddComponent<EnemyColumn>();
            var spawnedEnemies = new List<Enemy>();

            float startYOffset = totalHeight * 0.5f;

            for (int i = 0; i < enemyCount; i++)
            {
                float yOffset = startYOffset - i * _config.verticalSpacing;
                Vector3 pos = columnObj.transform.position + new Vector3(0, yOffset, 0);

                Enemy prefab = GetRandomPrefabByTier(maxTier);
                if (prefab == null) prefab = _config.tier1Prefabs[0]; // fallback

                Enemy enemy = _spawnerService.Spawn(prefab, pos, Quaternion.identity, columnObj.transform);
                spawnedEnemies.Add(enemy);
            }

            column.Setup(spawnedEnemies, _config.normalMoveSpeed);
        }

        private Enemy GetRandomPrefabByTier(int maxTier)
        {
            List<Enemy> available = new List<Enemy>();

            if (maxTier >= 1 && _config.tier1Prefabs != null) available.AddRange(_config.tier1Prefabs);
            if (maxTier >= 2 && _config.tier2Prefabs != null) available.AddRange(_config.tier2Prefabs);
            if (maxTier >= 3 && _config.tier3Prefabs != null) available.AddRange(_config.tier3Prefabs);
            if (maxTier >= 4 && _config.tier4Prefabs != null) available.AddRange(_config.tier4Prefabs);

            if (available.Count == 0) return null;

            return available[Random.Range(0, available.Count)];
        }

        public void SpawnBoss()
        {
            if (_config.bossPrefab == null) return;

            float centerY = (_config.minY + _config.maxY) / 2f;

            GameObject columnObj = new GameObject("BossColumn");
            columnObj.transform.SetParent(_enemyRoot, false);
            columnObj.transform.position = new Vector3(_config.spawnX, centerY, 0);

            var column = columnObj.AddComponent<EnemyColumn>();
            Enemy boss = _spawnerService.Spawn(_config.bossPrefab, columnObj.transform.position, Quaternion.identity,
                columnObj.transform);

            boss.Initialize(column, 0, _config.bossHp);
            column.Setup(new List<Enemy> { boss }, _config.bossMoveSpeed);
        }

        public void Dispose()
        {
            _isSpawning = false;
            _updateService.Remove(this);
        }
    }
}