using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    Color color;
    bool fading = false;

    float fadeUpSpeed = 1.2f;
    float fadeDownSpeed = 2f;
    float stretchSpeed = 3f;
    float maxLength = 3;

    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!fading) {
            color.a += Time.deltaTime * fadeUpSpeed;
            GetComponent<SpriteRenderer>().color = color;
            if (color.a >= 1) { fading = true; }
        } else {
            color.a -= Time.deltaTime * fadeDownSpeed;
            GetComponent<SpriteRenderer>().color = color;
            if (color.a <= 0) { Destroy(gameObject); }
        }
        if (transform.localScale.x < maxLength)
        {
            transform.localScale = new Vector2(transform.localScale.x + (stretchSpeed * Time.deltaTime), 1);
        }

        transform.position += transform.right * Time.deltaTime * 5;

    }
}
