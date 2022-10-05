using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform leftHandPos;
    [SerializeField] private Transform rightHandPos;
    [SerializeField] private GameObject[] differentProjectiles;

    private Vector3 castDestination;

    private float castTime;
    private float castCooldown = 1f;
    public bool leftHand { get; private set; }
    public bool hasCastAnimation;

    private Vector3 projectileDirection;
    public float projectileSpeed = 600f;
    private int attackID = 0;
    private float spread = 0;
    private int shotNumber = 1;
    private float upwardsForce;
    public float xSpread;
    public float ySpread;

    private void Update()
    {
        GetAttack();
        CycleAttack();
    }

    private void CycleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            attackID = 0;
            projectileSpeed = 600f;
            castCooldown = 1f;
            shotNumber = 1;
            spread = 0;
            upwardsForce = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            attackID = 1;
            projectileSpeed = 200f;
            castCooldown = 2f;
            shotNumber = 4;
            spread = 0.5f;
            upwardsForce = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            attackID = 2;
            projectileSpeed = 400f;
            castCooldown = 2f;
            shotNumber = 1;
            spread = 1f;
            upwardsForce = 400f;
        }
    }

    private void GetAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > castTime + castCooldown)
            {
                CastSpell();
                castTime = Time.time;
            }
        }
    }

    private void CastSpell()
    {
        Ray aimRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(aimRay, out hit))
        {
            castDestination = hit.point;
        }
        else
        {
            castDestination = aimRay.GetPoint(100f);
        }

        hasCastAnimation = true;
        leftHand = !leftHand;
    }

    //SpawnProjectile method is called by an animation event and initiates a projectile's velocity and direction
    private void SpawnProjectile()
    {
        Transform startingPoint = transform;
        if (leftHand)
        {
            startingPoint = leftHandPos;
        }
        else
        {
            startingPoint = rightHandPos;
        }

        for (int k = 0; k < shotNumber; k++)
        {
            xSpread = Random.Range(-spread, spread);
            ySpread = Random.Range(-spread, spread);
            projectileDirection = castDestination - startingPoint.position;
            projectileDirection += new Vector3(xSpread, ySpread, 0f);
            GameObject projectileObject = Instantiate(differentProjectiles[attackID], startingPoint.position, Quaternion.identity);
            projectileObject.transform.forward = projectileDirection.normalized;
            projectileObject.GetComponentInChildren<Rigidbody>().AddForce(projectileObject.transform.forward * projectileSpeed, ForceMode.Force);
            projectileObject.GetComponentInChildren<Rigidbody>().AddForce(projectileObject.transform.up * upwardsForce, ForceMode.Acceleration);
        }
    }
}
