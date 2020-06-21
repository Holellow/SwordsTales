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

    public Transform attackPoint;
    private CircleCollider2D _attackCollider2D;
    public float attackRange;

    public LayerMask enemyLayers;
    // Start is called before the first frame update
    void Start()
    {
        
        _playerAnimator = GetComponent<Animator>();
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void Attack()
    {
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
