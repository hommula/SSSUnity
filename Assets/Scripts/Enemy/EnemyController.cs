using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public DetectionZone cliffDetectionZone;
    public DetectionZone wallDetectionZone;
    public DetectionZone enemyHitboxZone;
    DamageSystem _damageSystem;
    EnemyStats _enemyStats;
    Rigidbody2D rb;
    Transform tf;

    [SerializeField] float defaultEnemyMovingSpeed;
    float enemyMovingSpeed;
    [SerializeField] float flipBuffering;


    //Ground Check variables
    public Vector2 gcBoxSize;
    public float gcCastDistance = 0.2f;
    public LayerMask groundLayer;


    void Start()
    {
        flipBuffering = 0f;
        enemyMovingSpeed = defaultEnemyMovingSpeed;
        
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        _damageSystem = GetComponent<DamageSystem>();
        _enemyStats = GetComponent<EnemyStats>();
    }

    
    void Update()
    {
        tf.localScale = new Vector2(Mathf.Sign(-enemyMovingSpeed), tf.localScale.y);
        flipBuffering += Time.deltaTime;
        if(isGrounded() && flipBuffering >= 0.2f && cliffDetectionZone.detectedColliders.Count == 0 || wallDetectionZone.detectedColliders.Count > 0 ) 
        {
            flipBuffering = 0;
            enemyMovingSpeed *= -1;
            
        }

        if(enemyHitboxZone.detectedColliders.Count != 0)
        {
            _damageSystem.hit(10, enemyHitboxZone.detectedColliders[0].transform);
        }

        /*
        if (rb.velocity.x > 0)
        {
            tf.localScale = new Vector2(-1, tf.localScale.y);
        }
        else 
        {
            tf.localScale = new Vector2(1, tf.localScale.y);
        }*/
    }

    private void FixedUpdate()
    {
        if(!_enemyStats.IsInvincible)
        { 
            rb.velocity = new Vector2(enemyMovingSpeed, rb.velocity.y); 
        }
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
