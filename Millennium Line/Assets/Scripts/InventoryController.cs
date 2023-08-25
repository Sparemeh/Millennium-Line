using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InventoryPage inventoryUI;
    
    public int inventorySize = 10;

    private void Start() 
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }

    void OnInventoryToggle()
    {
        if (inventoryUI.isActiveAndEnabled == false)
        {
            inventoryUI.Show();
        }
        else
        {
            inventoryUI.Hide();
        }
    }
}
