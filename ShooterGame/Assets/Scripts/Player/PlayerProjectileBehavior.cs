using UnityEngine;

public class PlayerProjectileBehavior : MonoBehaviour
{
    [SerializeField] EnemyBehavior enemyBehavior;
    [SerializeField] GameObject hitEnemyEffect;
    [SerializeField] GameObject hitEnvironmentEffect;

    private bool hasCollided;
    private float startingTimeAlive;
    private float maxTimeAlive = 5f;

    private void Awake()
    {
        startingTimeAlive = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name != "Player" && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Enemy") && !hasCollided)
        {
            hasCollided = true;
            Instantiate(hitEnvironmentEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !hasCollided)
        {
            hasCollided = true;
            enemyBehavior = other.GetComponent<EnemyBehavior>();
            enemyBehavior.TakeDamage(101);
            Instantiate(hitEnemyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Time.time - startingTimeAlive > maxTimeAlive)
        {
            Destroy(gameObject);
        }
    }
}
