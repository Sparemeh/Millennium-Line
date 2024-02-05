/*

States:
Normal
Ladder
Hanging on Ledge

Control:

When space is pressed:
- if near an obstacle, try to vault
- if on a ladder, let go and change state from ladder to normal
- if hanging, climb upwards and change state to normal 

When up is pressed
- if aligned with a ladder, climb onto lader and change state from normal to ladder
    - if hanging, climb upwards and change state to normal

When down is pressed
- if on a one-way platform and not super close to the ground, go down and change state from normal to hanging

*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float groundSpeed;
    [SerializeField] float ladderSpeed;
    [SerializeField] float hangSpeed;
    [SerializeField] Collider2D[] heightCheckColliders;

    // The layers in which the player can mount/move on
    [SerializeField] LayerMask environmentLayers;

    // The layers that can be used for climbing (ladders for example)
    [SerializeField] LayerMask climbableLayers;

    Vector2 movementInput;
    bool facingRight = true;
    Rigidbody2D rb;

    bool gravityEnabled = true; //variable to indicate whether gravity on (off when hanging on ledges or climbing ladders)
    bool playerControlled = true; //variable to indicate whether player has control (off when mantling)

    bool jumpPressed = false;

    enum MovementState
    {
        Normal,
        Animation,
        Ladder,
        Hanging,
        SideHanging
    }

    MovementState movementState;

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();

    }

    // The function below runs when the space button is clicked
    void OnJump(InputValue movementValue)
    {
        
        if(movementState == MovementState.Normal)
        {
            if(movementInput.y < 0)
            {
                RaycastHit2D platformDetection = Physics2D.Raycast(transform.position - Vector3.up * 0.1f, Vector3.up, 0.1f, environmentLayers);

                // If the platform exists AND the platform is thin enough, then we can hang (a thin platform will return 0.03). Otherwise, valut as usuall
                if (platformDetection.collider != null && transform.position.y - platformDetection.collider.transform.position.y < 0.1f)
                {
                    StartCoroutine(StartHang());
                }
                else
                {
                    StartCoroutine(Vault());
                }
            }
            else
            {
                // If player is not trying to hang, then vault
                StartCoroutine(Vault());
            }
            
        }

        if(movementState == MovementState.Ladder)
        {
            RaycastHit2D upperPlatformDetection = Physics2D.Raycast(transform.position + Vector3.up * 2.1f, Vector3.down, 0.15f, environmentLayers);
            Debug.DrawRay(transform.position + Vector3.up * 2.1f, Vector3.down, Color.red, 1f);

            Debug.Log(upperPlatformDetection.collider == null); 

            // A platform is up top - climb onto it
            if(upperPlatformDetection.collider != null)
            {
                StartCoroutine(ClimbUpLedge());
            }
            else // Otherwise - let go of ladder
            {
                rb.velocity = Vector2.zero;
                movementState = MovementState.Normal;
            }
            
        }

        if(movementState == MovementState.Hanging)
        {
            if(movementInput.y > 0)
            {
                StartCoroutine(ClimbUpLedge());
            }
            else
            {
                movementState = MovementState.Normal;
            }
        }

    }

    void OnUp(InputValue movementValue)
    {
        // If player is not hanging/climbing, try to climb
        if (movementState == MovementState.Normal)
        {
            StartCoroutine(GoOnLadder());
        }

        if (movementState == MovementState.Hanging)
        {
            // If there is a ladder on the player, then go onto the ladder.
            RaycastHit2D ladderDetection = Physics2D.Raycast(transform.position, Vector3.down, 0.1f, climbableLayers);
            if (ladderDetection.collider != null)
            {
                StartCoroutine(GoOnLadder());
            }

        }
    }

    void OnDown(InputValue movementValue)
    {
        if (movementState == MovementState.Normal)
        {
            StartCoroutine(ClimbDownToLadder());
        }

        if (movementState == MovementState.Hanging)
        {
            // If there is a ladder on the player, then go onto the ladder. Otherwise drop down.
            RaycastHit2D ladderDetection = Physics2D.Raycast(transform.position, Vector3.down, 0.1f, climbableLayers);
            if (ladderDetection.collider != null)
            {
                StartCoroutine(GoOnLadder());
            }
            else
            {
                movementState = MovementState.Normal;
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementState = MovementState.Normal;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Depending on the movement state, a different part of the code will run. 

        if(movementState == MovementState.Normal)
        {
            rb.gravityScale = 1;
            rb.isKinematic = false;
            if (playerControlled)
            {
                float currentMovementX = movementInput.x * groundSpeed;
                facingRight = Mathf.Sign(currentMovementX) == 1;
                rb.velocity = new Vector2(currentMovementX, rb.velocity.y);
            }

        }

        if(movementState == MovementState.Animation)
        {
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }

        if(movementState == MovementState.Ladder)
        {
            rb.gravityScale = 0;
            rb.isKinematic = false;
            if (playerControlled)
            {
                RaycastHit2D upperLadderDetection = Physics2D.Raycast(transform.position + Vector3.up * 2.0f, Vector3.down, 0.1f, climbableLayers);

                // If the player has not reached the end of the ladder:
                if (upperLadderDetection.collider != null)
                {
                    float currentMovementY = movementInput.y * ladderSpeed;
                    rb.velocity = new Vector2(0, currentMovementY);
                }
                else
                {
                    float currentMovementY = Mathf.Min(movementInput.y * ladderSpeed, 0);
                    rb.velocity = new Vector2(0, currentMovementY);
                }
                
            }

            RaycastHit2D ladderDetection = Physics2D.Raycast(transform.position + Vector3.up * 0.05f, Vector3.down, 0.1f, climbableLayers);

            if(ladderDetection.collider == null)
            {
                rb.velocity = Vector2.zero;
                movementState = MovementState.Normal;   
            }
        }

        if(movementState == MovementState.Hanging)
        {
            rb.gravityScale = -1f; 
            rb.isKinematic = false;
            if (playerControlled)
            {
                RaycastHit2D upperPlatformDetection = Physics2D.Raycast(transform.position + Vector3.up * 2f, Vector3.up, 0.1f, environmentLayers);

                // Check if the player is still under a platform
                if (upperPlatformDetection.collider != null)
                {
                    float currentMovementX = movementInput.x * hangSpeed;
                    rb.velocity = new Vector2(currentMovementX, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x,0);
                    movementState = MovementState.Normal;
                }

            }
        }
        
    }

    IEnumerator Vault()
    {
        // If the obstacle exceeds the maximum vault height, then exit
        if (heightCheckColliders[0].GetComponent<PlayerCollider>().IsColliding())
        {
            yield break;
        }

        // Is there an object? Is it not too low? If so, then vault
        Vector3 direction = facingRight ? Vector3.right : Vector3.left;
        RaycastHit2D forwardHit = Physics2D.Raycast(heightCheckColliders[0].transform.position + heightCheckColliders[0].bounds.size.x / 2 * direction, Vector3.down, 1, environmentLayers);
        Debug.DrawRay(heightCheckColliders[0].transform.position + heightCheckColliders[0].bounds.size.x / 2 * direction, Vector3.down, Color.red);

        if(forwardHit.collider != null)
        {
            gravityEnabled = false;
            playerControlled = false;

            movementState = MovementState.Animation;

            float newTargetYPosition = forwardHit.point.y;

            float initialY = rb.position.y;
            float timeElapsed = 0;
            float duration = 0.3f; // You can adjust the duration for how fast/slow you want the vaulting to be

            while (timeElapsed < duration)
            {
                float newY = Mathf.Lerp(initialY, newTargetYPosition, timeElapsed / duration);
                rb.position = new Vector2(rb.position.x, newY);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            gravityEnabled = true;
            playerControlled = true;

            movementState = MovementState.Normal;
        }
        else
        {
            
            yield return null;
        }
    }

    IEnumerator GoOnLadder()
    {

        RaycastHit2D ladderDetection = Physics2D.Raycast(heightCheckColliders[0].transform.position + Vector3.up * 0.1f, Vector3.down, 0.15f, climbableLayers);
        if (ladderDetection.collider != null)
        {

            movementState = MovementState.Animation;

            float initialX = rb.position.x;
            float timeElapsed = 0;
            float duration = 0.2f; // You can adjust the duration for how fast/slow you want the vaulting to be


            while (timeElapsed < duration)
            {
                float newX = Mathf.Lerp(initialX, ladderDetection.collider.transform.position.x, timeElapsed / duration);
                rb.position = new Vector2(newX, rb.position.y);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            movementState = MovementState.Ladder;

        }
        else
        {
            yield return null;
        }
    }

    IEnumerator ClimbDownToLadder()
    {
        RaycastHit2D ladderDetection = Physics2D.Raycast(transform.position, Vector3.down, 0.15f, climbableLayers);
        if (ladderDetection.collider != null)
        {
            movementState = MovementState.Animation;

            float newTargetYPosition = transform.position.y - 2f;

            float initialX = rb.position.x;
            float initialY = rb.position.y;
            float timeElapsed = 0;
            float duration = 0.7f; // You can adjust the duration for how fast/slow you want the vaulting to be

            while (timeElapsed < duration)
            {
                float newX = Mathf.Lerp(initialX, ladderDetection.collider.transform.position.x, timeElapsed / duration);
                float newY = Mathf.Lerp(initialY, newTargetYPosition, timeElapsed / duration);
                rb.position = new Vector2(newX, newY);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            gravityEnabled = true;
            playerControlled = true;

            movementState = MovementState.Ladder;
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator StartHang()
    {
        RaycastHit2D platformDetection = Physics2D.Raycast(transform.position - Vector3.up * 0.1f, Vector3.up, 0.1f, environmentLayers);
        if (platformDetection.collider != null)
        {
            movementState = MovementState.Animation;

            float newTargetYPosition = platformDetection.point.y - 2f;

            float initialY = rb.position.y;
            float timeElapsed = 0;
            float duration = 0.7f; // You can adjust the duration for how fast/slow you want the vaulting to be

            while (timeElapsed < duration)
            {
                float newY = Mathf.Lerp(initialY, newTargetYPosition, timeElapsed / duration);
                rb.position = new Vector2(rb.position.x, newY);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector3.zero;

            gravityEnabled = false;
            playerControlled = true;

            movementState = MovementState.Hanging;
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator ClimbUpLedge()
    {
        RaycastHit2D groundDetection = Physics2D.Raycast(transform.position + Vector3.up * 2.1f, Vector3.down, 0.15f, environmentLayers);
        if (groundDetection.collider != null)
        {
            movementState = MovementState.Animation;

            float newTargetYPosition = groundDetection.point.y;

            float initialY = rb.position.y;
            float timeElapsed = 0;
            float duration = 0.7f; // You can adjust the duration for how fast/slow you want the vaulting to be

            while (timeElapsed < duration)
            {
                float newY = Mathf.Lerp(initialY, newTargetYPosition, timeElapsed / duration);
                rb.position = new Vector2(rb.position.x, newY);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            gravityEnabled = true;
            playerControlled = true;

            movementState = MovementState.Normal;
        }
        else
        {
            yield return null;
        }
    }
}
