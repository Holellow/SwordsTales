using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private CameraFollowScript playerCamera;

    [SerializeField] private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;
    
    private void Update()
    {
        CheckRespawn();
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void CheckRespawn()
    {
        if (!(Time.time >= respawnTimeStart + respawnTime) || !respawn) return;
        var playerTemp = Instantiate(player, respawnPoint);
        playerCamera.player = playerTemp.transform;
        
        respawn = false;
        
    }
}
    

