using Dialogue_System;
using Sound;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Vector2 _v2Direction;
        [SerializeField] private Vector2 _knockbackSpeed;
        
        [SerializeField] private AudioClip _runSound;
        [SerializeField] private AudioClip _receiveDamageSound;
        [SerializeField] private AudioClip _hitSwordSword;
        
        [SerializeField] private Animator _playerAnimator;

        [SerializeField] private float _jumpGravity;
        [SerializeField] private float _knockbackDuration;
        [SerializeField] private float _jumpForce = 15.0f;
        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private float _direction = 1;
        [SerializeField] private float _jumpPressedTime;
        [SerializeField] private float _jumpRememberer;
        [SerializeField] private float _groundRememberer;
        [SerializeField] private float _groundRemembererTime;
        [SerializeField] private bool _knockback;

        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _isFacingRight = true;

        private Rigidbody2D _rigidbody;
       
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsJumping = Animator.StringToHash("isJumping");

        private float _dashTimeLeft;
        private float _lastImageXpos;
        private float _lastDash;
        private float _movementInputDirection;
        private float _truedirection;
        private float _knockbackStartTime;
        
        private bool _isDashing;
        private bool _isJumping;
        private bool jumpedOnce;
            
        public Transform groundCheck;

        public LayerMask whatIsGround;
        
        public float groundCheckRadius;
        public float dashTime;
        public float dashSpeed;
        public float distanceBetweenImages;
        public float dashCooldown;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>();
        }
        
        private void FixedUpdate()
        {
            Grounded();
        }
        
        private void Update()
        {
            CheckInput();
            CheckMovementDirection();
            CheckDash();
            CheckKnockback();
        }
        
        public bool GetDashState()
        {
            return _isDashing;
        }
        
        public void knockback(int direction)
        {
            _knockback = true;
            _knockbackStartTime = Time.time;
            _rigidbody.velocity = new Vector2(_knockbackSpeed.x * direction,_knockbackSpeed.y);
        }

        private void CheckKnockback()
        {
            if (Time.time >= _knockbackStartTime + _knockbackDuration && _knockback)
            {
                _knockback = false;
                _rigidbody.velocity = new Vector2(0.0f,_rigidbody.velocity.y);
            }
        }
        
        private void Flip()
        {
            _direction *= -1;
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        
        private void CheckDash()
        {
            if (!_isDashing) return;
            

            if (_dashTimeLeft > 0)
            {
                _rigidbody.velocity = new Vector2(dashSpeed * _direction,_rigidbody.velocity.y);
                _dashTimeLeft -= Time.deltaTime;

                if (!(Mathf.Abs(transform.position.x - _lastImageXpos) > distanceBetweenImages))
                {
                    return;
                }
                
                PlayerAfterImagePool.Instance.SpawnFromPool();
                _lastImageXpos = transform.position.x;
            }
            else 
            {
                _rigidbody.velocity = Vector2.zero;
                    
                _isDashing = false;
            }
        }

        private void CheckMovementDirection()
        {
            _playerAnimator.SetFloat(Speed,Mathf.Abs(_direction));
            if(_isFacingRight && _direction < 0 && !_knockback)
            {
                Flip();
            }
            else if(!_isFacingRight && _direction > 0 && !_knockback)
            {
                Flip();
            }
        }
        
        private void AttemptToDash()
        {
            _isDashing = true;
            _dashTimeLeft = dashTime;
            _lastDash = Time.time;

            PlayerAfterImagePool.Instance.SpawnFromPool();
            _lastImageXpos = transform.position.x;
        }

        private void Jump()
        {
            _rigidbody.velocity = Vector2.up * _jumpForce;
        }

        private void Grounded()
        {
            _isJumping = !_isGrounded;
            _playerAnimator.SetBool(IsJumping,_isJumping);
           _isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius,whatIsGround);
           if (_isGrounded)
           {
               _groundRememberer = _groundRemembererTime;
               jumpedOnce = false;
           }
           else
           {
               _groundRememberer -= Time.deltaTime;
           }
           
        }

        private void CheckInput()
        {
            _direction = Input.GetAxisRaw("Horizontal");
            //Jump block ----------------------------------------------------------------
            _jumpRememberer -= Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space) )
            {
                if (_rigidbody.velocity.y > 0)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * _jumpGravity);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)&& _isGrounded)
            {
                _jumpRememberer = _jumpPressedTime;
            }

            if (_jumpRememberer > 0 && !_knockback && _groundRememberer > 0 && !jumpedOnce)
            {
                jumpedOnce = true;
                _jumpRememberer = 0;
                _groundRememberer = 0;
            
                Jump();
                _playerAnimator.SetBool(IsJumping, true);
                
            }
            //-----------------------------------------------------------------------  
            if (Input.GetButton("Horizontal") && !_knockback ) 
            {
                Run();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && !_knockback)
            {
                if (Time.time >= _lastDash + dashCooldown)
                {
                    AttemptToDash();
                }
            }
        }

        private void PlayRunSound()
        {
            SoundManager.Instance.PlaySound(_runSound);
        }
        
        private void PlaySwordSound()
        {
            SoundManager.Instance.PlaySound(_hitSwordSword);
        }
        
        private void PlayReceiveDamageSound()
        {
            SoundManager.Instance.PlaySound(_receiveDamageSound);
        }
        
        private void Run()
        {
            _v2Direction.x = _direction;
            _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _rigidbody.position + _v2Direction, _speed * Time.deltaTime);
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<NPC>())
            {
                other.gameObject.SendMessage("StartDialogue");
            }
        }

    }
}