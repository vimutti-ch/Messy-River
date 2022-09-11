using UnityEngine;

public class Crochead : MonoBehaviour
{
    public Animator selfLA;

    private void OnCollisionEnter(Collision collision)
    {
        selfLA.ResetTrigger("OpenMounth");
        selfLA.SetTrigger("OpenMounth");
    }
}
