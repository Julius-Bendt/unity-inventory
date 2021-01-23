using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Juto;
using System.Linq;

public static class RecipeHelper
{

    public static Item RecipeToItem(Item[,] items)
    {
        return new Item();
    }

    public static string RecipeToString(Item[,] items)
    {
        int column = 0, rows = 0;
        string _return = string.Empty;

        int low_x = -1, high_x = -1;
        int low_y = -1, high_y = -1;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if(items[x,y] != null)
                {
                    _return += $"[{items[x, y].id}]";

                    if (low_x == -1 || low_x > x)
                        low_x = x;
                    if (low_y == -1 || low_y > y)
                        low_y = y;

                    if (high_x < x) high_x = x;
                    if (high_y < y) high_y = y;
                }
                else
                {
                    _return += "0";
                }
            }
        }

        column = (high_x + 1) - low_x;
        rows = (high_y + 1) - low_y;

        _return = $"[{column}x{rows}]" + _return;
        Debug.Log(_return);

        return _return;
    }

    public static string ItemToString(Item item)
    {
        return $"[{item.id}]";
    }

    public static void StringToRecipe()
    {

    }

    public static Dictionary<string, string> LoadRecipe()
    {
        Dictionary<string, string> r = new Dictionary<string, string>();

        if (File.Exists(Application.persistentDataPath + "/recipes.json"))
        {
            return Serialization.Load<Dictionary<string, string>>(Application.persistentDataPath + "/recipes.json");
        }


        Debug.LogError("No recipes found! returning null");
        return null;
    }

    public static bool SaveRecipe(string recipe,string item)
    {
        Debug.Log("Saving to " + Application.persistentDataPath + "/recipes.json");
        try
        {
            Dictionary<string, string> dict = LoadRecipe();
            dict.Add(recipe, item);

            var entries = dict.Select(d =>
                    string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));
            string json =  "{" + string.Join(",", entries) + "}";

            File.WriteAllText(Application.persistentDataPath + "/recipes.json", json);

            Debug.Log(json);
            Debug.Log("hihu");
            return true;
        }
        catch(System.Exception e)
        {
            Debug.LogError("Error saving recipes: " + e);
            return false;
        }
    }

    struct holder
    {
        keys[] keys;
    }

    struct keys
    {
        public string key, value;
    }


}
