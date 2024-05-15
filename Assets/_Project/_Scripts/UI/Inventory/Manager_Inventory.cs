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
    private static Action<Slot_Inventory_Item> m_selectionAction = (_) => { };
    public static Action<Slot_Inventory_Item> SelectionAction => m_selectionAction;
    private static Action m_setupAction = () => { };
    public static Action SetupAction => m_setupAction;

    [Header("References")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Image _imgBackground;
    [SerializeField] private GameObject _ctnInventory;
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private Slot_Inventory_Item _slotPrefab;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    private ObjectPool<Slot_Inventory_Item> m_slots;
    private readonly List<Slot_Inventory_Item> m_usedSlots = new();
    private int m_currentSlotIndex = -1;

    public static bool m_show = false;
    public static bool Show => m_show;

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

    private void ShowHideInventory()
    {
        m_show = !m_show;

        if (m_show)
        {
            m_currentSlotIndex = -1;

            Setup(true);
        }

        _ctnInventory.SetActive(m_show);
    }

    private void Setup(bool changeSelection)
    {
        foreach (var slot in m_usedSlots)
            m_slots.Release(slot);

        m_usedSlots.Clear();

        var bodyParts = PlayerController.Instance.BodyParts;

        foreach (var item in System_Inventory.Inventory)
        {
            var slot = m_slots.Get();
            slot.Setup(item);
            slot.Equiped(bodyParts.Contains(item));
            m_usedSlots.Add(slot);
        }

        if (m_currentSlotIndex < 0 || (changeSelection && !m_usedSlots.IsEmpty()))
            m_currentSlotIndex = 0;

        m_usedSlots[m_currentSlotIndex].Select(true);
    }

    private void Setup()
    {
        Setup(false);
    }

    private void Selection(Slot_Inventory_Item slot)
    {
        var index = m_usedSlots.IndexOf(slot);

        if (m_currentSlotIndex == index)
            return;

        foreach (var item in m_usedSlots)
            item.Select(false);

        slot.Select(true);
        m_currentSlotIndex = index;

        Vector2 margin = new(0f, _gridLayoutGroup.padding.vertical / 2f);
        _scroll.SnapToTarget(slot.RectTransform, margin);
    }

    void OnEnable()
    {
        m_showHideInventoryAction += ShowHideInventory;
        m_selectionAction += Selection;
        m_setupAction += Setup;
    }

    void OnDisable()
    {
        m_showHideInventoryAction -= ShowHideInventory;
        m_selectionAction -= Selection;
        m_setupAction -= Setup;
    }

}
