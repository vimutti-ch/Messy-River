using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private bool Invert;
    [SerializeField] private float speed;

    void Update()
    {
        if (Invert == false)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(-90, 0, 180);
        }
        if (Invert == true)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Border")
        {
            Destroy(gameObject);
        }
    }

    public void Set(float s)
    {
        speed = s;
    }
    public void SetDirection(bool d)
    {
        Invert = d;
    }
}
