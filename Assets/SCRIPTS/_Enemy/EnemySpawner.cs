
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyAI enemyPrefab;
    [SerializeField] private CrystalCoreHealth crystalCore;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Settings")]
    [SerializeField, Min(0)] private int startEnemyCount = 3;
    [SerializeField, Min(0.1f)] private float spawnInterval = 3f;
    [SerializeField, Min(1)] private int maxAliveEnemies = 25;

    private readonly List<EnemyAI> aliveEnemies = new();
    private int nextSpawnIndex;

    private void Start()
    {
        if (enemyPrefab == null || crystalCore == null)
        {
            Debug.LogError("EnemySpawner: Missing references!");
            return;
        }

        // Spawn ngay khi bắt đầu game
        int count = Mathf.Min(startEnemyCount, maxAliveEnemies);

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }

        // Tiếp tục spawn theo thời gian
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (crystalCore != null && !crystalCore.IsDead)
        {
            yield return new WaitForSeconds(
                Mathf.Max(0.1f, spawnInterval)
            );

            aliveEnemies.RemoveAll(
                enemy => enemy == null || enemy.IsDead
            );

            if (aliveEnemies.Count >= maxAliveEnemies)
                continue;

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = transform;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform selected =
                spawnPoints[nextSpawnIndex % spawnPoints.Length];

            nextSpawnIndex++;

            if (selected != null)
                spawnPoint = selected;
        }

        EnemyAI enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        enemy.Initialize(crystalCore);
        aliveEnemies.Add(enemy);
    }
}
