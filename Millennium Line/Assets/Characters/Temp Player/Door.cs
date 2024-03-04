using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Door : Toggleable
{
    // Start is called before the first frame update
    private SpriteRenderer sr;
    private bool isOpen;
    public BoxCollider2D gateCollider;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closed;
    }

    public override void toggle()
    {
        if (isOpen)
        {
            sr.sprite = closed;
        }
        else
        {
            sr.sprite = open;
            
        }
        gateCollider.enabled = isOpen;
        isOpen = !isOpen;
    }
}
