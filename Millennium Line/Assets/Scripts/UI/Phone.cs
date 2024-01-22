using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Phone : MonoBehaviour
{
    public GameObject phone;
    public GameObject[] tabButtons;
    public GameObject[] tabs;
    public GameObject inventory;
    public GameObject inventoryButton;
    public GameObject quests;
    public GameObject questsButton;
    public GameObject contacts;
    public GameObject contactsButton;
    public int numTabs = 3;
    private int activeTab = 42069;


    public GameObject[] contactButtons;
    public GameObject[] contactInfos;
    public int numContacts;
    public int contactID;
    public GameObject contactInfoHolder;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("egg 1");
        contactInfoHolder.SetActive(true);
        phone.SetActive(true);

        Debug.Log("egg 2");

        tabs = new GameObject[numTabs];
        tabButtons = new GameObject[numTabs];
        tabs[0] = inventory;
        Debug.Log(tabs[0]);
        tabs[1] = quests;
        tabs[2] = contacts;

        tabButtons[0] = inventoryButton;
        tabButtons[1] = questsButton;
        tabButtons[2] = contactsButton;

        contactButtons = new GameObject[numContacts];
        contactInfos = new GameObject[numContacts];
        numContacts = 2;


        /*
         *PRETTY SURE WE DON'T NEED THIS CODE, BUT IT WAS ANNOYING TO TYPE SO IM LEAVING IT HERE JUST IN CASE
         * 
         * 
        //initializes the contact buttons
        for (int x = 0; x <= numContacts; x++)
        {
            contactButtons[x] = GameObject.Find("/Canvas/PlayerUI/Phone/ContactList/Scroll/Panel/Contact" + x + "/Button");
            Debug.Log("Button: " + contactButtons[x]);
        }
        */


        //initializes the contact info pages
        for (int x = 0; x <= numContacts; x++)
        {
            contactInfos[x] = GameObject.Find("/Canvas/PlayerUI/Phone/ContactInfo/contactInfo" + x);
            Debug.Log("Info page: " + contactInfos[x]);
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
        contactInfoHolder.SetActive(false);
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

    public void ContactClick(int id)
    {
        contactInfoHolder.SetActive(true);

        foreach (GameObject x in contactInfos)
        {
            Debug.Log(x);
            if (x != null)
            {
                x.SetActive(false);
            }
        }

        tabs[activeTab].SetActive(false);
        contactInfos[id].SetActive(true);
        contactID = id;
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
        foreach (GameObject x in contactInfos)
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
                contactInfos[contactID].SetActive(false);
                contactInfoHolder.SetActive(false);
                tabs[2].SetActive(true);
                activeTab = 2;
            }
        }
    }
}
