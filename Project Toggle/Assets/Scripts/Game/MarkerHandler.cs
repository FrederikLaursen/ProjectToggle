using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerHandler : MonoBehaviour
{
    void OnDestroy()
    {
        if (Time.timeScale > 0)
        {
            GameObject mapHandler = GameObject.FindGameObjectWithTag("mapHandler");
            if (mapHandler != null)
                mapHandler.GetComponent<MapGenerator>().RegenerateMap();
        }
    }
}

