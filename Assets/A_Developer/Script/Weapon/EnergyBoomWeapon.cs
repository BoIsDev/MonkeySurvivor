using System.Collections.Generic;
using UnityEngine;

public class EnergyBoomWeaponBase : WeaponBase
{
    [Header("Boom Setting")]
    [SerializeField] private GameObject boomPrefab;
    [SerializeField] private float spawnHeight = 1f;
    protected override bool CanAttack()
    {
        if (!base.CanAttack()) return false;

        return FindTargets().Count > 0;
    }
    protected override void SpawnProjectile(Transform target)
    {
        throw new System.NotImplementedException();
    }

    protected override void SpawnLightning(List<Transform> targets)
    {
        throw new System.NotImplementedException();
    }

    protected override void AuraDamage(List<Transform> targets)
    {
        throw new System.NotImplementedException();
    }

    protected override void SpawnMeteor(List<Transform> targets)
    {
        foreach (Transform target in targets)
        {
            Vector3 spawnPos = target.position + Vector3.up * spawnHeight;

            Instantiate(boomPrefab, spawnPos, Quaternion.identity);
        }
    }
}
