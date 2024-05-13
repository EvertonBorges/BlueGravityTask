using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartController : MonoBehaviour
{

    private Action<BodyPartEnum, SO_BodyPart> m_changeBodyPartAction = (_, __) => { };
    public Action<BodyPartEnum, SO_BodyPart> ChangeBodyPart => m_changeBodyPartAction;

    [SerializeField] private SO_BodyPart[] _bodyPartsTest;
    [SerializeField] private BodyPart[] _bodyPart;

    private readonly Dictionary<BodyPartEnum, List<BodyPart>> m_bodyParts = new();

    void Awake()
    {
        foreach (var item in _bodyPart)
        {
            if (!m_bodyParts.ContainsKey(item.SoBodyPart.bodyPartEnum))
                m_bodyParts.Add(item.SoBodyPart.bodyPartEnum, new() { item });
            else
                m_bodyParts[item.SoBodyPart.bodyPartEnum].Add(item);
        }
    }

    void Start()
    {
        OnTestBodyParts();
    }

    private void OnChangeBodyPart(BodyPartEnum bodyPartEnum, SO_BodyPart soBodyPart)
    {
        var bodyParts = m_bodyParts[bodyPartEnum];

        if (soBodyPart == bodyParts[0].SoBodyPart && bodyParts[0].gameObject.activeSelf)
            return;

        CheckHideAsset(bodyPartEnum, soBodyPart);

        foreach (var bodyPart in bodyParts)
            bodyPart.SetSOBodyPart(soBodyPart);
    }

    private void CheckHideAsset(BodyPartEnum bodyPartEnum, SO_BodyPart soBodyPart)
    {
        if (soBodyPart.name.EndsWith("_00"))
            return;

        switch (bodyPartEnum)
        {
            case BodyPartEnum.HOOD:
                m_bodyParts[BodyPartEnum.HOOD].ForEach(x => x.ShowAsset());
                m_bodyParts[BodyPartEnum.HAIR].ForEach(x => x.HideAsset());
                break;
            case BodyPartEnum.HAIR:
                m_bodyParts[BodyPartEnum.HAIR].ForEach(x => x.ShowAsset());
                m_bodyParts[BodyPartEnum.HOOD].ForEach(x => x.HideAsset());
                break;
        }
    }

    private void OnTestBodyParts()
    {
        foreach (var bodyPart in _bodyPartsTest)
            ChangeBodyPart.Invoke(bodyPart.bodyPartEnum, bodyPart);
    }

    void OnEnable()
    {
        m_changeBodyPartAction += OnChangeBodyPart;
    }

    void OnDisable()
    {
        m_changeBodyPartAction -= OnChangeBodyPart;
    }

}
