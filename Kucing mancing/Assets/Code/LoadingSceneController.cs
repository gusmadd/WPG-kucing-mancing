using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public static class Loader
{
    public enum Scene
    {
        MainMenu,
        Gameplay,
        Loading
    }

    private static Scene targetScene;

    public static void Load(Scene scene)
    {
        targetScene = scene;
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void OnLoaderCallback()
    {
        SceneManager.LoadSceneAsync(targetScene.ToString());
    }
}

    public class LoadingSceneController : MonoBehaviour
    {
        public Slider progressBar;
        public TMP_Text progressText;

        void Start()
        {
            StartCoroutine(LoadAsyncScene());
        }

        IEnumerator LoadAsyncScene()
        {
            yield return new WaitForSeconds(0.5f); // delay sedikit supaya UI muncul
            AsyncOperation operation = SceneManager.LoadSceneAsync(Loader.Scene.Gameplay.ToString());

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                if (progressBar != null)
                    progressBar.value = progress;

                if (progressText != null)
                    progressText.text = $"Loading {Mathf.RoundToInt(progress * 100)}%";

                yield return null;
            }
        }
    }

