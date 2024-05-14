using System;
using UnityEngine;

public class Manager_Game : MonoBehaviour
{

    private static Action<int> m_onAddCoinAction = (_) => { };
    public static Action<int> OnAddCoinAction => m_onAddCoinAction;

    private int m_coins = 0;

    private void OnAddCoin(int value)
    {
        m_coins += value;
        
        Manager_UI.OnUpdateCoinsAction(m_coins);
    }

    void OnEnable()
    {
        m_onAddCoinAction += OnAddCoin;
    }

    void OnDisable()
    {
        m_onAddCoinAction -= OnAddCoin;
    }

}
