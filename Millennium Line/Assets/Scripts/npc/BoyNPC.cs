using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyNPC : MonoBehaviour
{
    [SerializeField] GameObject musicBoxItem;
    [SerializeField] GameObject collider;
    [SerializeField] float runSpeed = 2f;

    bool startRun = false;
    bool droppedMusicBox = false;

    private void Update()
    {
        if (startRun)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(runSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    // Drop the music box by spawing the box
    public void DropMusicBox()
    {
        if (!droppedMusicBox)
        {
            Instantiate(musicBoxItem, transform.position, transform.rotation);
            droppedMusicBox = true;
        }
        
    }

    // Set a velocity going to the right, and despawn after 5 seconds
    public void RunAway()
    {
        collider.SetActive(true);
        startRun = true;
        Destroy(gameObject, 5);
    }
}
