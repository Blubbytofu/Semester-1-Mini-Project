using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] GameObject player;

    private Vector3 centerPoint = new Vector3(0, 0, 0);

    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int enemyCount;

    public int xRange = 99;
    public int zRange = 99;

    private void Start()
    {
        SpawnEnemyWave(1);
    }

    private void Update()
    {
        enemyCount = FindObjectsOfType<EnemyBehavior>().Length;
        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveNumber);
            waveNumber++;
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
        Vector3 spawnPos = new Vector3(0, 0, 0);
        spawnPos = centerPoint + GetSpawnPoint();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    private Vector3 GetSpawnPoint()
    {
        return new Vector3(Random.Range(-xRange, xRange), 0f, Random.Range(-zRange, zRange));
    }
}
