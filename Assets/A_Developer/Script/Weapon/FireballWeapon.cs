    using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spins a ring of orbit points (set up in the prefab) and parents a fireball to
/// each so they orbit for free. One per attack interval detaches and strikes the
/// nearest enemy on a parabolic arc; its point refills after a delay.
/// </summary>
public class FireballWeapon : WeaponBase
{
    [SerializeField] private Transform[] points;     // orbit anchors (child objects in the prefab)
    [SerializeField] private float orbitSpeed = 180f; // degrees/second around Y
    [SerializeField] private float respawnDelay = 1f;

    private Fireball[] occupants;
    private float[] respawnTime;
    private int nextAttacker;

    private void Awake()
    {
        occupants = new Fireball[points.Length];
        respawnTime = new float[points.Length];
    }

    public override void HandleAttack()
    {
        transform.Rotate(0f, orbitSpeed * Time.deltaTime, 0f);   // spin → children orbit free

        int count = Mathf.Min(MaxTarget, points.Length);
        RefillPoints(count);

        if (!CanAttack()) return;
        List<Transform> targets = FindTargets();
        if (targets.Count == 0) return;

        int slot = NextReadySlot(count);
        if (slot < 0) return;

        Fireball fb = occupants[slot];
        fb.transform.SetParent(null);   // leave the ring, keep world position
        fb.LaunchAt(targets[0], Damage, () => OnFinished(fb));   // pass the Transform to home onto
        occupants[slot] = null;
        respawnTime[slot] = Time.time + respawnDelay;   // start the refill countdown at launch
        lastAttackTime = Time.time;
    }

    // Fill any free point whose refill delay has passed.
    private void RefillPoints(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (occupants[i] != null || Time.time < respawnTime[i]) continue;

            GameObject go = PoolManager.Instance.Spawn(EffectPrefab, points[i].position, Quaternion.identity);
            go.transform.SetParent(points[i]);
            go.transform.localPosition = Vector3.zero;   // snap onto the point, ride its rotation
            occupants[i] = go.GetComponent<Fireball>();
        }
    }

    // Pick the next idle fireball in orbit order (round-robin).
    private int NextReadySlot(int count)
    {
        for (int n = 0; n < count; n++)
        {
            int i = (nextAttacker + n) % count;
            if (occupants[i] != null && !occupants[i].IsAttacking)
            {
                nextAttacker = (i + 1) % count;
                return i;
            }
        }
        return -1;
    }

    private void OnFinished(Fireball fb)
    {
        PoolManager.Instance.Despawn(EffectPrefab, fb.gameObject);
    }

    private void OnDestroy()
    {
        if (PoolManager.Instance == null || occupants == null) return;
        foreach (Fireball fb in occupants)
            if (fb != null) PoolManager.Instance.Despawn(EffectPrefab, fb.gameObject);
    }

    // Required by WeaponBase (abstract); this weapon manages everything in HandleAttack.
    protected override void SpawnEffect(List<Transform> targets) { }
}
