using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private bool _invert;

    void Update()
    {
        if (_invert == false)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(-90, 0, 180);
        }
        if (_invert == true)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
    
    public void SetDirection(bool d)
    {
        _invert = d;
    }
}
