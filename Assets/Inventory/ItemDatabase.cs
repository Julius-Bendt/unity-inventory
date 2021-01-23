using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<Weapon> weapons = new List<Weapon>();
    public Item FindItem(string _name)
    {
        foreach (Item item in items)
        {
            if (item.name.ToLower() == _name.ToLower())
                return item;
        }


        Debug.LogError("Could not find the name " + _name + " in the item database!");
        return null;
    }

    public Item FindItem(int _id)
    {
        foreach (Item item in items)
        {
            if (item.id == _id)
                return item;
        }

        Debug.LogError("Could not find the id " + _id + " in the item database!");

        return null;
    }
}
