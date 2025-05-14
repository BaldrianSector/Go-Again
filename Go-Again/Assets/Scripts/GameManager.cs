using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gameplay Stats")]
    public int deathCount = 0;
    public int jumpCount = 0;
    public float distanceTraveled = 0f;
    public float timeInLevel = 0f;

    private Vector3 lastPosition;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            lastPosition = Vector3.zero;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        timeInLevel += Time.deltaTime;

        if (PlayerExists()) // Optional check if player was instantiated
        {
            Vector3 playerPos = GetPlayer().position;
            distanceTraveled += Vector3.Distance(playerPos, lastPosition);
            lastPosition = playerPos;
        }
    }

    public void RegisterJump() => jumpCount++;
    public void RegisterDeath() => deathCount++;

    Transform GetPlayer() => GameObject.FindWithTag("Player")?.transform;
    bool PlayerExists() => GameObject.FindWithTag("Player") != null;
}
