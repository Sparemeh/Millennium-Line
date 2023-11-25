using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerSide : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] Animator spriteAnimator;
    [SerializeField] Transform headRaycastPoint;
    [SerializeField] Transform groundRaycastPoint;
    [SerializeField] LayerMask collisionLayerMask;
    [SerializeField] PlayerCollider headCollider;
    [SerializeField] PlayerCollider heightCollider;
    [SerializeField] PlayerCollider frontCollider;
    [SerializeField] PlayerCollider ladderCollider;
    private bool isJumping = false;
    private bool isGrounded;
    Vector2 movementInput = Vector2.zero;

    private Rigidbody2D rb;

    private Vector2 boxSize = new Vector2(0.2f, 0.2f);
    bool goingToVault = false;
    bool climbing = false;
    bool onLadder = false;

    [SerializeField] float climbCheckDistance = 0.1f;
    [SerializeField] float maxClimbHeight = 2f; // Maximum height for climbable surfaces.
    bool hanging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnJump(InputValue movementValue)
    {
        goingToVault = movementValue.Get<float>() != 0;
        if (goingToVault && !isJumping)
        {
            if (hanging)
            {
                hanging = false;
                rb.isKinematic = false;
                spriteAnimator.SetBool("Hanging", false);
                TryVault(true);
            }
            else
            {
                TryVault(false);
            }

        }
    }

    void Update()
    {
        // Movement
        float moveX = hanging ? 0 : movementInput.x;
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        //if (rb.velocity.y < 0.5f)
        //{
        //    spriteAnimator.SetBool("Falling", true);
        //}
        //else
        //{
        //    spriteAnimator.SetBool("Falling", false);

        //}
        if (movementInput.y > 0.5f)
        {
            if(hanging) StopHanging();
            

            if(ladderCollider.IsColliding())
            {
                rb.isKinematic = true;
                onLadder = true;
            }
        }

        if (!ladderCollider.IsColliding())
        {
            rb.isKinematic = false;
            onLadder = false;
        }

        if (onLadder)
        {
            rb.velocity = new Vector2(rb.velocity.x, climbSpeed * movementInput.y);
        }

        //Ledge grab check when not grounded and falling
        if (!isGrounded && rb.velocity.y < 0 && !hanging && !climbing && !onLadder)
        {
            CheckForLedge();
        }


        if (rb.velocity.x < 0)
        {
            spriteAnimator.transform.eulerAngles = Vector3.up * 180;
            spriteAnimator.SetBool("Moving", true);
        }
        else if (rb.velocity.x > 0)
        {
            spriteAnimator.transform.eulerAngles = Vector3.zero;
            spriteAnimator.SetBool("Moving", true);
        }
        else spriteAnimator.SetBool("Moving", false);

    }

    void TryVault(bool climb)
    {
        if (frontCollider.IsColliding() && !heightCollider.IsColliding())
        {
            Vector2 direction = (spriteAnimator.transform.eulerAngles.y == 0) ? Vector2.right : Vector2.left;
            RaycastHit2D downwardHit = Physics2D.Raycast(new Vector2(transform.position.x + direction.x * climbCheckDistance, groundRaycastPoint.transform.position.y + maxClimbHeight), Vector2.down, 0.1f, collisionLayerMask);
            Debug.DrawRay(new Vector2(transform.position.x + direction.x * climbCheckDistance, headCollider.transform.position.y), Vector2.down, Color.red, 0.1f);
            if (downwardHit.collider != null)
            {
                StartCoroutine(VaultSmoothly(downwardHit.point.y, climb));
            }
        }
    }

    void TryVaultOrClimb()
    {
        // Raycast forward to check for an obstacle
        Vector2 direction = (spriteAnimator.transform.eulerAngles.y == 0) ? Vector2.right : Vector2.left;
        RaycastHit2D forwardHit = Physics2D.Raycast(groundRaycastPoint.position + Vector3.up * 0.05f, direction, climbCheckDistance, collisionLayerMask);
       
        if (forwardHit.collider != null)
        {
            // Raycast upwards to ensure the obstacle isn't too high
            RaycastHit2D upwardHit = Physics2D.Raycast(new Vector2(transform.position.x + direction.x * climbCheckDistance, groundRaycastPoint.transform.position.y + maxClimbHeight), Vector2.up, 0.5f, collisionLayerMask);
            if (upwardHit.collider == null)
            {
                RaycastHit2D downwardHit = Physics2D.Raycast(new Vector2(transform.position.x + direction.x * climbCheckDistance, groundRaycastPoint.transform.position.y + maxClimbHeight), Vector2.down, 10, collisionLayerMask);
                Debug.DrawRay(new Vector2(transform.position.x + direction.x * climbCheckDistance, groundRaycastPoint.transform.position.y + maxClimbHeight), Vector2.down, Color.red, 10f);
                if (downwardHit.collider != null)
                {
                    StartCoroutine(VaultSmoothly(downwardHit.point.y, false));
                }
            }
        }
    }

    IEnumerator VaultSmoothly(float targetYPosition, bool climb)
    {
        climbing = true;
        spriteAnimator.SetTrigger("Climb");

        float newTargetYPosition = targetYPosition;
        if (headCollider.IsColliding() && !climb)
        {
            newTargetYPosition -= 0.25f;
        }

        Debug.Log(newTargetYPosition - transform.position.y);

        float initialY = rb.position.y;
        float timeElapsed = 0;
        float duration = 0.3f; // You can adjust the duration for how fast/slow you want the vaulting to be

        while (timeElapsed < duration && !hanging)
        {
            float newY = Mathf.Lerp(initialY, newTargetYPosition, timeElapsed / duration);
            rb.position = new Vector2(rb.position.x, newY);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if(newTargetYPosition == targetYPosition)
        {
            rb.position = new Vector2(rb.position.x, newTargetYPosition); // Ensure we end at the exact target position
            climbing = false;
        }
        else
        {
            Debug.Log("hang");
            CheckForLedge();
        }
    }

    void CheckForLedge()
    {
        // Raycast for ledges. Depending on your setup, you may need to adjust the ray's direction, length, or origin.
        Vector3 direction = (spriteAnimator.transform.eulerAngles.y == 0) ? Vector3.right : Vector3.left;
        //RaycastHit2D hit = Physics2D.Raycast(headRaycastPoint.position + Vector3.up * 0.05f + direction * climbCheckDistance, Vector3.down, climbCheckDistance, collisionLayerMask);

        //if (hit.collider != null)
        //{
        //    hanging = true;
        //    rb.velocity = Vector2.zero;
        //    rb.isKinematic = true; // Disable physics temporarily while hanging
        //    spriteAnimator.SetBool("Hanging", true);

        //    // Optionally, adjust player's position to align perfectly with the ledge
        //    transform.position = new Vector2(transform.position.x, hit.point.y - (headRaycastPoint.transform.position.y - groundRaycastPoint.transform.position.y));
        //}

        if(frontCollider.IsColliding() && headCollider.IsColliding() && !heightCollider.IsColliding())
        {
            hanging = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true; // Disable physics temporarily while hanging
            spriteAnimator.SetBool("Hanging", true);
        }
    }

    // Use this function to stop hanging, e.g., if the player presses a button
    public void StopHanging()
    {
        if (hanging)
        {
            hanging = false;
            rb.isKinematic = false;
            spriteAnimator.SetBool("Hanging", false);
            // ... return to regular movement state, possibly play a "get up" animation, etc. ...
        }
    }


    // This is a simple ground check using Unity's physics layers
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnInteraction() //When interaction key is pressed
    {
        CheckInteraction();
    }

    private void CheckInteraction() //Uses raycast to check for collision
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero); //stores all hit objects in array

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>()) //check if the hit obj is interactable
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return; //Return so that we only interact with one item at a time.
                }
            }
        }

    }

    public void DisplayInteraction() //Display interaction text
    {
        //InteractText.SetActive(true);
    }

    public void HideInteraction() //Hide interaction text
    {
        //InteractText.SetActive(false);
    }
}
