using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Interactable
{
    
    public override bool Interact(bool r)
    {
        Debug.Log("interacting with player");
        return false;
    }

    public override void Interact()
    {
        Debug.Log("interacting with player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
