using UnityEngine;
using UnityEngine.UI;

public class FishingQTE : MonoBehaviour
{
    public RectTransform handle;
    public RectTransform greenZone;
    public RectTransform barArea;
    public GameObject panel; // <- ini buat reference Panel
    public float speed = 300f;
    private bool goingRight = true;
    private bool isFishing = false;

    private float leftLimit;
    private float rightLimit;

    void Start()
    {
        panel.SetActive(false); // <- Panel disembunyikan di awal
    }

    void Update()
    {
        // Tunggu tekan tombol 'E' untuk mulai
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartFishing();
        }

        if (!isFishing) return;

        MoveHandle();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
    }

    void MoveHandle()
    {
        float move = speed * Time.deltaTime;

        if (goingRight)
        {
            handle.anchoredPosition += new Vector2(move, 0);
            if (handle.anchoredPosition.x >= rightLimit)
                goingRight = false;
        }
        else
        {
            handle.anchoredPosition -= new Vector2(move, 0);
            if (handle.anchoredPosition.x <= leftLimit)
                goingRight = true;
        }
    }

    void CheckSuccess()
    {
        float handleCenter = handle.anchoredPosition.x;
        float greenLeft = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float greenRight = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        if (handleCenter >= greenLeft && handleCenter <= greenRight)
        {
            Debug.Log("Sukses nangkap ikannya! 🎣");
        }
        else
        {
            Debug.Log("Gagal... Ikan kabur! 🐟");
        }

        isFishing = false;
        panel.SetActive(false); // <- Hide panel setelah selesai
    }

    public void StartFishing()
    {
        isFishing = true;
        panel.SetActive(true); // <- Munculin panel saat mulai
        handle.anchoredPosition = new Vector2(-barArea.rect.width / 2f + handle.rect.width / 2f, handle.anchoredPosition.y);

        // Hitung ulang batas kiri-kanan
        leftLimit = -barArea.rect.width / 2f + handle.rect.width / 2f;
        rightLimit = barArea.rect.width / 2f - handle.rect.width / 2f;
    }
}
