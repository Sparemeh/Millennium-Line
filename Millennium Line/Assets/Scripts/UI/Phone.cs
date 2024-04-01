using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Phone : MonoBehaviour
{
    public GameObject phone;
    public GameObject[] tabButtons;
    public GameObject[] tabs;
    public int numTabs = 3;
    private int activeTab = 42069;

    private bool moving;
    private bool up;


    public GameObject[] questButtons;
    public GameObject[] questInfos;

    public int numQuests;
    public int questID;
    public GameObject questInfoHolder;

    // Start is called before the first frame update
    void Start()
    {
        phone.SetActive(true);
        questInfoHolder.SetActive(true);

        numQuests = 2;
        questButtons = new GameObject[numQuests];
        questInfos = new GameObject[numQuests+1];

        moving = false;
        up = false;

        

        //initializes the quest info pages
        for (int x = 0; x <= numQuests; x++)
        {
            questInfos[x] = GameObject.Find("/Canvas/PlayerUI/Phone/QuestInfo/Panel/questInfo" + x);
            Debug.Log(questInfos[x]);
        }
        
        //initializes every possible tab
        foreach (GameObject x in tabs)
        {
            x.SetActive(false);
        }

        //initializes every possible tab Button
        foreach (GameObject x in tabButtons)
        {
            x.SetActive(true);
        }

        //initializes contact info page
        questInfoHolder.SetActive(false);
        //phone.SetActive(false);


    }
    private Vector3 targetPosition;
    bool phoneUp = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            phoneUp = !phoneUp;
            if (phoneUp) phone.GetComponent<Animator>().SetTrigger("PhoneUp");
            else phone.GetComponent<Animator>().SetTrigger("PhoneDown");
        }

    }

    public void OpenPhone()
    {
        phone.GetComponent<Animator>().SetTrigger("PhoneUp");
        phoneUp = true;
    }

    public void ClosePhone()
    {
        phone.GetComponent<Animator>().SetTrigger("PhoneDown");
        phoneUp = false;
    }

    private void phoneAnimation()
    {
        
    }

    public void ButtonClick(int button)
    {
        Debug.Log(button);
        tabs[button].SetActive(true);

        activeTab = button;
        foreach (GameObject y in tabButtons)
        {
            y.SetActive(false);
        }
    }

    public void QuestClick(int id)
    {
        Debug.Log("Quest clicked!");
        questInfoHolder.SetActive(true);

        foreach (GameObject x in questInfos)
        {
            if (x != null)
            {
                x.SetActive(false);
            }
        }

        tabs[activeTab].SetActive(false);
        questInfos[id].SetActive(true);
        questID = id;
        activeTab = 11;
    }

    public void homeScreen()
    {
        foreach (GameObject x in tabButtons)
        {
            x.SetActive(true);
        }
        foreach (GameObject x in tabs)
        {
            x.SetActive(false);
        }
        foreach (GameObject x in questInfos)
        {
            x.SetActive(false);
        }
    }
    public void BackButtonClick()
    {
        if (activeTab != 42069)
        {
            if (activeTab != 11)
            {
                tabs[activeTab].SetActive(false);
                foreach (GameObject x in tabButtons)
                {
                    x.SetActive(true);
                }
            }
            else
            {
                questInfos[questID].SetActive(false);
                questInfoHolder.SetActive(false);
                tabs[1].SetActive(true);
                activeTab = 1;
            }
        }
    }
}
