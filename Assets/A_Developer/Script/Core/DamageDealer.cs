using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private bool continuous;
    [SerializeField] private float tickRate = 0.5f;

    [Header("Activation")]
    // Effect spawns with collisions OFF, then turns them on after this many seconds.
    // 0 = active immediately on spawn.
    [SerializeField] private float activationDelay = 0f;
    // When ON, the effect returns to the pool the moment it damages an enemy
    // (e.g. a projectile). When OFF it lives on / pierces until its lifetime ends.
    [SerializeField] private bool despawnOnHit = false;

    [Header("Hit")]
    // Optional VFX spawned at the impact point each time this deals damage.
    [SerializeField] private GameObject hitEffect;

    [Header("Half Sphere Filter")]
    [SerializeField] private bool useHalfSphere = false;
    [SerializeField] private float halfAngle = 90f;

    private float nextTickTime;
    private float damage;
    private Collider selfCollider;
    private HiddenEffect hidden;

    private void Awake()
    {
        selfCollider = GetComponentInChildren<Collider>();
        hidden = GetComponent<HiddenEffect>();
    }
    
    private void OnEnable()
    {
        nextTickTime = 0f;
        if (selfCollider != null) selfCollider.enabled = false;

        CancelInvoke(nameof(EnableCollisions));
        if (activationDelay > 0f)
            Invoke(nameof(EnableCollisions), activationDelay);
        else
            EnableCollisions();
    }
    
    private void OnDisable() => CancelInvoke(nameof(EnableCollisions));

    private void EnableCollisions()
    {
        if (selfCollider != null) selfCollider.enabled = true;
    }

    public void SetDamage(float value) => damage = value;

    private bool IsInFront(Collider col)
    {
        Vector3 dir = (col.transform.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, dir) < halfAngle;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (continuous) return;
        if (useHalfSphere && !IsInFront(col)) return;

        var target = col.GetComponent<IDamageable>();
        if (target == null) return;

        target.TakeDamage(damage);

        // Spawn hit VFX at the point on the enemy closest to this effect.
        if (hitEffect != null)
            PoolManager.Instance.Spawn(hitEffect, col.ClosestPoint(transform.position), Quaternion.identity);

        if (despawnOnHit && hidden != null)
            hidden.DespawnNow();
    }

    private void OnTriggerStay(Collider col)
    {
        if (!continuous) return;
        if (useHalfSphere && !IsInFront(col)) return;
        if (Time.time < nextTickTime) return;
        nextTickTime = Time.time + tickRate;
        col.GetComponent<IDamageable>()?.TakeDamage(damage);
    }
    
    
}
