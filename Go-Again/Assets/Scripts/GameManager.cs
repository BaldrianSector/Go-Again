using UnityEngine;
using TMPro;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gameplay Stats")]
    public int deathCount = 0;
    public int jumpCount = 0;
    public float distanceTraveled = 0f;
    public float timeInLevel = 0f;

    [Header("Lives and Time")]
    public int livesLeft = 9;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timeText;

    [Header("Win/Lose UI")]
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    private Vector3 lastPosition;

    private bool isTimerRunning = true;

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
        ResetLives();
        HideEndTexts();
    }

    void Start()
    {
        AudioManager.Instance.Play("backgroundMusic");
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timeInLevel += Time.deltaTime;
            UpdateTimeUI();
        }

        if (PlayerExists())
        {
            Vector3 playerPos = GetPlayer().position;
            distanceTraveled += Vector3.Distance(playerPos, lastPosition);
            lastPosition = playerPos;
        }
    }

    public void RegisterJump() => jumpCount++;

    public void RegisterDeath()
    {
        deathCount++;
        DecreaseLife();
        AudioManager.Instance.Play("meow");
    }

    public void DecreaseLife()
    {
        livesLeft = Mathf.Max(0, livesLeft - 1);
        UpdateLivesUI();

        if (livesLeft == 0)
            TriggerLose();
    }

    public void IncreaseLife()
    {
        livesLeft = Mathf.Min(9, livesLeft + 1);
        UpdateLivesUI();
    }

    public void ResetLives(int value = 9)
    {
        livesLeft = value;
        UpdateLivesUI();
        HideEndTexts();
    }

    public void ResetTime()
    {
        timeInLevel = 0f;
        UpdateTimeUI();
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = $"{livesLeft}/9";
    }

    private void UpdateTimeUI()
    {
        if (timeText == null) return;
        timeText.text = FormatTime(timeInLevel);
    }

    public void PauseAndLogTime()
    {
        isTimerRunning = false;

        Debug.Log($"Level ended.");
        Debug.Log($"Time in level: {FormatTime(timeInLevel)}");
        Debug.Log($"Deaths: {deathCount}");
        Debug.Log($"Jumps: {jumpCount}");
        Debug.Log($"Distance traveled: {distanceTraveled:F2} units");
    }

    private string FormatTime(float time)
    {
        int totalSeconds = Mathf.FloorToInt(time);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        float seconds = time % 60f;

        if (hours > 0)
            return $"{hours:00}:{minutes:00}:{seconds:00.0}";
        else
            return $"{minutes:00}:{seconds:00.0}";
    }

    public void TriggerWin()
    {
        if (winText != null) winText.gameObject.SetActive(true);
        PauseAndLogTime();
        AudioManager.Instance.Play("meow");
        Debug.Log("Player has reached the end flag and triggered TriggerWin()!");
    }

    private void TriggerLose()
    {
        if (loseText != null) loseText.gameObject.SetActive(true);

            // Disable ThirdPersonController and destroy SoftBody object
            Transform player = GetPlayer();
        if (player != null)
        {
            ThirdPersonController controllerScript = player.GetComponent<ThirdPersonController>();
            if (controllerScript != null)
            {
                controllerScript.enabled = false;
            }

            GameObject softBody = GameObject.FindGameObjectWithTag("SoftBody");
            if (softBody != null)
            {
                Destroy(softBody);
            }

            PauseAndLogTime();
            AudioManager.Instance.Play("meow");
            }

    }

    private void HideEndTexts()
    {
        if (winText != null) winText.gameObject.SetActive(false);
        if (loseText != null) loseText.gameObject.SetActive(false);
    }

    Transform GetPlayer() => GameObject.FindWithTag("Player")?.transform;
    bool PlayerExists() => GameObject.FindWithTag("Player") != null;
}
