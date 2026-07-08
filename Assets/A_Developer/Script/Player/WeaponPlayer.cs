using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    [SerializeField] private List<WeaponDataSO> weaponsStore = new List<WeaponDataSO>();

    private Dictionary<WeaponType, WeaponBase> weaponsEquip = new Dictionary<WeaponType, WeaponBase>();

    public void AddWeaponInventory(WeaponDataSO weaponData)
    {
        if (weaponsEquip.ContainsKey(weaponData.weaponType))
        {
            var weapon = weaponsEquip[weaponData.weaponType];
            var currentData = weapon.WeaponData;

            if (weapon.NextLevelIsMax && !weapon.IsEvolved && currentData.evolvedPrefab != null)
                EvolveWeapon(weaponData.weaponType, currentData);
            else
                weapon.LevelUp(); // caps at max; no-op when already evolved
            return;
        }

        WeaponBase newWeapon = Instantiate(weaponData.weaponPrefab, transform);
        newWeapon.Init(weaponData);
        weaponsEquip.Add(weaponData.weaponType, newWeapon);
    }

    // Evolution = swap the weapon prefab in place; same SO, same slot, stats locked at max level.
    private void EvolveWeapon(WeaponType type, WeaponDataSO data)
    {
        Destroy(weaponsEquip[type].gameObject);

        WeaponBase evolved = Instantiate(data.evolvedPrefab, transform);
        evolved.InitAsEvolved(data);

        weaponsEquip[type] = evolved;

        Debug.Log($"[WeaponPlayer] {type} evolved");
    }

    public bool HasWeapon(WeaponType type) => weaponsEquip.ContainsKey(type);

    void Update()
    {
        foreach (var weapon in weaponsEquip.Values)
            weapon.Tick();
    }
}
