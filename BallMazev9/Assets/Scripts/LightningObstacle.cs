using UnityEngine;

public class LightningObstacle : MonoBehaviour
{
    private  bool isEnabled = true;
    public SpriteRenderer[] objs;
    public static float damageCaused = 0f;

    void Start()
    {        
        InvokeRepeating("LightningObstacleTimed", 2f, 2f);
    }

    void LightningObstacleTimed()
    {
        if (isEnabled)
        {
            damageCaused = 150f;
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].color = new Color(1f, 1f, 0f, 1f);
            }
            isEnabled = false;

        }
        else
        {
            damageCaused = 0f;
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].color = new Color(1f, 1f, 1f, 1f);
            }
            isEnabled = true;
        }
    }

}
