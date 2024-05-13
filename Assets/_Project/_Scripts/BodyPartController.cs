using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartController : MonoBehaviour
{

    private static BodyPartController m_instance;
    public static BodyPartController Instance => m_instance;

    private Action<BodyPartEnum, Sprite> m_changeBodyPartAction = (_, __) => { };
    public Action<BodyPartEnum, Sprite> ChangeBodyPart => m_changeBodyPartAction;

    [SerializeField] private BodyPart[] _bodyPart;

    private readonly Dictionary<BodyPartEnum, BodyPart> m_bodyParts = new();

    void Awake()
    {
        if (m_instance == null)
            m_instance = this;

        foreach (var item in _bodyPart)
            m_bodyParts.Add(item.BodyPartEnum, item);
    }

    private void OnChangeBodyPart(BodyPartEnum bodyPartEnum, Sprite sprite)
    {
        var bodyPart = m_bodyParts[bodyPartEnum];
        var bodyPartSprite = bodyPart.Sprite;

        if (sprite == bodyPartSprite)
            return;

        bodyPart.SetSprite(sprite);
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
