using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Data")] [SerializeField] public WeaponDataSO weaponData;
    [SerializeField] protected LayerMask enemyLayer;

    [Header("Config")] [SerializeField] protected TargetType targetType;
    [SerializeField] protected AttackType attackType;

    private int currentLevel = 0;
    protected float lastAttackTime;

    protected WeaponDataSO Data => weaponData;
    protected float AttackRate => weaponData.levels[currentLevel].attackRate;
    protected float Range => weaponData.levels[currentLevel].range;
    protected int Damage => weaponData.levels[currentLevel].damage;
    protected int MaxTarget => weaponData.levels[currentLevel].maxTarget;

    public void LevelUp()
    {
        if (currentLevel < weaponData.levels.Length - 1)
            currentLevel++;
    }

    // =========================
    // MAIN
    // =========================

    public void HandleAttack(Transform holdSpawn)
    {
        if (!CanAttack())
            return;

        List<Transform> targets = FindTargets();

        if (targets.Count == 0)
            return;

        ExecuteAttack(targets,holdSpawn);

        lastAttackTime = Time.time;
    }

    protected virtual bool CanAttack()
    {
        return Time.time >= lastAttackTime + AttackRate;
    }

    // =========================
    // TARGET
    // =========================

    protected virtual List<Transform> FindTargets()
    {
        switch (targetType)
        {
            case TargetType.Closest: return FindClosestTarget();
            case TargetType.Furthest: return FindFurthestTarget();
            case TargetType.Random: return FindRandomTarget(MaxTarget);
            case TargetType.AllInRange: return FindAllTargets();
        }

        return new List<Transform>();
    }

    protected virtual List<Transform> FindAllTargets()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Range, enemyLayer);

        List<Transform> result = new List<Transform>();

        foreach (var hit in hits)
            result.Add(hit.transform);

        return result;
    }

    protected virtual List<Transform> FindClosestTarget()
    {
        List<Transform> allTargets = FindAllTargets();

        if (allTargets.Count == 0)
            return new List<Transform>();

        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var target in allTargets)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target;
            }
        }

        return new List<Transform>() { closest };
    }

    protected virtual List<Transform> FindFurthestTarget()
    {
        List<Transform> allTargets = FindAllTargets();

        if (allTargets.Count == 0)
            return new List<Transform>();

        Transform furthest = null;
        float maxDistance = 0f;

        foreach (var target in allTargets)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthest = target;
            }
        }

        return new List<Transform>() { furthest };
    }

    protected virtual List<Transform> FindRandomTarget(int count)
    {
        List<Transform> allTargets = FindAllTargets();

        List<Transform> result = new List<Transform>();

        while (allTargets.Count > 0 && result.Count < count)
        {
            int randomIndex = Random.Range(0, allTargets.Count);
            result.Add(allTargets[randomIndex]);
            allTargets.RemoveAt(randomIndex);
        }

        return result;
    }

    // =========================
    // ATTACK
    // =========================

    protected virtual void ExecuteAttack(List<Transform> targets,Transform holdSpawn)
    {
        switch (attackType)
        {
            case AttackType.Projectile: SpawnProjectile(targets[0],holdSpawn); break;
            case AttackType.Lightning: SpawnLightning(targets,holdSpawn); break;
            case AttackType.Aura: AuraDamage(targets,holdSpawn); break;
            case AttackType.Meteor: SpawnMeteor(targets,holdSpawn); break;
        }
    }

    // =========================
    // ATTACK LOGIC
    // =========================

    protected virtual void SpawnProjectile(Transform target, Transform holdSpawn)
    {
    }

    protected virtual void SpawnLightning(List<Transform> targets, Transform holdSpawn)
    {
    }

    protected virtual void AuraDamage(List<Transform> targets, Transform holdSpawn)
    {
    }

    protected virtual void SpawnMeteor(List<Transform> targets, Transform holdSpawn)
    {
    }

    // =========================
    // DEBUG
    // =========================

    protected virtual void OnDrawGizmosSelected()
    {
        if (weaponData == null || weaponData.levels.Length == 0) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}

public enum TargetType
{
    Closest,
    Furthest,
    Random,
    AllInRange
}

public enum AttackType
{
    Projectile,
    Lightning,
    Aura,
    Meteor
}