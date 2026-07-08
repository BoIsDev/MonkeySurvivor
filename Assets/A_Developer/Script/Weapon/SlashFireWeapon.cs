using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a slash effect in front of the player, facing the closest enemy.
/// </summary>
public class SlashFireWeapon : WeaponBase
{
    [SerializeField] private float spawnDistance = 1f;
    protected override void SpawnEffect(List<Transform> targets)
    {
        Transform target = targets[0];
        // Launch point: at the player, raised to torso height.
        Vector3 spawnPos = transform.position;
        spawnPos.y += 0.86f;

        // Determine the real 3D aim direction from the launch point to the target
        // (no horizontal flattening, so it can angle up or down).
        Vector3 dir = (target.position - spawnPos).normalized;

        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject proj = PoolManager.Instance.Spawn(EffectPrefab, spawnPos, rot);
        proj.transform.localScale = Vector3.one * EffectScale;
        proj.GetComponent<Projectile>()?.Launch(dir);
        InitDamageDealer(proj);

        // Debug: red ray showing the shot direction, visible ~1s in the Scene view.
        Debug.DrawRay(spawnPos, dir * Range, Color.red, 1f);
    }
}
