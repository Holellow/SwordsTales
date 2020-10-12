using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class FinalState : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
