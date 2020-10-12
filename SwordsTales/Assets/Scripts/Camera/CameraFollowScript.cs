using UnityEngine;

namespace Camera
{
    public class CameraFollowScript : MonoBehaviour
    {
        public Transform player;
    
        public Vector3 offset;
    
        public float smoothing;
    
        void FixedUpdate()
        {
            if (player != null)
            {
                Vector3 newPosition = Vector3.Lerp(transform.position,player.transform.position + offset, smoothing);
                transform.position = newPosition;
            }
        }
    }
}