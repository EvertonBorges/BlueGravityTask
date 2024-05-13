using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _speed = 3f;

    [SerializeField] private SO_BodyPart[] _bodyPartsTest;

    private bool m_interacting = false;

    private Vector3 m_move = default;

    void Start()
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
            BodyPartController.Instance.ChangeBodyPart.Invoke(bodyPart.bodyPartEnum, bodyPart);
    }

    private void OnMove(InputValue value)
    {
        m_move = value.Get<Vector2>();
    }

}
