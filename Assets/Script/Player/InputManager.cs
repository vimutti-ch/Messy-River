using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;

    private Camera _mainCamera;
    private PlayerControl _playerControl;
    
    public static InputManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _playerControl = new PlayerControl();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _playerControl.Enable();
    }

    private void OnDisable()
    {
        _playerControl.Disable();
    }

    private void Start()
    {
        _playerControl.Touch.PrimaryContract.started += ctx => StartTouchPrimary(ctx);
        _playerControl.Touch.PrimaryContract.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (OnStartTouch != null) OnStartTouch(Util.ScreenToWorld(_mainCamera, _playerControl.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.time);
    }
    
    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (OnEndTouch != null) OnEndTouch(Util.ScreenToWorld(_mainCamera, _playerControl.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.time);
    }

    public Vector2 PrimaryPosition()
    {
        return Util.ScreenToWorld(_mainCamera, _playerControl.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}
