using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class npcObject : Interactable
{
    NonPlayerCharacter npc0;
    public Speech speech;
    public GameObject NPCDialogue;
    public Sprite myImage;
    public bool uniqueDialogue;
    public bool interactable = true;
    public UnityEvent onStartInteraction;
    public UnityEvent onInteract;
    public UnityEvent onEndInteraction;
    public bool disableDistanceCheck;

    bool startedInteraction = false;

    public override void Interact()
    {
        Debug.Log("interact");
        if(interactable) TriggerDialogue();

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
            // Start the dialogue. The if statement checks if the start or end of the conversation is reached. 
            if(GameObject.Find("UniqueDialogue").GetComponent<DialogueManager>().StartDialogue(speech, myImage, gameObject, disableDistanceCheck))
            {
                if(startedInteraction)
                {
                    onEndInteraction.Invoke();
                }
                else
                {
                    onStartInteraction.Invoke();
                }

                startedInteraction = !startedInteraction;
            }

            onInteract.Invoke();
        }
        else
        {
            // Start the dialogue. The if statement checks if the start or end of the conversation is reached. 
            if (GameObject.Find("UniqueDialogue").GetComponent<DialogueManager>().StartDialogue(speech, myImage, gameObject, disableDistanceCheck))
            {
                if (startedInteraction)
                {
                    onEndInteraction.Invoke();
                }
                else
                {
                    onStartInteraction.Invoke();
                }

                startedInteraction = !startedInteraction;
            }

            onInteract.Invoke();
        }
    }

    public void ForceEndDialogue()
    {
        GameObject.Find("UniqueDialogue").GetComponent<DialogueManager>().EndDialogue();
        onEndInteraction.Invoke();
        startedInteraction = false;
    }
}

