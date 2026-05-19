using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Data")] [SerializeField] private WeaponDataSO weaponData;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Effect")] [SerializeField] private GameObject effectPrefab;
    protected GameObject EffectPrefab => effectPrefab;

    [Header("Config")] [SerializeField] protected TargetType targetType;
    [SerializeField] protected AttackType attackType;

    private int currentLevel = 0;
    protected float lastAttackTime;

    protected float AttackRate => weaponData.levels[currentLevel].attackRate;
    protected float Range => weaponData.levels[currentLevel].range;
    protected int Damage => weaponData.levels[currentLevel].damage;
    protected int MaxTarget => weaponData.levels[currentLevel].maxTarget;

    // Called by WeaponPlayer after instantiation to inject weapon data at runtime.
    public void Init(WeaponDataSO data) => weaponData = data;

    public void Tick()
    {
        if (weaponData == null) return;
        HandleAttack();
    }

    public void LevelUp()
    {
        if (currentLevel < weaponData.levels.Length - 1)
            currentLevel++;
    }

    // =========================
    // MAIN
    // =========================

    public void HandleAttack()
    {
        if (!CanAttack()) return;

        List<Transform> targets = FindTargets();
        if (targets.Count == 0) return;

        ExecuteAttack(targets);
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
        List<Transform> all = FindAllTargets();
        if (all.Count == 0) return all;

        Transform closest = null;
        float minDist = Mathf.Infinity;
        foreach (var t in all)
        {
            float d = Vector3.Distance(transform.position, t.position);
            if (d < minDist)
            {
                minDist = d;
                closest = t;
            }
        }

        return new List<Transform>() { closest };
    }

    protected virtual List<Transform> FindFurthestTarget()
    {
        List<Transform> all = FindAllTargets();
        if (all.Count == 0) return all;

        Transform furthest = null;
        float maxDist = 0f;
        foreach (var t in all)
        {
            float d = Vector3.Distance(transform.position, t.position);
            if (d > maxDist)
            {
                maxDist = d;
                furthest = t;
            }
        }

        return new List<Transform>() { furthest };
    }

    protected virtual List<Transform> FindRandomTarget(int count)
    {
        List<Transform> all = FindAllTargets();
        List<Transform> result = new List<Transform>();
        while (all.Count > 0 && result.Count < count)
        {
            int i = Random.Range(0, all.Count);
            result.Add(all[i]);
            all.RemoveAt(i);
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
            case AttackType.Projectile: SpawnProjectile(targets[0]); break;
            case AttackType.Slash: SpawnSlash(targets[0]); break;
            case AttackType.Aura: AuraDamage(targets); break;
            case AttackType.Meteor: SpawnMeteor(targets); break;
        }
    }

    // Passes the current damage value to the DamageDealer on a spawned effect prefab.
    protected void InitDamageDealer(GameObject go)
    {
        go.GetComponent<DamageDealer>()?.SetDamage(Damage);
        Debug.Log("Game Object Damage Dealer " + go + " damage " + Damage );
    }

    protected virtual void SpawnProjectile(Transform target)
    {
    }

    protected virtual void SpawnSlash(Transform targets)
    {
    }

    protected virtual void AuraDamage(List<Transform> targets)
    {
    }

    protected virtual void SpawnMeteor(List<Transform> targets)
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
    Slash,
    Aura,
    Meteor
}