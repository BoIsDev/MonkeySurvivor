using System.Collections.Generic;
using UnityEngine;

public class EnergyBoomWeapon : WeaponBase
{
    [Header("Boom Setting")]
    [SerializeField] private float spawnHeight = 1f;

    protected override void SpawnMeteor(List<Transform> targets)
    {
        foreach (Transform target in targets)
        {
            Vector3 spawnPos = target.position + Vector3.up * spawnHeight;
            var go = Instantiate(EffectPrefab, spawnPos, Quaternion.identity);
            InitDamageDealer(go);
        }
    }
}
