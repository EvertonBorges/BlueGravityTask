using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Manager_Inventory : MonoBehaviour
{

    private static Action m_showHideInventoryAction = () => { };
    public static Action ShowHideInventoryAction => m_showHideInventoryAction;
    private static Action<SO_BodyPart[]> m_startInventoryAction = (_) => { };
    public static Action<SO_BodyPart[]> StartInventoryAction => m_startInventoryAction;

    public static bool m_show = false;
    public static bool Show => m_show;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Image _imgBackground;
    [SerializeField] private GameObject _ctnInventory;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private Slot_Inventory_Item _slotPrefab;

    [SerializeField] private List<SO_BodyPart> _inventory;

    private ObjectPool<Slot_Inventory_Item> m_slots;
    private readonly List<Slot_Inventory_Item> m_usedSlots = new();

    private readonly List<SO_BodyPart> m_startInventory = new();

    void Awake()
    {
        _playerCamera.backgroundColor = _imgBackground.color;

        m_slots = new(
            () => Instantiate(_slotPrefab, _slotParent),
            slot => 
            {
                slot.gameObject.SetActive(true);
                slot.transform.SetAsLastSibling();
            },
            slot => slot.gameObject.SetActive(false),
            slot => Destroy(slot.gameObject),
            true,
            10,
            10000
        );

        _ctnInventory.SetActive(false);
    }

    private void StartInventory(SO_BodyPart[] soBodyParts)
    {
        m_startInventory.Clear();
        m_startInventory.AddRange(soBodyParts.ToList().FindAll(x => x.sprite != null || x.leftSprite != null).Distinct());

        _inventory.AddRange(m_startInventory);
        _inventory = _inventory.Distinct().ToList();
    }

    private void ShowHideInventory()
    {
        m_show = !m_show;

        if (m_show)
            Setup();

        _ctnInventory.SetActive(m_show);
    }

    private void Setup()
    {
        foreach (var slot in m_usedSlots)
            m_slots.Release(slot);

        m_usedSlots.Clear();

        foreach (var item in _inventory)
        {
            var slot = m_slots.Get();
            slot.Setup(item);
            slot.Equiped(m_startInventory.Contains(item));
            m_usedSlots.Add(slot);
        }
    }

    void OnEnable()
    {
        m_showHideInventoryAction += ShowHideInventory;
        m_startInventoryAction += StartInventory;
    }

    void OnDisable()
    {
        m_showHideInventoryAction -= ShowHideInventory;
        m_startInventoryAction -= StartInventory;
    }

}
