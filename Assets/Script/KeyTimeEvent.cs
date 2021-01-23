using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyTimeEvent : MonoBehaviour
{
    KeyCode key1 = KeyCode.Q, key2 = KeyCode.E;

    bool active = true;

    float timeElapsed = 0;
    float maxTime, reachPress;
    int presses;
    Callback c;

    public GameObject key1UI, key2UI;
    private TextMeshProUGUI key1Text, key2Text;
    public Image progress, time;

    public float Progress
    {
        get
        {
            return presses / reachPress;
        }
    }

    public float TimeProgress
    {
        get
        {
            return 1-(timeElapsed / maxTime);
        }
    }

    public delegate void Callback (bool success);
    
    public void StartEvent(Callback _callback,int _presses = 20,float _maxTime = Mathf.Infinity)
    {
        if(active)
        {
            Debug.LogError("KeyTimeEvent.StartEvent was called, but an event is already active!");
            c.Invoke(false);
        }

        if(key1Text == null ||key2Text == null)
        {
            key1Text = key1UI.GetComponentInChildren<TextMeshProUGUI>();
            key2Text = key2UI.GetComponentInChildren<TextMeshProUGUI>();
        }

        timeElapsed = 0;
        reachPress = _presses;
        active = true;

        key1Text.text = key1.ToString();
        key2Text.text = key2.ToString();

        progress.transform.parent.gameObject.SetActive(true);
        progress.fillAmount = 0;

        if(_maxTime == Mathf.Infinity)
        {
            maxTime = -1;
        }
        else
        {
            time.transform.parent.gameObject.SetActive(true);
            time.fillAmount = 0;
            maxTime = _maxTime;
        }

        if (_callback != null)
            c = _callback;

        if (Random.Range(0, 100) >= 50)
            key1UI.SetActive(true);
        else
            key2UI.SetActive(true);
    }

    private void StopEvent()
    {
        active = false;

        key1UI.SetActive(false);
        key2UI.SetActive(false);
        progress.transform.parent.gameObject.SetActive(false);
        time.transform.parent.gameObject.SetActive(false);
    }
    public void CancelEvent()
    {
        StopEvent();

        if(c != null)
            c.Invoke(false);
    }

    private void Update()
    {
        if(active)
        {
            if(maxTime != -1)
            {
                timeElapsed += Time.deltaTime;

                time.fillAmount = TimeProgress;

                if (timeElapsed > maxTime)
                    CancelEvent();
            }
            
            if(key1UI.activeSelf)
            {
                if(Input.GetKeyDown(key1))
                {
                    presses++;
                    key1UI.SetActive(false);
                    key2UI.SetActive(true);
                }
            }

            if (key2UI.activeSelf)
            {
                if (Input.GetKeyDown(key2))
                {
                    presses++;
                    key2UI.SetActive(false);
                    key1UI.SetActive(true);
                }
            }

            progress.fillAmount = Progress;

            if (presses >= reachPress)
            {
                StopEvent();

                if (c != null)
                    c.Invoke(true);
            }
        }
    }
}
