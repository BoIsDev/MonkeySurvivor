using System.Collections.Generic;
using UnityEngine;

public class IceAuraWeaponBase : WeaponBase
{
    [Header("IceAura Setting")]
    [SerializeField] private GameObject iceAuraPrefab;


    protected override void SpawnProjectile(Transform target)
    {
        throw new System.NotImplementedException();
    }

    protected override void SpawnLightning(List<Transform> targets)
    {
        throw new System.NotImplementedException();
    }

    protected override void AuraDamage(List<Transform> targets)
    {
        iceAuraPrefab.SetActive(true);
    }

    protected override void SpawnMeteor(List<Transform> targets)
    {
        throw new System.NotImplementedException();
    }
}
