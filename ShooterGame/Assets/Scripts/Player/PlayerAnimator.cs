using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private float moveLerpSpeed = 100f;

    private float idleTime;
    private bool startIdling;
    public bool isIdling { get; private set; }
    private float idleThreshold = 2f;

    private bool alreadyDead;

    private void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (!gameManager.universalGameOver)
        {
            playerAnimator.SetLayerWeight(1, 1);

            MovementAnimations();
            JumpingAnimations();
            ArmAvatarAnimations();
        }
        else
        {
            playerAnimator.SetLayerWeight(1, 0);

            if (!alreadyDead)
            {
                playerAnimator.SetTrigger("dead");
                alreadyDead = true;
            }
        }
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
                playerAnimator.SetFloat("vMovement", Mathf.Lerp(currentV, playerMovement.forwardInput * shiftMultiplier, 1 / moveLerpSpeed));
            }
            else
            {
                playerAnimator.SetFloat("vMovement", Mathf.Lerp(currentV, 0f, 1 / moveLerpSpeed));
            }

            if (playerMovement.sideInput != 0)
            {
                playerAnimator.SetFloat("hMovement", Mathf.Lerp(currentH, playerMovement.sideInput * shiftMultiplier, 1 / moveLerpSpeed));
            }
            else
            {
                playerAnimator.SetFloat("hMovement", Mathf.Lerp(currentH, 0f, 1 / moveLerpSpeed));
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

        if (playerAttack.hasCastAnimation)
        {
            startIdling = false;
            isIdling = false;
            playerAnimator.SetBool("armIdle", false);

            if (playerAttack.leftHand)
            {
                playerAnimator.SetTrigger("leftCast");
            }
            else
            {
                playerAnimator.SetTrigger("rightCast");
            }
            playerAttack.hasCastAnimation = false;
        }
    }
}
