using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Store_Item : MonoBehaviour
{

    [Header("References")]
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

    [Header("Parameters")]
    [SerializeField] private bool _canSellEquipedAsset = false;

    [Header("Colors")]
    [SerializeField] private Color _purchasedColor;
    [SerializeField] private Color _sellEquipedColor;
    [SerializeField] private Color _canBuyColor;
    [SerializeField] private Color _cantBuyColor;

    private bool m_selected = false, m_canBuyOrSell = true, m_equiped = false;
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
        m_canBuyOrSell = true;
        m_equiped = false;

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
                m_canBuyOrSell = false;
                _imgBuyBackground.color = _purchasedColor;
                _txtBuy.SetText($"<i><s>{text}</s></i>");
            }
            else
            {
                m_canBuyOrSell = System_Inventory.Coins >= m_soBodyPart.price;
                _imgBuyBackground.color = m_canBuyOrSell ? _canBuyColor : _cantBuyColor;
            }
        }
        else
        {
            m_equiped = PlayerController.Instance.BodyParts.Contains(soBodyPart);

            if (_canSellEquipedAsset)
            {
                var inventory = System_Inventory.Inventory;
                var itemsWithSameGroup = inventory.ToList().FindAll(x => x.bodyPartEnum == m_soBodyPart.bodyPartEnum);

                if (itemsWithSameGroup.IsEmpty() || itemsWithSameGroup.Count < 2)
                    m_canBuyOrSell = false;

                _imgBuyBackground.color = m_canBuyOrSell ? (m_equiped ? _sellEquipedColor : _purchasedColor) : _cantBuyColor;
            }
            else
            {
                if (m_equiped)
                    m_canBuyOrSell = false;

                _imgBuyBackground.color = m_canBuyOrSell ? _purchasedColor : _cantBuyColor;
            }
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
            if (m_canBuyOrSell)
            {
                System_Inventory.AddItem(m_soBodyPart);
                System_Inventory.AddCoin(-m_soBodyPart.price);

                Manager_Store.SetupAction();
            }
            else
            {
                string text = $"You already have the item: {m_soBodyPart.title}";
                
                Manager_Warning.OnShowAction(_button, text);
            }
        }
        else
        {
            if (m_canBuyOrSell)
            {
                System_Inventory.RemoveItem(m_soBodyPart);
                System_Inventory.AddCoin(m_soBodyPart.price / 2);

                if (_canSellEquipedAsset && m_equiped)
                {
                    var itemsWithSameGroup = System_Inventory.Inventory.ToList().FindAll(x => x.bodyPartEnum == m_soBodyPart.bodyPartEnum);
                    var randomSoBodyType = itemsWithSameGroup[Random.Range(0, itemsWithSameGroup.Count)];

                    PlayerController.OnChangeBodyPartAction(randomSoBodyType.bodyPartEnum, randomSoBodyType);
                }

                Manager_Store.SetupAction();
            }
            else
            {
                string text;

                if (!_canSellEquipedAsset && m_equiped)
                    text = $"Unable to sell an equipped item: {m_soBodyPart.title}";
                else
                    text = $"There are no items in the same group: {m_soBodyPart.bodyPartEnum}";
                
                Manager_Warning.OnShowAction(_button, text);
            }
        }
    }

}
