using System;
using UnityEngine;
using UnityEngine.Serialization;


namespace Player
{
    
    
    public class PlayerController : Unit
    {
        [SerializeField] private float knockbackDuration;
        [SerializeField] private float jumpForce = 15.0f;
        [SerializeField] private float speed = 3.0f;
        [SerializeField] private float direction;
        [SerializeField] private float receiveDamageJump;
        
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isFacingRight = true;
        
        [SerializeField] private Vector2 v2Direction;
        [SerializeField] private Vector2 knockbackSpeed;
        
        [SerializeField] private Animator playerAnimator;
        
        [SerializeField] private GameObject player;
        
        public Transform groundCheck;

        public LayerMask whatIsGround;
        
        private SpriteRenderer _playerSpriteRenderer;
        
        private Rigidbody2D _rigidbody;
       
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsJumping = Animator.StringToHash("isJumping");
        private static readonly int IsJumpingEnd = Animator.StringToHash("isJumpingEnd");
        
        private float _dashTimeLeft;
        private float _lastImageXpos;
        private float _lastDash;
        private float _movementInputDirection;
        private float _truedirection;
        private float knockbackStartTime;
        
        private bool _isDashing;
        private bool _isJumping;
        private bool _knockback;
        
        public float groundCheckRadius;
        public float dashTime;
        public float dashSpeed;
        public float distanceBetweenImages;
        public float dashCooldown;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            playerAnimator = GetComponent<Animator>();
        }
        
        private void Start()
        {
            _playerSpriteRenderer = GetComponent<SpriteRenderer>();

        }
        
        private void FixedUpdate()
        {
            Grounded();
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
                playerAnimator.SetBool(IsJumping,true);
            }
        }
        
        private void Update()
        {
            direction = Input.GetAxisRaw("Horizontal");
            CheckMovementDirection();
            CheckDash();
            
            if (Input.GetButton("Horizontal") ) 
            {
                Run();
            }

            if (Input.GetButtonDown("Dash"))
            {
                if (Time.time >= _lastDash + dashCooldown)
                {
                    AttemptToDash();
                }
            }
        }
        
        private void Flip()
        {
            
                direction *= -1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            
        }
        
        private void CheckDash()
        {
            if (!_isDashing)
            {
                return;
            }

            if (_dashTimeLeft > 0)
            {
                _rigidbody.velocity = new Vector2(dashSpeed * direction,_rigidbody.velocity.y);
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
            playerAnimator.SetFloat(Speed,Mathf.Abs(direction));
            if(isFacingRight && direction < 0)
            {
                Flip();
            }
            else if(!isFacingRight && direction > 0)
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

        public override void ReceiveDamage(int damage)
        {
           
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(transform.up * receiveDamageJump , ForceMode2D.Impulse);
        }

        private void Jump()
        {
            _rigidbody.velocity = Vector2.up * jumpForce;
            
        }

        private void Grounded()
        {
            _isJumping = !isGrounded;
            playerAnimator.SetBool(IsJumping,_isJumping);
           isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius,whatIsGround);
        }

        private void Run()
        {
            v2Direction.x = direction;
            _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _rigidbody.position + v2Direction, speed * Time.deltaTime);
        }
    }
}