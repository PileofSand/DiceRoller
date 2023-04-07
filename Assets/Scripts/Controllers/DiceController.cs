using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class DiceController : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private string destinationTag = "DropArea";
    [SerializeField, Tooltip("Speed multiplier for the dragging, used on physics object.")]
    private float mouseDragPhysicsSpeed = 10;
    [SerializeField, Tooltip("Speed damp for the dragging, used on a non-physics object.")]
    private float mouseDragSpeed = .1f;

    private Vector3 velocity = Vector3.zero;
    private Rigidbody _rigidbody;

    #endregion

    #region UNITY_METHODS

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnMouseUp()
    {
        _rigidbody.useGravity = true;
    }

    private void OnMouseDown()
    {
        _rigidbody.useGravity = false;
    }

    private void OnMouseDrag()
    {
        float initialDistance = Vector3.Distance(transform.position, _mainCamera.transform.position);
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 direction = (ray.GetPoint(initialDistance) - transform.position).normalized;
        _rigidbody.velocity = direction * mouseDragPhysicsSpeed;
        transform.position = Vector3.SmoothDamp(transform.position, ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
    }

    #endregion



}
