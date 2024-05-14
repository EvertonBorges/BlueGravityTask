using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Inventory_Item : MonoBehaviour
{

    [SerializeField] private Image _imgLeft;
    [SerializeField] private Image _imgRight;
    [SerializeField] private TextMeshProUGUI _txtTitle;
    [SerializeField] private GameObject _ctnEquiped;

    private bool m_equiped = false;
    private SO_BodyPart m_soBodyPart = null;

    public void Setup(SO_BodyPart soBodyPart)
    {
        m_soBodyPart = soBodyPart;

        _txtTitle.SetText(soBodyPart.title);
        _imgLeft.sprite = soBodyPart.leftSprite;

        _imgRight.color = soBodyPart.rightSprite == null ? Color.clear : Color.white;
        _imgRight.sprite = soBodyPart.rightSprite;

        Equiped(false);
    }

    public void Equiped(bool value)
    {
        m_equiped = value;
        _ctnEquiped.SetActive(m_equiped);
    }

}
