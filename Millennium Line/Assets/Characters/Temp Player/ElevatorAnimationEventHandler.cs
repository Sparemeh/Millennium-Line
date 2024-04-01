using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorAnimationEventHandler : MonoBehaviour
{
    public Animator animator;

    public void WalkToElevator()
    {
        animator.SetTrigger("walkToElevator");
    }

    public void LoadScene2()
    {
        Debug.Log("Loading Scene2");
        SceneManager.LoadScene("Scene2");
    }
}
