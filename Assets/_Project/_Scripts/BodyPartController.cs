using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartController : MonoBehaviour
{

    private static BodyPartController m_instance;
    public static BodyPartController Instance => m_instance;

    private Action<BodyPartEnum, SO_BodyPart> m_changeBodyPartAction = (_, __) => { };
    public Action<BodyPartEnum, SO_BodyPart> ChangeBodyPart => m_changeBodyPartAction;

    [SerializeField] private BodyPart[] _bodyPart;

    private readonly Dictionary<BodyPartEnum, List<BodyPart>> m_bodyParts = new();

    void Awake()
    {
        if (m_instance == null)
            m_instance = this;

        foreach (var item in _bodyPart)
        {
            if (!m_bodyParts.ContainsKey(item.SoBodyPart.bodyPartEnum))
                m_bodyParts.Add(item.SoBodyPart.bodyPartEnum, new() { item });
            else
                m_bodyParts[item.SoBodyPart.bodyPartEnum].Add(item);
        }
    }

    private void OnChangeBodyPart(BodyPartEnum bodyPartEnum, SO_BodyPart soBodyPart)
    {
        var bodyParts = m_bodyParts[bodyPartEnum];

        if (soBodyPart == bodyParts[0].SoBodyPart)
            return;

        foreach (var bodyPart in bodyParts)
            bodyPart.SetSOBodyPart(soBodyPart);
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
