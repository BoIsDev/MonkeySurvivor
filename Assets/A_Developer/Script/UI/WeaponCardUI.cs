using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button selectButton;

    private WeaponDataSO weaponData;
    private UITestManager manager;

    public void Setup(WeaponDataSO data, UITestManager uiManager)
    {
        weaponData = data;
        manager    = uiManager;
        nameText.text = data.weaponName;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnSelect);
    }

    private void OnSelect() => manager.SelectWeapon(weaponData);
}
