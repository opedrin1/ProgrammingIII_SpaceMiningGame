using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpawner : MonoBehaviour
{
    [System.Serializable]
    public class OreSpawnEntry
    {
        public GameObject orePrefab;
        
        public float weight = 1f;
    }

    [Header("Spawnable Ores")]
    [SerializeField] private List<OreSpawnEntry> spawnableOres;

    [Header("Respawn Cooldown (seconds)")]
    [SerializeField] private float minCooldown = 60f;
    [SerializeField] private float maxCooldown = 90f;

    private GameObject _currentOreInstance;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnRandomOre();

            // Wait until the current ore has been fully mined
            yield return new WaitUntil(() => _currentOreInstance == null);

            float cooldown = Random.Range(minCooldown, maxCooldown);
            yield return new WaitForSeconds(cooldown);
        }
    }

    private void SpawnRandomOre()
    {
        GameObject prefab = PickRandomOrePrefab();
        if (prefab == null)
        {
            Debug.LogWarning($"{name}: no ore prefabs assigned, nothing to spawn.");
            return;
        }

        _currentOreInstance = Instantiate(prefab, transform.position, Quaternion.identity);
    }

    private GameObject PickRandomOrePrefab()
    {
        float totalWeight = 0f;
        foreach (var entry in spawnableOres)
            totalWeight += entry.weight;

        if (totalWeight <= 0f) return null;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in spawnableOres)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
                return entry.orePrefab;
        }

        return spawnableOres[spawnableOres.Count - 1].orePrefab;
    }
}