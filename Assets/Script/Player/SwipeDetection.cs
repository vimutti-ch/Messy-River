using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private Move player;
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;
    
    private InputManager _inputManager;

    private Vector2 _startPosition;
    private float _startTime;
    
    private Vector2 _endPosition;
    private float _endTime;
    
    private void Awake()
    {
        _inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        _inputManager.OnStartTouch += SwipeStart;
        _inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        _inputManager.OnStartTouch -= SwipeStart;
        _inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        _startPosition = position;
        _startTime = time;
    }
    
    private void SwipeEnd(Vector2 position, float time)
    {
        _endPosition = position;
        _endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector2.Distance(_startPosition, _endPosition) >= minimumDistance &&
            (_endTime - _startTime) <= maximumTime)
        {
            Debug.Log("Swipe Detected");
            Debug.DrawLine(_startPosition, _endPosition, Color.red, 5f);
            
            Vector2 direction = _endPosition - _startPosition;
            Vector2 direction2D = direction.normalized;
            
            SwipeDirection(direction2D);
        }
        else if ((_endTime - _startTime) <= maximumTime)
        {
            player.Forward();
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        Debug.Log($"in Case of Up {Vector2.Dot(Vector2.up, direction)}");
        Debug.Log($"in Case of Down {Vector2.Dot(Vector2.down, direction)}");
        Debug.Log($"in Case of Left {Vector2.Dot(Vector2.left, direction)}");
        Debug.Log($"in Case of Right {Vector2.Dot(Vector2.right, direction)}");
        
        if (Vector2.Dot(Vector2.up, direction) > (directionThreshold * .4f))
        {
            Debug.Log("Swipe Up");
            player.Forward();
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold * .4f)
        {
            Debug.Log("Swipe Down");
            player.Back();
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
            player.Left();
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
            player.Right();
        }
    }
}
