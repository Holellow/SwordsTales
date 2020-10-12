using UnityEngine;

public class Death : MonoBehaviour
{
    private PlayerStats _playerStats;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerStats>())
        {
            _playerStats = other.GetComponent<PlayerStats>();
            _playerStats.Die();
        }
    }
}
