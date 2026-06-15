using System.Collections.Generic;
using UnityEngine;

public class SlashShootWeapon : WeaponBase
{
    [SerializeField] private float spawnDistance = 1f;

    protected override void SpawnEffect(List<Transform> targets)
    {
        Transform target = targets[0];

        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;

        Vector3 spawnPos = transform.position + dir * spawnDistance;
        // Raise the slash from ground level up to the character's torso height.
        spawnPos.y += 0.86f;

        Quaternion rot = Quaternion.LookRotation(-dir);

        GameObject slash = PoolManager.Instance.Spawn(EffectPrefab, spawnPos, rot);
        slash.transform.localScale = Vector3.one * EffectScale;
        InitDamageDealer(slash);
    }
}
