using UnityEngine;
using Random = UnityEngine.Random;

public class FinishPoint : MonoBehaviour
{
    public int positiveFinish;
    public int negativeFinish;
    
    void Start()
    {
        RandomPosition();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            Debug.Log("Suffocated in Tree");
            RandomPosition();
        }
    }

    private void RandomPosition()
    {
        Vector3 currentPosition = transform.position;
        
        int xPos = Random.Range(negativeFinish, positiveFinish);
        transform.position = new Vector3(xPos, currentPosition.y, currentPosition.z);
    }
}