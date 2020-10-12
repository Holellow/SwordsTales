using Sound;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
    
        [SerializeField] private LayerMask _enemyLayers;
        
        [SerializeField] private Transform _attackHitBoxPos;
        
        [SerializeField] private AudioClip _receiveDamageSound;
    
        [SerializeField] private float _inputTimer;
        [SerializeField] private float _attackRadius;
        
        [SerializeField] private int _attackDamage;
    
        [SerializeField] private bool _combatEnabled;
    
        private PlayerController _PC;
        
        private CircleCollider2D _attackCollider2D;
        
        private Animator _playerAnimator;

        private PlayerStats _playerStats;
        
        public Transform attackPoint;

        private bool _gotInput;
        private bool _isFirstAttack;
        private bool _isAttacking;

        private float _lastInputTime = Mathf.NegativeInfinity;
        private float[] _attackDetails = new float[2];

        private static readonly int Attack1 = Animator.StringToHash("Attack1");
        private static readonly int FirstAttack = Animator.StringToHash("FirstAttack");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int Attack3 = Animator.StringToHash("Attack3");
        private static readonly int CanAttack = Animator.StringToHash("canAttack");
        private static readonly int Attack = Animator.StringToHash("Attack");
    
        void Start()
        {
            _playerAnimator = GetComponent<Animator>();
            _playerAnimator.SetBool(CanAttack, _combatEnabled);
            _PC = GetComponent<PlayerController>();
            _playerStats = GetComponent<PlayerStats>();
        }
    
        void Update()
        {
            CheckCombatInput();
        }

        private void CheckCombatInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_combatEnabled)
                {
                    _gotInput = true;
                    _lastInputTime = Time.time;
                    _playerAnimator.SetTrigger(Attack);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
            {
                return;
            }
            Gizmos.DrawWireSphere(_attackHitBoxPos.position,_attackRadius); 
        }

        private void CheckAttacks()
        {
            if (_gotInput)
            {
                if(!_isAttacking)
                {
                    _gotInput = false;
                    _isAttacking = true;
                    _isFirstAttack = !_isFirstAttack;
                    _playerAnimator.SetBool(Attack1, true);
                    _playerAnimator.SetBool(FirstAttack, _isFirstAttack);
                }
            }

            if (Time.time >= _lastInputTime + _inputTimer)
            {
                _gotInput = false;
            }
        }

        private void CheckAttackHitBox()
        {
            var detectedObjects = Physics2D.OverlapCircleAll(_attackHitBoxPos.position,_attackRadius,_enemyLayers);

            _attackDetails[0] = _attackDamage;
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

            _playerAnimator.SetBool(IsAttacking, _isAttacking);
            _playerAnimator.SetBool(Attack1, false);
            _playerAnimator.SetBool(FirstAttack,_isFirstAttack);
      
        }
    
        private void FinishAttack2()
        {
            _isAttacking = false;
            _playerAnimator.SetBool(IsAttacking, _isAttacking);

        }
    
        private void FinishAttack3()
        {
            _isAttacking = false;
            _playerAnimator.SetBool(IsAttacking, _isAttacking);
            _playerAnimator.SetBool(Attack3, false);
        }

        private void Damage(float[] attackDetails)
        {
            if (_PC.GetDashState()) return;
            
            int direction;
            
            SoundManager.Instance.PlaySound(_receiveDamageSound);
            
            _playerStats.DecreaseHealth(attackDetails[0]);
            
            if (attackDetails[1] < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            _PC.knockback(direction);
        }
    }
}
