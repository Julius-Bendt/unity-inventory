using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractText : MonoBehaviour
{
    private static TextMeshProUGUI t;

    public void Start()
    {
        t = GetComponent<TextMeshProUGUI>(); 
    }
    public static void ChangeText(string text)
    {
        t.text = text;
    }
}
