using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    public List<WeaponBase> weaponsStore = new List<WeaponBase>();
    public List<WeaponBase> weaponsEquip = new List<WeaponBase>();
    public void AddWeapon(WeaponBase weaponBasePrefab)
    {
        WeaponBase newWeaponBase = Instantiate(weaponBasePrefab, transform);
        weaponsStore.Add(newWeaponBase);
    }

    void Update()
    {
        foreach (var weapon in weaponsEquip)
        {
            weapon.HandleAttack();
        }
    }
}
