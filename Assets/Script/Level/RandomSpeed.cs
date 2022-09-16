using UnityEngine;

public class RandomSpeed : MonoBehaviour
{
    public bool invert;
    public float speed => _speed;
    
    [SerializeField, Range(0.1f, 2)] private float speedMin = 0.1f;
    [SerializeField, Range(2,5)] private float speedMax = 2f;

    private float _speed;
    
    void Awake()
    {
        _speed = Random.Range(speedMin, speedMax);
    }
}
