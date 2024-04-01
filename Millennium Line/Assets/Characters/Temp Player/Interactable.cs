using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public abstract class Interactable : MonoBehaviour
{
    
    private void Reset() 
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public abstract bool Interact(bool returnSuccess);
    public abstract void Interact();

    
}
