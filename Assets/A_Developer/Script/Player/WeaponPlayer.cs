using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    // Set sẵn trong Editor — pool vũ khí có thể nhận
    [SerializeField] private List<WeaponDataSO> weaponsStore = new List<WeaponDataSO>();

    // Runtime — vũ khí đang trang bị (tự chạy Update của riêng chúng)
    private Dictionary<WeaponType, WeaponBase> weaponsEquip = new Dictionary<WeaponType, WeaponBase>();

    public void AddWeaponInventory(WeaponDataSO weaponData)
    {
        if (weaponsEquip.ContainsKey(weaponData.weaponType))
        {
            weaponsEquip[weaponData.weaponType].LevelUp();
            return;
        }

        WeaponBase newWeapon = Instantiate(weaponData.weaponPrefab, transform);
        newWeapon.weaponData = weaponData;
        weaponsEquip.Add(weaponData.weaponType, newWeapon);
    }

    public bool HasWeapon(WeaponType type) => weaponsEquip.ContainsKey(type);

    void Update()
    {
        foreach (var weapon in weaponsEquip.Values)
            weapon.Tick();
    }
}
