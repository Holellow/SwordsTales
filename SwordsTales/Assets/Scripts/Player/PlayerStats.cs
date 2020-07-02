using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private HealthBar healthBar;
    
    [SerializeField] private float maxHealth;
    
    [SerializeField] private float currentHealth;
    
    
    private GameManager _gameManager;
    private void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHealth = maxHealth;
        healthBar.HealthBarValue(currentHealth);
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        healthBar.HealthBarValue(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _gameManager.Respawn();
        Destroy(gameObject);
    }
}
