using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private bool activeRound;
    [SerializeField] private float roundBreak = 1f;
    public int waveNumber { get; private set; }
    public int enemyCount { get; private set; }

    [SerializeField] private float spawnRange = 20f;
    [SerializeField] private float arenaRadius = 49f;

    private void Start()
    {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        SpawnEnemyWave();
    }

    private void Update()
    {
        enemyCount = FindObjectsOfType<EnemyAttack>().Length;

        if (enemyCount == 0 && activeRound && !gameManager.universalGameOver)
        {
            activeRound = false;
            Invoke("SpawnEnemyWave", roundBreak);
        }
    }

    private void SpawnEnemyWave()
    {
        waveNumber++;
        activeRound = true;
        for (int k = 0; k < waveNumber; k++)
        {
            Instantiate(enemyPrefab, GetSpawnPoint(), Quaternion.identity);
        }
    }

    private Vector3 GetSpawnPoint()
    {
        Vector2 posInUnitCircle = Random.insideUnitCircle.normalized;
        Vector3 spawnPoint =  player.transform.position + Random.Range(spawnRange, arenaRadius) * new Vector3(posInUnitCircle.x, 0, posInUnitCircle.y);
        spawnPoint.y = 0;

        if (Mathf.Abs(spawnPoint.x) > arenaRadius)
        {
            spawnPoint.x *= -1;
        }

        if (Mathf.Abs(spawnPoint.z) > arenaRadius)
        {
            spawnPoint.z *= -1;
        }

        return spawnPoint;
    }
}
