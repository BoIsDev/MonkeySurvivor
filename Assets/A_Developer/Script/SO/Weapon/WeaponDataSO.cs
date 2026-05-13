using UnityEngine;

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
    public WeaponBase weaponPrefab;  // để weaponData = null trong prefab

    [Header("Levels")]
    public WeaponLevelData[] levels;
}

[System.Serializable]
public class WeaponLevelData
{
    public float attackRate = 1f;
    public float range = 5f;
    public int damage = 10;
    public int maxTarget = 1;
}
