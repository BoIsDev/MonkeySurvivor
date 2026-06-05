using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool continuous;
    [SerializeField] private float tickRate = 0.5f;

    [Header("Half Sphere Filter")]
    [SerializeField] private bool useHalfSphere = false;
    [SerializeField] private float halfAngle = 90f;

    private float nextTickTime;

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
