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
    string staticMap = "";
    [SerializeField]
    ScreenOrientation screenOrient;

    List<GameObject> platforms;
    int platformPoolAmount;

    public Guid mapSeed;
    public bool shouldSpawn;

    System.Random rand;
    float width;
    float height;
    int minX = -20;
    int maxX = 25;
    int minY = -7;
    int maxY = 4;
    int highestY, lowestY;

    PerlinNoise noise;
    
    // Use this for initialization
    void Start () {

        //platforms = new List<GameObject>();
        //platformPoolAmount = 100;

        //for (int i = 0; i < platformPoolAmount; i++)
        //{
        //    GameObject obj = Instantiate(platformPrefab);
        //    obj.transform.parent = gameObject.transform;
        //    obj.SetActive(false);
        //    platforms.Add(obj);
        //}

        Screen.orientation = screenOrient;
        if (GameObject.Find("Gameobject") != null && DataHolder.Instance.Seed != null)
            staticMap = DataHolder.Instance.Seed;
        else
            mapSeed = Guid.NewGuid();

        if (staticMap == ""){
            mapSeed = Guid.NewGuid();
            seedDisplay.GetComponent<InputField>().text = mapSeed.ToString();
            rand = new System.Random(mapSeed.ToString().GetHashCode());
            noise = new PerlinNoise(Mathf.Abs(mapSeed.GetHashCode()));
        }
        else
        {
            seedDisplay.GetComponent<InputField>().text = staticMap;
            rand = new System.Random(staticMap.GetHashCode());
            noise = new PerlinNoise(Mathf.Abs(staticMap.GetHashCode()));
        }
        
        //Get the width and height adjusted by the localscale of the platformPrefab
        Vector2 scale = new Vector2(platformPrefab.transform.localScale.x, platformPrefab.transform.localScale.y);
        height = platformPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x * scale.x;
        width = platformPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y * Math.Abs(scale.y);
        RegenerateMap(0);
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }    

    public void RegenerateMap(int startX)
    {
        RegenerateFloor(startX);
        RegenerateRoof(startX);
    }
    public void RegenerateMap()
    {
        int offset = (int)(GameObject.FindGameObjectWithTag("mapMarker").transform.position.x - width * 2);

        RegenerateFloor(offset);
        RegenerateRoof(offset);
        
        noise.seed = rand.Next(1000000, 9999999);
    }

    int lastEndFloor = 0;
    private void RegenerateFloor(float startX)
    {
        if (startX != 0)
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
                //if (y >= minY + columnHeight - 2)
                //{
                if (x == minX && y < lowestY - columnHeight - 1)
                {
                    lowestStart = y;
                }

                if (x == maxX - 1 && y > lowestY)
                    lowestY = y;

                if (y == minY + columnHeight - 1)
                {
                    int luck = rand.Next(0, 11);
                    if (luck < 1)
                    {
                        // GameObject pickup = Instantiate(pickupPrefab, new Vector2(startX * 2 + x * width, y * height + 1.71f), Quaternion.identity);
                        GameObject pickup = Instantiate(pickupPrefab, new Vector2(startX * 2 + x * width, y * height + 0.7f), Quaternion.identity);
                    }

                    // Take an object from the object pool
                    //for (int i = 0; i < platformPoolAmount; i++)
                    //{
                    //    if (!platforms[i].activeInHierarchy)
                    //    {
                    //        platforms[i].transform.position = new Vector2(startX * 2 + x * width, y * height);
                    //        platforms[i].SetActive(true);
                    //        break;
                    //    }
                    //}

                    GameObject platform = Instantiate(platformFloorPrefab, new Vector2(startX * 2 + x * width, y * height), Quaternion.identity);
                    platform.transform.parent = gameObject.transform;
                }
                else
                {
                    GameObject platform = Instantiate(platformUnderground, new Vector2(startX * 2 + x * width, y * height), Quaternion.identity);
                    platform.transform.parent = gameObject.transform;
                }
                //}
            }

            if (x == maxX - 1)
            {
                Instantiate(mapEndMarker, new Vector2(startX * 2 + x * width, 0), Quaternion.identity);
                break;
            }
        }

        //Debug.Log("LowestStart: " + lowestStart + " - lastEndFloor: " + lastEndFloor);
        //if (lowestStart > lastEndFloor)
        //    for (int i = lastEndFloor + 1; i < lowestStart - 1; i++)
        //    {
        //        GameObject platform = Instantiate(platformPrefab, new Vector2(startX + width * 2, i * height), Quaternion.identity);
        //    }

        //lastEndFloor = lowestY;

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
                //if (y <= minY - columnHeight + 2)
                //{
                    if (x == minX && y > startHeight)
                    {
                        startHeight = y;
                    }

                    if (x == maxX - 1 && y < highestY) //On last x iteration
                        highestY = y;

                    if (y == minY - columnHeight + 1)
                    {
                    int luckRoof = rand.Next(0, 11);
                    if (luckRoof < 1)
                    {
                        // GameObject pickup = Instantiate(pickupPrefab, new Vector2(startX * 2 + x * width, y * height + 1.71f), Quaternion.identity);
                        GameObject pickup = Instantiate(pickupPrefab, new Vector2(startX * 2 + x * width, y * height + 0.3f), Quaternion.identity);
                        pickup.transform.localScale *= -1;
                    }
                    // Take an object from the object pool
                    for (int i = 0; i < platformPoolAmount; i++)
                        {
                            if (!platforms[i].activeInHierarchy)
                            {
                                platforms[i].transform.position = new Vector2(startX * 2 + x * width, y * height);
                                platforms[i].SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        GameObject platform = Instantiate(platformUnderground, new Vector2(startX * 2 + x * width, y * height), Quaternion.identity);
                        platform.transform.parent = gameObject.transform;
                    }
                //}
            }
        }


        //if (startHeight < lastEndRoof)
        //    for (int i = startHeight + 1; i < lastEndRoof; i++)
        //    {
        //        GameObject platform = Instantiate(platformPrefab, new Vector2(startX + width * 2, i * height), Quaternion.identity);
        //    }

        //lastEndRoof = highestY;
    }
}
