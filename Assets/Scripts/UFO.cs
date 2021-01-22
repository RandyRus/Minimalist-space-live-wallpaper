using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public float xSpeed = 0;
    public float ySpeed = 0;
    public float strafeWidth = 0;
    float timer;

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        Vector2 newPosition = transform.position;
        newPosition.x += xSpeed * Time.deltaTime;
        newPosition.y += Mathf.Sin(timer * ySpeed) * Time.deltaTime * strafeWidth;
        transform.position = newPosition;

        if (transform.position.x > Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1)
        {
            Destroy(gameObject);
        }
    }
}
