using System.Collections;
using UnityEngine;

public class HulkAttack : EnemyAttackBase
{
    [SerializeField] private float chargeSpeed = 15f;
    [SerializeField] private float chargeDuration = 0.4f;
    [SerializeField] private float chargeDistance = 10f;
    
    private EnemyBrain enemyBrain;
    private bool hitPlayer;
    
    public override void Init(EnemyDataSO data)
    {
        base.Init(data);
        enemyBrain = GetComponent<EnemyBrain>();
    }
    
    protected override void ExecuteAttack(Transform player)
    {
        StartCoroutine(ChargeRoutine(player));
        Debug.Log("Hulk Attack ");
    }
    
    // Charge straight toward player — EnemyBrain handles movement, trigger collider handles damage
    private IEnumerator ChargeRoutine(Transform player)
    {
        IsExecuting = true;
        hitPlayer = false;

        // Lock direction toward player, charge fixed chargeDistance
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        Vector3 chargeTarget = transform.position + direction * chargeDistance;

        float elapsed = 0f;
        while (elapsed < chargeDuration && !hitPlayer)
        {
            enemyBrain.MoveTo(chargeTarget, chargeSpeed); // EnemyBrain owns movement
            elapsed += Time.deltaTime;
            yield return null;
        }

        IsExecuting = false;
    }

    // Trigger collider on prefab detects player hit → deal damage
    private void OnTriggerEnter(Collider other)
    {
        if (!IsExecuting) return;
    
        var damageable = other.GetComponent<IDamageable>();
        if (damageable == null) return;
    
        damageable.TakeDamage(damage);
        hitPlayer = true;
    }
    
    public override void Cancel()
    {
        StopAllCoroutines();
        base.Cancel();
    }
}
