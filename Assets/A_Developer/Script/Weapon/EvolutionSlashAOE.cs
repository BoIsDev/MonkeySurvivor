using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Evolved form of the slash weapon: spawns a self-centered AOE effect.
/// </summary>
public class EvolutionSlashAOE : WeaponBase
{
    // Ignores targets — FindTargets only gates the attack on having an enemy in range.

    protected override void SpawnEffect(List<Transform> targets)
    {
        GameObject slash = PoolManager.Instance.Spawn(EffectPrefab, transform.position, transform.rotation);
        InitDamageDealer(slash);
    }
}
