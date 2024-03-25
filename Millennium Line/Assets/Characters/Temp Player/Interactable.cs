using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public abstract class Interactable : MonoBehaviour
{

    [SerializeField] GameObject interactionBox;
    
    private void Reset() 
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public abstract void Interact();

    private void OnTriggerEnter2D(Collider2D collision) //Runs DisplayInteraction() if the detected obj is player
    {
        //if(collision.CompareTag("Player")) 
        //{
        //    interactionBox.SetActive(true);
        //    collision.GetComponent<PlayerControllerSide>().DisplayInteraction(); 
        //}
    }

    private void OnTriggerExit2D(Collider2D collision) //Runs HideInteraction() if the detected obj is player
    {
        //if(collision.CompareTag("Player"))
        //{
        //    interactionBox.SetActive(false);
        //    collision.GetComponent<PlayerControllerSide>().HideInteraction();
        //}
    }
}
