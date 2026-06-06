using UnityEngine;

public abstract class EnemyAttackBase : MonoBehaviour
{
    protected float damage;
    protected float attackRate;
    private float nextAttackTime;

    public bool IsExecuting { get; protected set; }

    public virtual void Init(EnemyDataSO data)
    {
        damage = data.damage;
        attackRate = data.attackRate;
        nextAttackTime = 0f;
        IsExecuting = false;
    }

    public bool TryAttack(Transform player)
    {
        if (IsExecuting) return false;
        if (Time.time < nextAttackTime) return false;
        nextAttackTime = Time.time + attackRate;
        ExecuteAttack(player);
        return true;
    }

    protected abstract void ExecuteAttack(Transform player);

    public virtual void Cancel()
    {
        IsExecuting = false;
    }
}
