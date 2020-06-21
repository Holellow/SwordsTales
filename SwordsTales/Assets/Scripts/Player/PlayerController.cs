using System;
using UnityEngine;

namespace Player
{
    
    
    public class PlayerController : Unit
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Vector3 vector3direction;
        [SerializeField] private Vector2 direction;
        private float trueDirection;
        private Rigidbody2D _rigidbody;
        [SerializeField]  private bool isGrounded;
        public Transform groundCheck;
       
        [SerializeField] private float speed = 3.0f;

        private SpriteRenderer _playerSpriteRenderer; 
        [SerializeField] private float lives = 5;

        public int extraJumps;
        [SerializeField] private float jumpForce = 15.0f;
        
        private bool _jumpAir;
        public int extraJumpsValue;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsJumping = Animator.StringToHash("isJumping");

        private void Start()
        {
            
            extraJumps = extraJumpsValue;
            _playerSpriteRenderer = GetComponent<SpriteRenderer>();
            var position = transform.position;
            vector3direction = new Vector3(position.x,position.y);
        }
        
        private void FixedUpdate()
        {
            Grounded();


        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            playerAnimator.SetFloat(Speed, trueDirection);
            trueDirection = Math.Abs(Input.GetAxisRaw("Horizontal"));
            if (isGrounded)
            {
                extraJumps = extraJumpsValue;
            }
            if (Input.GetKey(KeyCode.Space) && extraJumps > 0 && isGrounded)
            {
                Jump();
                playerAnimator.SetBool(IsJumping,true);
            }

            if (Input.GetButton("Horizontal"))
            {
                Run();
            }

        }

        protected override void Die()
        {
        
        } 
    
    

        public override void ReceiveDamage()
        {
           
            Debug.Log(lives);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(transform.up * 5 , ForceMode2D.Impulse);
        }

        private void Jump()
        {
            _rigidbody.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }

        private void Grounded()
        {
            var colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.1f);
            if (colliders.Length > 1)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
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
            
            
            if (direction == Vector2Int.left)
            {
                _playerSpriteRenderer.flipX = false;
            }
            
            if (direction == Vector2Int.right)
            {
                _playerSpriteRenderer.flipX = true;
            }
                
        }
    
}
   

}