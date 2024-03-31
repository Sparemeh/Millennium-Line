using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private Queue<string> speakers;
    private Queue<Sprite> profileSprites;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image profileImage;
    public GameObject dialogueInstance;
    public float maxDistance = 3f;

    bool inConversation = false;
    bool disableDistanceCheck;
    Transform npcPos;
    Sprite profileSprite;

    string npcName;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        speakers = new Queue<string>();
        profileSprites = new Queue<Sprite>();
    }

    // Starts dialogue, returns whether or not the dialogue just started or ended
    public bool StartDialogue(Speech speech, Sprite Givenimage, GameObject npc, bool disableDistanceCheck)
    {
        this.disableDistanceCheck = disableDistanceCheck;

        if(sentences.Count == 0 && !inConversation) {
            npcName = speech.name;
            profileSprite = Givenimage;

            sentences.Clear();
            speakers.Clear();
            profileSprites.Clear();

            dialogueInstance.SetActive(true);

            foreach (string sentence in speech.sentences)
            {
                sentences.Enqueue(sentence);
            }

            foreach (string speaker in speech.speakers)
            {
                speakers.Enqueue(speaker);
            }

            foreach (Sprite profile in speech.profileSprites)
            {
                profileSprites.Enqueue(profile);
            }

            inConversation = true;
            npcPos = npc.transform;

            DisplayNextSentence();

            return true;
        }

        return DisplayNextSentence();
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

        try
        {
            next = speakers.Dequeue();
            nameText.text = next;

            if (next.Equals(""))
            {
                nameText.text = npcName;
            }
        }
        catch (Exception e) {
            nameText.text = npcName;
        }

        
        try
        {
            Sprite newSprite = profileSprites.Dequeue();
            profileImage.sprite = newSprite;
        }
        catch (Exception e)
        {
            if(profileImage != null && profileSprite != null)
                profileImage.sprite = profileSprite;
        }


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
