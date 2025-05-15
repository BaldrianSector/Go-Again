using UnityEngine;
using TMPro;

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

    void Update()
    {
        timeInLevel += Time.deltaTime;
        UpdateTimeUI();

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
            livesText.text = $"Lives: {livesLeft}/9";
    }

    private void UpdateTimeUI()
    {
        if (timeText != null)
            timeText.text = $"Time: {timeInLevel:F1}s";
    }

    public void TriggerWin()
    {
        if (winText != null) winText.gameObject.SetActive(true);
    }

    private void TriggerLose()
    {
        if (loseText != null) loseText.gameObject.SetActive(true);
    }

    private void HideEndTexts()
    {
        if (winText != null) winText.gameObject.SetActive(false);
        if (loseText != null) loseText.gameObject.SetActive(false);
    }

    Transform GetPlayer() => GameObject.FindWithTag("Player")?.transform;
    bool PlayerExists() => GameObject.FindWithTag("Player") != null;
}
