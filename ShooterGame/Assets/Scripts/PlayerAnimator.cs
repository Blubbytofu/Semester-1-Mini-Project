using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform leftHandPos;
    [SerializeField] private Transform rightHandPos;
    public GameObject cast1Projectile;

    public float velY;

    [SerializeField] private float moveLerpSpeed = 4f;

    private float idleTime;
    private bool startIdling;
    public bool isIdling { get; private set; }
    private float idleThreshold = 2f;

    private float castTime;
    private float castCooldown = 1f;
    private bool leftHand;

    private Vector3 castDestination;

    private void Start()
    {
        
    }

    private void Update()
    {
        MovementAnimations();
        JumpingAnimations();
        ArmAvatarAnimations();

        velY = playerRigidbody.velocity.y;
    }

    private void MovementAnimations()
    {
        float currentV = playerAnimator.GetFloat("vMovement");
        float currentH = playerAnimator.GetFloat("hMovement");
        float shiftMultiplier;

        if (playerMovement.isRunning)
        {
            shiftMultiplier = 2f;
        }
        else
        {
            shiftMultiplier = 1f;
        }

        if (playerMovement.forwardInput == 0 && playerMovement.sideInput == 0)
        {
            playerAnimator.SetBool("isMoving", false);
        }
        else
        {
            playerAnimator.SetBool("isMoving", true);
        }

        if (playerMovement.isGrounded)
        {
            if (playerMovement.forwardInput != 0)
            {
                playerAnimator.SetFloat("vMovement", Mathf.Lerp(currentV, playerMovement.forwardInput * shiftMultiplier, moveLerpSpeed * Time.deltaTime));
            }
            else
            {
                playerAnimator.SetFloat("vMovement", Mathf.Lerp(currentV, 0f, moveLerpSpeed * Time.deltaTime));
            }

            if (playerMovement.sideInput != 0)
            {
                playerAnimator.SetFloat("hMovement", Mathf.Lerp(currentH, playerMovement.sideInput * shiftMultiplier, moveLerpSpeed * Time.deltaTime));
            }
            else
            {
                playerAnimator.SetFloat("hMovement", Mathf.Lerp(currentH, 0f, moveLerpSpeed * Time.deltaTime));
            }

        }
    }

    private void JumpingAnimations()
    {
        if (!playerMovement.isGrounded)
        {
            if (playerRigidbody.velocity.y > 0.1f)
            {
                playerAnimator.SetBool("jump", true);
            }

            if (playerRigidbody.velocity.y < -0.2f)
            {
                playerAnimator.SetBool("jump", false);
                playerAnimator.SetBool("fall", true);
            }
        }
        else
        {
            if (playerAnimator.GetBool("fall"))
            {
                playerAnimator.SetBool("jump", false);
                playerAnimator.SetBool("fall", false);
                playerAnimator.SetBool("land", true);
                Invoke("EndLand", 0.05f);
            }
        }
    }

    private void EndLand()
    {
        playerAnimator.SetBool("land", false);
    }

    private void ArmAvatarAnimations()
    {
        playerAnimator.SetLayerWeight(1, 1);

        if (playerMovement.forwardInput == 0 && playerMovement.sideInput == 0)
        {
            if (!startIdling)
            {
                idleTime = Time.time;
                startIdling = true;
            }

            if (Time.time > idleTime + idleThreshold)
            {
                isIdling = true;
                playerAnimator.SetBool("armIdle", true);
            }
        }
        else
        {
            startIdling = false;
            isIdling = false;
            playerAnimator.SetBool("armIdle", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > castTime + castCooldown)
            {
                CastSpell();
                castTime = Time.time;
                startIdling = false;
                isIdling = false;
                playerAnimator.SetBool("armIdle", false);
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
            castDestination = aimRay.GetPoint(500f);
        }
        if (leftHand)
        {
            playerAnimator.SetTrigger("leftCast1");
            InstantiateProjectile(leftHandPos);
        }
        else
        {
            playerAnimator.SetTrigger("rightCast1");
            InstantiateProjectile(rightHandPos);
        }
        leftHand = !leftHand;
    }

    private void InstantiateProjectile(Transform startingPoint)
    {
        var projectileObject = Instantiate(cast1Projectile, startingPoint.position, Quaternion.identity) as GameObject;
    }
}
