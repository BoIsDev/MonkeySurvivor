using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    [SerializeField] private List<WeaponDataSO> weaponsStore = new List<WeaponDataSO>();

    private Dictionary<WeaponType, WeaponBase> weaponsEquip = new Dictionary<WeaponType, WeaponBase>();
    private Dictionary<WeaponType, WeaponDataSO> weaponDataMap = new Dictionary<WeaponType, WeaponDataSO>();

    public void AddWeaponInventory(WeaponDataSO weaponData)
    {
        if (weaponsEquip.ContainsKey(weaponData.weaponType))
        {
            var weapon = weaponsEquip[weaponData.weaponType];
            var currentData = weaponDataMap[weaponData.weaponType];

            if (weapon.IsMaxLevel && currentData.evolvedData != null)
                EvolveWeapon(weaponData.weaponType, currentData.evolvedData);
            else
                weapon.LevelUp();
            return;
        }

        WeaponBase newWeapon = Instantiate(weaponData.weaponPrefab, transform);
        newWeapon.Init(weaponData);
        weaponsEquip.Add(weaponData.weaponType, newWeapon);
        weaponDataMap.Add(weaponData.weaponType, weaponData);
    }

    private void EvolveWeapon(WeaponType type, WeaponDataSO evolvedData)
    {
        Destroy(weaponsEquip[type].gameObject);

        WeaponBase evolved = Instantiate(evolvedData.weaponPrefab, transform);
        evolved.Init(evolvedData);

        weaponsEquip[type] = evolved;
        weaponDataMap[type] = evolvedData;

        Debug.Log($"[WeaponPlayer] {type} evolved → {evolvedData.weaponName}");
    }

    public bool HasWeapon(WeaponType type) => weaponsEquip.ContainsKey(type);

    void Update()
    {
        foreach (var weapon in weaponsEquip.Values)
            weapon.Tick();
    }
}
