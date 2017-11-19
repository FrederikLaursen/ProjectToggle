using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class GhostMovement : MonoBehaviour {    

    public List<TimeStamp> times = new List<TimeStamp>();
    private bool gravityOn;
    string playthroughXml;
    XDocument playthroughXmlDocument;
    List<XElement> timeStamps = new List<XElement>();
    float xOffset;
    float yOffset;
    SpriteRenderer playerSpriteRend;

    // Use this for initialization
    void Start()
    {
        playerSpriteRend = GetComponent<SpriteRenderer>();
        if (DataHolder.Instance != null)
        {
            if (!DataHolder.Instance.Offline)
            {
                Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

                gravityOn = true;
                GetComponent<Rigidbody2D>().gravityScale = 4;

                xOffset = GetComponent<BoxCollider2D>().bounds.extents.x + 0.2f;
                yOffset = GetComponent<BoxCollider2D>().bounds.extents.y;


                if (GameObject.Find("DatabasserHandler") != null && DataHolder.Instance.Playthrough != null)
                {
                    playthroughXml = DataHolder.Instance.Playthrough;
                }


                if (!string.IsNullOrEmpty(playthroughXml))
                {
                    playthroughXmlDocument = XDocument.Parse(playthroughXml);

                    timeStamps = playthroughXmlDocument.Root.Elements().ToList();

                    foreach (XElement ts in timeStamps)
                    {
                        float time = float.Parse(ts.Value.Split('@')[0]);
                        string unformattedPos = ts.Value.Split('@')[1];
                        float xPos = float.Parse(unformattedPos.Split('|')[0]);
                        float yPos = float.Parse(unformattedPos.Split('|')[1]);
                        Vector2 pos = new Vector2(xPos, yPos);
                        times.Add(new TimeStamp(time, pos));
                    }
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
            gameObject.SetActive(false);
    }

    void Update()
    {
        for (int i = 0; i < times.Count; i++)
        {
            if (Time.timeSinceLevelLoad >= times[i].time - Time.deltaTime * 4 && !times[i].hasBeenReached)
            {
                SwitchGravity();
                times[i].hasBeenReached = true;

                if(Vector2.Distance(transform.position, times[i].position) > 3)
                {
                    Debug.Log(Vector2.Distance(transform.position, times[i].position));
                    transform.position = times[i].position;
                }
            }
        }
    }

    private void SwitchGravity()
    {
        GetComponent<Rigidbody2D>().gravityScale *= -1;
        playerSpriteRend.flipY = !playerSpriteRend.flipY;
    }
}

public class TimeStamp
{
    public float time { get; set; }
    public Vector2 position { get; set; }
    public bool hasBeenReached { get; set; }

    public TimeStamp(float time, Vector2 pos)
    {
        this.time = time;
        position = pos;
        hasBeenReached = false;
    }
}
