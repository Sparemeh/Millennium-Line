using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.InputSystem;

public class circuitPuzzle : Toggleable
{
    private SpriteRenderer myRenderer;
    public GameObject board;
    private bool boardOpen;
    private bool interacting = false;
    public GameObject player;
    public PlayerInput input;
    public Sprite lightOn;
    public Sprite lightOff;

    public GameObject[] lights;
    public SpriteRenderer[] lightSprites;
    public int[] correctSeq;
    public int[] inputSeq;


    void Start()
    {
        lightSprites = new SpriteRenderer[9];
        inputSeq = correctSeq;

        Debug.Log("is this thing on??");
        input = player.GetComponent<PlayerInput>();
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.sprite = closed;
        boardOpen = false;

        for(int i = 0; i < lights.Length; i++)
        {
            lightSprites[i] = lights[i].GetComponent<SpriteRenderer>();

            Debug.Log("successful on round" + i);

            if (correctSeq[i] == 1)
            {
                lightSprites[i].sprite = lightOn;
            }
            else
            {
                lightSprites[i].sprite = lightOff;
            }
        }
    }

    public override void toggle()
    {
        interacting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (interacting && Input.GetKeyDown(KeyCode.F) && !boardOpen)
        {
            myRenderer.sprite = open;
            board.SetActive(true);
            input.DeactivateInput();
            boardOpen = true;
        }
        else if(interacting && Input.GetKeyDown(KeyCode.F) && boardOpen)
        {
            myRenderer.sprite = closed;
            board.SetActive(false);
            input.ActivateInput();
            boardOpen = false;
            interacting = false;
        }
        if (allGood())
        {
            Debug.Log("puzzle solved");
        }
    }
    
    public void switchFlick(int switchNo)
    {
        Debug.Log("toggling light number: " + switchNo);
        if (inputSeq[switchNo] == 1)
        {
            inputSeq[switchNo] = 0;
            lightSprites[switchNo].sprite = lightOff;
        }
        else
        {
            inputSeq[switchNo] = 1;
            lightSprites[switchNo].sprite = lightOn;
        }

    }

    bool allGood()
    {
        for(int i = 0; i < inputSeq.Length; i++) {
            if (inputSeq[i] == 0) { return false; }
        }
        return true;
    }

}