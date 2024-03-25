using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    bool isOpen;
    public Sprite openDoorImg;
    public Sprite closedDoorImg;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Toggle the door state
            isOpen = !isOpen;

            // Call the method to open/close the door
            if (isOpen)
            {
                GetComponent<SpriteRenderer>().sprite = closedDoorImg;
                GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = openDoorImg;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
