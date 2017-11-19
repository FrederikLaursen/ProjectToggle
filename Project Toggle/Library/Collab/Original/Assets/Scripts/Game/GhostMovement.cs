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
    public Transform playerPrefab;

    Vector3 lDir = new Vector3(Mathf.Cos(0.45f), Mathf.Sin(0.45f), 0);
    Vector3 RDir = new Vector3(Mathf.Cos(-0.45f), Mathf.Sin(-0.45f), 0);
    Vector2 rightCol;
    Vector2 upCol;
    Vector2 downCol;
    float xOffset;
    float yOffset;

    // Use this for initialization
    void Start()
    {
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
                else
                {
                    Debug.Log("No GHOST playthrough");
                    gameObject.SetActive(false);
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
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
            gameObject.SetActive(false);
    }


    // Update is called once per frame
    //void Update () {

    //   }

    void Update()
    {
        MoveGhost();
        for (int i = 0; i < times.Count; i++)
        {
            if (Time.timeSinceLevelLoad >= times[i].time - Time.deltaTime * 4 && !times[i].hasBeenReached)
            {
                //Debug.Log(Time.timeSinceLevelLoad - (times[i].time - Time.deltaTime * 4));
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

    private void MoveGhost()
    {
        //rightCol = new Vector2(transform.position.x + xOffset, transform.position.y);
        //upCol = new Vector2(transform.position.x, transform.position.y + yOffset + 0.2f);
        //downCol = new Vector2(transform.position.x, transform.position.y - yOffset - 0.2f);

        //if (Physics2D.Raycast(rightCol, (Vector2.right), 0f) && Physics2D.Raycast(upCol, transform.right + lDir * 3f, 1f) && Physics2D.Raycast(downCol, transform.right + RDir * 3f, 1f))
        //{
            //gameObject.transform.Translate((Vector2.left * Time.deltaTime) * 20f);
        //}
    }

    private void SwitchGravity()
    {
        //gravityOn = !gravityOn;
        GetComponent<Rigidbody2D>().gravityScale *= -1;

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
