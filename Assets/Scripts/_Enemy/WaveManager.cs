using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CrystalCoreHealth crystalCore;
    [SerializeField] private TypeSpawner meleeSpawner;
    [SerializeField] private TypeSpawner rangedSpawner;
    [SerializeField] private TypeSpawner tankSpawner;
    [SerializeField] private GameXP gameXP; // Optional: for XP rewards

    [Header("Wave Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField, Min(0.1f)] private float timeBetweenSpawns = 0.3f;
    [SerializeField, Min(1f)] private float timeBetweenWaves = 5f;

    private bool isWaveActive = false;

    private void Start()
    {
        if (crystalCore == null || meleeSpawner == null || rangedSpawner == null || tankSpawner == null)
        {
            Debug.LogError("WaveManager: Missing references!");
            return;
        }

        // Initialize spawner references
        meleeSpawner.Initialize(crystalCore);
        rangedSpawner.Initialize(crystalCore);
        tankSpawner.Initialize(crystalCore);

        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (crystalCore != null && !crystalCore.IsDead)
        {
            isWaveActive = true;

            // 1. Get enemy counts based on your level table
            int normalCount, rangedCount, tankCount;
            GetWaveComposition(currentLevel, out normalCount, out rangedCount, out tankCount);

            // 2. Build and shuffle the queue of spawn actions
            List<System.Action> spawnQueue = new List<System.Action>();
            for (int i = 0; i < normalCount; i++) spawnQueue.Add(() => meleeSpawner.SpawnSingleEnemy());
            for (int i = 0; i < rangedCount; i++) spawnQueue.Add(() => rangedSpawner.SpawnSingleEnemy());
            for (int i = 0; i < tankCount; i++) spawnQueue.Add(() => tankSpawner.SpawnSingleEnemy());

            // Shuffle queue so they mix together
            ShuffleList(spawnQueue);

            // 3. Execute spawns with a delay
            foreach (var spawnAction in spawnQueue)
            {
                if (crystalCore == null || crystalCore.IsDead) yield break;

                spawnAction.Invoke();
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            // 4. Wait until all enemies in the scene are dead
            yield return new WaitUntil(() =>
            {
                EnemyAI[] remainingEnemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
                int aliveCount = 0;
                foreach (var e in remainingEnemies)
                {
                    if (e != null && !e.IsDead) aliveCount++;
                }
                return aliveCount == 0;
            });

            isWaveActive = false;
            Debug.Log($"--- Wave {currentLevel} Completed ---");

            // Give XP reward for clearing the wave
            if (gameXP != null)
            {
                gameXP.AddXP(currentLevel * 50);
            }

            // 5. Intermission break before next wave
            yield return new WaitForSeconds(timeBetweenWaves);
            currentLevel++;
        }
    }

    private void GetWaveComposition(int level, out int normal, out int ranged, out int tank)
    {
        // Based on your progression table
        switch (level)
        {
            case 1: normal = 10; ranged = 15; tank = 0; break;
            case 2: normal = 20; ranged = 25; tank = 0; break;
            case 3: normal = 30; ranged = 35; tank = 1; break;
            case 4: normal = 40; ranged = 45; tank = 2; break;
            case 5: normal = 50; ranged = 55; tank = 4; break;
            default:
                normal = level * 10;
                ranged = (level * 10) + 5;
                tank = Mathf.Max(0, Mathf.RoundToInt(Mathf.Pow(level - 2, 1.5f)));
                break;
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


}