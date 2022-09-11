using UnityEngine;

public class RandomSpeed : MonoBehaviour
{
    public bool Invert;
    [Range(0.1f, 2)] [SerializeField] private float speedMin = 0.1f;
    [Range(2,5)] [SerializeField] private float speedMax = 2f;

    private float speed;

    // Start is called before the first frame update
    void Awake()
    {
        speed = Random.Range(speedMin, speedMax);
    }

    public float Get()
    {
        return speed;
    }
}
