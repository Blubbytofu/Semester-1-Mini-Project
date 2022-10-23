using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public bool doAttackAnimation { get; private set; }

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            doAttackAnimation = true;
            gameManager.GameOver();
        }
    }
}
