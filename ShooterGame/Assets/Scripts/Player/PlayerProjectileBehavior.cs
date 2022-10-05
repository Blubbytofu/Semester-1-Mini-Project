using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileBehavior : MonoBehaviour
{
    private bool hasCollided;
    private float startingTimeAlive;
    private float maxTimeAlive = 5f;

    private void Awake()
    {
        startingTimeAlive = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name != "Player" && !collision.gameObject.CompareTag("Bullet") && !hasCollided)
        {
            hasCollided = true;
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
