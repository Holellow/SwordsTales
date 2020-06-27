using System;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombat : MonoBehaviour, IPointerClickHandler
{ 
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform attackHitBoxPos;
    
    [SerializeField] private float inputTimer;
    [SerializeField] private float attackRadius;
    
    [SerializeField] private int attackDamage;
    
    [SerializeField] private bool combatEnabled;
    
    private PlayerController PC;
    private CircleCollider2D _attackCollider2D;
    private Animator _playerAnimator;
    private Rigidbody2D _rigidbody2D;
    
    public Transform attackPoint;

    private bool _gotInput;
    private bool _isFirstAttack;
    private bool _isSecondAttack;
    private bool _isThirdAttack;
    private bool _isAttacking;

    private float _lastInputTime = Mathf.NegativeInfinity;
    private float[] _attackDetails = new float[2];

    private static readonly int Attack1 = Animator.StringToHash("Attack1");
    private static readonly int FirstAttack = Animator.StringToHash("FirstAttack");
    private static readonly int SecondAttack = Animator.StringToHash("SecondAttack");
    private static readonly int ThirdAttack = Animator.StringToHash("ThirdAttack");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int Attack3 = Animator.StringToHash("Attack3");
    private static readonly int Attack2 = Animator.StringToHash("Attack2");
    private static readonly int CanAttack = Animator.StringToHash("canAttack");
    private static readonly int Attack = Animator.StringToHash("Attack");
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerAnimator.SetBool(CanAttack, combatEnabled);
        PC = GetComponent<PlayerController>();
    }
    
    void Update()
    {
        CheckCombatInput();
        /*if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f * attackRate;
            }
        }*/
    }

    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled)
            {
                _gotInput = true;
                _lastInputTime = Time.time;
                _playerAnimator.SetTrigger(Attack);
               // CheckAttacks();
               
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackHitBoxPos.position,attackRadius); 
    }

    private void CheckAttacks()
    {
        if (_gotInput)
        {
            if(!_isAttacking)
            {
                Debug.Log("asdasd");
                _gotInput = false;
                _isAttacking = true;
                _isFirstAttack = !_isFirstAttack;
                _playerAnimator.SetBool(Attack1, true);
                _playerAnimator.SetBool(FirstAttack, _isFirstAttack);
            }
        }

        if (Time.time >= _lastInputTime + inputTimer)
        {
            _gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        var detectedObjects = Physics2D.OverlapCircleAll(attackHitBoxPos.position,attackRadius,enemyLayers);

        _attackDetails[0] = attackDamage;
        _attackDetails[1] = transform.position.x;
        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("ReceiveDamage",_attackDetails);
        }
        
    }
    private void FinishAttack1()
    {
        _isFirstAttack = false;
        _isAttacking = false;
        _isSecondAttack = true;
        
        _playerAnimator.SetBool(IsAttacking, _isAttacking);
        _playerAnimator.SetBool(Attack1, false);
        _playerAnimator.SetBool(FirstAttack,_isFirstAttack);
      
    }
    
    private void FinishAttack2()
    {
        _isSecondAttack = false;
        _isAttacking = false;
        _playerAnimator.SetBool(IsAttacking, _isAttacking);

    }
    
    private void FinishAttack3()
    {
        _isAttacking = false;
        _playerAnimator.SetBool(IsAttacking, _isAttacking);
        _playerAnimator.SetBool(Attack3, false);
        _isThirdAttack = false;
    }

    private void Damage(float[] attackDetails)
    {
        Debug.Log("Hit");
        if (PC.GetDashState()) return;
        int direction;
        if (attackDetails[1] < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        PC.knockback(direction);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
    
}
