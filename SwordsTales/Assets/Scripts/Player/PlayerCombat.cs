using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Timeline;
using Enemy;
public class PlayerCombat : MonoBehaviour
{
    private Animator _playerAnimator;
    
    private static readonly int Attack1 = Animator.StringToHash("Attack");

    private Rigidbody2D _rigidbody2D;
    public Transform attackPoint;
    private CircleCollider2D _attackCollider2D;
    public float attackRange;
    public float attackRate = 2f;
    public float nextAttackTime = 0f;

    public LayerMask enemyLayers;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }

   
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f * attackRate;
            }
        }
        
    }

    private void Attack()
    {
        _rigidbody2D.velocity = Vector2.zero;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        _playerAnimator.SetTrigger(Attack1);
        foreach (Collider2D enemy in hitEnemies)
        {
          enemy.GetComponent<Enemy.Enemy>().ReceiveDamage(5);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange); 
    }
}
