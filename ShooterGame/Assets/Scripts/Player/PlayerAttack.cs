using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform leftHandPos;
    [SerializeField] private Transform rightHandPos;
    [SerializeField] private GameObject projectile;

    private Vector3 castDestination;

    private float castTime;
    private float castCooldown = 1f;
    public bool leftHand { get; private set; }
    public bool hasCastAnimation;

    private Vector3 projectileDirection;
    public float projectileSpeed = 1000f;

    private void Update()
    {
        GetAttack();
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

        GameObject projectileObject = Instantiate(projectile, startingPoint.position, Quaternion.identity);

        projectileDirection = castDestination - startingPoint.position;
        projectileObject.transform.forward = projectileDirection.normalized;

        projectileObject.GetComponentInChildren<Rigidbody>().AddForce(projectileObject.transform.forward * projectileSpeed, ForceMode.Force);
    }
    
}
