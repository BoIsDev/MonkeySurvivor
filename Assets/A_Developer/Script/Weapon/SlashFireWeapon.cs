using UnityEngine;

public class SlashFireWeapon : WeaponBase
{
    [SerializeField] private float spawnDistance = 1f;

    protected override void SpawnSlash(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;

        Vector3 spawnPos = transform.position + dir * spawnDistance;
        spawnPos.y += 0.86f;

        Quaternion rot = Quaternion.LookRotation(-dir);

        GameObject slash = Instantiate(EffectPrefab, spawnPos, rot);
        slash.transform.SetParent(transform);
        InitDamageDealer(slash);
    }
}
