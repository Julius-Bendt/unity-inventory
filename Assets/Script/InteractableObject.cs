using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Q;
    Transform player;
    public string interactText = "Press 'e' to start chopping";
    bool canStartEvent = true;

    public int keypresses = 50;
    public float maxTime = 10f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(Vector2.Distance(player.position,transform.position) < 1)
        {
            InteractText.ChangeText(interactText);

            if(Input.GetKeyDown(interactKey) && canStartEvent)
            {
                canStartEvent = false;
                App.Instance.keyTimeEvent.StartEvent(OnEventOver, keypresses,maxTime);
            }
        }
    }

    public void OnEventOver(bool success)
    {
        if(success)
        {
            //Give resource
            Item item = App.Instance.inventory.itemDB.FindItem("logs");
            App.Instance.inventory.AddItem(item, 5);

            Destroy(gameObject);
        }
        else
        {
            canStartEvent = true;
        }
       
    }
}
