/*using System;
using System.Collections;
using UnityEngine;

namespace Dialogue_System
{
    public class DialogueHolder : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(DialogueSequance());
        }

        private IEnumerator DialogueSequance()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }
        }

        private void Deactivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            } 
        }
        
    }
}*/
