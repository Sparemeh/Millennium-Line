using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Phone : MonoBehaviour
{
    public GameObject phone;
    private GameObject[] tabButtons;
    private GameObject[] tabs;
    public GameObject inventory;
    public GameObject inventoryButton;
    public GameObject quests;
    public GameObject questsButton;
    public GameObject contacts;
    public GameObject contactsButton;
    private int activeTab = 42069;

    // Start is called before the first frame update
    void Start()
    {
        tabs = new GameObject[3];
        tabButtons = new GameObject[3];
        tabs[0] = inventory;
        tabs[1] = quests;
        tabs[2] = contacts;

        tabButtons[0] = inventoryButton;
        tabButtons[1] = questsButton;
        tabButtons[2] = contactsButton;

        foreach(GameObject x in tabs)
        {
            x.SetActive(false);
        }

        foreach (GameObject y in tabButtons)
        {
            y.SetActive(true);
            Debug.Log(y.ToString());
        }
        phone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            phone.SetActive(!phone.activeSelf);
        }
    }

    public void ButtonClick(int button)
    {
        tabs[button].SetActive(true);
        activeTab = button;
        foreach (GameObject y in tabButtons)
        {
            y.SetActive(false);
        }

    }

    public void BackButtonClick()
    {
        if (activeTab != 42069)
        {
            tabs[activeTab].SetActive(false);
            foreach (GameObject y in tabButtons)
            {
                y.SetActive(true);
            }
        }
    }
}
