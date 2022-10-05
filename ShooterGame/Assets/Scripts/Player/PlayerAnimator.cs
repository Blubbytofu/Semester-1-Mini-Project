using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Rigidbody playerRigidbody;

    [SerializeField] private float moveLerpSpeed = 4f;

    private float idleTime;
    private bool startIdling;
    public bool isIdling { get; private set; }
    private float idleThreshold = 2f;

    private void Start()
    {
        
    }

    private void Update()
    {
        MovementAnimations();
        JumpingAnimations();
        ArmAvatarAnimations();
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
