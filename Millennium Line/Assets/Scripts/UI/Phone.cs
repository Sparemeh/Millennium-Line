using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public GameObject phone;
    public GameObject questLog;
    public GameObject questButton;
    
    // Start is called before the first frame update
    void Start()
    {
        phone.SetActive(false);
        questLog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            phone.SetActive(!phone.activeSelf);
        }

    }

    public void QuestButtonClick()
    {
        questButton.SetActive(false);
        questLog.SetActive(true);
    }

    public void BackButtonClick()
    {
        if (phone.activeSelf && questLog.activeSelf)
        {
            questLog.SetActive(false);
            questButton.SetActive(true);
        }
        else if (phone.activeSelf)
        {
            phone.SetActive(false);
        }
    }
}
