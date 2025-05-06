using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FishingQTEController : MonoBehaviour
{
    [Header("UI & Audio")]
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public GameObject panel;
    public RectTransform handle;
    public RectTransform greenZone;
    public RectTransform barArea;

    [Header("Animasi & Audio")]
    public Animator playerAnimator;
    public AudioSource waterSound;
    public AudioSource successSFX;
    public AudioSource endQTESFX;

    [Header("Gameplay")]
    public float timeLimit = 10f;
    public float speed = 400f;
    public Vector2 greenZoneDefaultSize = new Vector2(350f, 500f);

    private float currentTime;
    private bool timerRunning = false;
    private bool isQTEActive = false;
    private bool movingRight = true;
    private bool canThrow = true;

    private int comboCount = 0;
    private int lastScoreAdded = 0;
    private int score = 0;
    private int baseScore = 10;

    public bool isRunning { get; private set; }

    void Start()
    {
        panel.SetActive(false);
        playerAnimator.Play("Idle"); // ✅ Mulai dengan animasi Idle
    }

    void Update()
    {
        if (!isQTEActive && Input.GetKeyDown(KeyCode.Space) && canThrow)
        {
            canThrow = false;
            playerAnimator.SetTrigger("LemparTrigger");
            StartCoroutine(WaitAndStartQTE());
        }

        if (!isQTEActive) return;

        MoveHandle();

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckResult();
        }

        if (timerRunning)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                timerRunning = false;
                timeText.text = "Waktu: 0";
                EndQTE();
            }
            else
            {
                timeText.text = $"Waktu: {Mathf.CeilToInt(currentTime)}";
            }
        }
    }

    IEnumerator WaitAndStartQTE()
    {
        yield return new WaitForSeconds(2f); // ⏱️ tunggu animasi lempar

        waterSound.Play(); // 🔊 suara cipratan

        StartQTE(); // 🎮 mulai QTE

        while (isRunning)
        {
            yield return null;
        }

        canThrow = true; // bisa lempar lagi
    }

    public void StartQTE()
    {
        if (isQTEActive) return;

        panel.SetActive(true);
        isQTEActive = true;
        isRunning = true;
        comboCount = 0;

        currentTime = timeLimit;
        timerRunning = true;
        timeText.gameObject.SetActive(true);

        ResetHandlePosition();
        MoveGreenZone();

        playerAnimator.SetTrigger("UsahaTrigger");
    }

    void MoveHandle()
    {
        float barHalfWidth = barArea.rect.width * 0.5f;
        float handleHalfWidth = handle.rect.width * 0.5f;

        float minX = -barHalfWidth + handleHalfWidth;
        float maxX = barHalfWidth - handleHalfWidth;

        float direction = movingRight ? 1f : -1f;
        handle.anchoredPosition += new Vector2(direction * speed * Time.deltaTime, 0f);

        float currentX = handle.anchoredPosition.x;

        if (currentX >= maxX)
        {
            handle.anchoredPosition = new Vector2(maxX, handle.anchoredPosition.y);
            movingRight = false;
        }
        else if (currentX <= minX)
        {
            handle.anchoredPosition = new Vector2(minX, handle.anchoredPosition.y);
            movingRight = true;
        }
    }

    void CheckResult()
    {
        float handleLeft = handle.anchoredPosition.x - handle.rect.width / 2;
        float handleRight = handle.anchoredPosition.x + handle.rect.width / 2;

        float greenLeft = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float greenRight = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        if (handleLeft >= greenLeft && handleRight <= greenRight)
        {
            comboCount++;
            speed += 50f;
            successSFX.Play();

            if (comboCount > 10)
            {
                EndQTE();
                return;
            }

            int bonus = baseScore + ((comboCount - 1) * 5);
            score += bonus;
            lastScoreAdded = bonus;

            Debug.Log($"✅ Sukses! Combo: {comboCount}, +{bonus} poin. Total: {score}");

            MoveGreenZone();
        }
        else
        {
            Debug.Log($"❌ Gagal! Kombo di-reset. Skor dikurangi {lastScoreAdded} poin.");
            score -= lastScoreAdded;
            comboCount = 0;
            lastScoreAdded = 0;
            EndQTE();
        }

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }

    void ResetHandlePosition()
    {
        float startX = -barArea.rect.width / 2 + handle.rect.width / 2;
        handle.anchoredPosition = new Vector2(startX, handle.anchoredPosition.y);
        movingRight = true;
    }

    void MoveGreenZone()
    {
        greenZone.sizeDelta = greenZoneDefaultSize;

        float barHalfWidth = barArea.rect.width / 2f;
        float greenHalfWidth = greenZone.rect.width / 2f;

        float minX = -barHalfWidth + greenHalfWidth;
        float maxX = barHalfWidth - greenHalfWidth;

        float randomX = Random.Range(minX, maxX);

        greenZone.anchoredPosition = new Vector2(randomX, greenZone.anchoredPosition.y);
    }

    public void EndQTE()
    {
        playerAnimator.SetTrigger("Nariktrigger");
        isQTEActive = false;
        isRunning = false;
        panel.SetActive(false);
        timerRunning = false;
        timeText.gameObject.SetActive(false);

        endQTESFX.Play();
    }
}
