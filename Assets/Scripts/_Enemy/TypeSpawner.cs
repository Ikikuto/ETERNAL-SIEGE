using System.Collections.Generic;
using UnityEngine;

public class TypeSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private EnemyAI enemyPrefab;
    [SerializeField] private Transform[] spawnPoints; // Assign the 4 points (_01 to _04) here

    private CrystalCoreHealth crystalCore;

    public void Initialize(CrystalCoreHealth core)
    {
        crystalCore = core;
    }

    public void SpawnSingleEnemy()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0) return;

        // Pick a random spawn point out of the 4 available for this spawner
        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        EnemyAI enemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);
        enemy.Initialize(crystalCore);
    }
}