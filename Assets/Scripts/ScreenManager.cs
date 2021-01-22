using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager _instance;
    public static ScreenManager Instance { get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScreenManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        ConfigureScreen();
        //Application.targetFrameRate = 60;
    }

    private Vector2 resolution;
    private float maxRadius;

    private void Update()
    {
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            ConfigureScreen();
            SpawnPlanets.Instance.StartSpawn(true, true, true, true);
        }
    }

    private void ConfigureScreen()
    {
        Camera.main.transform.position = Vector2.zero;
        Camera.main.transform.position = new Vector3(0, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y, -10);
        resolution = new Vector2(Screen.width, Screen.height);
        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        maxRadius = (topRight - Vector2.zero).magnitude;
    }

    public float GetMaxRadius()
    {
        return maxRadius;
    }
}
