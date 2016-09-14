using UnityEngine;
using System.Collections;

public class ObjectDestroyManager : MonoBehaviour
{
    //--------- This destroys the transforms that were instantiated

    public int objIndex;

    void OnEnable()
    {
        EnemyBehavior.completed += objDestroy;
    }

    void OnDisable()
    {
        EnemyBehavior.completed -= objDestroy;
    }

    void objDestroy(int index)
    {
        if (name.Contains("(Clone)") && objIndex == index)
            Destroy(gameObject, 2f);
    }
}
