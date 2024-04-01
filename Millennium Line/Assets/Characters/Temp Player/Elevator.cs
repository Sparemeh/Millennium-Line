using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Interactable
{
    public Animator animator;
    
    public override void Interact()
    {
        animator.SetTrigger("ActivateElevator");
    }
}
