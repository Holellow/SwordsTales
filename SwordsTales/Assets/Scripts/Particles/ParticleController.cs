using UnityEngine;

namespace Particles
{
    public class ParticleController : MonoBehaviour
    {
        private void FinishAnim()
        {
            Destroy(gameObject);
        }
    }
}
