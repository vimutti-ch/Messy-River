using UnityEngine;

public class Jump : MonoBehaviour
{
    private Animator animation;

    void Start()
    {
        animation = GetComponent<Animator>();
    }
    
    public void JumpAnim()
    {
        animation.SetTrigger("Jump");
    }
}
