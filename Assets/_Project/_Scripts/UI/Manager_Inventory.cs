using System;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Inventory : MonoBehaviour
{

    private static Action m_showHideInventoryAction = () => { };
    public static Action ShowHideInventoryAction => m_showHideInventoryAction;

    public static bool m_show = false;
    public static bool Show => m_show;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Image _imgBackground;
    [SerializeField] private GameObject _ctnInventory;

    void Awake()
    {
        _playerCamera.backgroundColor = _imgBackground.color;

        _ctnInventory.SetActive(false);
    }

    private void ShowHideInventory()
    {
        m_show = !m_show;

        _ctnInventory.SetActive(m_show);
    }

    void OnEnable()
    {
        m_showHideInventoryAction += ShowHideInventory;
    }

    void OnDisable()
    {
        m_showHideInventoryAction -= ShowHideInventory;
    }

}
