using System;
using UnityEngine;

public class Manager_Sound : MonoBehaviour
{

    [SerializeField] private AudioSource _coinAudio;
    [SerializeField] private AudioSource _diamondAudio;

    private static Action m_onGetCoinAction = () => { };
    public static Action OnGetCoinAction => m_onGetCoinAction;

    private static Action m_onGetDiamondAction = () => { };
    public static Action OnGetDiamondAction => m_onGetDiamondAction;

    private void OnGetCoin()
    {
        _coinAudio.Play();
    }

    private void OnGetDiamond()
    {
        _diamondAudio.Play();
    }

    void OnEnable()
    {
        m_onGetCoinAction += OnGetCoin;
        m_onGetDiamondAction += OnGetDiamond;
    }

    void OnDisable()
    {
        m_onGetCoinAction -= OnGetCoin;
        m_onGetDiamondAction -= OnGetDiamond;
    }
}
