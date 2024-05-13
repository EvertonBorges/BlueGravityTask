using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private PlayerAnimatorController _animator;
    [SerializeField] private float _speed = 3f;

    private bool m_interacting = false, m_facingLeft = false;
    private Vector3 m_startScale = default;

    private Vector3 m_move = default;

    void Awake()
    {
        m_startScale = transform.localScale;
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

        if (m_move.x != 0f)
            CheckFaceSide(Mathf.Sign(m_move.x) < 0f);

        _animator.SetSpeed(m_move.magnitude);
    }

    private void OnMove(InputValue value)
    {
        m_move = value.Get<Vector2>();
    }

    private void CheckFaceSide(bool left)
    {
        var scale = transform.localScale;

        if (m_facingLeft && !left)
            scale.x = m_startScale.x;
        else if (!m_facingLeft && left)
            scale.x = -m_startScale.x;

        m_facingLeft = left;
        transform.localScale = scale;
    }

}
