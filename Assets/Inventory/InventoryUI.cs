using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public int width = 5, height = 3;


    public GameObject itemSlot;
    public Transform grid;

    
    [HideInInspector]
    public ItemSlot[,] itemSlots;


    public InventoryDragHandler InvDragHandler;

    private void Update()
    {
        if(InvDragHandler != null)
        { 
            InvDragHandler.Update();
        }
    }

    public void SetupInventory()
    {
        InvDragHandler.ui = this;
        InvDragHandler.handler.RemoveItem(); //clears the ui

        itemSlots = new ItemSlot[width, height];

        for(int y = height-1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject g = Instantiate(itemSlot, grid);
                g.name = $"Item node [{x},{y}]";
                itemSlots[x, y] = g.GetComponent<ItemSlot>();
                itemSlots[x, y].location = new Vector2Int(x, y);
                itemSlots[x, y].UpdateUI();                
                   
            }
        }
    }

    public void AddItem(InventoryItem item, bool forceNew = false)
    {
        Vector2Int index = Vector2Int.zero;
        if (!forceNew)
            index = FindItemSlot(item);
        else
            index = FindEmptySlot();

        itemSlots[index.x, index.y].AddItem(item.item,item.amount);
    }

    public void AddItem(InventoryItem item, Vector2Int location)
    {
        itemSlots[location.x, location.y].AddItem(item.item, item.amount);
    }

    public void AddAmountToItem(InventoryItem item)
    {
        Vector2Int index = FindItemSlot(item);
        ItemSlot slot = itemSlots[index.x, index.y];
        slot.AddItem(item.item, item.amount);
        slot.UpdateUI();
    }

    public Vector2Int FindEmptySlot()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(itemSlots[x,y].Empty)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return Vector2Int.one * -1;
    }

    public Vector2Int FindItemSlot(InventoryItem item)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(itemSlots[x,y].item != null)
                {
                    if (itemSlots[x, y].item.item == item.item)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
        }

        return FindEmptySlot();
    }


}
[System.Serializable]
public class InventoryDragHandler
{

    [HideInInspector]
    public ItemSlot currentSlot; //The slot we're inside rn

    [HideInInspector]
    public InventoryUI ui;

    public DragHandler handler = new DragHandler();

    [HideInInspector]
    public ItemSlot currentItem; //The start itemslot.

    public bool dragging
    {
        get
        {
            if (handler.item != null)
                return handler.item.amount > 0;
            else
                return false;
        }
    }

    [System.Serializable]
    public struct DragHandler
    {
        public RectTransform handler;
        public Image icon;
        public TextMeshProUGUI amount;
        [HideInInspector]
        public InventoryItem item;



        public void RemoveItem()
        {
            handler.anchoredPosition = Vector3.zero;
            icon.sprite = null;
            icon.color = Color.clear;
            amount.text = "";
            item = null;
        }

        public void AddItem(InventoryItem _item)
        {
            item = _item;
            UpdateUI();
        }

        public void UpdateUI()
        {
            amount.text = item.amount.ToString();
            icon.color = Color.white;
            icon.sprite = item.item.icon;
        }
    }




    public InventoryDragHandler(InventoryUI _ui)
    {
        ui = _ui;
    }

    public void StartDragging(ItemSlot _itemSlot,InventoryItem split = null)
    {
        if (_itemSlot.Empty)
            return;

        currentItem = _itemSlot;
        if (split == null)
        {
            handler.AddItem(currentItem.item);
            currentItem.RemoveItem();
        }
        else
            handler.AddItem(split);


    }

    public void StopDragging(ItemSlot _currentSlot)
    {
        if(_currentSlot == null && currentSlot == null)
        {
            Debug.Log("something went wrong!");
            //draggingRect.anchoredPosition = Vector2.zero;
            //draggingRect = null;
            //currentDragging = null;
            currentItem.AddItem(handler.item.item, handler.item.amount); //jump back to original spot
            return;
        }

        currentSlot = _currentSlot;

        if (currentSlot.Empty) //Current node is empty, just add the item.
        {

            currentSlot.AddItem(handler.item.item, handler.item.amount);   
        }
        else
        {
            if (currentSlot.item.item.id == handler.item.item.id) //plus the two together
            {
                //check stacking
                if (currentSlot.item.amount + handler.item.amount > handler.item.item.stack)
                {
                    int diff = (currentSlot.item.amount + handler.item.amount) - handler.item.item.stack;
                    currentSlot.item.amount = currentSlot.item.item.stack;
                    currentSlot.UpdateUI();

                    handler.item.amount = diff;
                    handler.UpdateUI();

                    return; //Return - we dont want to stop dragging yet.
                }
                else                //else just add the amount 
                    currentSlot.AddItem(handler.item.item, handler.item.amount);
            }
            else
            {
                //find out whats currently under
                InventoryItem under = currentSlot.item;
                //remove whats currently under, and add the handler item
                currentSlot.RemoveItem();
                currentSlot.AddItem(handler.item.item, handler.item.amount);
                //add what was under, to holder
                handler.RemoveItem();   
                handler.AddItem(under);

                return;
            }

        }

        handler.RemoveItem();
        currentItem = null;
    }

    public void Update()
    {
        if(dragging)
        {
            handler.handler.position = Input.mousePosition;
        }
       
           
    }

}
