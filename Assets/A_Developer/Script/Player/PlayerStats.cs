using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerDataSO data;

    public float MoveSpeed { get; private set; }
    public float Damage { get; private set; }
    public int CurrentExp { get; private set; }
    public int Level { get; private set; } = 1;

    private void Awake()
    {
        MoveSpeed = data.moveSpeed;
        Damage = data.damage;
    }

    public void AddExp(int amount)
    {
        CurrentExp += amount;
        if (CurrentExp >= Level * 10) LevelUp();
    }

    private void LevelUp()
    {
        Level++;
        CurrentExp = 0;
        UITestManager.Instance.OpenPaneWeapon();
        Debug.Log("Level Up!");
    }
}
