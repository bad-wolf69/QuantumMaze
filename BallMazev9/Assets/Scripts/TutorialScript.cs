using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{

    private Text tutorialTxt;
    public string[] dialouge;

    void OnEnable()
    {
        TutorialEventManager.firstTutorial += firstTutEvent;
        TutorialEventManager.secondTutorial += secondTutEvent;
        TutorialEventManager.thirdTutorial += thirdTutEvent;
    }


    void Start()
    {
       
        tutorialTxt = GetComponentInChildren<Text>();
        tutorialTxt.text = "";
        StartCoroutine(typeText(dialouge[0]));
    }

    void firstTutEvent()
    {
        tutorialTxt.text = "";
        StopAllCoroutines();
        StartCoroutine(typeText(dialouge[1]));
    }

    void secondTutEvent()
    {
        tutorialTxt.text = "";
        StopAllCoroutines();
        StartCoroutine(typeText(dialouge[2]));
    }

    void thirdTutEvent()
    {
        tutorialTxt.text = "";
        StopAllCoroutines();
        StartCoroutine(typeText(dialouge[3]));
    }

    IEnumerator typeText(string txt)
    {
        Time.timeScale = 0f;
        UIforTutorial.canMovePlayer = false;
        foreach (char l in txt.ToCharArray())
        {
            if (l == '#')
                tutorialTxt.text += '\n';
            else
                tutorialTxt.text += l;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return new WaitForSecondsRealtime(0.75f);
        Time.timeScale = 1f;
        UIforTutorial.canMovePlayer = true;
    }

    void OnDisable()
    {
        TutorialEventManager.firstTutorial -= firstTutEvent;
        TutorialEventManager.secondTutorial -= secondTutEvent;
        TutorialEventManager.thirdTutorial -= thirdTutEvent;
    }
}
