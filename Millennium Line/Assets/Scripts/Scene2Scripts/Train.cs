using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Train : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayableDirector timeline;
    public StrangerNPC stranger;
    public Canvas phone;

    bool inTrigger;

    void Start()
    {
        inTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inTrigger && stranger.finishedCall)
        {
            Debug.Log("Sucess!");
            timeline.Play();
            phone.enabled = false;
        }
        Debug.Log(stranger.finishedCall);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inTrigger = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inTrigger = false;
    }

}
