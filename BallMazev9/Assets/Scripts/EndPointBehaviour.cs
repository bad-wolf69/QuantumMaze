using UnityEngine;
using UnityEngine.UI;

public class EndPointBehaviour : MonoBehaviour
{
    private float stayTime = 1.0f;
    public GameObject timerTxt;
    public GameObject[] lightningObjs;
    public static bool gameWon;
    private static bool inTrigger;

    void Start()
    {
        gameWon = false;
        stayTime = 1.0f;
        inTrigger = false;
        for (int i = 0; i < lightningObjs.Length; i++)
            lightningObjs[i].SetActive(false);       
    }

    void Update()
    {
        if (inTrigger)
        {
            stayTime -= Time.deltaTime;
            if (stayTime <= 0 && !gameWon)
            {
                gameWon = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < lightningObjs.Length; i++)
                lightningObjs[i].SetActive(true);
            if (ScreenCarryOverData.isVibrationOn)
                Vibration.Vibrate(1300);
            inTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            stayTime = 1.0f;
            inTrigger = false;
        }
    }

}
