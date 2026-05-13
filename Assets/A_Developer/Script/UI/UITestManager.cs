using System.Collections.Generic;
using UnityEngine;

public class UITestManager : MonoBehaviour
{
    public static UITestManager Instance;

    [SerializeField] private WeaponPlayer weaponPlayer;
    [SerializeField] private Transform panelChoiceWeapon;
    [SerializeField] private List<WeaponDataSO> allWeapons = new List<WeaponDataSO>();
    [SerializeField] private WeaponCardUI[] weaponCards;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        panelChoiceWeapon.gameObject.SetActive(false);
    }

    // Gán vào OnClick nút "Level Up"
    public void OnClickLevelUp()
    {
        panelChoiceWeapon.gameObject.SetActive(true);
        Time.timeScale = 0f;

        for (int i = 0; i < weaponCards.Length; i++)
        {
            if (i < allWeapons.Count)
            {
                weaponCards[i].gameObject.SetActive(true);
                weaponCards[i].Setup(allWeapons[i], this);
            }
            else
            {
                weaponCards[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectWeapon(WeaponDataSO weaponData)
    {
        weaponPlayer.AddWeaponInventory(weaponData);
        ClosePanel();
    }

    public void ClosePanel()
    {
        panelChoiceWeapon.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenPaneWeapon()
    {
        panelChoiceWeapon.gameObject.SetActive(true);
    }
}
