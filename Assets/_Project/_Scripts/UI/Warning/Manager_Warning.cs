using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Warning : MonoBehaviour
{

    private static Action<Selectable, string> m_onShowAction = (_, __) => { };
    public static Action<Selectable, string> OnShowAction => m_onShowAction;

    private static bool m_show = false;
    public static bool Show => m_show;

    [Header("References")]
    [SerializeField] private GameObject _container;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;

    private Selectable m_lastSelection = null;

    void Awake()
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(BTN_Callback);

        _container.SetActive(false);
    }

    private void OnShow(Selectable lastSelection, string text)
    {
        m_show = true;
        m_lastSelection = lastSelection;

        _container.SetActive(true);
        _text.SetText(text);

        StartCoroutine(WaitFrame(() => _button.Select()));
    }

    private IEnumerator WaitFrame(Action callback)
    {
        yield return new WaitForEndOfFrame();

        callback();
    }

    private void BTN_Callback()
    {
        m_show = false;

        _container.SetActive(false);

        StartCoroutine(WaitFrame(() =>
        {
            m_lastSelection.Select();
            m_lastSelection = null;
        }));
    }

    void OnEnable()
    {
        m_onShowAction += OnShow;
    }

    void OnDisable()
    {
        m_onShowAction -= OnShow;
    }

}
