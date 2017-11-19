using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour {
    
    [SerializeField]
    InputField seedDisplay;
    [SerializeField]
    GameObject pickupPrefab, mapMarker, mapEndMarker,platformPrefab, platformFloorPrefab, platformUnderground;

    [SerializeField]
    ScreenOrientation screenOrient;

    List<GameObject> platforms;
    int platformPoolAmount;

    public string staticMap = "";
    public Guid mapSeed;
    public bool shouldSpawn;

    System.Random rand;
    float width;
    float height;
    float groundOffset;
    int minX = -20;
    int maxX = 25;
    int minY = -7;
    int maxY = 4;
    int highestY, lowestY;

    PerlinNoise noise;
    
    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        Screen.orientation = screenOrient;
        if (GameObject.Find("DatabasserHandler") != null && DataHolder.Instance.Seed != null)
            staticMap = DataHolder.Instance.Seed;
        else
        {
            mapSeed = Guid.NewGuid();
        }


        if (staticMap == ""){
            mapSeed = Guid.NewGuid();
            seedDisplay.GetComponent<InputField>().text = mapSeed.ToString();
            rand = new System.Random(Mathf.Abs(mapSeed.ToString().GetHashCode()));
            noise = new PerlinNoise(Mathf.Abs(mapSeed.GetHashCode()));
            staticMap = mapSeed.ToString();
        }
        else
        {
            seedDisplay.GetComponent<InputField>().text = staticMap;
            rand = new System.Random(Mathf.Abs(staticMap.GetHashCode()));
            noise = new PerlinNoise(Mathf.Abs(staticMap.GetHashCode()));
        }

        //Get the width and height adjusted by the localscale of the platformPrefab
        Vector2 scale = new Vector2(platformPrefab.transform.localScale.x, platformPrefab.transform.localScale.y);
        height = platformPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x * scale.x;
        width = platformPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y * Math.Abs(scale.y);
        groundOffset = platformUnderground.GetComponent<BoxCollider2D>().size.y / 2 - height / 2;
        RegenerateMap(3);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }    

    public void RegenerateMap(float startX)
    {
        noise.seed = rand.Next(1000000, 9999999);
        RegenerateFloor(startX);
        RegenerateRoof(startX);
    }
    public void RegenerateMap()
    {
        float offset = GameObject.FindGameObjectWithTag("mapMarker").transform.position.x;
        noise.seed = rand.Next(1000000, 9999999);

        RegenerateFloor(offset);
        RegenerateRoof(offset);       

    }
    
    private void RegenerateFloor(float startX)
    {
        if(startX != 0)
            startX += (width / 2 - 0.05f);

        minY = -7;
        maxY = 4;
        int lowestStart = lowestY;
        lowestY = int.MinValue;

        Instantiate(mapMarker, new Vector2(startX * 2 + minX * width, 0), Quaternion.identity);

        for (int x = minX; x < maxX; x++)
        {                                      
            int columnHeight = 1+ noise.GetNoise(x - minX, maxY-minY);
            for (int y = minY; y < minY + columnHeight; y++)
            {
                if (x == minX && y < lowestY - columnHeight - 1)
                {
                    lowestStart = y;
                }

                if (x == maxX - 1 && y > lowestY)
                    lowestY = y;

                if (y == minY + columnHeight - 1)
                {
                    //int luck = rand.Next(0, 25); // TODO: Use seed
                    //if (luck < 1)
                    //{
                    //    GameObject pickup = Instantiate(pickupPrefab, new Vector2(startX * 2 + x * width, y * height + 0.7f), Quaternion.identity);
                    //}

                    GameObject platform = Instantiate(platformFloorPrefab, new Vector2(startX * 2 + x * width, y * height), Quaternion.identity);
                    platform.transform.parent = gameObject.transform;
                }
                else if(y == minY + columnHeight - 2)
                {                    
                    GameObject platform = Instantiate(platformUnderground, new Vector2(startX * 2 + x * width, (y + 1) * height - groundOffset), Quaternion.identity);
                    platform.transform.parent = gameObject.transform;
                }
            }

            if (x == maxX - 1)
            {
                if(GameObject.FindGameObjectWithTag("mapMarker") == null)
                    Instantiate(mapEndMarker, new Vector2(x * width, 0), Quaternion.identity);

                break;
            }
        }
    }

    int lastEndRoof = 0;

    private void RegenerateRoof(float startX)
    {
        if (startX != 0)
            startX += (width/2 -0.05f);
        
        minY = 7;
        maxY = 8;
        int startHeight = 0;
        highestY = int.MaxValue;

        for (int x = minX; x < maxX; x++)
        {
            int columnHeight = noise.GetNoise(x - minX, maxY + minY -3);
            for (int y = maxY; y > minY - columnHeight; y--)
            {
                    if (x == minX && y > startHeight)
                    {
                        startHeight = y;
                    }

                    if (x == maxX - 1 && y < highestY) //On last x iteration
                        highestY = y;

                    if (y == minY - columnHeight + 1)
                    {
                        GameObject platform = Instantiate(platformPrefab, new Vector2(startX * 2 + x * width, y * height), Quaternion.identity);
                        platform.transform.parent = gameObject.transform;
                        //int luckRoof = rand.Next(0, 11);
                        //if (luckRoof < 1)
                        //{
                        //    GameObject pickup = Instantiate(pickupPrefab, new Vector2(startX * 2 + x * width, y * height - 0.7f), Quaternion.identity);
                        //    pickup.transform.localScale *= -1;
                        //}
                    }
                    else if (y == minY - columnHeight + 2)
                    {

                        GameObject platform = Instantiate(platformUnderground, new Vector2(startX * 2 + x * width, (y - 1) * height + groundOffset), Quaternion.identity);
                        platform.transform.parent = gameObject.transform;
                    }
                }
            }
        }
    }
