using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{

    private float movespeed, sight, resistant;
    private float thristDrain = 0.5f, hungerDrain = 0.2f, staminaDrain = 2f;

    public float thirst = 100,hunger = 100 ,health = 100, stamina = 50;


    public Stat(float _movespeed, float _sight, float _resistant )
    {
        movespeed = _movespeed;
        sight = _sight;
        resistant = _resistant;
    }

    public float MoveSpeed
    {
        get
        {
            float change = 0;

            if (thirst <= 50)
                change -= (thirst/50) * 0.25f;

            if (hunger <= 25)
                change -= (thirst / 25) * 0.25f;

            if (health <= 50)
                change -= (thirst / 50) * 0.10f;

            if (stamina <= 10)
                change -= (thirst / 10) * 0.15f;
            return (1+change) * movespeed;
        }
    }

    public float Sight
    {
        get
        {
            float change = 0;

            if (thirst <= 50)
                change -= (thirst / 50) * 0.35f;

            if (hunger <= 25)
                change -= (thirst / 25) * 0.1f;


            return (1+change) * sight;
        }
    }

    public float Resistant
    {
        get
        {
            float change = 0;

            if (thirst <= 50)
                change -= (thirst / 50) * 0.25f;

            if (hunger <= 25)
                change -= (hunger / 25) * 0.25f;

            if (health <= 50)
                change -= (health / 50) * 1.1f;

            if (health >= 80)
                change -= (health / 80) * 0.1f;


            if (stamina >= 80)
                change -= (stamina / 80) * 1.15f;

            if (stamina <= 10)
                change -= (stamina / 10) * 0.15f;


            return (1-change) * resistant;
        }
    }

    public void UpdateStats(bool running)
    {
        if (running)
            stamina -= staminaDrain * Time.deltaTime;

        hunger -= hungerDrain * Time.deltaTime;
        thirst -= thristDrain * Time.deltaTime;
    }
}
