using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private Rigidbody[] ragdollRb;
    [SerializeField] private Collider[] ragdollCollider;
    [SerializeField] private CharacterJoint[] ragdollJoints;
    [SerializeField] private GameObject enemyOrientation;
    [SerializeField] private GameObject player;
    [SerializeField] private CapsuleCollider enemyCollider;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private GameObject enemyAttackRange;
    [SerializeField] private GameManager gameManager;

    private bool hasDied;

    private float turnSpeed = 360f;

    private float maxHealth = 100f;
    private float health;

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        ragdollRb = GetComponentsInChildren<Rigidbody>();
        ragdollCollider = GetComponentsInChildren<Collider>();
        ragdollJoints = GetComponentsInChildren<CharacterJoint>();

        player = GameObject.Find("Player");
        enemyCollider = GetComponent<CapsuleCollider>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        foreach (var joint in ragdollJoints)
        {
            joint.enableProjection = true;
        }

        enemyAnimator.SetInteger("movementType", Random.Range(0, 3));
        if (enemyAnimator.GetInteger("movementType") == 2)
        {
            enemyCollider.center = new Vector3(0, 0.4f, 0);
            enemyCollider.height = 0.7f;
        }

        DisableRagdoll();

        enemyAttackRange.SetActive(true);
        health = maxHealth;
    }

    private void Update()
    {
        if (health > 0)
        {
            FacePlayer();
        }

        if (transform.position.y < -10f)
        {
            gameManager.enemiesDestroyed++;
            Destroy(gameObject);
        }

        if (gameManager.universalGameOver && !hasDied)
        {
            hasDied = true;
            if (enemyAttack.doAttackAnimation && (enemyAnimator.GetInteger("movementType") == 0 || enemyAnimator.GetInteger("movementType") == 1))
            {
                enemyAnimator.SetTrigger("hitPlayer");
            }
            else
            {
                TakeDamage(maxHealth);
            }
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        Quaternion rotateToPlayer = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateToPlayer, turnSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EnableRagdoll();
            enemyAttackRange.SetActive(false);
            gameManager.enemiesDestroyed++;
            Destroy(gameObject, 3f);
        }
    }

    private void DisableRagdoll()
    {
        enemyCollider.enabled = true;
        enemyAnimator.enabled = true;
        foreach (var rb in ragdollRb)
        {
            rb.isKinematic = true;
        }
        foreach (var collider in ragdollCollider)
        {
            collider.isTrigger = true;
        }
    }

    private void EnableRagdoll()
    {
        enemyCollider.enabled = false;
        enemyAnimator.enabled = false;

        foreach (var collider in ragdollCollider)
        {
            collider.isTrigger = false;
        }

        foreach (var rb in ragdollRb)
        {
            rb.isKinematic = false;

            int randomDirection = Random.Range(0, 6);
            var launchDirection = enemyOrientation.transform.up;
            int randomForce = Random.Range(1, 150);

            if (randomDirection == 0)
            {
                launchDirection = enemyOrientation.transform.up;
            }
            else if (randomDirection == 1)
            {
                launchDirection = -enemyOrientation.transform.up;
            }
            else if (randomDirection == 2)
            {
                launchDirection = enemyOrientation.transform.forward;
            }
            else if (randomDirection == 3)
            {
                launchDirection = -enemyOrientation.transform.forward;
            }
            else if (randomDirection == 4)
            {
                launchDirection = enemyOrientation.transform.right;
            }
            else if (randomDirection == 5)
            {
                launchDirection = -enemyOrientation.transform.right;
            }

            rb.AddForce(launchDirection * randomForce, ForceMode.Impulse);
        }
    }
}