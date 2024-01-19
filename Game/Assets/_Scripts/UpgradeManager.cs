using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    public UnityAction DataUpdated;

    public float PlayerHealth => 100 + 25 * YandexGame.savesData.healthUps;

    public float EnergyPerEgg => 1.0f + 0.4f * YandexGame.savesData.energyUps;

    public float PlayerFireCost => 1.0f / (2.0f + YandexGame.savesData.efficiencyUps);
    public int Coins => YandexGame.savesData.coins;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += OnSaveLoaded;
    }

    void OnDisable()
    {
        YandexGame.GetDataEvent -= OnSaveLoaded;
    }

    public bool TryUpgrade(UpgradeType type)
    {
        var coins = YandexGame.savesData.coins;
        var cost = GetUpgradeCost(type);
        if (cost > coins)
            return false;

        YandexGame.savesData.coins = coins - cost;
        switch (type)
        {
            case UpgradeType.HEALTH:
                YandexGame.savesData.healthUps += 1;
                break;

            case UpgradeType.ENERGY_AMOUNT:
                YandexGame.savesData.energyUps += 1;
                break;

            case UpgradeType.ENERGY_EFFICIENCY:
                YandexGame.savesData.efficiencyUps += 1;
                break;
        }

        DataUpdated.Invoke();
        return true;
    }

    public int GetUpgradeCost(UpgradeType type)
    {
        var save = YandexGame.savesData;
        switch (type)
        {
            case UpgradeType.HEALTH:
            {
                var curUpgrades = save.healthUps;

                var baseCost = 10.0;
                var extraCost = 2.0 + 0.1 * curUpgrades;
                var rawCost = baseCost + extraCost * curUpgrades;
                return (int)System.Math.Ceiling(rawCost);
            }
                

            case UpgradeType.ENERGY_AMOUNT:
            {
                var curUpgrades = save.energyUps;

                var baseCost = 5.0;
                var extraCost = 2.5 + 0.2 * curUpgrades;
                var rawCost = baseCost + extraCost * curUpgrades;
                return (int)System.Math.Ceiling(rawCost);
            }

            case UpgradeType.ENERGY_EFFICIENCY:
            {
                var curUpgrades = save.efficiencyUps;

                var baseCost = 8.0;
                var extraCost = 2.0 + 0.3 * curUpgrades;
                var rawCost = baseCost + extraCost * curUpgrades;
                return (int)System.Math.Ceiling(rawCost);
            }
        }

        throw new System.NotImplementedException();
    }

    private void OnSaveLoaded()
    {
        DataUpdated.Invoke();
    }

    public enum UpgradeType
    {
        HEALTH, ENERGY_AMOUNT, ENERGY_EFFICIENCY
    }
}