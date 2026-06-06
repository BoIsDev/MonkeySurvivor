using System.Collections;
using UnityEngine;

public class MeleeAttack : EnemyAttackBase
{
    protected override void ExecuteAttack(Transform player)
    {
        player.GetComponent<IDamageable>()?.TakeDamage(damage);
        StartCoroutine(LockStateAttack());
    }

    private IEnumerator LockStateAttack()
    {
        IsExecuting = true;
        yield return new WaitForSeconds(attackRate);
        IsExecuting = false;
    }

    public override void Cancel()
    {
        StopAllCoroutines();
        base.Cancel();
    }
}
