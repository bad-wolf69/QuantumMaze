using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject settingsPanel;
    public Slider sensitivitySlider;
    public Toggle vibrationToggle;
    public  Toggle soundToggle;
    public Button[] set_lvl_Buttons;
    public GameObject outroLayer;
    public GameObject calibrateMessage;

    private int levelCompleted;

    void Awake()
    {        
        Screen.orientation = ScreenOrientation.Portrait;
        Time.timeScale = 1f;
        levelCompleted = PlayerPrefs.GetInt("levelcompleted", 0) + 2;
    }

    void Start()
    {
        
        outroLayer.SetActive(false);
        levelPanel.SetActive(false);
        settingsPanel.SetActive(false);
        calibrateMessage.SetActive(false);
        sensitivitySlider.value = ScreenCarryOverData.sensitivity;
        vibrationToggle.isOn = ScreenCarryOverData.isVibrationOn;
        soundToggle.isOn = ScreenCarryOverData.isSoundOn;
       
    }

    public void startOnClick()
    {
        if (levelCompleted >= 16)
            loadLevel(16);
        else
            loadLevel(levelCompleted);
    }

    public void levelMenuButtonOnClick()
    {        
        levelPanel.SetActive(true);
        levelSelectDisable();
    }

    public void levelSelectMenuBackButtonOnClick()
    {
        levelPanel.SetActive(false);
    }

    public void levelSelectOnClick(int levelIndex)
    {
        loadLevel(levelIndex);
    }

    public void setingsButtonOnClick()
    {
        
        settingsPanel.SetActive(true);
    }

    public void sliderSensitivityOnValueChange()
    {
        ScreenCarryOverData.sensitivity = sensitivitySlider.value;
    }

    public void settingsPanelBackButtonOnClick()
    {
        settingsPanel.SetActive(false);
    }

    public void vibrationToggleOnValueChange()
    {
        ScreenCarryOverData.isVibrationOn = vibrationToggle.isOn;
    }

    public void soundToggleOnValueChange()
    {
        ScreenCarryOverData.isSoundOn = soundToggle.isOn;
        GameObject.FindObjectOfType<AudioSource>().mute = !soundToggle.isOn;
    }

    public void calibrateButtonOnClick()
    {
        ScreenCarryOverData.yAccOffset = Mathf.Clamp(-Input.acceleration.y, -0.4f, 0.4f);
        calibratedMessage();
    }

    public void loadTutorial()
    {
        loadLevel(17);
    }

    public void loadLevel(int index)
    {          
        StartCoroutine(loadLevelCoR(index));
    }

    IEnumerator loadLevelCoR(int index = 2)
    {        
        outroLayer.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        disableButtons();
        SceneManager.LoadScene(index);
    }

    void disableButtons()
    {       
        for (int i = 0; i < set_lvl_Buttons.Length; i++)
            set_lvl_Buttons[i].enabled = false;     

    }
    void levelSelectDisable(){
        for (int i = levelCompleted-1; i <= 14; i++)
        {
            set_lvl_Buttons[i].image.color = new Color(0f, 0f, 0f);
            set_lvl_Buttons[i].interactable = false;
        }
    }

    void calibratedMessage()
    {
        StartCoroutine(calibrationCoRoutine());
    }

    IEnumerator calibrationCoRoutine()
    {
        calibrateMessage.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        calibrateMessage.SetActive(false);
    }
        
}
