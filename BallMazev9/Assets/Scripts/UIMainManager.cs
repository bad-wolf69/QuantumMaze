using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIMainManager : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject winScreenSub;
    public GameObject looseScreen;
    public Image scoreBar;
    public Text timerTxt;
    public Text timeTakenTxt;
    public Text countDownTxtVal;
    private AudioSource wallHitSFX;
    public Button pauseButton;
    public Button backButton;
    public Sprite[] pause_resumeSprites;

    public float timeForLevelFinishBonus;
    public GameObject pauseScreen;
    public GameObject layerBGforTimeCount;
    public GameObject outroLayer;
    public Button[] buttonsToBedisabled;
    public GameObject levelNumText;
    public Image enemyAlertLayer;

    private string timerString;
    private float timeSinceLevelLoad;
    private float secondHolder;
    private bool gameHasEnded;
    public static bool canMovePlayer;
    private bool isPaused;
    private AudioSource[] audS;

    void Awake()
    {        
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        StopAllCoroutines();
        timeForLevelFinishBonus = ScreenCarryOverData.S_timeForBonus[SceneManager.GetActiveScene().buildIndex - 2];
        audS = GameObject.FindObjectsOfType<AudioSource>();
        levelNumText.GetComponent<Text>().text = "CIRCUIT- " + (SceneManager.GetActiveScene().buildIndex - 1);
    }

    void Start()
    {
        enemyAlertLayer.color = new Color(0, 0, 0, 0f);
        for (int i = 0; i < audS.Length; i++)
        {
            audS[i].mute = !ScreenCarryOverData.isSoundOn;
        }
        outroLayer.SetActive(false);
        layerBGforTimeCount.SetActive(true);
        pauseButton.enabled = true;
        levelNumText.SetActive(true);
        StartCoroutine(countDownTxt());
        backButton.enabled = false;
        wallHitSFX = GameObject.Find("WallHitSFX").GetComponent<AudioSource>();
        gameHasEnded = false;
        timeSinceLevelLoad = 0;
        winScreen.SetActive(false);
        winScreenSub.SetActive(false);
        looseScreen.SetActive(false);
        pauseScreen.SetActive(false);
       
    }

    void Update()
    {      
        timeSinceLevelLoad += Time.deltaTime;
        secondHolder += Time.deltaTime;
        timerString = "" + timeSinceLevelLoad.ToString("F1") + " s";
        timerTxt.text = timerString;
       

        if (isPaused)
            Vibration.Cancel();
      
        if (PlayerController.isGameLost && !gameHasEnded)
        {            
            looseScreen.SetActive(true);
            gameHasEnded = true;
        }
        if (EndPointBehaviour.gameWon && !gameHasEnded)
        {                
            formattedTimeString();
            winScreen.SetActive(true);
            winScreenSub.SetActive(true);
            float tempF = Mathf.Clamp01(((PlayerController.health / 100f) + (timeForLevelFinishBonus - timeSinceLevelLoad) / timeForLevelFinishBonus) / 2f);
            float temp = Mathf.Ceil(tempF * 10f) / 10f;
            StartCoroutine("scoreBarTrans", temp);
            if (PlayerPrefs.GetInt("levelcompleted", 0) < SceneManager.GetActiveScene().buildIndex - 1)
                PlayerPrefs.SetInt("levelcompleted", SceneManager.GetActiveScene().buildIndex - 1);
            gameHasEnded = true;
        }           

        if (gameHasEnded)
        {
            backButton.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            backButton.enabled = true;
            wallHitSFX.mute = true;
            Vibration.Cancel();
            Time.timeScale = 0f;
        }
    }


    void formattedTimeString()
    {       

        int minutes;
        float sec = (secondHolder) % 60;
        minutes = ((int)secondHolder) / 60;
        timeTakenTxt.text = "" + minutes + "m " + sec.ToString("F1") + "s";
    }

    public void restartOnClick()
    {
        timeTakenTxt.gameObject.SetActive(true);
        winScreen.SetActive(false);
        Time.timeScale = 1f;       
        loadLevel(SceneManager.GetActiveScene().buildIndex);

    }

    public void continueOnClick()
    {
        int index = 0;
        if (SceneManager.sceneCountInBuildSettings - 1 > (SceneManager.GetActiveScene().buildIndex + 1))
            index = SceneManager.GetActiveScene().buildIndex + 1;
        else
            index = 1;
        loadLevel(index);
    }

    public void backButtonOnClick()
    {
        Time.timeScale = 1f;
        loadLevel(1);
    }

    public IEnumerator scoreBarTrans(float val)
    {
        float k = 0f;
        yield return new WaitForSecondsRealtime(1f);
        while (k <= val + 0.1f)
        {
            scoreBar.fillAmount = k;
            k += 0.1f;
            yield return null;
        }
    }

    public IEnumerator countDownTxt()
    {
        pauseScreen.SetActive(false);
        int x = 3;
        pauseButton.interactable = false;
        canMovePlayer = false;
        Time.timeScale = 0f;
        Vibration.Cancel();
        while (x >= 1)
        {
            countDownTxtVal.text = "" + x;
            x--;
            yield return new WaitForSecondsRealtime(1f);
        }
        if (layerBGforTimeCount.activeSelf)
            layerBGforTimeCount.SetActive(false);
        Time.timeScale = 1f;
        countDownTxtVal.enabled = false;
        levelNumText.SetActive(false);
        canMovePlayer = true;
        pauseButton.interactable = true;           
        wallHitSFX.mute = false;
    }

    public void pauseButtonOnClick()
    {
        if (isPaused == false)
        {
            Time.timeScale = 0f;
            canMovePlayer = false;
            isPaused = true;
            pauseButton.image.sprite = pause_resumeSprites[1];
            pauseScreen.SetActive(true);
            wallHitSFX.mute = true;
        }
        else
        {
            pauseButton.image.sprite = pause_resumeSprites[0];
            countDownTxtVal.enabled = true;
            StartCoroutine(countDownTxt());
            isPaused = false;
        }            
    }

    public void loadLevel(int index)
    {          
        StartCoroutine(loadLevelCoR(index));
    }

    IEnumerator loadLevelCoR(int index)
    {
        disableButtons();
        outroLayer.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(index);        
    }

    void disableButtons()
    {
        for (int i = 0; i < buttonsToBedisabled.Length; i++)
        {
            buttonsToBedisabled[i].enabled = false;
        }
    }
   
}
