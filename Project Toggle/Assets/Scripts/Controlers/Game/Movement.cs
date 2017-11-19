using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    private float currentSpeed = 5;
    private float maxSpeed = 10;
    // Use this for initialization
    void Start ()
    {
        currentSpeed = 0.25f * Time.timeSinceLevelLoad + 5f;
    }

    void Update()
    {
        gameObject.transform.Translate((Vector2.left * Time.deltaTime) * currentSpeed);
        if (currentSpeed <= maxSpeed)
        {
            currentSpeed = 0.25f * Time.timeSinceLevelLoad + 5f;
        }
        else
        {
            currentSpeed = maxSpeed;
        }

        if (gameObject.transform.position.x < -16.4)
        {
            Destroy(gameObject);
        }
    }

}
