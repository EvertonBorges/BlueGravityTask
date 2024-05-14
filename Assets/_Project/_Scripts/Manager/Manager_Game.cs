using System;
using UnityEngine;

public class Manager_Game : MonoBehaviour
{

    [SerializeField] private int _startCoins = 10000;

    void Awake()
    {
        System_Inventory.StartCoin(_startCoins);
    }

    void Start()
    {
        Manager_UI.OnUpdateCoinsAction();
    }

}
