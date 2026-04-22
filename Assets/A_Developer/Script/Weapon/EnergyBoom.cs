using System.Collections.Generic;
using UnityEngine;

public class EnergyBoom : Weapon
{
    [Header("Boom Setting")]
    public float range = 10f;
    public int maxTarget = 2;
    public GameObject boomPrefab;
    public float spawnHeight = 1f;

    protected override void Awake()
    {
        base.Awake();
        attackRate = 3f; // cooldown 3 giây
    }
    
    protected override void Attack()
    {
        List<Transform> targets = FindTargets();

        if (targets.Count == 0) return;

        foreach (var target in targets)
        {
            SpawnBoom(target);
        }
    }

    protected override bool CanAttack()
    {
        if (!base.CanAttack()) return false;

        return FindTargets().Count > 0;
    }
    
    // ================= LOGIC =================

    List<Transform> FindTargets()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        List<Transform> result = new List<Transform>();

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            result.Add(hit.transform);

            if (result.Count >= maxTarget)
                break;
        }

        return result;
    }

    void SpawnBoom(Transform target)
    {
        Vector3 spawnPos = target.position + Vector3.up * spawnHeight;

        Instantiate(boomPrefab, spawnPos, Quaternion.identity);
    }
}
