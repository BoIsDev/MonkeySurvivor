using UnityEngine;

[CreateAssetMenu(menuName = "EnemySO/Enemy Data")]
public class EnemyDataSO : ScriptableObject
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
    Magic
}