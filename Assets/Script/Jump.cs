
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Animator anim;
    private bool isJump;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    public void JumpAnim()
    {
            anim.SetTrigger("jump");
        //SoundManager.instance.Play(SoundManager.SoundName.Jump);
    }
}
