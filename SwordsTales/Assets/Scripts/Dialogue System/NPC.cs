using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

namespace Dialogue_System
{
   public class NPC : MonoBehaviour
   {

      [SerializeField] private DialogueHolder dialogueHolder;
      
      private Collider2D player;

      private IEnumerator StartDialogue()
      {
         yield return new WaitUntil(()=>Input.GetMouseButton(0));
         if (player != null && dialogueHolder._talkedOnce == false)
         {
            dialogueHolder.gameObject.SetActive(true);
            dialogueHolder._talkedOnce = true;
         }
      }

      private void OnTriggerEnter2D(Collider2D other)
      {
         if (other.CompareTag("Player"))
         {
            player = other;
         }
      }
      private void OnTriggerExit2D(Collider2D other)
      {
         player = null;
      }
   }
}
