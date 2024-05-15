using System;
using UnityEngine;

public class Manager_Sound : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private AudioSource _coinAudio;
    [SerializeField] private AudioSource _diamondAudio;
    [SerializeField] private AudioSource _changeBodyPartAudio;

    private static Action m_onGetCoinAction = () => { };
    public static Action OnGetCoinAction => m_onGetCoinAction;

    private static Action m_onGetDiamondAction = () => { };
    public static Action OnGetDiamondAction => m_onGetDiamondAction;

    private static Action m_onChangeBodyParteAction = () => { };
    public static Action OnChangeBodyParteAction => m_onChangeBodyParteAction;

    private void OnGetCoin()
    {
        _coinAudio.Play();
    }

    private void OnGetDiamond()
    {
        _diamondAudio.Play();
    }

    private void OnChangeBodyPart()
    {
        _changeBodyPartAudio.Play();
    }

    void OnEnable()
    {
        m_onGetCoinAction += OnGetCoin;
        m_onGetDiamondAction += OnGetDiamond;
        m_onChangeBodyParteAction += OnChangeBodyPart;
    }

    void OnDisable()
    {
        m_onGetCoinAction -= OnGetCoin;
        m_onGetDiamondAction -= OnGetDiamond;
        m_onChangeBodyParteAction -= OnChangeBodyPart;
    }
}
