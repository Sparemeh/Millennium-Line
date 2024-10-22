using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Interactable
{
    public Animator animator;
    public Animator playerAnimator;
    
    public override void Interact()
    {
        animator.SetTrigger("ActivateElevator");
    }

    public override bool Interact(bool returnSuccess)
    {
        animator.SetTrigger("ActivateElevator");
        return true;
    }
}
