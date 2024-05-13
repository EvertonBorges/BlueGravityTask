using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private PlayerAnimatorController _animator;
    [SerializeField] private float _speed = 3f;

    private bool m_interacting = false;

    private Vector3 m_move = default;

    void Update()
    {
        if (m_interacting)
            return;

        Move();
    }

    private void Move()
    {
        transform.position += _speed * Time.deltaTime * m_move;

        _animator.SetSpeed(m_move.magnitude);
    }

    private void OnMove(InputValue value)
    {
        m_move = value.Get<Vector2>();
    }

}
