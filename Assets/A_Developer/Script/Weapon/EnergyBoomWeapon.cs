using System.Collections.Generic;
using UnityEngine;

public class EnergyBoomWeaponBase : WeaponBase
{
    [Header("Boom Setting")]
    [SerializeField] private float spawnHeight = 1f;

    protected override void SpawnMeteor(List<Transform> targets, Transform holdSpawn)
    {
        foreach (Transform target in targets)
        {
            Vector3 spawnPos = target.position + Vector3.up * spawnHeight;
            WeaponBase effect = Instantiate(weaponData.weaponPrefab, spawnPos, Quaternion.identity);
            effect.transform.parent = holdSpawn;
        }
    }
}
