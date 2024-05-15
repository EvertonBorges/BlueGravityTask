using UnityEngine;
using UnityEngine.UI;

public class ShopkeeperController : MonoBehaviour
{

    [SerializeField] private GameObject _canvasStore;
    [SerializeField] private Button _btnBuy;

    private bool IsManagerStoreShowing => Manager_Store.Show;

    private static bool m_show = false;
    public static bool Show => m_show;

    void Awake()
    {
        _canvasStore.SetActive(false);
    }

    public void Interact()
    {
        if (IsManagerStoreShowing)
            Manager_Store.HideAction();
        else
        {
            m_show = !m_show;

            _canvasStore.SetActive(m_show);

            if (m_show)
                _btnBuy.Select();
        }
    }

    public void BTN_StoreSell()
    {
        Manager_Store.ShowAction(StoreType.SELL);
        m_show = false;
        _canvasStore.SetActive(m_show);
    }

    public void BTN_StoreBuy()
    {
        Manager_Store.ShowAction(StoreType.BUY);
        m_show = false;
        _canvasStore.SetActive(m_show);
    }

}
