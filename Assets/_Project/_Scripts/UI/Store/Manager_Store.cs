using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Manager_Store : MonoBehaviour
{

    private static Action m_showHideAction = () => { };
    public static Action ShowHideAction => m_showHideAction;
    private static Action<Slot_Store_Item> m_selectionAction = (_) => { };
    public static Action<Slot_Store_Item> SelectionAction => m_selectionAction;
    private static Action m_setupAction = () => { };
    public static Action SetupAction => m_setupAction;

    [SerializeField] private List<SO_BodyPart> _itemsToSell;
    
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private GameObject _ctnStore;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private Slot_Store_Item _slotPrefab;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

    private ObjectPool<Slot_Store_Item> m_slots;
    private readonly List<Slot_Store_Item> m_usedSlots = new();
    private int m_currentSlotIndex = -1;

    public static bool m_show = false;
    public static bool Show => m_show;

    void Awake()
    {
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

        _ctnStore.SetActive(false);
    }

    private void ShowHideStore()
    {
        m_show = !m_show;

        if (m_show)
        {
            m_currentSlotIndex = -1;

            Setup(true);
        }

        _ctnStore.SetActive(m_show);
    }

    private void Setup(bool changeSelection)
    {
        foreach (var slot in m_usedSlots)
            m_slots.Release(slot);

        m_usedSlots.Clear();

        foreach (var item in _itemsToSell)
        {
            var slot = m_slots.Get();
            slot.Setup(item);
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

    private void Selection(Slot_Store_Item slot)
    {
        var index = m_usedSlots.IndexOf(slot);

        if (m_currentSlotIndex == index)
            return;

        foreach (var item in m_usedSlots)
            item.Select(false);

        slot.Select(true);
        m_currentSlotIndex = index;

        Vector2 margin = new(0f, _verticalLayoutGroup.padding.vertical / 2f);
        _scroll.SnapToTarget(slot.RectTransform, margin);
    }

    void OnEnable()
    {
        m_showHideAction += ShowHideStore;
        m_selectionAction += Selection;
        m_setupAction += Setup;
    }

    void OnDisable()
    {
        m_showHideAction -= ShowHideStore;
        m_selectionAction -= Selection;
        m_setupAction -= Setup;
    }

}
