using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [System.Serializable]
    public class TestBodyPart
    {
        public BodyPartEnum bodyPartEnum;
        public Sprite sprite;
    }

    [SerializeField] private float _speed = 3f;

    [SerializeField] private TestBodyPart[] _bodyPartsTest;

    private bool m_interacting = false;

    private Vector3 m_move = default;

    void Awake()
    {
        OnTestBodyParts();
    }

    void Update()
    {
        if (m_interacting)
            return;

        Move();
    }

    private void Move()
    {
        transform.position += _speed * Time.deltaTime * m_move;
    }

    private void OnTestBodyParts()
    {
        foreach (var bodyPart in _bodyPartsTest)
            BodyPartController.Instance.ChangeBodyPart.Invoke(bodyPart.bodyPartEnum, bodyPart.sprite);
    }

    private void OnMove(InputValue value)
    {
        m_move = value.Get<Vector2>();
    }

}
