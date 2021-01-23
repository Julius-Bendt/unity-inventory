using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon", order = 2)]
public class Weapon : Item
{
    [Space(10)]
    public string desc;
    public bool unlocked;
    public int cost;
    public int maxAmmo;
    public float fireRate;
    public bool autoFire;
    public GameObject bullet, muzzle;
}
