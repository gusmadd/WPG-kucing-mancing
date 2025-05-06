using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
       {
        Loader.Load(Loader.Scene.Gameplay);
       }

   public void ExitGame()
       {
           Debug.Log("Keluar dari game...");
           Application.Quit();
       }
}
