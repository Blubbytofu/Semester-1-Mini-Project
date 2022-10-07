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

        if (collision.collider.name != "Player" && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Enemy") && !hasCollided)
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
            if (gameObject.name.Equals("ErekiBall2(Clone)"))
            {
                enemyBehavior.TakeDamage(34);
                Destroy(gameObject);
            }
            else if (gameObject.name == "frameBall(Clone)")
            {
                enemyBehavior.TakeDamage(51);
                Destroy(gameObject);
            }
            else if (gameObject.name == "Singularity(Clone)")
            {
                enemyBehavior.TakeDamage(101);
            }
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
