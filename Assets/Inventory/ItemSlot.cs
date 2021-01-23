using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;

    public Image icon, background;
    public TextMeshProUGUI amountText;
    public Vector2Int location;

    public bool Empty
    {
        get
        {
            if (item == null)
                return true;
            if (item.amount == 0)
                return true;

            return false;
        }
    }


    public void AddItem(Item _item, int _amount)
    {
        if (!Empty)
        {
            item.amount += _amount;
        }
        else
            item = new InventoryItem(_item, _amount);


        UpdateUI();
    }

    public void RemoveItem()
    {
        item = null;
        UpdateUI();
    }

    public void UpdateUI()
    {

        if (item == null)
        {
            ClearUI();
            return;
        }

        if (item.item == null)
        {
            ClearUI();
            return;
        }

        if(item.amount <= 0)
        {
            RemoveItem();
        }

        amountText.text = item.amount.ToString();
        icon.color = Color.white;
        icon.sprite = item.item.icon;
    }

    public void ClearUI()
    {
        amountText.text = "";
        icon.color = Color.clear;
        icon.sprite = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if( eventData.button == PointerEventData.InputButton.Left) //lifting all items
        {
            if (App.Instance.inventory.invUI.InvDragHandler.dragging)
            {
                App.Instance.inventory.invUI.InvDragHandler.StopDragging(this);

            }
            else
            {
                if (!Empty)
                    App.Instance.inventory.invUI.InvDragHandler.StartDragging(this);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right) //split
        {
            if(!Empty)
            {
                InventoryItem splitItem = new InventoryItem(item.item,0);
                if (item.amount % 2 == 0) //even number
                {
                    splitItem.amount = item.amount / 2;
                    item.amount /= 2;

                }
                else //odd
                {
                    splitItem.amount = (item.amount / 2); //round down
                    item.amount /= 2;
                    item.amount += 1; //round 
                }

                App.Instance.inventory.invUI.InvDragHandler.StartDragging(this, splitItem);
                UpdateUI();
            }
        }
      

    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = new Color(0,0,0,0.5f);
        App.Instance.inventory.invUI.InvDragHandler.currentSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = Color.clear;
        App.Instance.inventory.invUI.InvDragHandler.currentSlot = null;
    }
}
