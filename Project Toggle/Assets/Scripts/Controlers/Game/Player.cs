using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<string> playthrough;

    bool landed;
    bool gravityOn;
    bool canSwitch;
    bool scoreSet = false;
    XElement xmlElements;
    SpriteRenderer playerSpriteRend;
    Vector3 lDir = new Vector3(Mathf.Cos(0.45f), Mathf.Sin(0.45f), 0);
    Vector3 RDir = new Vector3(Mathf.Cos(-0.45f), Mathf.Sin(-0.45f), 0);
    Vector2 rightCol;
    Vector2 upCol;
    Vector2 downCol;
    float xOffset;
    float yOffset;
    private int chargeCount = 0;
    [SerializeField]
    Text chargeCounter;
    // Use this for initialization
    void Start()
    {
        gravityOn = true;
        canSwitch = false;
        playthrough = new List<string>();
        GetComponent<Rigidbody2D>().gravityScale = 4;
        playerSpriteRend = GetComponent<SpriteRenderer>();
        xOffset = GetComponent<BoxCollider2D>().bounds.extents.x + 0.2f;
        yOffset = GetComponent<BoxCollider2D>().bounds.extents.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (isOutsideMap())
        {
            if (DataHolder.Instance != null && !scoreSet)
            {
                if (!DataHolder.Instance.Offline)
                {
                    string id = DataHolder.Instance.ID;
                    string mapSeed = GameObject.FindGameObjectWithTag("mapHandler").GetComponent<MapGenerator>().staticMap;
                    string playthroughString = xmlElements.ToString();
                    DBHandler.Instance.StartCoroutine(DBHandler.Instance.SetScore(id, (int)Time.timeSinceLevelLoad, mapSeed, playthroughString));
                    scoreSet = true;
                    AudioCtrl.vibrate();
                }
                Time.timeScale = 0;
            }
            GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>().GameOver = true;
        }

        rightCol = new Vector2(transform.position.x + xOffset, transform.position.y - 0.1f);
        upCol = new Vector2(transform.position.x, transform.position.y + yOffset + 0.2f);
        downCol = new Vector2(transform.position.x, transform.position.y - yOffset - 0.2f);


        if (Physics2D.Raycast(rightCol, Vector2.right, 0f) && Physics2D.Raycast(upCol, transform.right + lDir * 3f, 1f) && Physics2D.Raycast(downCol, transform.right + RDir * 3f, 1f))
        {
            gameObject.transform.Translate((Vector2.left * Time.deltaTime) * 20f);
        }

        if (Physics2D.Raycast(upCol, Vector2.up, 0.1f))
        {
            if (!landed && !gravityOn)
            {
                AudioCtrl.playSFX(gameObject.GetComponent<AudioSource>(), gameObject.GetComponent<AudioSource>().clip, 0.3f);
                landed = true;
                canSwitch = true;
            }
        }
        if (Physics2D.Raycast(downCol, Vector2.down, 0.1f))
        {
            if (!landed && gravityOn)
            {
                AudioCtrl.playSFX(gameObject.GetComponent<AudioSource>(), gameObject.GetComponent<AudioSource>().clip, 0.3f);
                landed = true;
                canSwitch = true;
            }
        }

        if (Input.GetKeyDown("space") && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (chargeCount > 0 || canSwitch)
            {
                if (!canSwitch)
                {
                    if (chargeCount > 0)
                    {
                        chargeCount--;
                        chargeCounter.text = chargeCount.ToString();
                    }
                }
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().gravityScale *= -1;
                gravityOn = !gravityOn;
                playerSpriteRend.flipY = !playerSpriteRend.flipY;

                playthrough.Add(Time.timeSinceLevelLoad.ToString("0.00") + "@" + transform.position.x + "|" + transform.position.y);
                xmlElements = new XElement("times", playthrough.Select(i => new XElement("ts", i)));

                landed = false;
                canSwitch = false;
            }
        }
    }

    private bool isOutsideMap()
    {
        return transform.position.x <= -16;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pickup")
        {
            chargeCount++;
            chargeCounter.text = chargeCount.ToString();
        }
    }
}
