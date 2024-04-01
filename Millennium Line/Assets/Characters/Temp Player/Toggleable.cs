using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public abstract class Toggleable : Interactable //Inherits from Interactable class
{
    public Sprite open;
    public Sprite closed;
    

    private SpriteRenderer sr;
    private bool isOpen;

    public override bool Interact(bool r) //Toggles between open and closed sprite
    {
        
        toggle();
        Debug.Log("interacting");
        return false;
    }

    public override void Interact()
    {
        toggle();
        Debug.Log("interacting");
    }

    public abstract void toggle();
    //{
    //    if (isOpen)
    //    {
    //        sr.sprite = closed;
    //    }
    //    else
    //    {
    //        sr.sprite = open;
    //    }

    //    isOpen = !isOpen;
    //}
    
    private void Start() 
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closed;   
    }
}
