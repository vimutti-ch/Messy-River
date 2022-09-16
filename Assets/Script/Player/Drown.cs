using System.Collections;
using UnityEngine;

public class Drown : MonoBehaviour
{
    #region - Variable Declaration (การประกาศตัวแปร) -

    public float limit;

    private Restart _restart;
    private float _time;

    #endregion

    #region - Unity's Method (คำสั่งของ Unity เอง) -

    private void Awake()
    {
        _restart = GameObject.FindGameObjectWithTag("GameController").GetComponent<Restart>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Restart();
        }
    }

    #endregion

    #region - Custom Method (คำสั่งที่เขียนขึ้นมาเอง) -

    public void Restart()
    {
        if(gameObject.activeSelf)
            StartCoroutine(Fall());
    }

    // Update is called once per frame
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(limit);
        _restart.ResetGame();
    }

    #endregion
}