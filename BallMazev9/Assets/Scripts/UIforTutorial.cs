using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIforTutorial : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject looseScreen;
    private AudioSource wallHitSFX;
  
    //The outro animation layer
    public GameObject outroLayer;
    //The text shown as instructions
    public GameObject dialogBox;

    private bool gameHasEnded;
    public static bool canMovePlayer;
    private AudioSource[] audS;
    public Image enemyAlertLayer;
    public GameObject welcomeScreen;
    public GameObject backButton;

    void Awake()
    {                
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        audS = GameObject.FindObjectsOfType<AudioSource>();
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("hasPlayed", 0) != 1)
        {
            welcomeScreen.SetActive(true);
            backButton.SetActive(false);
        }
        else
            welcomeScreen.SetActive(false);
        enemyAlertLayer.color = new Color(0, 0, 0, 0f);
        dialogBox.SetActive(true);
        Time.timeScale = 1f;
        for (int i = 0; i < audS.Length; i++)
        {
            audS[i].mute = !ScreenCarryOverData.isSoundOn;
        }
        outroLayer.SetActive(false);
        wallHitSFX = GameObject.Find("WallHitSFX").GetComponent<AudioSource>();
        gameHasEnded = false;
        winScreen.SetActive(false);
        looseScreen.SetActive(false);
    }

    void Update()
    {      


        if (PlayerController.isGameLost && !gameHasEnded)
        {            
            looseScreen.SetActive(true);
            gameHasEnded = true;
        }
        if (EndPointBehaviour.gameWon && !gameHasEnded)
        {                
            winScreen.SetActive(true);
            PlayerPrefs.SetInt("hasPlayed", 1);
            gameHasEnded = true;
        }           

        if (gameHasEnded)
        {
            wallHitSFX.mute = true;
            dialogBox.SetActive(false);
            Vibration.Cancel();
            Time.timeScale = 0f;
        }
    }

    public void restartOnClick()
    {
        winScreen.SetActive(false);               
        loadLevel(SceneManager.GetActiveScene().buildIndex);
       
    }

    public void continueOnClick()
    {
        loadLevel(1);
    }

    public void backButtonOnClick()
    {
        loadLevel(1);
    }

    public void loadLevel(int index)
    {          
        StartCoroutine(loadLevelCoR(index));
    }

    IEnumerator loadLevelCoR(int index)
    {        
        outroLayer.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(index);        
    }

           

}
