using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private PlayerAnimatorController _animator;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private LayerMask _layerInteractable;
    [SerializeField] private Canvas _canvasInteraction;

    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _interactableSizeMultiplier = 1.05f;

    private bool m_interacting = false, m_facingLeft = false;
    private Vector3 m_startScale = default;

    private Vector3 m_move = default;

    void Awake()
    {
        m_startScale = transform.localScale;

        _canvasInteraction.gameObject.SetActive(false);
    }

    void Update()
    {
        if (m_interacting)
            return;

        Move();

        CheckInteraction();
    }

    private void Move()
    {
        transform.position += _speed * Time.deltaTime * m_move;

        if (m_move.x != 0f)
            CheckFaceSide(Mathf.Sign(m_move.x) < 0f);

        _animator.SetSpeed(m_move.magnitude);
    }

    private void CheckInteraction()
    {
        var point = transform.position;
        var size = _collider.size * _interactableSizeMultiplier * m_startScale.magnitude;
        var direction = _collider.direction;
        var angle = 0f;

        var collider = Physics2D.OverlapCapsule(point, size, direction, angle, _layerInteractable);

        _canvasInteraction.gameObject.SetActive(collider != null && collider.TryGetComponent(out ShopkeeperController _));
    }

    private void Interact()
    {
        var point = transform.position;
        var size = _collider.size * _interactableSizeMultiplier * m_startScale.magnitude;
        var direction = _collider.direction;
        var angle = 0f;

        var collider = Physics2D.OverlapCapsule(point, size, direction, angle, _layerInteractable);

        if (collider != null && collider.TryGetComponent(out ShopkeeperController shopkeeper))
            shopkeeper.Interact();
    }

    private void OnMove(InputValue value)
    {
        m_move = value.Get<Vector2>();
    }

    private void OnInteract(InputValue value)
    {
        Interact();
    }

    private void CheckFaceSide(bool left)
    {
        var scale = transform.localScale;
        var scaleCanvas = Vector3.one;

        if (m_facingLeft && !left)
        {
            scale.x = m_startScale.x;
            scaleCanvas.x = 1f;
        }
        else if (!m_facingLeft && left)
        {
            scale.x = -m_startScale.x;
            scaleCanvas.x = -1f;
        }

        m_facingLeft = left;
        transform.localScale = scale;
        _canvasInteraction.transform.localScale = scaleCanvas;
    }

}
