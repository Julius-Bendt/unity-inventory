using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftHandler
{
    private Vector2Int m_craftingGridSize; //3x3
    private bool m_break = false;
    Dictionary<string, string> recipes = new Dictionary<string, string>();

    public void Initialize()
    {
        recipes.Clear();
        recipes = RecipeHelper.LoadRecipe();
    }

    public Item Craft(Item[,] items)
    {
        
        string id;
        recipes.TryGetValue(RecipeHelper.RecipeToString(items), out id);

        //Then find the id in the item database


        //return found item, or null if none
        return null;
    }

    public bool AddRecipe(string keyJson,string itemJson)
    {
        try
        {
            recipes.Add(keyJson, itemJson);
            return true;
        }
        catch //item already exists
        {
            return false;
        }
        
    }
}
