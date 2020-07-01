using UnityEngine;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        private AudioSource _source;
    
        private void Awake()
        {
            Instance = this;
            Debug.Log(Instance);
            _source = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip sound)
        {
            _source.PlayOneShot(sound);
        }
    
    }
}
