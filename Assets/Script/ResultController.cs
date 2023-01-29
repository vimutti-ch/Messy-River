using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResultController : MonoBehaviour
{
    public RectTransform canvas;
    public static ResultController instant;
    private Vector2 CenterPoint;
    private Vector2 StartPoint;
    private RectTransform ResultScreen;
    public float duration;
    private void Awake()
    {
        instant = this;
        ResultScreen = GetComponent<RectTransform>();
        CalculatePosition();
    }
    private void CalculatePosition()
    {
        CenterPoint = new Vector2(0f, -canvas.rect.height / 2f);
        StartPoint = CenterPoint;
        StartPoint.y = Mathf.Abs(StartPoint.y);
        StartPoint.y *= 4;
        ResultScreen.anchoredPosition = StartPoint;
    }
    public void movein()
    {
        CalculatePosition();
        ResultScreen.DOAnchorPos(CenterPoint, duration);
    }

}
