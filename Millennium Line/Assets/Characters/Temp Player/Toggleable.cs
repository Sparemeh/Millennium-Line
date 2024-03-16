using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Toggleable : Interactable //Inherits from Interactable class
{
    public Sprite open;
    public Sprite closed;
    public GameObject displayText;

    public GameObject interactCanvas;

    private SpriteRenderer sr;
    private bool isOpen;

    public override void Interact() //Toggles between open and closed sprite
    {
        if(isOpen)
        {
            sr.sprite = closed;
        }
        else
        {
            sr.sprite = open;
        }

        isOpen = !isOpen;

        interactCanvas.SetActive(true);
    }
    
    private void Start() 
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closed;   
    }

    private void OnTriggerEnter2D(Collider2D collision) //Runs DisplayInteraction() if the detected obj is player
    {
        if (collision.CompareTag("Player"))
        {
            displayText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Runs HideInteraction() if the detected obj is player
    {
        if (collision.CompareTag("Player"))
        {
            displayText.SetActive(false);
        }
    }
}
