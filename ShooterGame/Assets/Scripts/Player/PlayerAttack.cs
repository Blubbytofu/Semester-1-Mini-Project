using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
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
    [SerializeField] private float projectileSpeed = 1000f;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        leftHandPos = GameObject.Find("Left Hand Cast Position").GetComponent<Transform>();
        rightHandPos = GameObject.Find("Right Hand Cast Position").GetComponent<Transform>();
    }

    private void Update()
    {
        if (!gameManager.universalGameOver)
        {
            GetAttack();
        }
    }

    private void GetAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > castTime + castCooldown)
            {
                hasCastAnimation = true;
                leftHand = !leftHand;
                castTime = Time.time;
            }
        }
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


        projectileDirection = castDestination - startingPoint.position;
        projectileObject.transform.forward = projectileDirection.normalized;

        projectileObject.GetComponentInChildren<Rigidbody>().AddForce(projectileObject.transform.forward * projectileSpeed, ForceMode.Force);
    }
    
}
