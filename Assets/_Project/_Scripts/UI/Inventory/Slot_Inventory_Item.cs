using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Inventory_Item : MonoBehaviour
{

    [SerializeField] private RectTransform _rectTransform;
    public RectTransform RectTransform => _rectTransform;
    [SerializeField] private Button _button;
    [SerializeField] private Image _imgSelection;
    [SerializeField] private Image _imgLeft;
    [SerializeField] private Image _imgRight;
    [SerializeField] private TextMeshProUGUI _txtTitle;
    [SerializeField] private GameObject _ctnEquiped;

    private bool m_selected = false;
    private bool m_equiped = false;
    private SO_BodyPart m_soBodyPart = null;

    void Awake()
    {
        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(BTN_Callback);
    }

    public void Setup(SO_BodyPart soBodyPart)
    {
        m_soBodyPart = soBodyPart;

        _txtTitle.SetText(soBodyPart.title);
        _imgLeft.sprite = soBodyPart.leftSprite;

        _imgRight.color = soBodyPart.rightSprite == null ? Color.clear : Color.white;
        _imgRight.sprite = soBodyPart.rightSprite;

        Equiped(false);
        Select(false);
    }

    public void Equiped(bool value)
    {
        m_equiped = value;
        _ctnEquiped.SetActive(m_equiped);
    }

    public void Select(bool value)
    {
        m_selected = value;

        _imgSelection.gameObject.SetActive(m_selected);

        if (value)
            _button.Select();
    }

    public void OnSelect()
    {
        Manager_Inventory.SelectionAction(this);
    }

    private void BTN_Callback()
    {
        PlayerController.OnChangeBodyPartAction(m_soBodyPart.bodyPartEnum, m_soBodyPart);
        Manager_Inventory.SetupAction();
    }

}
