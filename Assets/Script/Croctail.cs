using UnityEngine;

public class Croctail : MonoBehaviour
{
    public Animator selfLA;

    private void OnCollisionEnter(Collision collision)
    {
        selfLA.ResetTrigger("OpenTail");
        selfLA.SetTrigger("OpenTail");
    }
}