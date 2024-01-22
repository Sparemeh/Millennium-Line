using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Collider2D[] heightCheckColliders;
    [SerializeField] LayerMask vaultableLayers;
    [SerializeField] LayerMask climbableLayers;

    Vector2 movementInput;
    bool facingRight = true;
    Rigidbody2D rb;

    bool gravityEnabled = true; //variable to indicate whether gravity on (off when hanging on ledges or climbing ladders)
    bool playerControlled = true; //variable to indicate whether player has control (off when mantling)

    bool jumpPressed = false;

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnJump(InputValue movementValue)
    {
        jumpPressed = movementValue.Get<float>() != 0;
        HeightCheck();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gravityEnabled)
        {
            rb.gravityScale = 1f;
        }
        else
        {
            rb.gravityScale = 0f;
        }

        if (playerControlled)
        {
            float currentMovementX = movementInput.x * movementSpeed;
            facingRight = Mathf.Sign(currentMovementX) == 1;
            rb.velocity = new Vector2(currentMovementX, rb.velocity.y);
        }
        else
        {
            
        }

        //run at the end of update
        
    }

    void HeightCheck()
    {
        // If the obstacle does not exceed the maximum vault height, then start the vaulting animation
        if (!heightCheckColliders[0].GetComponent<PlayerCollider>().IsColliding())
        {
            
            StartCoroutine(VaultAnimation());
        }

        jumpPressed = false;
    }

    IEnumerator VaultAnimation()
    {
        // Is there an object? Is it not too low? If so, then vault
        Vector3 direction = facingRight ? Vector3.right : Vector3.left;
        RaycastHit2D forwardHit = Physics2D.Raycast(heightCheckColliders[0].transform.position + heightCheckColliders[0].bounds.size.x / 2 * direction, Vector3.down, 1, vaultableLayers);
        Debug.DrawRay(heightCheckColliders[0].transform.position + heightCheckColliders[0].bounds.size.x / 2 * direction, Vector3.down, Color.red);

        if(forwardHit.collider != null)
        {
            Debug.Log(forwardHit.collider.name);
            gravityEnabled = false;
            playerControlled = false;

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
        }
        else
        {
            yield return null;
        }
    }
}
