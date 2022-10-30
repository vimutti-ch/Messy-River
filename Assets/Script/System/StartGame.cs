using System;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject start;
    public GameObject timer;

    private PlayerControl _controller;
    
    private void Awake()
    {
        _controller = new PlayerControl();
    }

    private void OnEnable()
    {
        _controller.Enable();
    }

    private void OnDisable()
    {
        _controller.Disable();
    }

    private void Start()
    {
        _controller.Move.Forward.performed += ctx => StartPlay();
        _controller.Move.Backward.performed += ctx => StartPlay();
        _controller.Move.Left.performed += ctx => StartPlay();
        _controller.Move.Right.performed += ctx => StartPlay();
        _controller.Touch.PrimaryContract.performed += ctx => StartPlay();
    }

    private void StartPlay()
    {
        start.SetActive(false);
        timer.GetComponent<Timer>().SetStatus(true);
        
        Destroy(this);
    }
}
