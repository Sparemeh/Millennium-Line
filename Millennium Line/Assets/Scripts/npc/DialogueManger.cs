using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Sprite profileImage;
    public GameObject dialogueInstance;
    public float maxDistance = 3f;

    bool inConversation = false;
    bool disableDistanceCheck;
    Transform npcPos;

    // Starts dialogue, returns whether or not the dialogue just started or ended
    public bool StartDialogue(Speech speech, Sprite Givenimage, GameObject npc, bool disableDistanceCheck)
    {
        this.disableDistanceCheck = disableDistanceCheck;

        if(sentences.Count == 0 && !inConversation) {
            nameText.text = speech.name;

            sentences.Clear();
            dialogueInstance.SetActive(true);

            foreach (string sentence in speech.sentences)
            {
                sentences.Enqueue(sentence);
            }

            profileImage = Givenimage;

            inConversation = true;
            npcPos = npc.transform;

            DisplayNextSentence();

            return true;
        }

        return DisplayNextSentence();
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    // Displays the next sentence, returns true if the end of the dialogue is reached, false otherwise
    public bool DisplayNextSentence() {

        Debug.Log("Sentences Left: " + sentences.Count);

        if (sentences.Count == 0)
        {
            EndDialogue();
            return true;
        }

        string next = sentences.Dequeue();
        dialogueText.text = next;

        return false;
    }

    public void EndDialogue() {
        sentences.Clear();
        inConversation = false;
        dialogueInstance.SetActive(false);
        npcPos = null;
    }

    private void Update()
    {
        if(npcPos != null && 
            Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, npcPos.position) > maxDistance && !disableDistanceCheck)
        {
            Debug.Log(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, npcPos.position));
            EndDialogue();
            
        }
    }

}
