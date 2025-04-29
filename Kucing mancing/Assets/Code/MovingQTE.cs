using UnityEngine;
using UnityEngine.UI;

public class FishingQTE : MonoBehaviour
{
    public GameObject panel;
    public RectTransform handle;
    public RectTransform greenZone;
    public RectTransform barArea;

    public int score = 0;
    public int scorePerSuccess = 10;
    public Text scoreText;

    public float speed = 400f;

    private bool isQTEActive = false;
    private bool movingRight = true;

    private int successCount = 0;
    private int requiredSuccess = 3;

    private int comboCount = 0;
    public float baseSpeed = 600f;
    public float speedIncreasePerCombo = 150f;

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        // Tekan E untuk memulai QTE (jika tidak sedang aktif)
        if (Input.GetKeyDown(KeyCode.E) && !isQTEActive)
        {
            StartQTE();
        }

        if (!isQTEActive)
            return;

        MoveHandle();

        if (Input.GetKeyDown(KeyCode.W))
        {
            CheckResult();
        }
    }

    void StartQTE()
    {
        successCount = 0;
        isQTEActive = true;
        panel.SetActive(true);
        ResetHandlePosition();
    }

    void MoveHandle()
    {
        float direction = movingRight ? 1 : -1;
        handle.anchoredPosition += new Vector2(direction * speed * Time.deltaTime, 0);

        // Batas kiri dan kanan area
        float barWidth = barArea.rect.width;
        float halfHandle = handle.rect.width / 2;

        if (handle.anchoredPosition.x > barWidth / 2 - halfHandle)
        {
            movingRight = false;
        }
        else if (handle.anchoredPosition.x < -barWidth / 2 + halfHandle)
        {
            movingRight = true;
        }
    }

    void CheckResult()
    {
        Rect handleRect = new Rect(
            handle.anchoredPosition.x - handle.rect.width / 2,
            handle.anchoredPosition.y - handle.rect.height / 2,
            handle.rect.width,
            handle.rect.height
        );

        Rect greenRect = new Rect(
            greenZone.anchoredPosition.x - greenZone.rect.width / 2,
            greenZone.anchoredPosition.y - greenZone.rect.height / 2,
            greenZone.rect.width,
            greenZone.rect.height
        );

        if (handleRect.Overlaps(greenRect))
        {
            successCount++;
            comboCount++;
            speed = baseSpeed + (speedIncreasePerCombo * comboCount);

            Debug.Log($"SUKSES! Combo: {comboCount} Speed: {speed}");

            if (successCount >= requiredSuccess)
            {
                SuccessFishing();
            }
            else
            {
                ResetHandlePosition();
            }
        }
        else
        {
            comboCount = 0;
            speed = baseSpeed;
            Debug.Log("GAGAL! Combo reset.");
            FailFishing();
        }
    }




    void ResetHandlePosition()
    {
        handle.anchoredPosition = new Vector2(0, handle.anchoredPosition.y);
        movingRight = true;
        Debug.Log("Reset handle ke tengah: " + handle.anchoredPosition);
    }

    void SuccessFishing()
    {
        isQTEActive = false;
        panel.SetActive(false);

        // Tambah skor
        score += scorePerSuccess;
        UpdateScoreUI();

        Debug.Log("🎉 MANCING SUKSES! Skor: " + score);

        // Tambahkan animasi, hadiah, atau logika menang di sini
    }

    void FailFishing()
    {
        isQTEActive = false;
        panel.SetActive(false);
        Debug.Log("💥 MANCING GAGAL!");
        // Tambahkan efek gagal atau penalti di sini
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
