using UnityEngine;
using UnityEditor;
using System.Collections;

class CraftingEditor : EditorWindow
{
    //300x300

    Item[,] source;
    Texture arrow;

    Item craftingItem;

    private Vector2 nodeSize = new Vector2(64, 64);
    private float offset = 20;

    [MenuItem("Window/Inventory/Crafting")]
    public static void ShowWindow()
    {

        GetWindow(typeof(CraftingEditor));
    }

    public void Setup()
    {
        arrow = (Texture)EditorGUIUtility.Load("tab_next@2x");
        source = new Item[3, 3];
    }

    void OnGUI()
    {
        if (arrow == null)
            Setup();

        GUILayout.Label("Item crafting creator", EditorStyles.boldLabel);
        GUILayout.Space(15);
        GUILayout.Label("Recipe", EditorStyles.boldLabel);

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                Rect r = new Rect(nodeSize.x * x + offset, nodeSize.y * y + offset + 50, nodeSize.x, nodeSize.y);
                source[x,y] = (Item)EditorGUI.ObjectField(r, source[x,y], typeof(Item),false);
            }
        }

        Rect arrowRect = new Rect((nodeSize.x + offset) * 3 + 16, (nodeSize.y + offset) * 1.5f - 32, 64, 64);
        EditorGUI.DrawTextureAlpha(arrowRect, arrow);

        Rect craftingRect = new Rect(arrowRect.x + arrow.width + nodeSize.x,arrowRect.y, nodeSize.x,nodeSize.y);
        craftingItem = (Item)EditorGUI.ObjectField(craftingRect, craftingItem, typeof(Item), false);

        float buttonWidth = (craftingRect.x - arrowRect.x) + (craftingRect.width + arrowRect.width);
        Rect buttonRect = new Rect(arrowRect.x - offset,arrowRect.y + arrowRect.height*1.2f,buttonWidth, 40);

        if(GUI.Button(buttonRect,"Create recipe"))
        {
            if (craftingItem != null)
            {
                string json = RecipeHelper.RecipeToString(source);
                string itemJson = RecipeHelper.ItemToString(craftingItem);
                RecipeHelper.SaveRecipe(json, itemJson);
            }
                

        }

    }

    void DrawLine(int i_height = 1)
    {
        EditorGUILayout.Space();
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);

        rect.height = i_height;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space();

    }

}