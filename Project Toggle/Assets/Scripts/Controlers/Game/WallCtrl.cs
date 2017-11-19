using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCtrl : MonoBehaviour {

    private float speedIncreaseInterval = 5f;
    private float speedIncrease = 1f;
    private float currentSpeed = 5;
    private float maxSpeed = 10;
    private float timer;
    // Use this for initialization
    void Start()
    {
        timer = speedIncreaseInterval;
        currentSpeed = 0.25f * Time.time + 5f;
    }

    void FixedUpdate ()
    {
        gameObject.transform.Translate((Vector2.left * Time.deltaTime) * currentSpeed); //move right
            if (currentSpeed <= maxSpeed)
            {
                currentSpeed = 0.25f * Time.time + 5f;
            }
            else
            {
                currentSpeed = maxSpeed;
            }

        if (transform.position.x < -20)
        {
            Destroy(gameObject);
        }
    }
}
