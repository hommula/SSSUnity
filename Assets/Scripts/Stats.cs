using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] protected float healthPoint;
    [SerializeField] protected int attactPower;
    [SerializeField] protected float knockbackDistance;
    [SerializeField] protected float invDuration;
    [SerializeField] protected float invTimer;
    [SerializeField] protected bool isInvincible;
    
    
    protected bool isAlive;
    protected Animator animator;

    protected string currentAnimationState;
    public float HealthPoint
    {
        get { return healthPoint; }
        set { healthPoint = value; }
    }

    public int AttactPower
    {
        get { return attactPower; }
        set { attactPower = value; }
    }

    public float InvDuration
    {
        get { return invDuration; }
        set { invDuration = value; }
    }

    public bool IsInvincible
    {
        get { return isInvincible; }        
        set { isInvincible = value; }
    }

    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

    public float KnockbackDistance
    {
        get { return knockbackDistance; }
        set { knockbackDistance = value; }
    }

    protected void Update()
    {
        if (isInvincible && invTimer < invDuration)
        {
            invTimer += Time.deltaTime;
        }
        else
        {
            isInvincible = false;
            invTimer = 0;
        }

        if (healthPoint <= 0)
        {
            isAlive = false;
        }
    }

    private void onDeath()
    {

    }
}
