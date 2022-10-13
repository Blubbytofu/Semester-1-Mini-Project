using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Animator enemyAnimator;
    public Rigidbody[] ragdollRb;
    public Collider[] ragdollCollider;
    public CharacterJoint[] ragdollJoints;
    public GameObject enemyOrientation;
    public GameObject player;
    public Rigidbody enemyRb;
    public Collider enemyCollider;

    public float maxHealth = 100f;
    public float health;

    private void Awake()
    {
        health = maxHealth;
        player = GameObject.Find("Player");
        ragdollRb = GetComponentsInChildren<Rigidbody>();
        ragdollCollider = GetComponentsInChildren<Collider>();
        ragdollJoints = GetComponentsInChildren<CharacterJoint>();
        enemyCollider = GetComponent<Collider>();

        foreach (var joint in ragdollJoints)
        {
            joint.enableProjection = true;
        }

        DisableRagdoll();
    }

    private void Update()
    {
        transform.LookAt(player.transform.position);
        if (transform.position.y > 0)
        {
            transform.Translate(0, -0.1f, 0);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EnableRagdoll();
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
            rb.AddTorque(new Vector3(RandomTorque(), RandomTorque(), RandomTorque()), ForceMode.Impulse);
            Invoke("DestroyThis", 2f);
        }
    }

    private float RandomTorque()
    {
        return Random.Range(-0.1f, 0.1f);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}