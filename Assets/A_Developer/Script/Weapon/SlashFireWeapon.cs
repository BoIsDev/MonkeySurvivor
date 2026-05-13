using UnityEngine;

public class SlashFireWeapon : WeaponBase
{
    [SerializeField] private float spawnDistance = 1f;

    protected override void SpawnSlash(Transform target)
    {
        // Hướng từ player đến enemy
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;

        // Spawn cách player 1m theo hướng đó, nâng lên 0.86 trên Y
        Vector3 spawnPos = transform.position + dir * spawnDistance;
        spawnPos.y += 0.86f;

        // Xoay effect về hướng player → enemy (lật ngược dir)
        Quaternion rot = Quaternion.LookRotation(-dir);

        // Spawn làm con của Player → di chuyển theo player
        GameObject slash = Instantiate(effectPrefab, spawnPos, rot);
        slash.transform.SetParent(transform);
    }
}
