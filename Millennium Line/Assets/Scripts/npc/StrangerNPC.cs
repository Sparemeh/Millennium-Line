using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangerNPC : MonoBehaviour
{
    [SerializeField] Phone phone;
    [SerializeField] GameObject callPanel;

    bool calledPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && calledPlayer)
        {
            TriggerStrangerDialogue();
            calledPlayer = false;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            CallPlayer();
        }
    }


    public void CallPlayer()
    {
        phone.OpenPhone();
        callPanel.SetActive(true);
        calledPlayer = true;
    }

    public void TriggerStrangerDialogue()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetInteractableNPC(GetComponent<npcObject>());
        callPanel.SetActive(false);
        GetComponent<npcObject>().Interact();
    }
}
