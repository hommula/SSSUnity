using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    Stats objectStats;
    Rigidbody2D rb;
    Transform tf;
    private void Awake()
    {
        objectStats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
    }

    public void hit(int damage, Transform _transform)
    {
     Debug.Log("Hit " + _transform.name);   
        
        if(!objectStats.IsInvincible && objectStats.IsAlive)
        {
            objectStats.HealthPoint -= damage;
            objectStats.IsInvincible = true;
            knockback(_transform);
        }
    }

    void knockback(Transform _transform)
    {
        rb.velocity = Vector2.zero;
        //rb.AddForce(Vector2.up * objectStats.KnockbackDistance / 2, ForceMode2D.Impulse);
        rb.AddForce(_transform.position.x < tf.position.x ? Vector2.right * objectStats.KnockbackDistance : Vector2.left * objectStats.KnockbackDistance, ForceMode2D.Impulse);
    }
}
