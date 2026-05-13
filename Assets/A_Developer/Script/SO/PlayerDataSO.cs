using UnityEngine;

[CreateAssetMenu(menuName = "PlayerSO/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public float maxHealth;
    public float moveSpeed;
    public float damage;

    // public Weapon defaultWeapon;
}