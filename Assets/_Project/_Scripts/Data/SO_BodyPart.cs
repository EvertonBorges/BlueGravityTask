using UnityEngine;

[CreateAssetMenu(fileName = "SO_BodyPart", menuName = "BlueGravity/SO_BodyPart", order = 0)]
public class SO_BodyPart : ScriptableObject
{
    public BodyPartEnum bodyPartEnum;
    public string title;
    public Sprite sprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public int price;
}
