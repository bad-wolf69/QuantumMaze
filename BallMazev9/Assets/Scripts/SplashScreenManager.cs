using UnityEngine;
using UnityEngine.SceneManagement;


public class SplashScreenManager : MonoBehaviour
{
    public float time;
    private int hasAlreadyPlayed;
    //for not yet played - 0 else 1
    void Awake()
    {       
        hasAlreadyPlayed = PlayerPrefs.GetInt("hasPlayed", 0);
    }

    void Update()
    {
        if (hasAlreadyPlayed == 1)
            hasPlayed();
        else
            hasntPlayed();        
    }

    void hasPlayed()
    {
        if (time <= 0)
            SceneManager.LoadScene(1);
        time -= Time.deltaTime;
    }

    void hasntPlayed()
    {       
        if (time <= 0)
            SceneManager.LoadScene(17);
        time -= Time.deltaTime;
    }
}
