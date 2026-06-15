using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a single persistent aura that follows the player and damages enemies on contact.
/// </summary>
public class IceAuraWeapon : WeaponBase
{
    private GameObject spawnedAura;

    protected override void SpawnEffect(List<Transform> targets)
    {
        // The aura is spawned once and lives forever; later attacks are no-ops.
        if (spawnedAura != null) return;

        spawnedAura = PoolManager.Instance.Spawn(EffectPrefab, transform.position, Quaternion.identity);
        // Parented so the aura follows the player for its whole lifetime.
        spawnedAura.transform.SetParent(transform);
        InitDamageDealer(spawnedAura);
    }

    private void OnDestroy()
    {
        // Return the aura to the pool when this weapon object is destroyed (e.g. on evolution).
        if (spawnedAura != null && PoolManager.Instance != null)
            PoolManager.Instance.Despawn(EffectPrefab, spawnedAura);
    }
}
