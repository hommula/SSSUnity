using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : Stats
{
    [SerializeField] PlayerController playerController;

    
    string[] WEAPON_TYPE = new string[3] {"_Fist", "_Katana", "_Gun"};
    int currentWeaponType;
    public UnityEvent onPlayerDeath;

    private void Start()
    {
        currentWeaponType = 0;
        animator = GetComponent<Animator>();

        HealthPoint = 100;
        IsAlive = true;
        IsInvincible = false;
        InvDuration = 0.4f;
        KnockbackDistance = 10;
    }
    private new void Update()
    {
        base.Update();

        ChangeAnimationState(playerController.currentAnimation + WEAPON_TYPE[currentWeaponType]);
    }

    void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;
        animator.Play(newState);
    }

    public void decreaseHealth(int damage = 1)
    {
        this.healthPoint -= damage;
        if(healthPoint <= 0 )
        {
            onPlayerDeath?.Invoke();
        }
    }
}
