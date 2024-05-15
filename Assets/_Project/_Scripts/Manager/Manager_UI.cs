using System;
using TMPro;
using UnityEngine;

public class Manager_UI : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _txtCoin;

    private static Action m_onUpdateCoinsAction = () => { };
    public static Action OnUpdateCoinsAction => m_onUpdateCoinsAction;

    void Awake()
    {
        OnUpdateCoins();
    }

    private void OnUpdateCoins()
    {
        var coins = System_Inventory.Coins;

        _txtCoin.SetText($"{coins}");
    }

    void OnEnable()
    {
        m_onUpdateCoinsAction += OnUpdateCoins;
    }

    void OnDisable()
    {
        m_onUpdateCoinsAction -= OnUpdateCoins;
    }

}
