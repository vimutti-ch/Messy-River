using System.Collections;
using UnityEngine;

public class Drown : MonoBehaviour
{
    private Restart _re;
    private float _time;
    public float limit;

    private void Awake()
    {
        _re = GameObject.FindGameObjectWithTag("GameController").GetComponent<Restart>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Restart();
        }
    }

    public void Restart()
    {
        StartCoroutine(Fall());
    }

    // Update is called once per frame
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(limit);
        _re.resetgame();
    }
}
