using UnityEngine;

public class CanvasSingleton : MonoBehaviour
{
    public static CanvasSingleton instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }
}
