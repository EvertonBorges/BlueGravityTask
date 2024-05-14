using System;
using TMPro;
using UnityEngine;

public class Manager_UI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _txtCoin;

    private static Action<int> m_onUpdateCoinsAction = (_) => { };
    public static Action<int> OnUpdateCoinsAction => m_onUpdateCoinsAction;

    void Awake()
    {
        OnUpdateCoins(0);
    }

    private void OnUpdateCoins(int value)
    {
        _txtCoin.SetText($"{value}");
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
