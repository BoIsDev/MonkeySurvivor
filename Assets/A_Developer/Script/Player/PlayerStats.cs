using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Data")]
    public PlayerDataSO data; // ScriptableObject

    [Header("Runtime")]
    public float currentHealth;
    public int currentExp;
    public int level = 1;

    public float MoveSpeed { get; private set; }
    public float Damage { get; private set; }

    void Awake()
    {
        Init();
    }

    void Init()
    {
        MoveSpeed = data.moveSpeed;
        Damage = data.damage;

        currentHealth = data.maxHealth;
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        if (currentExp >= GetExpToNextLevel())
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        currentExp = 0;

        Debug.Log("Level Up!");
    }

    int GetExpToNextLevel()
    {
        return level * 10; // ví dụ
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Player Dead");
    }
}