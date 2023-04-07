using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Side
{
    public int sideValue;
    public DiceSide diceSide;
}

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class DiceController : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    private List<Side> _diceSides = new List<Side>();

    [SerializeField]
    private Camera _mainCamera;
    [SerializeField, Tooltip("Speed multiplier for the dragging, used on object.")]
    private float _mouseDragSpeed = 0.5f;
    [SerializeField, Tooltip("Value used to boost throw speed, Set to [1] to use realistic physics")]
    private float m_throwAccelerationValue = 1f;
    [SerializeField, Tooltip("Height of dice when picking it up")]
    private float m_pickUpHeight = 0.5f;
    [SerializeField, Tooltip("Speed of picking up dice")]
    private float m_pickUpSpeed = 2f;


    private Rigidbody _rigidbody;
    private Vector3 m_velocity;
    private Plane _plane;
    private bool m_isDragging;
    private Vector3 m_throwVelocity;

    #endregion

    #region UNITY_METHODS

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        for (int i = 0; i < _diceSides.Count; i++)
        {
            _diceSides[i].diceSide.Setup(_diceSides[i].sideValue);
        }

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
            m_throwVelocity = (dragPosition - transform.position);
            transform.position = Vector3.SmoothDamp(transform.position, dragPosition, ref m_velocity, _mouseDragSpeed);
        }
    }

    private void OnMouseUp()
    {
        m_isDragging = false;
        _rigidbody.useGravity = true;
        _rigidbody.velocity = m_throwVelocity * m_throwAccelerationValue;
    }

    private void OnMouseDown()
    {
        m_isDragging = true;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        transform.Translate(Vector3.up * m_pickUpHeight * m_pickUpSpeed);
    }

    #endregion
}
