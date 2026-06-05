using System.Collections;
using UnityEngine;

public class HulkAttack : EnemyAttackBase
{
    [SerializeField] private float chargeSpeed = 15f;
    [SerializeField] private float chargeDuration = 0.4f;
    
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
    
    // Lao thẳng về phía player — EnemyBrain xử lý movement, trigger collider xử lý damage
    private IEnumerator ChargeRoutine(Transform player)
    {
        IsExecuting = true;
        hitPlayer = false;
    
        // Lock target tại thời điểm bắt đầu charge
        Vector3 chargeTarget = new Vector3(player.position.x, transform.position.y, player.position.z);
    
        float elapsed = 0f;
        while (elapsed < chargeDuration && !hitPlayer)
        {
            enemyBrain.MoveTo(chargeTarget, chargeSpeed); // EnemyBrain làm chủ movement
            elapsed += Time.deltaTime;
            yield return null;
        }
    
        IsExecuting = false;
    }
    
    // Trigger collider trên prefab phát hiện chạm player → deal damage
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
