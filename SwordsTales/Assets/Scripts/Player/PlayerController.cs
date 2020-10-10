using System;
using Dialogue_System;
using Sound;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Vector2 v2Direction;
        [SerializeField] private Vector2 knockbackSpeed;
        
        [SerializeField] private AudioClip runSound;
        [SerializeField] private AudioClip receiveDamageSound;
        [SerializeField] private AudioClip hitSwordSword;
        
        [SerializeField] private Animator playerAnimator;
        
        [SerializeField] private GameObject player;

        [SerializeField] private NPC npc;
        
        [SerializeField] private float jumpGravity;
        [SerializeField] private float knockbackDuration;
        [SerializeField] private float jumpForce = 15.0f;
        [SerializeField] private float speed = 3.0f;
        [SerializeField] private float direction = 1;
        [SerializeField] private float jumpPressedTime;
        [SerializeField] private float jumpRememberer;
        [SerializeField] private float groundRememberer;
        [SerializeField] private float groundRemembererTime;
        [SerializeField] private bool _knockback;

        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isFacingRight = true;
        
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
        private float _knockbackStartTime;
        
        private bool _isDashing;
        private bool _isJumping;
        private bool jumpedOnce = false;
            
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
            _rigidbody.velocity = new Vector2(knockbackSpeed.x * direction,knockbackSpeed.y);
        }

        private void CheckKnockback()
        {
            if (Time.time >= _knockbackStartTime + knockbackDuration && _knockback)
            {
                _knockback = false;
                _rigidbody.velocity = new Vector2(0.0f,_rigidbody.velocity.y);
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
            if (!_isDashing) return;
            

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
            if(isFacingRight && direction < 0 && !_knockback)
            {
                Flip();
            }
            else if(!isFacingRight && direction > 0 && !_knockback)
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
            _rigidbody.velocity = Vector2.up * jumpForce;
            
        }

        private void Grounded()
        {
            _isJumping = !isGrounded;
            playerAnimator.SetBool(IsJumping,_isJumping);
           isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius,whatIsGround);
           if (isGrounded)
           {
               groundRememberer = groundRemembererTime;
               jumpedOnce = false;
           }
           else
           {
               groundRememberer -= Time.deltaTime;
           }
           
        }

        private void CheckInput()
        {
            direction = Input.GetAxisRaw("Horizontal");
            //Jump block ----------------------------------------------------------------
            jumpRememberer -= Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space) )
            {
                if (_rigidbody.velocity.y > 0)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * jumpGravity);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)&& isGrounded)
            {
                jumpRememberer = jumpPressedTime;
            }

            if (jumpRememberer > 0 && !_knockback && groundRememberer > 0 && !jumpedOnce)
            {
                jumpedOnce = true;
                jumpRememberer = 0;
                groundRememberer = 0;
            
                Jump();
                playerAnimator.SetBool(IsJumping, true);
                
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
            SoundManager.Instance.PlaySound(runSound);
        }
        
        private void PlaySwordSound()
        {
            SoundManager.Instance.PlaySound(hitSwordSword);
        }
        
        private void PlayReceiveDamageSound()
        {
            SoundManager.Instance.PlaySound(receiveDamageSound);
        }
        
        private void Run()
        {
            v2Direction.x = direction;
            _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _rigidbody.position + v2Direction, speed * Time.deltaTime);
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("NPC"))
            {
                other.gameObject.SendMessage("StartDialogue");
            }
        }

    }
}