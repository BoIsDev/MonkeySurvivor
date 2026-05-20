using System.Collections.Generic;
using UnityEngine;

public class IceAuraWeapon : WeaponBase
{
    private GameObject spawnedAura;

    protected override void AuraDamage(List<Transform> targets)
    {
        if (spawnedAura != null) return;

        spawnedAura = PoolManager.Instance.Spawn(EffectPrefab, transform.position, Quaternion.identity);
        spawnedAura.transform.SetParent(transform);
        InitDamageDealer(spawnedAura);
    }

    private void OnDestroy()
    {
        if (spawnedAura != null && PoolManager.Instance != null)
            PoolManager.Instance.Despawn(EffectPrefab, spawnedAura);
    }
}
