using System.Collections;
using System.Collections.Generic;
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