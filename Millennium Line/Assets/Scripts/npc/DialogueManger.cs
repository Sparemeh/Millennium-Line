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
    public float maxDistance = 0.5f;

    bool inConversation = false;
    Transform npcPos;

    public void StartDialogue(Speech speech, Sprite Givenimage, GameObject npc)
    {
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
        }

        DisplayNextSentence();
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void DisplayNextSentence() {

        Debug.Log("Sentences Left: " + sentences.Count);

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string next = sentences.Dequeue();
        Debug.Log(next);
        dialogueText.text = next;
    }

    public void EndDialogue() {
        Debug.Log("end of conversation");
        sentences.Clear();
        inConversation = false;
        dialogueInstance.SetActive(false);
        npcPos = null;
    }

    private void Update()
    {
        if(npcPos != null &&
            Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, npcPos.position) > maxDistance)
        {
            EndDialogue();
            
        }
    }

}
