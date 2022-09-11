using UnityEngine;

public class Jump : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    public void JumpAnim()
    {
        anim.SetTrigger("jump");
    }
}
