using UnityEngine;

public class AudioManager : MonoBehaviour
{

    void Awake()
    {        
        if (FindObjectsOfType(typeof(AudioManager)).Length > 1)
        {
            DestroyImmediate(gameObject);
        }
        else
            GameObject.DontDestroyOnLoad(gameObject);
    }
}
