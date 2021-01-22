using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShootingStars : MonoBehaviour
{
    public GameObject shootingStar;
    float normal_minSpawnWaitTime = 100f;
    float normal_maxSpawnWaitTime = 300;
    float storm_minSpawnWaitTime = 0.2f;
    float storm_maxSpawnWaitTime = 0.3f;
    float minSpawnWaitTime;
    float maxSpawnWaitTime;
    bool isStorm;
    private IEnumerator coroutine = null;

    private static SpawnShootingStars _instance;
    public static SpawnShootingStars Instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpawnShootingStars>();
            }
            return _instance;
        }
    }

    public void StartSpawn()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = Spawn();
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnWaitTime, maxSpawnWaitTime));

        float randomPosX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - 2, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - 2);
        float randomPosY = Random.Range(3, 11);
        GameObject obj = Instantiate(shootingStar, new Vector2(randomPosX, randomPosY), Quaternion.identity);
        obj.transform.Rotate(0, 0, -45);

        StartSpawn();
    }

    public void startStorm()
    {
        if (!isStorm) {
            StartCoroutine(endStorm());
            isStorm = true;
            minSpawnWaitTime = storm_minSpawnWaitTime;
            maxSpawnWaitTime = storm_maxSpawnWaitTime;
            StartSpawn();
        }
    }

    IEnumerator endStorm()
    {
        yield return new WaitForSeconds(Random.Range(30, 60));

        isStorm = false;
        minSpawnWaitTime = normal_minSpawnWaitTime;
        maxSpawnWaitTime = normal_maxSpawnWaitTime;
        StartSpawn();
    }
    public void ChangeShootingStarFrequency(int min, int max)
    {
        normal_minSpawnWaitTime = min;
        normal_maxSpawnWaitTime = max;

        minSpawnWaitTime = normal_minSpawnWaitTime;
        maxSpawnWaitTime = normal_maxSpawnWaitTime;
    }
}
