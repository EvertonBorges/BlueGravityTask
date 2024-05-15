using UnityEngine;

public class BodyPart : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SO_BodyPart _soBodyPart;
    public SO_BodyPart SoBodyPart => _soBodyPart;
    [SerializeField] private BodyPartEnum _bodyPart;
    public BodyPartEnum BodyPartEnum => _bodyPart;
    [SerializeField] private SpriteRenderer _sprite;

    public void SetSOBodyPart(SO_BodyPart soBodyPart)
    {
        if (_bodyPart.ToString().EndsWith("_l"))
            _sprite.sprite = soBodyPart.leftSprite;
        else if (_bodyPart.ToString().EndsWith("_r"))
            _sprite.sprite = soBodyPart.rightSprite;
        else
            _sprite.sprite = soBodyPart.sprite;

        _soBodyPart = soBodyPart;
    }

    public void ShowAsset()
    {
        gameObject.SetActive(true);
    }

    public void HideAsset()
    {
        gameObject.SetActive(false);
    }

}
