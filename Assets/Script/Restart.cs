using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
    public void resetgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DelayReset()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
