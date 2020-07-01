using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform touchDamageCheck;
    
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask PlayerMask;
    
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float touchDamage;
    [SerializeField] private float touchDamageCooldown;
    [SerializeField] private float touchDamageWidth;
    [SerializeField] private float touchDamageHeight;
    
    [SerializeField] private float knockbackDuration;
    
    [SerializeField] private Vector2 knockbackSpeed;
    [SerializeField] private GameObject hitParticle;
    
    private Vector2 movement;
    private Vector2 touchDamageBotLeft;
    private Vector2 touchDamageTopRight;
    
    private GameObject alive;
    private Rigidbody2D aliveRb;
    private Animator aliveAnim;
    
    private enum State
    {
        Walking,
        Dead,
        Knockback,
    }
    
    private State _currentState;

    private float _currentHealth;
    private float knockbackStartTime;
    private float[] attackDetails = new float[2];
    private float lastTouchDamageTime;
    
    private int _facingDirection;
    private int _damageDirection;
    
    private bool groundDetected;
    private bool wallDetected;
    private static readonly int Knockback = Animator.StringToHash("Knockback");

    private void Start()
    {
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();
        _currentHealth = maxHealth;
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
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundMask);

        CheckTouchDamage();
        if (!groundDetected || wallDetected)
        {
            Flip();
        }
        else
        {
            movement.Set(movementSpeed * _facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
        }
    }

    private void ExitWalkingState()
    {
        
    }
    
 //Knockback state 
    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * _damageDirection, knockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool(Knockback,true);
    }

    private void UpdateKnockbackState()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Walking);
        }
    }

    private void ExitKnockbackState()
    {
        aliveAnim.SetBool(Knockback,false);
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

        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        if (attackDetails[1] > alive.transform.position.x)
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
        alive.transform.Rotate(0.0f, 180.0f,0.0f);
    }


    private void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight,PlayerMask);
            
            if (hit != null)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;
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
        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - touchDamageHeight / 2);
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - touchDamageHeight / 2);;
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + touchDamageHeight / 2);;
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + touchDamageHeight / 2);
        
        Gizmos.DrawLine(groundCheck.position,new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector2(wallCheck.position.x - wallCheckDistance, wallCheck.position.y));
       
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, botLeft);
        Gizmos.DrawLine(topRight, botRight);
    }
}
