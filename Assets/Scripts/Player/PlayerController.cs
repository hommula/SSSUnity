using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : MonoBehaviour
{

    /*
     * Katana
     * Glove
     * Gun
     */

    GameObject gb;
    Rigidbody2D rb;
    UnityEngine.Transform tf;
    DamageSystem _damageSystem;
    PlayerStats _playerStats;
    [SerializeField]DetectionZone hitboxZone;

    //Ground Check variables
    public Vector2 gcBoxSize;
    public float gcCastDistance = 0.2f;
    public LayerMask groundLayer;


    public string currentAnimation;
    [SerializeField] bool canMove;
    bool isMoving;
    bool isAttacking;
    float attackDuration = .5f;

    public float movementSpeed;
    public float gravity;
    Vector2 moveInput;
    [SerializeField] float jumpHeight;
    [SerializeField] float knockbackDistance;

    float coyoteTimeCounter;
    float coyoteTime = 0.2f;
    float jumpBufferTime = .3f;
    [SerializeField]float jumpBufferCounter;



    private void Start()
    {
        _damageSystem = GetComponent<DamageSystem>();
        _playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();   
        tf = GetComponent<UnityEngine.Transform>();
        gb = GetComponent<GameObject>();
        changeCharacterAnimation(AnimationString.PLAYER_IDLE);
        canMove = true;
        isAttacking = false;
    }
    private void Update()
    {
        if (isGrounded()) 
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;    
        }

        //Jumping
        if (!Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if(Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            coyoteTimeCounter = 0f;
        }
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            jumpBufferCounter = 0f;
            rb.velocity = new Vector2 (rb.velocity.x, 0);
            float jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * rb.gravityScale) * -2) * rb.mass;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (hitboxZone.detectedColliders.Count != 0) 
        {
            _damageSystem.hit(10, hitboxZone.detectedColliders[0].transform);
        }

        switchAnimation();

        if (!_playerStats.IsInvincible && canMove)
        {
            rb.velocity = new Vector2(moveInput.x * movementSpeed, rb.velocity.y);
        }

        /*
        if (Mathf.Abs(rb.velocity.x) > movementSpeedCap) 
        {
            rb.velocity = new Vector2(rb.velocity.x > 0 ? movementSpeedCap : -movementSpeedCap, rb.velocity.y);
        }*/
        rb.gravityScale = gravity;
    }

    private void FixedUpdate()
    {


    }



    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if(moveInput.x != 0 && canMove)
        {
            tf.localScale = new Vector2(moveInput.x > 0 ? 1 : -1, 1);
            isMoving = true;
        }
        else 
        {
            isMoving = false;
        }
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if(!isAttacking)
        {
            
            StartCoroutine(startAttack());
        }
    }

    private IEnumerator startAttack()
    {
        isAttacking = true;
        canMove = false;
        changeCharacterAnimation(AnimationString.PLAYER_ATTACK);
        yield return new WaitForSeconds(attackDuration);
        changeCharacterAnimation(AnimationString.PLAYER_IDLE);
        isAttacking = false;
        canMove = true;

    }

    public void onJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            jumpBufferCounter = jumpBufferTime;

        }
        
    }

    void switchAnimation()
    {
        if (isAttacking)
        {
            return;
        }
        else if (isMoving)
        {
            changeCharacterAnimation(AnimationString.PLAYER_WALK);
        }
        else
        {
            changeCharacterAnimation(AnimationString.PLAYER_IDLE);
        }
    }

    private void changeCharacterAnimation(string animation)
    {
        currentAnimation = animation;
    }


    bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, gcBoxSize, 0, -transform.up, gcCastDistance, groundLayer))
        {
            return true;
        }
        else return false;
    }

    //Drawing Ground check box
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * gcCastDistance, gcBoxSize);
    }
}
