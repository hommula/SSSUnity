using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStats : Stats
{
    DamageSystem ds;
    private void Awake()
    {
        ds = GetComponent<DamageSystem>();
    }
    private void Start()
    {
        HealthPoint = 100;
        IsAlive = true;
        IsInvincible = false;
        InvDuration = 0.4f;
        KnockbackDistance = 10;
    }

    private new void Update()
    {
        base.Update();
    }

}
