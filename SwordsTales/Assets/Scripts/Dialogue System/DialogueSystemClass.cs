using System.Collections;
using System.Net.Mime;
using Sound;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Dialogue_System
{
    public class DialogueSystemClass : MonoBehaviour
    {
        // public bool finished { get; private set; }
        
        protected IEnumerator WriteText(string input, Text textHolder,float delay,Color textColor, Font textFont, AudioClip sound)
        {
            textHolder.color = textColor;
            textHolder.font = textFont;
            
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                Debug.Log(SoundManager.Instance);
                SoundManager.Instance.PlaySound(sound);
                yield return  new WaitForSeconds(delay);
            }
           // finished = true;
        }
    }
}
