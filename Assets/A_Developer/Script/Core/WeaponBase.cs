using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base for all player weapons: owns attack timing, target finding and level stats.
/// Subclasses only implement SpawnEffect to define what the attack looks like.
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    [Header("Data")] [SerializeField] private WeaponDataSO weaponData;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Effect")] [SerializeField] private GameObject effectPrefab;
    protected GameObject EffectPrefab => effectPrefab;

    [Header("Config")] [SerializeField] protected TargetType targetType;

    private int currentLevel = 0;
    protected float lastAttackTime;

    protected float AttackRate => weaponData.levels[currentLevel].attackRate;
    protected float Range => weaponData.levels[currentLevel].range;
    protected int Damage => weaponData.levels[currentLevel].damage;
    protected int MaxTarget => weaponData.levels[currentLevel].maxTarget;
    protected float EffectScale => weaponData.levels[currentLevel].effectScale;

    // Called by WeaponPlayer after instantiation to inject weapon data at runtime.
    public void Init(WeaponDataSO data) => weaponData = data;

    // True once this instance is the evolved form — prevents WeaponPlayer
    // from evolving the same slot again (the evolved form shares the base SO).
    public bool IsEvolved { get; private set; }

    // Evolved form reuses the SAME data as the base weapon, locked at the last level entry.
    public void InitAsEvolved(WeaponDataSO data)
    {
        weaponData = data;
        currentLevel = data.levels.Length - 1;
        IsEvolved = true;
    }

    public WeaponDataSO WeaponData => weaponData;

    public void Tick()
    {
        if (weaponData == null) return;
        HandleAttack();
    }

    // True when one more upgrade would reach the LAST level entry.
    // Convention: the last entry holds the evolved form's stats, the second-to-last
    // is the base weapon's max. So WeaponPlayer evolves on this step instead of
    // leveling the base form into that final entry.
    public bool NextLevelIsMax => weaponData != null && currentLevel + 1 >= weaponData.levels.Length - 1;

    public void LevelUp()
    {
        if (currentLevel < weaponData.levels.Length - 1)
            currentLevel++;
    }

    // =========================
    // MAIN
    // =========================

    public virtual void HandleAttack()
    {
        if (!CanAttack()) return;

        List<Transform> targets = FindTargets();
        if (targets.Count == 0) return;

        SpawnEffect(targets);
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

    // The single extension point: each weapon decides how to spawn its own effect
    // (single slash, AOE, projectile per target...).
    // HandleAttack guarantees targets contains at least one element.
    protected abstract void SpawnEffect(List<Transform> targets);

    // Passes the current damage value to the DamageDealer on a spawned effect prefab.
    protected void InitDamageDealer(GameObject go)
    {
        go.GetComponent<DamageDealer>()?.SetDamage(Damage);
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