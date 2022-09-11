using UnityEngine;
using Random = UnityEngine.Random;

public class FinishPoint : MonoBehaviour
{
    public int positiveFinish;

    public int negativeFinish;

    // Start is called before the first frame update
    void Start()
    {
        RandomPosition();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            Debug.Log("Suffocate in Tree");
            RandomPosition();
        }
    }

    private void RandomPosition()
    {
        int xPos = Random.Range(negativeFinish, positiveFinish);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
}