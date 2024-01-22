using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    bool currentlyColliding;
    int collisionCount;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public bool IsColliding()
    {
        return currentlyColliding;
    }

    public int CollisionCount()
    {
        return collisionCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentlyColliding = true;
        collisionCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionCount--;
        if(collisionCount == 0) 
            currentlyColliding = false;
    }
}
