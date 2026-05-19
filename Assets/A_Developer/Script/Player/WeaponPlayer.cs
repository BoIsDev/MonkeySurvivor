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
            weaponsEquip[weaponData.weaponType].LevelUp();
            return;
        }

        WeaponBase newWeapon = Instantiate(weaponData.weaponPrefab, transform);
        newWeapon.Init(weaponData);
        weaponsEquip.Add(weaponData.weaponType, newWeapon);
    }

    public bool HasWeapon(WeaponType type) => weaponsEquip.ContainsKey(type);

    void Update()
    {
        foreach (var weapon in weaponsEquip.Values)
            weapon.Tick();
    }
}
