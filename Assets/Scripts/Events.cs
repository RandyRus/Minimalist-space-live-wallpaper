using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events : MonoBehaviour
{
    private int previousHour = -1;
    public GameObject spaceShip, UFO;
    private SpawnShootingStars shootingStars;
    private float timer;
    private float minRandomEventWaitTime = 200f;
    private float maxRandomEventWaitTime = 300f;

    private void Start()
    {
        shootingStars = GetComponent<SpawnShootingStars>();
        timer = UnityEngine.Random.Range(minRandomEventWaitTime, maxRandomEventWaitTime);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (UnityEngine.Random.Range(0,2) == 0)
            {
                //Create UFO
                Instantiate(UFO, new Vector2(-Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x -1, 5), Quaternion.identity);
            } else
            {
                shootingStars.startStorm();
            }
            timer = UnityEngine.Random.Range(minRandomEventWaitTime, maxRandomEventWaitTime);
        }

        DateTime time = DateTime.Now;
        int hour = time.Hour;
        int minute = time.Minute;

        //Spaceship event
        if (hour != previousHour && minute == 0)
        {
            previousHour = hour;
            Instantiate(spaceShip, new Vector2(0, 0), Quaternion.identity);
        }
    }
}
