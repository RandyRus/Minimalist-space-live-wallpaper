using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircularMovement : MonoBehaviour
{
    [SerializeField]
    private float semiOrbitTime;
    private float timer, theta;
    private int screenHeight;
    private bool spinning = false;
    private DateTime time;

    private void Awake() {
        theta = 0;
        timer = 0;
        time = DateTime.Now;
    }

    private void Update() {     
        if (!spinning) { return; }

        TimeSpan span = DateTime.Now - time;
        time = DateTime.Now;

        //Spawn new planets if it has been a while since last opened
        if (span.TotalSeconds > 1) {
            if (span.TotalSeconds > semiOrbitTime/2)
            {
                SpawnPlanets.Instance.StartSpawn(true, true, true, true);
                return;
            }
            timer += (float)span.TotalSeconds;
        }
        else {
            timer += Time.deltaTime;
        }

        if (semiOrbitTime != 0)
        {
            theta = (timer * (Mathf.PI / semiOrbitTime));
            transform.localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * theta, Vector3.back);
        } else
        {
            Debug.Log("semiOrbitTime cannot equal 0");
        }
    }

    public void StartSpin()
    {
        spinning = true;
        time = DateTime.Now;
        timer = 0;
    }
    public void StopSpin()
    {
        spinning = false;
    }

    public void ChangeTurnSpeed(int speed)
    {
        semiOrbitTime = speed;
    }
}