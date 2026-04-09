using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public EnemyStyle enemyStyle;
    public float maxHealth;
    public float moveSpeed;
    public float damage;

    public float attackRange;
    public float attackRate;
}

public enum EnemyStyle
{
    Melee,
    magic
}