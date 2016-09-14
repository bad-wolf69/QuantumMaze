using UnityEngine;

public class ScreenCarryOverData : MonoBehaviour
{

    public static float sensitivity = 1f;
    public static bool isVibrationOn = true;
    public float[] timeForBonus;
    public static float[] S_timeForBonus;
    public static float yAccOffset = 0f;
    public static bool isSoundOn = true;
    //---------To Hold The Time bonus values for all levels
    void Awake()
    {
        S_timeForBonus = new float[timeForBonus.Length];
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < timeForBonus.Length; i++)
        {
            S_timeForBonus[i] = timeForBonus[i];
        }
        if (FindObjectsOfType(typeof(ScreenCarryOverData)).Length > 1)
        {
            DestroyImmediate(gameObject);
        }
    }

   
}
