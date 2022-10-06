using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;
    public int xRange = 20;
    public int zRange = 20;

    void Start()
    {
        InvokeRepeating("InstantiateEnemy", 1f, 5f);
    }

    void InstantiateEnemy()
    {
        Vector3 spawnPos = player.transform.position + new Vector3(Random.Range(-xRange, xRange), 0f, Random.Range(-zRange, zRange));
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
