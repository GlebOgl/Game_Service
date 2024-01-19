using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class Upgrade : MonoBehaviour
{
    public TextMeshProUGUI costText, currentStatText;

    public UpgradeManager.UpgradeType upgradeType;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonClicked);
        UpdateText();
    }

    void OnEnable()
    {
        UpgradeManager.Instance.DataUpdated += UpdateText;
    }

    void OnDisable()
    {
        UpgradeManager.Instance.DataUpdated -= UpdateText;
    }

    private void ButtonClicked()
    {
        UpgradeManager.Instance.TryUpgrade(upgradeType);
    }

    private void UpdateText()
    {
        currentStatText.text = GetStatText();
        costText.text = "Стоимость: " + UpgradeManager.Instance.GetUpgradeCost(upgradeType);
    }

    private string GetStatText()
    {
        switch (upgradeType)
        {
            case UpgradeManager.UpgradeType.HEALTH:
                return "" + UpgradeManager.Instance.PlayerHealth;

            case UpgradeManager.UpgradeType.ENERGY_AMOUNT:
                return "" + UpgradeManager.Instance.EnergyPerEgg;

            case UpgradeManager.UpgradeType.ENERGY_EFFICIENCY:
                return "" + UpgradeManager.Instance.EnergyPerShot;
        }
        throw new System.NotImplementedException();
    }
}
