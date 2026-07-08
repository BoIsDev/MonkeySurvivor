using System.Collections;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask damageableLayer;

    private GameObject _prefabRef;
    private Vector3 _targetPos;
    private float _fallDuration;
    private float _explosionRadius;
    private float _damage;

    public void Launch(GameObject prefab, Vector3 target, float fall, float radius, float dmg)
    {
        _prefabRef = prefab;
        _targetPos = target;
        _fallDuration = fall;
        _explosionRadius = radius;
        _damage = dmg;
        StartCoroutine(FallRoutine());
    }

    private IEnumerator FallRoutine()
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < _fallDuration)
        {
            transform.position = Vector3.Lerp(startPos, _targetPos, elapsed / _fallDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = _targetPos;
        Explode();
    }

    // OverlapSphere is correct for bomb — instant AOE damages multiple targets at once
    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius, damageableLayer);
        foreach (var hit in hits)
            hit.GetComponent<IDamageable>()?.TakeDamage(_damage);

        PoolManager.Instance.Despawn(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
