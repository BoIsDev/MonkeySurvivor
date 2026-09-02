using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerDataSO data;
    [SerializeField] private Health health;

    public float MoveSpeed { get; private set; }
    public float Damage { get; private set; }
    public int CurrentExp { get; private set; }
    public int Level { get; private set; } = 1;
    public int ExpToNextLevel => Level * 10;

    public event Action<int, int> OnExpChanged;   // (current, needed)
    public event Action<int> OnLevelChanged;      // (level)

    private void OnEnable() => health.OnDied += OnDied;
    private void OnDisable() => health.OnDied -= OnDied;

    private void OnDied() => Debug.Log("Player Died");

    private void Awake()
    {
        MoveSpeed = data.moveSpeed;
        Damage = data.damage;
    }

    public void AddExp(int amount)
    {
        CurrentExp += amount;
        if (CurrentExp >= ExpToNextLevel) LevelUp();
        OnExpChanged?.Invoke(CurrentExp, ExpToNextLevel);
    }

    private void LevelUp()
    {
        Level++;
        CurrentExp = 0;
        OnLevelChanged?.Invoke(Level);
        UITestManager.Instance.OnClickLevelUp();   // opens AND repopulates the weapon-pick panel
    }
}
