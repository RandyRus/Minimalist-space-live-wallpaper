using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanets : MonoBehaviour
{
    [HideInInspector]
    public bool allSpawned = false;

    public GameObject planet, star, tempCollision, spinner;
    public Sprite[] largePlanetSprites;
    public Sprite[] mediumPlanetSprites;
    public Sprite[] smallPlanetSprites;
    public Sprite[] starSprites;
    public GameObject cover;

    private int starSpawnCount;
    private int largePlanetSpawnCount;
    private int mediumPlanetSpawnCount;
    private int smallPlanetSpawnCount;

    private float starMinRadius = 2;
    private float planetMinRadius = 3;
    private float previousAngle, currentAngle;

    private List<GameObject> q1 = new List<GameObject>();
    private List<GameObject> q2 = new List<GameObject>();
    private List<GameObject> q3 = new List<GameObject>();
    private List<GameObject> q4 = new List<GameObject>();

    private static SpawnPlanets _instance;
    public static SpawnPlanets Instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpawnPlanets>();
            }
            return _instance;
        }
    }
    private bool running = false;
    IEnumerator coroutine = null;

    public void StartSpawn(bool s1, bool s2, bool s3, bool s4)
    {
        if (running) {
            StopCoroutine(coroutine);
        }
        running = true;
        coroutine = Spawn(s1, s2, s3, s4);
        StartCoroutine(coroutine);
    }
    private IEnumerator Spawn(bool s1, bool s2, bool s3, bool s4)
    {
        ResizeCoverToScreen();
        cover.SetActive(true);
        spinner.GetComponent<CircularMovement>().StopSpin();

        allSpawned = false;
        if (s1)
        {
            spinner.transform.localRotation = Quaternion.AngleAxis(45, Vector3.forward);
            yield return 0;
            SpawnQuarter(q1);
        }
        if (s2)
        {
            spinner.transform.localRotation = Quaternion.AngleAxis(135, Vector3.forward);
            yield return 0;
            SpawnQuarter(q2);
        }
        if (s3)
        {
            spinner.transform.localRotation = Quaternion.AngleAxis(225, Vector3.forward);
            yield return 0;
            SpawnQuarter(q3);
        }
        if (s4)
        {
            spinner.transform.localRotation = Quaternion.AngleAxis(315, Vector3.forward);
            yield return 0;
            SpawnQuarter(q4);
        }
        previousAngle = 0;
        allSpawned = true;
        spinner.GetComponent<CircularMovement>().StartSpin();
        cover.SetActive(false);

        running = false;
    }

    private void Update() {

        if (!allSpawned) { return; }

        currentAngle = spinner.transform.rotation.eulerAngles.z;
        if (currentAngle <= 45 && previousAngle > 45)
        {
            if (Mathf.Abs(currentAngle - previousAngle) < 1)
            {
                SpawnQuarter(q1);
            }
            else
            {
                StartCoroutine(Spawn(true, false, false, false));
            }
        }
        else if (currentAngle <= 135 && previousAngle > 135)
        {
            if (Mathf.Abs(currentAngle - previousAngle) < 1)
            {
                SpawnQuarter(q2);
            }
            else
            {
                StartCoroutine(Spawn(false, true, false, false));
            }
        }
        else if (currentAngle <= 225 && previousAngle > 225)
        {
            if (Mathf.Abs(currentAngle - previousAngle) < 1)
            {
                SpawnQuarter(q3);
            }
            else
            {
                StartCoroutine(Spawn(false, false, true, false));
            }
        }
        else if (currentAngle <= 315 && previousAngle > 315)
        {
            if (Mathf.Abs(currentAngle - previousAngle) < 1)
            {
                SpawnQuarter(q4);
            }
            else
            {
                StartCoroutine(Spawn(false, false, false, true));
            }
        }
        previousAngle = currentAngle;
    }

    private void SpawnQuarter(List<GameObject> list) {
        foreach (GameObject obj in list)
        {
            Destroy(obj);
        }
        list.Clear();

        for (int i = 0; i < largePlanetSpawnCount; i++) {
            Sprite sprite = largePlanetSprites[UnityEngine.Random.Range(0, largePlanetSprites.Length)];
            SpawnObject(planet, sprite, list, planetMinRadius);
        }
        for (int i = 0; i < mediumPlanetSpawnCount; i++)
        {
            Sprite sprite = mediumPlanetSprites[UnityEngine.Random.Range(0, mediumPlanetSprites.Length)];
            SpawnObject(planet, sprite, list, planetMinRadius);
        }
        for (int i = 0; i < smallPlanetSpawnCount; i++)
        {
            Sprite sprite = smallPlanetSprites[UnityEngine.Random.Range(0, smallPlanetSprites.Length)];
            SpawnObject(planet, sprite, list, planetMinRadius);
        }
        for (int i = 0; i < starSpawnCount; i++)
        {
            Sprite sprite = starSprites[UnityEngine.Random.Range(0, starSprites.Length)];
            SpawnObject(star, sprite, list, starMinRadius);
        }
    }

    private void SpawnObject(GameObject prefab, Sprite sprite,  List<GameObject> list, float minRadius)
    {
        //Get random spawn position
        Vector2 randomPos = new Vector2();
        float theta = 0;
        {
            float maxRadius = ScreenManager.Instance.GetMaxRadius();
            if (maxRadius < minRadius)
            {
                Debug.Log("Minimum radius cannot be greater than maximum radius");
            }

            bool flag = false;
            while (!flag)
            {
                randomPos = new Vector2(UnityEngine.Random.Range(-maxRadius, maxRadius), UnityEngine.Random.Range(-maxRadius, 0));
                theta = Vector2.Angle(Vector2.left, randomPos);

                float distance = Vector2.Distance(randomPos, new Vector2(0, 0));
                if (distance > minRadius & distance < maxRadius & theta > 45 & theta < 135)
                {
                    flag = true;
                }
            }
        }
        //Spawn object
        {
            GameObject temp = Instantiate(tempCollision, randomPos, Quaternion.identity);
            temp.transform.SetParent(spinner.transform, true);
            Vector2 tempPos = temp.transform.position;

            Vector2 sprite_size = sprite.rect.size / sprite.pixelsPerUnit;
            Collider2D col = Physics2D.OverlapCircle(tempPos, sprite_size.x / 2, Physics2D.GetLayerCollisionMask(prefab.layer));
            if (col == null)
            {
                GameObject obj = Instantiate(prefab, randomPos, Quaternion.identity);
                obj.transform.Rotate(0f, 0f, 90 + theta);
                obj.transform.SetParent(spinner.transform, true);
                obj.GetComponent<SpriteRenderer>().sprite = sprite;
                obj.GetComponent<CircleCollider2D>().radius = sprite_size.x / 2 + 0.1f;
                list.Add(obj);

                //Configure glow size
                if (prefab == planet)
                {
                    float size = obj.transform.Find("Glow").GetComponent<SpriteRenderer>().sprite.bounds.size.x;
                    obj.transform.Find("Glow").localScale = new Vector2(sprite_size.y + 0.05f / size, sprite_size.y + 0.05f / size);
                }
            }
            Destroy(temp);
        }
    }

    private void ResizeCoverToScreen()
    {
        Sprite sprite = cover.GetComponent<SpriteRenderer>().sprite;
        cover.transform.localScale = new Vector3(1, 1, 1);
        float width = sprite.bounds.size.x;
        float height = sprite.bounds.size.y;
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        cover.transform.localScale = new Vector2(worldScreenWidth / width, worldScreenHeight / height);
        cover.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
    }

    public void ChangeStarDensity(int density)
    {
        starSpawnCount = density;
    }

    public void ChangePlanetsFrequency(int large, int med, int small)
    {
        largePlanetSpawnCount = large;
        mediumPlanetSpawnCount = med;
        smallPlanetSpawnCount = small;
    }
}
