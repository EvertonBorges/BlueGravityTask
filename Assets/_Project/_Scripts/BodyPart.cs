using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private BodyPartEnum _bodyPart;
    public BodyPartEnum BodyPartEnum => _bodyPart;
    [SerializeField] private SpriteRenderer _sprite;
    public Sprite Sprite => _sprite.sprite;

    public void SetSprite(Sprite sprite)
    {
        _sprite.sprite = sprite;
    }

}
