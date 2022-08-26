using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private FreeCameraControls _controls;
    private bool _activeRotate;

    [SerializeField] private Camera _camera;

    [SerializeField, Range(0.1f, 100f)] private float _moveSpeed = 10f;
    [SerializeField, Range(0.1f, 100f)] private float _rotateSpeed = 10f;
    [SerializeField, Range(0.1f, 100f)] private float _upDownSpeed = 10f;

    private void Awake()
    {
        _controls = new FreeCameraControls();
        _controls.Camera.Focus.performed += OnFocus;
        _controls.Camera.ActiveRotation.performed += OnRightClick;
        _controls.Camera.ActiveRotation.canceled += OnRightClick;
    }

    private void Update()
    {
        OnMoveAndRotate();
    }

    private void OnMoveAndRotate()
    {
        var direction = _controls.Camera.Move.ReadValue<Vector2>();
        transform.position += (transform.forward * direction.y + transform.right * direction.x) * _moveSpeed *
                              Time.deltaTime;
        if (!_activeRotate) return;

        direction = _controls.Camera.Rotate.ReadValue<Vector2>();
        var angle = transform.eulerAngles;
        angle.x -= direction.y * _rotateSpeed * Time.deltaTime;
        angle.y += direction.x * _rotateSpeed * Time.deltaTime;
        angle.z = 0f;

        transform.eulerAngles = angle;
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        _activeRotate = context.performed;
        Cursor.lockState = _activeRotate ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void OnFocus(InputAction.CallbackContext context)
    {
        transform.position += transform.up * context.ReadValue<float>() * _upDownSpeed * Time.deltaTime;
    }

    private void OnEnable()
    {
        _controls.Camera.Enable();
    }

    private void OnDisable()
    {
        _controls.Camera.Disable();
    }

    private void OnDestroy()
    {
        _controls.Dispose();
    }
}