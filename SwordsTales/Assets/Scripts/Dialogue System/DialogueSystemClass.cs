using System.Collections;
using Sound;
using UnityEngine;
using UnityEngine.UI;


namespace Dialogue_System
{
    public class DialogueSystemClass : MonoBehaviour
    {
         public bool finished { get; private set; }
        
        protected IEnumerator WriteText(string input, Text textHolder,float delay,Color textColor, Font textFont, AudioClip sound,float delayBetweenLines)
        {
            textHolder.color = textColor;
            textHolder.font = textFont;
            
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                SoundManager.Instance.PlaySound(sound);
                yield return  new WaitForSeconds(delay);
            }
            yield return new WaitUntil(()=>Input.GetMouseButton(0)); 
            finished = true;
        }
    }
}
