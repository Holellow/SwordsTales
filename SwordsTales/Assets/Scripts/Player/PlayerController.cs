using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    
    
    public class PlayerController : Unit
    {

        private bool isDashing;
        public float dashTime;
        public float dashSpeed;
        public float distanceBetweenImages;
        public float dashCooldawn;
        private float dashTimeLeft;
        private float lastImageXpos;
        private float lastDash;
        
        public float groundCheckRadius;
        public LayerMask whatIsGround;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Vector3 vector3direction;
        [SerializeField] private Vector2 direction;
        private float trueDirection;
        
        private Rigidbody2D _rigidbody;
        
        [SerializeField]  private bool isGrounded;
        public Transform groundCheck;
       
        [SerializeField] private float speed = 3.0f;

        [SerializeField] private int jump;
        private SpriteRenderer _playerSpriteRenderer; 
        [SerializeField] private float lives = 5;

        
        [SerializeField] private float jumpForce = 15.0f;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsJumping = Animator.StringToHash("isJumping");
        private static readonly int IsJumpingEnd = Animator.StringToHash("isJumpingEnd");

        private void Start()
        {
            
            _playerSpriteRenderer = GetComponent<SpriteRenderer>();
            var position = transform.position;
            vector3direction = new Vector3(position.x,position.y);
        }
        
        private void FixedUpdate()
        {
            Grounded();
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            playerAnimator = GetComponent<Animator>();
        }

        private void UpdateAnimations()
        {
            if (direction == Vector2Int.left)
            {
                _playerSpriteRenderer.flipX = false;
            }
            
            if (direction == Vector2Int.right)
            {
                _playerSpriteRenderer.flipX = true;
            }
            playerAnimator.SetBool(IsJumping,!isGrounded);
            playerAnimator.SetFloat(Speed, trueDirection);
        }
        
        private void Update()
        {
            CheckDash();
            UpdateAnimations();
           
            trueDirection = Math.Abs(Input.GetAxisRaw("Horizontal"));

            if (Input.GetButton("Horizontal") ) 
            {
                Run();
            }

            if (Input.GetButtonDown("Dash"))
            {
                if(Time.time >= (lastDash + dashCooldawn))
                    AttemptToDash();
            }
        }

        private void CheckDash()
        {
            if (isDashing)
            {
                if (dashTimeLeft > 0)
                {
                    _rigidbody.velocity = new Vector2(dashSpeed * vector3direction.x,_rigidbody.velocity.y);
                    dashTimeLeft -= Time.deltaTime;

                    if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                    {
                        PlayerAfterImagePool.Instance.GetFromPool();
                        lastImageXpos = transform.position.x;
                    }
                }

                if (dashTimeLeft <= 0)
                {
                    isDashing = false;
                }
            }
        }
        private void AttemptToDash()
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDash = Time.time;

            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
        }

        protected override void Die()
        {
        
        } 
    
    

        public override void ReceiveDamage(int damage)
        {
           
            Debug.Log(lives);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(transform.up * 5 , ForceMode2D.Impulse);
        }

        private void Jump()
        {
            _rigidbody.velocity = Vector2.up * jumpForce;
        }

        private void Grounded()
        {
           isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius,whatIsGround);
        }
        
        private void Run(){
           
            vector3direction = transform.right * Input.GetAxisRaw("Horizontal");
           
            var position = transform.position;
            position = Vector3.MoveTowards(position, position + vector3direction,speed * Time.deltaTime );
            transform.position = position;

            if (vector3direction.x < 0.9)
            {
               
                _playerSpriteRenderer.flipX = true;
                direction = Vector2Int.right;
            }
            
            if (vector3direction.x > 0.9)
            {
                
                _playerSpriteRenderer.flipX = false;
                direction = Vector2Int.left;
            }
            
           
                
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            jump = 0;
            isGrounded = true;
        }
    }
    
}