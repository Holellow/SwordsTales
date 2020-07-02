using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
   public AudioMixer AudioMixer;
   public void PlayGame()
   {
      SceneManager.LoadScene(1);
   }

   public void QuitGame()
   {
      Application.Quit();
      UnityEditor.EditorApplication.isPlaying = false;
   }

   public void setValue(float volume)
   {
      Debug.Log(volume);
      AudioMixer.SetFloat("Volume",volume);
   }

   public void SetFullScreen(bool isFullScreen)
   {
      Screen.fullScreen = isFullScreen;
   }
}
