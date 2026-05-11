using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Stats")] [SerializeField] private float rangeTarget = 10f;
    [SerializeField] protected float attackRate = 1f;
    [SerializeField] protected float range = 5f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int maxTarget = 2;
    [SerializeField] protected LayerMask enemyLayer;

    [Header("Target")] [SerializeField] protected WeaponType weaponType;
    [Header("Target")] [SerializeField] protected TargetType targetType;
    [Header("Attack")] [SerializeField] protected AttackType attackType;

    protected float lastAttackTime;

    // =========================
    // MAIN
    // =========================

    protected virtual void Update()
    {
        HandleAttack();
    }

    public void HandleAttack()
    {
        if (!CanAttack())
            return;

        List<Transform> targets = FindTargets();

        if (targets.Count == 0)
            return;

        ExecuteAttack(targets);

        lastAttackTime = Time.time;
    }

    protected virtual bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackRate;
    }

    // =========================
    // TARGET
    // =========================

    protected virtual List<Transform> FindTargets()
    {
        switch (targetType)
        {
            case TargetType.Closest:
                return FindClosestTarget();

            case TargetType.Furthest:
                return FindFurthestTarget();

            case TargetType.Random:
                return FindRandomTarget(maxTarget);

            case TargetType.AllInRange:
                return FindAllTargets();
        }

        return new List<Transform>();
    }

    protected virtual List<Transform> FindAllTargets()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            range,
            enemyLayer
        );

        List<Transform> result = new List<Transform>();

        foreach (var hit in hits)
        {
            result.Add(hit.transform);
        }

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
            float distance =
                Vector3.Distance(transform.position, target.position);

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
            float distance =
                Vector3.Distance(transform.position, target.position);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthest = target;
            }
        }

        return new List<Transform>() { furthest };
    }

    protected virtual List<Transform> FindRandomTarget(int maxTarget)
    {
        List<Transform> allTargets = FindAllTargets();

        List<Transform> result = new List<Transform>();

        while (allTargets.Count > 0 &&
               result.Count < maxTarget)
        {
            int randomIndex =
                Random.Range(0, allTargets.Count);

            result.Add(allTargets[randomIndex]);

            allTargets.RemoveAt(randomIndex);
        }

        return result;
    }

    // =========================
    // ATTACK
    // =========================

    protected virtual void ExecuteAttack(List<Transform> targets)
    {
        switch (attackType)
        {
            case AttackType.Projectile:
                SpawnProjectile(targets[0]);
                break;

            case AttackType.Lightning:
                SpawnLightning(targets);
                break;

            case AttackType.Aura:
                AuraDamage(targets);
                break;

            case AttackType.Meteor:
                SpawnMeteor(targets);
                break;
        }
    }

    // =========================
    // ATTACK LOGIC
    // =========================

    protected abstract void SpawnProjectile(Transform target);

    protected abstract void SpawnLightning(List<Transform> targets);

    protected abstract void AuraDamage(List<Transform> targets);

    protected abstract void SpawnMeteor(List<Transform> targets);
    
    // =========================
    // DEBUG
    // =========================

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
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
public enum WeaponType
{
    EnergyBoomWeapon,
    IceAuraWeapon,
    Fireball,
}
