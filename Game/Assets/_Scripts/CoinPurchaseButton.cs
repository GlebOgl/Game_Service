using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

public class CoinPurchaseButton : MonoBehaviour
{
    private static readonly Dictionary<int, string> COIN_PURCHASE_IDS = new Dictionary<int, string>()
    {
        [25] = "coins25",
        [50] = "coins50",
        [200] = "coins200",
        [1000] = "coins1000",
    };

    public TextMeshProUGUI buttonText;

    public int amount = 25;


    void Start()
    {
        if (!COIN_PURCHASE_IDS.ContainsKey(amount))
        {
            gameObject.SetActive(false);
            throw new System.Exception("Unregistered amount of coins: "+amount);
        }

        buttonText.text = $"Купить {amount} монет";
        GetComponent<Button>().onClick.AddListener(() => PurchaseCoins(amount));
    }

    private static void PurchaseCoins(int amount)
    {
        var pur = YandexGame.PurchaseByID(COIN_PURCHASE_IDS[amount]);
        if (pur == null)
            return;
        
        UpgradeManager.Instance.AddCoins(amount);

        YandexGame.ConsumePurchaseByID(pur.id);
    }
}
