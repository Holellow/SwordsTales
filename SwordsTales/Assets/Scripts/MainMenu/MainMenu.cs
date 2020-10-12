using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace MainMenu
{
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
}
