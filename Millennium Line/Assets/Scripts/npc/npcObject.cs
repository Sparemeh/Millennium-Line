using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcObject : Interactable
{
    NonPlayerCharacter npc0;
    public Speech speech;
    public GameObject NPCDialogue;
    public Sprite myImage;
    public bool uniqueDialogue;

    public override void Interact()
    {
        Debug.Log("interact");
        TriggerDialogue();
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.NPCDialogue = new GameObject();
        //this.npc0 = new NonPlayerCharacter("bob");
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.collider.gameObject == this.gameObject)
        //        {
        //        }
        //    }
        //}

    }

    void TriggerDialogue() {

        //FindObjectOfType<DialogueManager>().StartDialogue(speech, myImage);
        if (uniqueDialogue || GetComponent<DialogueManager>() == null)
        {
            GameObject.Find("UniqueDialogue").GetComponent<DialogueManager>().StartDialogue(speech, myImage, gameObject);
        }
        else
        {
            GetComponent<DialogueManager>().StartDialogue(speech, myImage, gameObject);
        }
    }
}

