using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerSide : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    private bool isJumping = false;
    private bool isGrounded;
    Vector2 movementInput = Vector2.zero;

    private Rigidbody2D rb;

    private Vector2 boxSize = new Vector2(0.2f, 0.2f);
    bool goingToJump = false;

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
        Debug.Log(movementValue.Get<float>());
        goingToJump = movementValue.Get<float>() != 0;
    }

    void Update()
    {
        // Movement
        float moveX = movementInput.x;
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Jump
        if (goingToJump && isGrounded)
        {
            isJumping = true;
            goingToJump = false;
        }

        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = false;
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
                Debug.Log("interacting");
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
