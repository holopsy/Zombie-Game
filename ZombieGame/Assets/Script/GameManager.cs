using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    [Header("Zombies")]
    public GameObject selectedZombie;
    public GameObject[] zombies;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject losePanel; // panel with "You lose" + Restart button

    [Header("Lose Settings")]
    public float fallY = -2f; // if zombie goes below this Y -> considered fallen

    public bool IsGameOver { get; private set; }

    int score = 0;
    float timeSinceStart = 0f;

    void Start()
    {
        // Your existing logic
        if (zombies != null && zombies.Length > 2 && zombies[2] != null)
        {
            Debug.Log(zombies[2].name);
            selectedZombie = zombies[2];
            Debug.Log("selected " + selectedZombie.name);
        }

        ResetUI();
    }

    void Update()
    {
        if (IsGameOver) return;

        // TIMER
        timeSinceStart += Time.deltaTime;
        if (timerText != null)
            timerText.text = $"Time: {timeSinceStart:0.00}";

        // LOSE CHECK
        if (AllZombiesFallen())
        {
            Lose();
        }
    }

    // Called by Collectible when a zombie touches it
    public void AddScore(int amount)
    {
        if (IsGameOver) return;

        score += amount;
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    void Lose()
    {
        IsGameOver = true;

        // Stop timer automatically because Update returns early now
        if (losePanel != null)
            losePanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    bool AllZombiesFallen()
    {
        if (zombies == null || zombies.Length == 0) return false;

        // If ANY zombie is still above fallY -> not lose
        for (int i = 0; i < zombies.Length; i++)
        {
            if (zombies[i] == null) continue;

            if (zombies[i].transform.position.y > fallY)
                return false;
        }

        return true;
    }

    void ResetUI()
    {
        IsGameOver = false;
        score = 0;
        timeSinceStart = 0f;

        if (scoreText != null) scoreText.text = "Score: 0";
        if (timerText != null) timerText.text = "Time: 0.00";
        if (losePanel != null) losePanel.SetActive(false);
    }
}