using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onEnter;
    [SerializeField] UnityEvent onExit;
    [SerializeField] bool canTrigger = true;

    bool playerEntered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && !playerEntered && canTrigger)
        {
            playerEntered = true;
            onEnter.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && playerEntered && canTrigger)
        {
            playerEntered = false;
            onExit.Invoke();
        }
    }

    public void EnableTrigger(bool canTrigger)
    {
        this.canTrigger = canTrigger;
    }
}
