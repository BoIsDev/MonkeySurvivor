using System.Collections;
using UnityEngine;

public class BombDropAttack : EnemyAttackBase
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float riseHeight = 5f;
    [SerializeField] private float riseDuration = 0.8f;
    [SerializeField] private float fallDuration = 0.6f;
    [SerializeField] private float explosionRadius = 3f;

    protected override void ExecuteAttack(Transform player)
    {
        StartCoroutine(BombRoutine(player.position));
    }

    private IEnumerator BombRoutine(Vector3 targetPos)
    {
        IsExecuting = true;

        Vector3 groundPos = transform.position;
        Vector3 peakPos = groundPos + Vector3.up * riseHeight;

        // Phase 1: Rise
        float elapsed = 0f;
        while (elapsed < riseDuration)
        {
            transform.position = Vector3.Lerp(groundPos, peakPos, elapsed / riseDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = peakPos;

        // Spawn bomb at peak position
        GameObject bomb = PoolManager.Instance.Spawn(bombPrefab, transform.position, Quaternion.identity);
        bomb.GetComponent<BombProjectile>()?.Launch(bombPrefab, targetPos, fallDuration, explosionRadius, damage);

        // Phase 2: Land in parallel with falling bomb
        elapsed = 0f;
        while (elapsed < fallDuration)
        {
            transform.position = Vector3.Lerp(peakPos, groundPos, elapsed / fallDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = groundPos;

        IsExecuting = false;
    }

    public override void Cancel()
    {
        StopAllCoroutines();
        base.Cancel();
    }
}
