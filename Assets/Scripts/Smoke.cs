using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        color.a -= 0.6f * Time.deltaTime;
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
        GetComponent<SpriteRenderer>().color = color;
    }
}
