using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileBehavior : MonoBehaviour
{
    [SerializeField] EnemyBehavior enemyBehavior;

    private bool hasCollided;
    private float startingTimeAlive;
    private float maxTimeAlive = 5f;

    private void Awake()
    {
        startingTimeAlive = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Physics.IgnoreCollision(gameObject.GetComponent<SphereCollider>(), collision.gameObject.GetComponent<SphereCollider>());
        }

        if (collision.collider.name != "Player" && !collision.gameObject.CompareTag("Bullet") && !hasCollided)
        {
            hasCollided = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyBehavior = other.GetComponent<EnemyBehavior>();
            enemyBehavior.TakeDamage(50);
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
