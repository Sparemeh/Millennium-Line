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

    public abstract void Interact();

    private void OnTriggerEnter2D(Collider2D collision) //Runs DisplayInteraction() if the detected obj is player
    {
        //if(collision.CompareTag("Player")) 
        //{
        //    collision.GetComponent<PlayerController>().DisplayInteraction(); 
        //}
    }

    private void OnTriggerExit2D(Collider2D collision) //Runs HideInteraction() if the detected obj is player
    {
        //if(collision.CompareTag("Player"))
        //{
        //    collision.GetComponent<PlayerController>().HideInteraction();
        //}
    }
}
