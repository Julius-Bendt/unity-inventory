using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public InventoryUI invUI;
    public ItemDatabase itemDB;
    public GameObject inventoryWindow;

    public bool InventoryOpen
    {
        get
        {
            return inventoryWindow.activeSelf;
        }
    }

    private void Start()
    {
        invUI.SetupInventory();
        AddPlaceholderItems();
    }

    private void Update()
    {
        if(Input.GetKeyDown(App.Instance.keyManager.ToggleInventory))
        {
            inventoryWindow.SetActive(!inventoryWindow.activeSelf);
        }
    }

    public void AddItem(Item _item, int _amount)
    {
        //Do we already have this item? then add the amounts
        foreach (InventoryItem item in items)
        {
            if(item.item == _item)
            {
                item.amount += _amount;
                int diff = 0;

                if (item.amount > item.item.stack)
                {
                    diff = item.amount - item.item.stack;
                    invUI.AddItem(new InventoryItem(_item, diff),true);
                }

                invUI.AddAmountToItem(new InventoryItem(_item,_amount-diff));


                return;
            }
               
        }


        items.Add(new InventoryItem(_item, _amount));
        invUI.AddItem(new InventoryItem(_item, _amount));
    }

    public void RemoveItem(InventoryItem item)
    {
        if(items.Contains(item))
        {
            items.Remove(item);
        }
    }

    public void AddPlaceholderItems()
    {
        
        AddItem(itemDB.FindItem("carrot"),5);
        AddItem(itemDB.FindItem("coal"), 5);
        AddItem(itemDB.FindItem("gold"), 5);
        AddItem(itemDB.FindItem("carrot"), 8);
        AddItem(itemDB.FindItem("logs"), 7);
        
    }

}

[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int amount;

    public InventoryItem (Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}

