using UnityEngine;
using UnityEditor;
using System.Collections;

class InventoryEditor : EditorWindow
{

    //Item
    private string itemName = "";
    private int stack = 10;
    private int itemId = -1;
    private Sprite icon;

    //weapon
    public string desc; //also armor
    public bool unlocked; // also armor
    public int cost; //also armor
    public int maxAmmo;
    public float fireRate;
    public bool autoFire;
    public GameObject bullet, muzzle;

    private Type type = Type.item;

    private enum Type
    {
        item,
        weapon
    };

    [MenuItem("Window/Inventory/Create item")]
    public static void ShowWindow()
    {
        
        GetWindow(typeof(InventoryEditor));
    }

    void OnGUI()
    {
        if (itemId == -1)
            itemId = FindID();
        
          

        itemUI();

         if(type == Type.weapon)
        {
            DrawLine();
            weaponUI();
        }

        if (itemId >= 0)
        {
            DrawLine();
            EditorGUILayout.Space();
            if (GUILayout.Button("Create"))
            {
                Object o = CreateObject();
                ItemDatabase db = FindObjectOfType<ItemDatabase>();

                if (db == null)
                {
                    Debug.LogError("Couldn't find item database, aborting");
                    return;
                }

                if (o == null)
                {
                    return;
                }


                switch (type)
                {
                    case Type.item:
                        db.items.Add((Item)o);
                        break;
                    case Type.weapon:
                        db.items.Add((Weapon)o);
                        break;
                }
            }
        }
    }

    public void itemUI()
    {
        GUILayout.Label("Item Settings", EditorStyles.boldLabel);

        itemName = EditorGUILayout.TextField("Name: ", itemName);

        if(type == Type.item)
            stack = EditorGUILayout.IntField("stack amount: ", stack);

        itemId = EditorGUILayout.IntField("id: ", itemId);

        icon = (Sprite)EditorGUILayout.ObjectField("Item icon ", icon, typeof(Sprite), false);


        type = (Type)EditorGUILayout.EnumPopup("item type", type);

        if(string.IsNullOrEmpty(itemName))
        {
            if (icon != null)
                itemName = icon.name;
        }
    }

    public void weaponUI()
    {
        GUILayout.Label("Weapon settings", EditorStyles.boldLabel);
        desc = EditorGUILayout.TextField("Desc: ", desc);

        cost = EditorGUILayout.IntField("Cost: ", cost);
        maxAmmo = EditorGUILayout.IntField("Max ammo: ", maxAmmo);
        fireRate = EditorGUILayout.FloatField("fire rate: ", fireRate);

        autoFire = EditorGUILayout.Toggle("Auto fire: ", autoFire);
        unlocked = EditorGUILayout.Toggle("Unlocked: ", unlocked);

        bullet = (GameObject)EditorGUILayout.ObjectField("Bullet ",bullet, typeof(GameObject), false);
        muzzle = (GameObject)EditorGUILayout.ObjectField("Muzzle ", muzzle, typeof(GameObject), false);

        stack = 1;

        if (fireRate <= 0)
            fireRate = 0.0001f;

        if (maxAmmo < 1)
            maxAmmo = 1;
    }

    public void armorUI()
    {
        stack = 1;
    }

    void DrawLine(int i_height = 1)
    {
        EditorGUILayout.Space();
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);

        rect.height = i_height;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space();

    }

    public int FindID()
    {


        ItemDatabase db = FindObjectOfType<ItemDatabase>();

        if(db == null)
        {
            Debug.LogError("Cant find item database. make sure there is one active in the scene.");
            return -2;
        }



        int highest = 0;

        if(db.items.Count > 0)
            if (db.items[db.items.Count - 1].id > highest)
                highest = db.items[db.items.Count - 1].id + 1;


        if (db.weapons.Count > 0)
            if (db.weapons[db.weapons.Count - 1].id > highest)
            highest = db.weapons[db.weapons.Count - 1].id + 1;


        return highest;

    }

    public Object CreateObject()
    {
        if(itemId == -2)
        {
            Debug.LogError("Can't assign proper id. aborting");
            return null;
        }

        string path = $"Assets/Inventory/{type.ToString()}s/{itemName}.asset";

        Debug.Log($"Creating item {itemName} type {type.ToString()} with the id {itemId} at {path}");

        switch (type)
        {
            case Type.item:
                Item item = ScriptableObject.CreateInstance<Item>();


                item.name = itemName;
                item.stack = stack;
                item.id = itemId;
                item.icon = icon;

                AssetDatabase.CreateAsset(item, path);
                AssetDatabase.SaveAssets();
                return item;
            case Type.weapon:
                Weapon weapon = ScriptableObject.CreateInstance<Weapon>();

                weapon.name = itemName;
                weapon.stack = 1;
                weapon.id = itemId;
                weapon.icon = icon;

                weapon.desc = desc;
                weapon.unlocked = unlocked;
                weapon.cost = cost;
                weapon.maxAmmo = maxAmmo;
                weapon.fireRate = fireRate;
                weapon.autoFire = autoFire;
                weapon.bullet = bullet;
                weapon.muzzle = muzzle;

                AssetDatabase.CreateAsset(weapon, path);
                return weapon;
        }

        Debug.Log("Item creation went wrong.");

        return null;
    }
}