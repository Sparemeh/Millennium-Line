using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangerNPC : MonoBehaviour
{
    [SerializeField] Phone phone;
    [SerializeField] GameObject callPanel;
    [SerializeField] circuitPuzzle puzzle;

    bool calledPlayer = false;
    bool calling = false;
    public bool finishedCall = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && calledPlayer)
        {
            TriggerStrangerDialogue();
            calledPlayer = false;
        }

        if (puzzle.puzzleFinished && !calling)
        {
            CallPlayer();
            calling = true;
        }
    }


    public void CallPlayer()
    {
        phone.OpenPhone();
        callPanel.SetActive(true);
        calledPlayer = true;
        finishedCall = true;
    }

    public void TriggerStrangerDialogue()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetInteractableNPC(GetComponent<npcObject>());
        callPanel.SetActive(false);
        GetComponent<npcObject>().Interact();
    }
}
