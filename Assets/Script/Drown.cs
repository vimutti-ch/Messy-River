using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drown : MonoBehaviour
{
    private Restart re;
    private float time;
    public float limit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            restart();
        }
    }

    public void restart()
    {
        re = GameObject.FindGameObjectWithTag("GameController").GetComponent<Restart>();
        StartCoroutine(fall());
    }

    // Update is called once per frame
    IEnumerator fall()
    {
        yield return new WaitForSeconds(limit);
        re.resetgame();
    }
}
