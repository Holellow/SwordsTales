using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private Transform _wallCheck;
        [SerializeField] private Transform _touchDamageCheck;
    
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _PlayerMask;
    
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] private float _wallCheckDistance;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _touchDamage;
        [SerializeField] private float _touchDamageCooldown;
        [SerializeField] private float _touchDamageWidth;
        [SerializeField] private float _touchDamageHeight;
    
        [SerializeField] private float _knockbackDuration;
    
        [SerializeField] private Vector2 _knockbackSpeed;
        [SerializeField] private GameObject _hitParticle;
    
        private Vector2 _movement;
        private Vector2 _touchDamageBotLeft;
        private Vector2 _touchDamageTopRight;
    
        private GameObject _alive;
        private Rigidbody2D _aliveRb;
        private Animator _aliveAnim;
    
        private enum State
        {
            Walking,
            Dead,
            Knockback,
        }
    
        private State _currentState;

        private float _currentHealth;
        private float _knockbackStartTime;
        private float[] attackDetails = new float[2];
        private float _lastTouchDamageTime;
    
        private int _facingDirection;
        private int _damageDirection;
    
        private bool _groundDetected;
        private bool _wallDetected;
        
        private static readonly int Knockback = Animator.StringToHash("Knockback");

        private void Start()
        {
            _alive = transform.Find("Alive").gameObject;
            _aliveRb = _alive.GetComponent<Rigidbody2D>();
            _aliveAnim = _alive.GetComponent<Animator>();
            _currentHealth = _maxHealth;
            _facingDirection = 1;
        }

   
        private void Update()
        {
            switch (_currentState)
            {
                case State.Walking:
                    UpdateWalkingState();
                    break;
            
                case State.Knockback:
                    UpdateKnockbackState();
                    break;
            
                case State.Dead:
                    UpdateDeadState();
                    break;
            }
        }
    
        //Walking State
        private void EnterWalkingState()
        {
        
        }

        private void UpdateWalkingState()
        {
            _groundDetected = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _groundMask);
            _wallDetected = Physics2D.Raycast(_wallCheck.position, transform.right, _wallCheckDistance, _groundMask);

            CheckTouchDamage();
            if (!_groundDetected || _wallDetected)
            {
                Flip();
            }
            else
            {
                _movement.Set(_movementSpeed * _facingDirection, _aliveRb.velocity.y);
                _aliveRb.velocity = _movement;
            }
        }

        private void ExitWalkingState()
        {
        
        }
    
        //Knockback state 
        private void EnterKnockbackState()
        {
            _knockbackStartTime = Time.time;
            _movement.Set(_knockbackSpeed.x * _damageDirection, _knockbackSpeed.y);
            _aliveRb.velocity = _movement;
            _aliveAnim.SetBool(Knockback,true);
        }

        private void UpdateKnockbackState()
        {
            if (Time.time >= _knockbackStartTime + _knockbackDuration)
            {
                SwitchState(State.Walking);
            }
        }

        private void ExitKnockbackState()
        {
            _aliveAnim.SetBool(Knockback,false);
        }
        //Dead state
        private void EnterDeadState()
        {
            //spawn particls
            Destroy(gameObject);
        }

        private void UpdateDeadState()
        {
            //Эта нужна, я еще не доделал прост
        }

        private void ExitDeadState()
        {
            //Эта нужна, я еще не доделал прост
        }
    
        //Others

        private void ReceiveDamage(float[] attackDetails)
        {
            _currentHealth -= attackDetails[0];

            Instantiate(_hitParticle, _alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            if (attackDetails[1] > _alive.transform.position.x)
            {
                _damageDirection = -1;
            }
            else
            {
                _damageDirection = 1;
            }
        
            //hitparticle
            if (_currentHealth > 0.0f)
            {
                SwitchState(State.Knockback);
            }
            else if (_currentHealth <= 0.0f)
            {
                SwitchState(State.Dead);
            }
        }
    
        private void Flip()
        {
            _facingDirection *= -1;
            _alive.transform.Rotate(0.0f, 180.0f,0.0f);
        }


        private void CheckTouchDamage()
        {
            if (Time.time >= _lastTouchDamageTime + _touchDamageCooldown)
            {
                _touchDamageBotLeft.Set(_touchDamageCheck.position.x - (_touchDamageWidth / 2), _touchDamageCheck.position.y - (_touchDamageHeight / 2));
                _touchDamageTopRight.Set(_touchDamageCheck.position.x + (_touchDamageWidth / 2), _touchDamageCheck.position.y + (_touchDamageHeight / 2));

                Collider2D hit = Physics2D.OverlapArea(_touchDamageBotLeft, _touchDamageTopRight,_PlayerMask);
            
                if (hit != null)
                {
                    _lastTouchDamageTime = Time.time;
                    attackDetails[0] = _touchDamage;
                    attackDetails[1] = _alive.transform.position.x;
                    hit.SendMessage("Damage",attackDetails);
                }
            }
        }
        private void SwitchState(State state)
        {
            switch (_currentState)
            {
                case State.Walking:
                    ExitWalkingState();
                    break;
            
                case State.Knockback:
                    ExitKnockbackState();
                    break;
                case State.Dead:
                    ExitDeadState();
                    break;
            }
        
            switch (state)
            {
                case State.Walking:
                    EnterWalkingState();
                    break;
            
                case State.Knockback:
                    EnterKnockbackState();
                    break;
                case State.Dead:
                    EnterDeadState();
                    break;
            }

            _currentState = state;
        }

        private void OnDrawGizmos()
        {
            Vector2 botLeft = new Vector2(_touchDamageCheck.position.x - _touchDamageWidth / 2, _touchDamageCheck.position.y - _touchDamageHeight / 2);
            Vector2 botRight = new Vector2(_touchDamageCheck.position.x + _touchDamageWidth / 2, _touchDamageCheck.position.y - _touchDamageHeight / 2);;
            Vector2 topRight = new Vector2(_touchDamageCheck.position.x + _touchDamageWidth / 2, _touchDamageCheck.position.y + _touchDamageHeight / 2);;
            Vector2 topLeft = new Vector2(_touchDamageCheck.position.x - _touchDamageWidth / 2, _touchDamageCheck.position.y + _touchDamageHeight / 2);
        
            Gizmos.DrawLine(_groundCheck.position,new Vector2(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
            Gizmos.DrawLine(_wallCheck.position,new Vector2(_wallCheck.position.x - _wallCheckDistance, _wallCheck.position.y));
       
            Gizmos.DrawLine(botLeft, botRight);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topLeft, botLeft);
            Gizmos.DrawLine(topRight, botRight);
        }
    }
}
