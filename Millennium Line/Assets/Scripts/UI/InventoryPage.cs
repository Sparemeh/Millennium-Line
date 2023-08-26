using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InventoryPage : MonoBehaviour
{
    [SerializeField]
    private UIInventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    [SerializeField]
    private InventoryDescription itemDescription;

    [SerializeField]
    private MouseFollower mouseFollower;

    List<UIInventoryItem> listOfItems = new List<UIInventoryItem>();

    public event Action<int> OnDescriptionRequested, OnItemRequested, OnStartDragging;

    public event Action<int, int> OnSwapItem;

    private int currentlyDraggedItemIndex = -1;
    
    private void Awake() 
    {
        Hide();
        mouseFollower.Toggle(false);
        itemDescription.ResetDescription();
    }

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i=0 ; i < inventorySize; i++)
        {
            UIInventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(contentPanel,false);
            listOfItems.Add(item);
            item.OnItemClicked += HandleItemSelection;
            item.OnItemBeginDrag += HandleBeginDrag;
            item.OnItemDroppedOn += HandleSwap;
            item.OnItemEndDrag += HandleEndDrag;
            item.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfItems.Count > itemIndex)
        {
            listOfItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    private void HandleItemSelection(UIInventoryItem InventoryItem)
    {
        int index = listOfItems.IndexOf(InventoryItem);
        if (index == -1)
            return;
        OnDescriptionRequested?.Invoke(index);
    }
    
    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = listOfItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }

    public void CreatedDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        int index = listOfItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnSwapItem?.Invoke(currentlyDraggedItemIndex, index);
    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        ResetDraggedItem();
    }

    private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
    {
        Debug.Log(inventoryItemUI.name);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        
        ResetSelection();
    }
    
    public void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    public void ResetSelection() 
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in listOfItems)
        {
            item.Deselect();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ResetDraggedItem();
    }

    internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        itemDescription.SetDescription(itemImage, name, description);
        DeselectAllItems();
        listOfItems[itemIndex].Select();
    }
}
