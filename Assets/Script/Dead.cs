using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    public GameObject rabbit;
    private void OnBecameInvisible()
    {
        rabbit.GetComponent<Drown>().restart();
    }
}
