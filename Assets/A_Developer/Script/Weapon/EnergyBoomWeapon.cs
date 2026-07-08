using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Drops an explosion effect on top of every target returned by FindTargets.
/// </summary>
public class EnergyBoomWeapon : WeaponBase
{
    [Header("Boom Setting")]
    [SerializeField] private float spawnHeight = 1f;

    protected override void SpawnEffect(List<Transform> targets)
    {
        foreach (Transform target in targets)
        {
            Vector3 spawnPos = target.position + Vector3.up * spawnHeight;
            var go = PoolManager.Instance.Spawn(EffectPrefab, spawnPos, Quaternion.identity);
            InitDamageDealer(go);
        }
    }
}
