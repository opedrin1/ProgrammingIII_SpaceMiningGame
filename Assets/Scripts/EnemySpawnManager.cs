using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints;
    
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;
    
    [Header("Wave Settings")]
    [SerializeField] private int enemiesPerWave = 4;
    [SerializeField] private float spawnInterval = 20f;
    [SerializeField] private float spawnJitterRadius = 1.5f;
    
    [Header("Difficulty Ramp")]
    [SerializeField] private float moveSpeedRampPerMinute = 0.1f;
    [SerializeField] private float fireRateRampPerMinute = 0.15f;

    private Transform _player;
    private float _elapsedTime;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;

        StartCoroutine(SpawnLoop());
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Count == 0 || _player == null)
            return;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnSingleEnemy();
        }
    }

    private void SpawnSingleEnemy()
    {
        Transform spawnPoint = PickSpawnPoint();
        if (spawnPoint == null) return;

        Vector2 jitter = Random.insideUnitCircle * spawnJitterRadius;
        Vector3 spawnPos = spawnPoint.position + (Vector3)jitter;

        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        if (enemyObj.TryGetComponent(out Enemy enemy))
        {
            float elapsedMinutes = _elapsedTime / 60f;
            float moveMultiplier = 1f + elapsedMinutes * moveSpeedRampPerMinute;
            float fireMultiplier = 1f + elapsedMinutes * fireRateRampPerMinute;
            enemy.ApplyDifficultyMultiplier(moveMultiplier, fireMultiplier);
        }
    }
    
    private Transform PickSpawnPoint()
    {
        float totalWeight = 0f;
        float[] weights = new float[spawnPoints.Count];

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            float dist = Vector2.Distance(_player.position, spawnPoints[i].position);
            weights[i] = dist;
            totalWeight += dist;
        }

        if (totalWeight <= 0f)
            return spawnPoints[Random.Range(0, spawnPoints.Count)];

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            cumulative += weights[i];
            if (roll <= cumulative)
                return spawnPoints[i];
        }

        return spawnPoints[spawnPoints.Count - 1];
    }
}