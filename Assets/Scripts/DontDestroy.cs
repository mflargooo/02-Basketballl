using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static GameObject instance { get; private set; }
    // Start is called before the first frame update
    public void Awake()
    {
        if (instance != null && instance != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = gameObject;
        }

        DontDestroyOnLoad(gameObject);
    }
}
