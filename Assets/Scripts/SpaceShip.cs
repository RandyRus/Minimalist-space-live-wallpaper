using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpaceShip : MonoBehaviour
{
    public GameObject smoke;

    private void Start()
    {
        StartCoroutine(SpawnSmoke());
    }
    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime);

        if (transform.position.y > 12)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnSmoke()
    {
        while (true)
        {
            GameObject obj = Instantiate(smoke, new Vector2(transform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f), transform.position.y - UnityEngine.Random.Range(0.2f, 0.3f)), Quaternion.identity);
            obj.transform.Rotate(0, 0, UnityEngine.Random.Range(0f, 360f));
            yield return new WaitForSeconds(0.3f);
        }
    }
}
