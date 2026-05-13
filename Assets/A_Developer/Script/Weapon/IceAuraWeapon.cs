using System.Collections.Generic;
using UnityEngine;

public class IceAuraGameObject : WeaponBase
{
    private GameObject spawnedAura;

    protected override void AuraDamage(List<Transform> targets)
    {
        if (spawnedAura != null) return;

        spawnedAura = Instantiate(effectPrefab, transform);
    }
}
