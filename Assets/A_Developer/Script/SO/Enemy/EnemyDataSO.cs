using UnityEngine;

[CreateAssetMenu(menuName = "EnemySO/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Type")]
    public ZombieType zombieType;
    public MovementStyle movementStyle;

    [Header("Stats")]
    public float maxHealth;
    public float moveSpeed;
    public float damage;

    [Header("Combat")]
    public float attackRange;
    public float attackRate;

    [Header("Reward")]
    public int expReward = 5;
}

public enum ZombieType
{
    Normal,
    Clothed,
    Special
}

public enum MovementStyle
{
    Tank,
    Normal,
    Crawl
}