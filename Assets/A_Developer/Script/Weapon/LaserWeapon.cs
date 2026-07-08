using System.Collections.Generic;
using UnityEngine;
using MasterFX;

/// <summary>
/// Evolved form of SlashShoot: a continuous beam that locks onto the nearest
/// enemy and damages it over time, with no fire cooldown.
/// </summary>
public class LaserWeapon : WeaponBase
{
    private GameObject beam;
    private MLaser laser;
    private float nextDamageTime;

    // Continuous weapon: ignore the cooldown gate, manage the beam every tick.
    public override void HandleAttack()
    {
        List<Transform> targets = FindTargets();
        if (targets.Count == 0)
        {
            StopBeam();
            return;
        }

        AimBeam(targets[0]);
        TickDamage(targets[0]);
    }

    private void AimBeam(Transform target)
    {
        Vector3 origin = transform.position;
        origin.y += 0.86f;

        Vector3 endPoint = target.position;
        var col = target.GetComponentInChildren<Collider>();
        if (col != null) endPoint = col.bounds.center;

        if (beam == null)
        {
            beam = Instantiate(EffectPrefab, origin, Quaternion.identity);
            laser = beam.GetComponent<MLaser>();
        }
        laser.SetLaser(origin, endPoint);
    }

    private void TickDamage(Transform target)
    {
        if (Time.time < nextDamageTime) return;
        nextDamageTime = Time.time + AttackRate;
        target.GetComponent<IDamageable>()?.TakeDamage(Damage);
    }

    private void StopBeam()
    {
        if (beam == null) return;
        if (laser != null) laser.StopLaser();
        Destroy(beam, 1f);
        beam = null;
        laser = null;
    }

    private void OnDestroy() => StopBeam();

    // Required by WeaponBase (abstract), intentionally empty — this weapon
    // overrides the whole attack loop instead of spawning a per-shot effect.
    protected override void SpawnEffect(List<Transform> targets) { }
}
