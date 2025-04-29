using UnityEngine;

public class FishingController : MonoBehaviour
{
    public Transform fishingRod;   // Reference to the fishing rod or line
    public float fishingSpeed = 2f; // Speed of line movement
    public float fishingDepth = -3f; // How deep the line goes
    private Vector3 startPos;
    private bool isFishing = false;

    private void Start()
    {
        startPos = fishingRod.localPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isFishing)
        {
            StartCoroutine(Fish());
        }
    }

    private System.Collections.IEnumerator Fish()
    {
        isFishing = true;
        
        // Lower the rod
        float timer = 0;
        Vector3 targetPos = new Vector3(startPos.x, fishingDepth, startPos.z);
        while (timer < 1f)
        {
            timer += Time.deltaTime * fishingSpeed;
            fishingRod.localPosition = Vector3.Lerp(startPos, targetPos, timer);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f); // Wait underwater (simulate fishing)

        Debug.Log("You caught a fish! 🐟"); // TEMPORARY Feedback

        // Raise the rod
        timer = 0;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fishingSpeed;
            fishingRod.localPosition = Vector3.Lerp(targetPos, startPos, timer);
            yield return null;
        }

        isFishing = false;
    }
}
