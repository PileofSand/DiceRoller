using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class DiceController : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField, Tooltip("Speed multiplier for releasing the dice")]
    private float _mouseDragPhysicsSpeed = 10;
    [SerializeField, Tooltip("Speed multiplier for the dragging, used on object.")]
    private float _mouseDragSpeed = 0.5f;
    [SerializeField, Tooltip("Height of dice when picking it up")]
    private float m_pickUpHeight = 0.5f;

    private Rigidbody _rigidbody;
    private Vector3 m_velocity;
    private Plane _plane;
    private bool m_isDragging;
    private Vector3 m_directionNormalized;

    #endregion

    #region UNITY_METHODS

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _plane = new Plane(Vector3.up, transform.position); // define a plane that is parallel to the ground and passes through the dice object
    }

    private void FixedUpdate()
    {
        if (!m_isDragging)
        {
            return;
        }

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out float initialDistance))
        {
            Vector3 dragPosition = ray.GetPoint(initialDistance);
            m_directionNormalized = (dragPosition - transform.position).normalized;
            transform.position = Vector3.SmoothDamp(transform.position, dragPosition, ref m_velocity, _mouseDragSpeed);
        }
    }

    private void OnMouseUp()
    {
        m_isDragging = false;
        _rigidbody.useGravity = true;
        _rigidbody.velocity = m_directionNormalized * _mouseDragPhysicsSpeed;
    }

    private void OnMouseDown()
    {
        m_isDragging = true;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        transform.Translate(Vector3.up * m_pickUpHeight);
    }

    #endregion
}
