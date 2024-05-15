using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private static PlayerController m_instance;
    public static PlayerController Instance => m_instance;

    private static Action<BodyPartEnum, SO_BodyPart> m_onChangeBodyPartAction = (_, __) => { };
    public static Action<BodyPartEnum, SO_BodyPart> OnChangeBodyPartAction => m_onChangeBodyPartAction;

    [Header("References")]
    [SerializeField] private PlayerAnimatorController _animator;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private LayerMask _layerInteractable;
    [SerializeField] private Canvas _canvasInteraction;
    [SerializeField] private BodyPartController _bodyPartController;
    public SO_BodyPart[] BodyParts => _bodyPartController.SoBodyParts;

    [Header("Parameters")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _interactableSizeMultiplier = 1.05f;

    private bool m_facingLeft = false;
    private bool Interacting => Manager_Inventory.Show || Manager_Store.Show || ShopkeeperController.Show;
    private Vector3 m_startScale = default;

    private Vector3 m_move = default;

    void Awake()
    {
        if (m_instance == null)
            m_instance = this;

        m_startScale = transform.localScale;

        _canvasInteraction.gameObject.SetActive(false);

        System_Inventory.StartInventory(BodyParts);
    }

    void Update()
    {
        if (Interacting)
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
        if (Manager_Inventory.Show || Manager_Warning.Show)
            return;

        var point = transform.position;
        var size = _collider.size * _interactableSizeMultiplier * m_startScale.magnitude;
        var direction = _collider.direction;
        var angle = 0f;

        var collider = Physics2D.OverlapCapsule(point, size, direction, angle, _layerInteractable);

        if (collider != null && collider.TryGetComponent(out ShopkeeperController shopkeeper))
            shopkeeper.Interact();
    }

    private void Inventory()
    {
        if (ShopkeeperController.Show || Manager_Store.Show)
            return;

        _animator.SetSpeed(0f);

        Manager_Inventory.ShowHideInventoryAction();
    }

    private void OnMove(InputValue value)
    {
        m_move = value.Get<Vector2>();
    }

    private void OnInteract(InputValue value)
    {
        Interact();
    }

    private void OnInventory(InputValue value)
    {
        Inventory();
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
        _canvasInteraction.transform.localScale = scale * 4f;
    }

    private void OnChangeBodyPart(BodyPartEnum bodyPartEnum, SO_BodyPart soBodyPart)
    {
        _bodyPartController.ChangeBodyPart(bodyPartEnum, soBodyPart);

        Manager_Sound.OnChangeBodyParteAction();
    }

    void OnEnable()
    {
        m_onChangeBodyPartAction += OnChangeBodyPart;
    }

    void OnDisable()
    {
        m_onChangeBodyPartAction -= OnChangeBodyPart;
    }

}
