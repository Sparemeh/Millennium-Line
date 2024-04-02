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
    public Light statusLight;
    public AudioSource audioSrc;


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
            statusLight.color = Color.red;
            
        }
        else
        {
            sr.sprite = open;
            statusLight.color = Color.green;
            audioSrc.Play();

        }
        gateCollider.enabled = isOpen;
        isOpen = !isOpen;
    }
}
