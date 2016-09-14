using UnityEngine;
using System.Collections;

public class TutorialEventManager : MonoBehaviour
{
    public delegate void OnNextEvent();

    public static event OnNextEvent firstTutorial;
    public static event OnNextEvent secondTutorial;
    public static event OnNextEvent thirdTutorial;

    public Transform[] positions;

    public static int eventNo = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (eventNo == 0)
                firstTutorial();
            else if (eventNo == 1)
                secondTutorial();
            else if (eventNo == 2)
                thirdTutorial();            
            if (eventNo <= 1)
                transform.position = positions[eventNo].position;
            eventNo++; 

        }
    }

}
