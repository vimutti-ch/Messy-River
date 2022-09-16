using System.Collections;
using UnityEngine;

public class LogAnimate : MonoBehaviour
{
    /*private Animator animation;
     
    void Start()
    {
        animation = GetComponent<Animator>();
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Sink(GetComponent<Animator>()));
    }
    
    public IEnumerator Sink(Animator anim)
    {
        anim.SetBool("Stand", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Stand", false);
    }
}
