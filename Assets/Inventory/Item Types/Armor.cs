using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Inventory/Armor", order = 3)]
public class Armor : Item
{
    [Space(10)]
    public string desc;
    public bool unlocked;
    public int cost;
}
