using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fires the effect prefab as a projectile toward the selected target.
/// Direction is computed like SlashFireWeapon; the Projectile component flies it.
/// </summary>
public class SlashShootWeapon : WeaponBase
{
    protected override void SpawnEffect(List<Transform> targets)
    {
        Transform target = targets[0];

        // Launch point: at the player, raised to torso height.
        Vector3 spawnPos = transform.position;
        spawnPos.y += 0.86f;

        // Aim at the enemy's collider center, not its pivot (which sits at the feet).
        Vector3 targetPoint = target.position;
        var targetCol = target.GetComponentInChildren<Collider>();
        if (targetCol != null) targetPoint = targetCol.bounds.center;

        // Real 3D aim from the launch point to the target (no horizontal flattening).
        Vector3 dir = (targetPoint - spawnPos).normalized;

        // Face the direction of travel (unlike the stationary slash, which faces back).
        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject proj = PoolManager.Instance.Spawn(EffectPrefab, spawnPos, rot);
        proj.transform.localScale = Vector3.one * EffectScale;

        // Look on self AND children so it still works if Projectile sits on a child.
        var projectile = proj.GetComponentInChildren<Projectile>();
        if (projectile != null)
            projectile.Launch(dir);
        else
            Debug.LogWarning("[SlashShoot] EffectPrefab thiếu component Projectile → đạn sẽ không bay.", proj);

        InitDamageDealer(proj);

        // Debug: red ray = shot direction (length = Range), shown ~1s.
        Debug.DrawRay(spawnPos, dir * Range, Color.red, 1f);
    }
}
