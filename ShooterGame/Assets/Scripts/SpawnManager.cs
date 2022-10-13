using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] GameObject player;

    private Vector3 arenaCenter = Vector3.zero;

    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int enemyCount;

    public float spawnRange = 40f;
    public float arenaRadius = 49f;

    private void Start()
    {
        SpawnEnemyWave(waveNumber);
    }

    private void Update()
    {
        enemyCount = FindObjectsOfType<EnemyBehavior>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
        }
    }

    private void SpawnEnemyWave(int number)
    {
        for (int k = 0; k < number; k++)
        {
            InstantiateEnemy();
        }
    }

    private void InstantiateEnemy()
    {
        Instantiate(enemyPrefab, GetSpawnPoint(), Quaternion.identity);
    }

    private Vector3 GetSpawnPoint()
    {
        return arenaCenter + new Vector3(GetReasonableDistance(player.transform.position.x + spawnRange), 0f, GetReasonableDistance(player.transform.position.z + spawnRange));
    }

    private float GetReasonableDistance(float positionComponent)
    {
        return Random.Range(Random.Range(-arenaRadius, -positionComponent), Random.Range(positionComponent, arenaRadius));
    }
}
