using UnityEngine;

namespace Camera
{
    public class CameraFollowScript : MonoBehaviour
    {
        [SeralizeField] private Transform player;
        [SeralizeField] private Vector3 offset = smt;
        [SeralizeField] private float smoothing = smt;
    
        private void Update()
        {
            if (player != null)
            {
                var tran = transform;
                var newPosition = Vector3.Lerp(tran.position, player.transform.position + offset, smoothing * Time.deltaTime);
                tran.position = newPosition;
            }
        }
    }
}
