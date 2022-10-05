using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Animator enemyAnimator;
    public Rigidbody[] ragdollRb;
    public CharacterJoint[] ragdollJoints;
    public GameObject enemyOrientation;

    public float maxHealth = 100f;
    public float health;

    private void Awake()
    {
        health = maxHealth;
        ragdollRb = GetComponentsInChildren<Rigidbody>();
        ragdollJoints = GetComponentsInChildren<CharacterJoint>();

        foreach (var joint in ragdollJoints)
        {
            joint.enableProjection = true;
        }

        DisableRagdoll();
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
        enemyAnimator.enabled = true;
        foreach (var rb in ragdollRb)
        {
            rb.isKinematic = true;
        }
    }

    private void EnableRagdoll()
    {
        enemyAnimator.enabled = false;
        foreach (var rb in ragdollRb)
        {
            rb.isKinematic = false;

            int randomDirection = Random.Range(0, 6);
            var launchDirection = enemyOrientation.transform.up;
            int randomForce = Random.Range(1, 100);

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

        Invoke("DisableRagdoll", 1.5f);
    }
}