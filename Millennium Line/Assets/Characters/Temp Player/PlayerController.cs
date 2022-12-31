using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffest = 0.05f;
    public ContactFilter2D movementFilter;
    private Vector2 boxSize = new Vector2(1f, 1f);
    GameObject InteractText;

    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InteractText = GameObject.Find("InteractIcon");
        InteractText.SetActive(false);
    }

    private void FixedUpdate(){
        if(movementInput != Vector2.zero){
            bool success = TryMove(movementInput);
            //check if can move horizontal
            if(!success){
                success = TryMove(new Vector2(movementInput.x, 0));
                //check if can move vertical
                if(!success){
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }

            animator.SetBool("isMoving", success);
        } else {
            animator.SetBool("isMoving", false);
        }

        //set direction of sprite to movement direction
        if(movementInput.x < 0){
            spriteRenderer.flipX = true;
        } else if (movementInput.x > 0){
            spriteRenderer.flipX = false;
        }
    }

    private bool TryMove(Vector2 direction){
        if(direction != Vector2.zero){
          int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffest);

            if(count == 0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
    }

    void OnInteraction() //When interaction key is pressed
    {
        CheckInteraction();
    }

    private void CheckInteraction() //Uses raycast to check for collision
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position,boxSize,0,Vector2.zero); //stores all hit objects in array

        if(hits.Length > 0)
        {
            foreach(RaycastHit2D rc in hits)
            {
                if(rc.transform.GetComponent<Interactable>()) //check if the hit obj is interactable
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return; //Return so that we only interact with one item at a time.
                }
            }
        }
        
    }

    public void DisplayInteraction() //Display interaction text
    {
        InteractText.SetActive(true);
    }

    public void HideInteraction() //Hide interaction text
    {
        InteractText.SetActive(false);
    }
}
