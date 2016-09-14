using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Transform), typeof(GameObject))]

public class EnemyBehavior : MonoBehaviour
{
    private GameObject Player;
    private bool isPlayerInRange;
    public LayerMask playerLayer;
    public int enemyIndex;
    public Transform aPoint;
    public Transform bPoint;
    public GameObject transformNode;
    private List<Transform> travelPath;
    private bool playerFindToggle;
    private bool toBToggle;
    public Image enemyAlertLayer;

    private int tick;
    private int x = 0;


    //-------The Event for the enemy returning back to base positions-----
    public delegate void OnEnemyReturned(int index);

    public static event OnEnemyReturned completed;

    //----Range  to check the player initially;
    const float initRange = 1.25f;
    //---Range to check the player after finding
    const float foundRange = 1.45f;
    private float range;
    private  Color nonAlertColour = new Color(0.97f, 0f, 0.25f, 0f);

    void Start()
    {        
        enemyAlertLayer.color = new Color(1f, 1f, 1f, 0);
        tick = 0;
        range = initRange;
        travelPath = new List<Transform>();
        Player = GameObject.Find("Player");
        playerFindToggle = false;
        toBToggle = true;
    }

    void FixedUpdate()
    {
        bool isInRangeForAlert = Physics2D.OverlapCircle(transform.position, 3.5f, playerLayer);
        if (isInRangeForAlert)
        {
            float distance = Vector2.Distance(Player.transform.position, transform.position);
            if (distance <= 3.0f)
            {
                float opacity = 1f - (Mathf.Clamp(distance - 0.55f, 0f, 3.0f) / 3.0f);
                enemyAlertLayer.color = new Color(0.97f, 0f, 0.25f, opacity);           
            }
            else
                enemyAlertLayer.color = nonAlertColour;           
        }

        isPlayerInRange = Physics2D.OverlapCircle(transform.position, range, playerLayer);
        if (isPlayerInRange)
        {            
            EnemyFollow();
            range = foundRange;
        }
        else
        {           
            if (!playerFindToggle)
            {
                ToFroMotion();
                range = initRange;
            }
            else
            {
                try{
                    pathTrace();
                }
                catch{
                    Debug.Log("Path trace issue");
                }
            }
        }
    }

    void ToFroMotion()
    {
       
        if (toBToggle)
        {
            transform.position = Vector2.MoveTowards(transform.position, aPoint.position, 1.7f * Time.deltaTime);
            if (transform.position == aPoint.position)
                toBToggle = !toBToggle;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, bPoint.position, 1.7f * Time.deltaTime);
            if (transform.position == bPoint.position)
                toBToggle = !toBToggle;
        }

    }

    void EnemyFollow()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, 1.7f * Time.deltaTime); 
        if (tick == 0)
        {
            GameObject tNode = Instantiate(transformNode, transform.position, Quaternion.identity) as GameObject;
            travelPath.Add(tNode.transform);
        }
        playerFindToggle = true;
        tick++;
        if (tick >= 50)
            tick = 0;
    }

    void pathTrace()
    {        
        transform.position = Vector2.MoveTowards(transform.position, travelPath[travelPath.Count - (x + 1)].position, 1.7f * Time.deltaTime);
        if (transform.position == travelPath[travelPath.Count - (x + 1)].position)
        {
            x++;
        }
        if (x == travelPath.Count - 1)
        {
            x = 0;
            travelPath.Clear();
            playerFindToggle = false;
            if (completed != null)
                completed(enemyIndex);
        }
    }
}
