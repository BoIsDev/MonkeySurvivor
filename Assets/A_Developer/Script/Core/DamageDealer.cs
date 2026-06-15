using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private bool continuous;
    [SerializeField] private float tickRate = 0.5f;

    [Header("Activation")]
    // Effect spawns with collisions OFF, then turns them on after this many seconds.
    // 0 = active immediately on spawn.
    [SerializeField] private float activationDelay = 0f;

    [Header("Half Sphere Filter")]
    [SerializeField] private bool useHalfSphere = false;
    [SerializeField] private float halfAngle = 90f;

    private float nextTickTime;
    private float damage;
    private Collider _col;

    private void Awake() => _col = GetComponent<Collider>();

    // Runs on every (re)spawn from the pool: hold collisions off, then arm after the delay.
    // Disabling the collider removes it from physics entirely, so no trigger fires from
    // either side regardless of which object owns the Rigidbody.
    private void OnEnable()
    {
        nextTickTime = 0f;
        if (_col != null) _col.enabled = false;

        CancelInvoke(nameof(EnableCollisions));
        if (activationDelay > 0f)
            Invoke(nameof(EnableCollisions), activationDelay);
        else
            EnableCollisions();
    }

    // Pool despawn (SetActive false) lands here — cancel the pending arm so it
    // never fires on a recycled object during its NEXT spawn.
    private void OnDisable() => CancelInvoke(nameof(EnableCollisions));

    private void EnableCollisions()
    {
        if (_col != null) _col.enabled = true;
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
        col.GetComponent<IDamageable>()?.TakeDamage(damage);
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
