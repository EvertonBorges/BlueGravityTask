using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Store_Item : MonoBehaviour
{

    [SerializeField] private RectTransform _rectTransform;
    public RectTransform RectTransform => _rectTransform;
    [SerializeField] private Button _button;
    [SerializeField] private Image _imgSelection;
    [SerializeField] private Image _imgLeft;
    [SerializeField] private Image _imgRight;
    [SerializeField] private Image _imgBuyBackground;
    [SerializeField] private TextMeshProUGUI _txtTitle;
    [SerializeField] private TextMeshProUGUI _txtPrice;
    [SerializeField] private TextMeshProUGUI _txtBuy;

    [SerializeField] private Color _purchasedColor;
    [SerializeField] private Color _canBuyColor;
    [SerializeField] private Color _cantBuyColor;

    private bool m_selected = false;
    private SO_BodyPart m_soBodyPart = null;
    private StoreType m_storeType = StoreType.BUY;

    void Awake()
    {
        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(BTN_Callback);
    }

    public void Setup(SO_BodyPart soBodyPart, StoreType storeType)
    {
        m_soBodyPart = soBodyPart;
        m_storeType = storeType;

        _txtTitle.SetText(soBodyPart.title);
        _txtPrice.SetText((soBodyPart.price / (storeType == StoreType.BUY ? 1 : 2)).ToString());
        _imgLeft.sprite = soBodyPart.leftSprite;

        _imgRight.color = soBodyPart.rightSprite == null ? Color.clear : Color.white;
        _imgRight.sprite = soBodyPart.rightSprite;

        var text = storeType.ToString();

        _txtBuy.SetText($"{text}");

        if (storeType == StoreType.BUY)
        {
            if (System_Inventory.Inventory.Contains(soBodyPart))
            {
                _imgBuyBackground.color = _purchasedColor;
                _txtBuy.SetText($"<i><s>{text}</s></i>");
            }
            else
                _imgBuyBackground.color = System_Inventory.Coins >= m_soBodyPart.price ? _canBuyColor : _cantBuyColor;
        }

        Select(false);
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
        Manager_Store.SelectionAction(this);
    }

    public void BTN_Callback()
    {
        if (m_storeType == StoreType.BUY)
        {
            System_Inventory.AddItem(m_soBodyPart);
            System_Inventory.AddCoin(-m_soBodyPart.price);
        }
        else
        {
            System_Inventory.RemoveItem(m_soBodyPart);
            System_Inventory.AddCoin(m_soBodyPart.price / 2);
        }

        Manager_Store.SetupAction();
    }

}
