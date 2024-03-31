using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;
    private bool canPickUp;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            canPickUp = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canPickUp)
        {

            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
                if (reminder == 0)
                    item.DestroyItem();
                else
                    item.Quantity = reminder;
            }
            canPickUp = false;
        }
    }
}
