using UnityEngine;

/// <summary>
/// A single fireball. Idle while parented to an orbit point; when launched it
/// detaches and flies straight to the target's position like a bullet, hitting
/// one enemy, then reports back to the weapon.
/// </summary>
public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 16f;
    [SerializeField] private float maxLifetime = 3f;  // give up if it never hits
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private LayerMask enemyLayer;

    private bool attacking;
    private Vector3 flyDir;
    private float elapsed;
    private int damage;
    private System.Action onFinished;

    public bool IsAttacking => attacking;

    private void OnEnable() => attacking = false;   // reset on pool reuse → idle

    public void LaunchAt(Transform target, int dmg, System.Action onFinishedCb)
    {
        flyDir = DirTo(target);                       // lock direction at launch
        if (flyDir == Vector3.zero) flyDir = transform.forward;
        damage = dmg;
        onFinished = onFinishedCb;
        elapsed = 0f;
        attacking = true;
        transform.rotation = Quaternion.LookRotation(flyDir);
        // thêm cuối LaunchAt:
        Debug.DrawLine(transform.position, target.position, Color.yellow, 1f);
    }

    private void Update()
    {
        if (!attacking) return;

        elapsed += Time.deltaTime;
        if (elapsed >= maxLifetime) { Finish(); return; }

        transform.position += flyDir * (speed * Time.deltaTime);   // straight line, like a bullet
    }

    private Vector3 DirTo(Transform t)
    {
        if (t == null) return Vector3.zero;
        Vector3 aim = t.position;
        Collider col = t.GetComponentInChildren<Collider>();
        if (col != null) aim = col.bounds.center;     // aim at body center, not the foot pivot
        return (aim - transform.position).normalized;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (!attacking) return;   // no damage while orbiting
        if ((enemyLayer.value & (1 << col.gameObject.layer)) == 0) return;
        col.GetComponent<IDamageable>()?.TakeDamage(damage);
        Finish();
    }

    private void Finish()
    {
        if (!attacking) return;
        attacking = false;
        if (hitEffect != null)
            PoolManager.Instance.Spawn(hitEffect, transform.position, Quaternion.identity);
        onFinished?.Invoke();     // weapon despawns me
    }
}
