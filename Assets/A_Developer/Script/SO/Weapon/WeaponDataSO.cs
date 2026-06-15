using UnityEngine;

// Identifies the weapon SLOT, not the form: every form in an evolution chain
// (e.g. Slash -> Evolution_Slash) shares the same value so it occupies one slot.
public enum WeaponType { IceAura, EnergyBoom, Slash }

[CreateAssetMenu(menuName = "WeaponSO/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Type")]
    public WeaponType weaponType;

    [Header("Info")]
    public string weaponName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("Prefab")]
    // The prefab must NOT have weaponData assigned — it is injected at runtime via Init().
    public WeaponBase weaponPrefab;

    [Header("Levels")]
    // Index 0 = level 1. Each entry upgrades all stats at once.
    public WeaponLevelData[] levels;

    [Header("Evolution")]
    // Evolved form swaps the prefab only — it reuses this SO, locked at the last level.
    // null = this weapon has no evolved form.
    public WeaponBase evolvedPrefab;
}

[System.Serializable]
public class WeaponLevelData
{
    public float attackRate = 1f;
    public float range = 5f;
    public int damage = 10;
    public int maxTarget = 1;
    public float effectScale = 1f;
}
