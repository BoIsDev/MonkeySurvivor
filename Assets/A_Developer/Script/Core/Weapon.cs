using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float attackRate = 1f;
    protected float lastAttackTime;

    protected virtual void Awake()
    {
    }
    
    public void HandleAttack()
    {
        if (!CanAttack()) return;

        Attack();
        lastAttackTime = Time.time;
    }

    protected virtual bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackRate;
    }

    protected abstract void Attack();

}