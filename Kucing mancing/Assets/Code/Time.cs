using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 180f;
    public TMP_Text timerText;

    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;

    public int currentScore = 0; // total skor permainan ini

    private float currentTime;
    private bool gameRunning = true;

    void Start()
    {
        currentTime = totalTime;
        gameOverPanel.SetActive(false);
        UpdateTimerText();
    }

    void Update()
    {
        if (!gameRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            gameRunning = false;
            UpdateTimerText();
            EndGame();
        }
        else
        {
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"Waktu: {minutes:00}:{seconds:00}";
    }

    void EndGame()
    {
        Debug.Log("⏰ Waktu habis! Permainan selesai.");

        gameOverPanel.SetActive(true);

        // High Score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        finalScoreText.text = $"Skor Akhir: {currentScore}";
        highScoreText.text = $"High Score: {highScore}";
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }
}
