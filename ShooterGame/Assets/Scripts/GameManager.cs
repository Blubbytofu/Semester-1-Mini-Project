using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private GameObject crosshairDot;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TextMeshProUGUI statsText;

    public int enemiesDestroyed;

    public bool universalGameOver { get; private set; }

    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        player = GameObject.Find("Player");
        waveText = GameObject.Find("Wave Text").GetComponent<TextMeshProUGUI>();
        enemiesText = GameObject.Find("Enemies Left Text").GetComponent<TextMeshProUGUI>();
        crosshairDot = GameObject.Find("Crosshair");
        gameOverMenu = GameObject.Find("Game Over Manager");
        statsText = GameObject.Find("Final Stat Text").GetComponent<TextMeshProUGUI>();

        crosshairDot.gameObject.SetActive(true);
        waveText.gameObject.SetActive(true);
        enemiesText.gameObject.SetActive(true);
        gameOverMenu.SetActive(false);
    }

    private void Update()
    {
        waveText.text = "WAVE NUMBER: " + spawnManager.waveNumber;
        enemiesText.text = "ENEMIES ALIVE: " + spawnManager.enemyCount;

        if (player.transform.position.y < -10f)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        crosshairDot.gameObject.SetActive(false);
        waveText.gameObject.SetActive(false);
        enemiesText.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
        universalGameOver = true;

        statsText.text = "YOU LASTED " + spawnManager.waveNumber + " WAVES AND BLASTED " + enemiesDestroyed + " ENEMIES!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}