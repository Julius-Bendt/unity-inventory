using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    
    [SerializeField]
    private Image thirst, hunger, health, stamina;

    [HideInInspector]
    public Player player;

    public void UpdateUI()
    {
        UpdateStatUI();
    }

    private void UpdateStatUI()
    {
        thirst.fillAmount = player.stats.thirst/100;
        hunger.fillAmount = player.stats.hunger/100;
        health.fillAmount = player.stats.health/100;
        stamina.fillAmount = player.stats.stamina/100;
    }
}
