using UnityEngine;

public class CrocodileBody : MonoBehaviour
{
    public LogAnimate selfLA;
    
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(selfLA.Sink(selfLA.gameObject.GetComponent<Animator>()));
    }
}
