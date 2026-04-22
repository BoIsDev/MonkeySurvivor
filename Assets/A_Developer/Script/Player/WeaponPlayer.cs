using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    public List<Weapon> weapons = new List<Weapon>();
    
    public void AddWeapon(Weapon weaponPrefab)
    {
        Weapon newWeapon = Instantiate(weaponPrefab, transform);
        weapons.Add(newWeapon);
    }

    void Update()
    {
        foreach (var weapon in weapons)
        {
            weapon.HandleAttack();
        }
    }
}
