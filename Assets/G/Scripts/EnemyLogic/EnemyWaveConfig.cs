using UnityEngine;

namespace G.Scripts.EnemyLogic
{
    [CreateAssetMenu(menuName = "Game/Enemy Wave Config")]
    public class EnemyWaveConfig : ScriptableObject
    {
        [Header("Wave Settings")]
        [Tooltip("Общее количество волн. -1 = бесконечно")]
        public int totalWaves = 15;

        public float timeBetweenWaves = 5.5f;

        [Header("Spawn Position")]
        public float spawnX = 13f;

        [Tooltip("Минимальная и максимальная высота, в пределах которой должна полностью помещаться вся колонна")]
        public float minY = -4f;
        public float maxY = 4f;

        [Header("Column Settings")]
        public float verticalSpacing = 1.65f;
        public int minEnemiesPerColumn = 2;
        public int maxEnemiesPerColumn = 6;

        [Header("Enemies")]
        public Enemy[] enemyPrefabs;                   // обычные враги

        [Header("Boss")]
        public Enemy bossPrefab;                       // отдельный префаб босса
        public float bossHp = 50f;
        public float bossMoveSpeed = 3.8f;

        [Header("Movement")]
        public float normalMoveSpeed = 0.5f;
        
        [Header("Сложность — Exponential Growth")]
        public float baseEnemies = 2f;                 // начальное количество
        public float enemyGrowthRate = 1.12f;          // экспоненциальный рост (1.12 = ~12% за волну)

        [Header("Уровни мышей (Tiers)")]
        public Enemy[] tier1Prefabs;                   // Мыши уровня 1
        public Enemy[] tier2Prefabs;                   // Мыши уровня 2
        public Enemy[] tier3Prefabs;
        public Enemy[] tier4Prefabs;

        [Header("Когда вводить новые тиры")]
        public int tier2UnlockWave = 8;                // с какой волны появляются Tier 2
        public int tier3UnlockWave = 16;
        public int tier4UnlockWave = 25;
    }
}