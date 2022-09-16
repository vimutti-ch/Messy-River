using UnityEngine;

public class Dead : MonoBehaviour
{
    public GameObject rabbit;
    
    private void OnBecameInvisible()
    {
        rabbit.GetComponent<Drown>().Restart();
    }
}
