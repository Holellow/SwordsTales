using UnityEngine;
using UnityEngine.Playables;

namespace Timelines
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] private Animator playerAnim;
        
        [SerializeField] private RuntimeAnimatorController playerContr;
        
        [SerializeField] private PlayableDirector _director;
        
        private bool fix;

        private void OnEnable()
        {
            playerContr = playerAnim.runtimeAnimatorController;
            bool fix = false;
            playerAnim.runtimeAnimatorController = null;
        }

        void Update()
        {
            if (_director.state != PlayState.Playing && !fix)
            {
                fix = true;
                playerAnim.runtimeAnimatorController = playerContr;
            } 
        }
    }
}
