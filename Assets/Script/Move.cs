using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rgb;
    public float distance;
    public ParticleSystem particle;
    public Timer timer;
    public GameObject inputName;
    public float force;
    public Jump jumpAni;
    public float blockScale = 1f;
    public LayerMask treeLayer;

    private Vector2 startPos, endPos;
    public float deadZone;

    private bool controlable = true;
    private Vector3 originalScale;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = Input.GetTouch(0).position;
                        break;
                case TouchPhase.Ended:
                    endPos = Input.GetTouch(0).position;

                    if (endPos.x < startPos.x - deadZone)
                    {
                        left();
                    }

                    else if (endPos.x > startPos.x + deadZone)
                    {
                        right();
                    }

                    else if (endPos.y < startPos.y - deadZone/1.5f)
                    {
                        back();
                    }

                    else
                    {
                        forward();
                    }
                    break;

            }
        }
    if (controlable)
        {
            PcControl(); 
        }




        CheckBelow();
    }
    private void Start()
    {
        rgb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
    }
    private void attach(Transform obj) {
        transform.SetParent(obj);
        rgb.constraints = RigidbodyConstraints.FreezePosition;
        ResetScale();
    }
    private void detach()
    {
        transform.SetParent(null);
        rgb.constraints = RigidbodyConstraints.None;
        rgb.constraints = RigidbodyConstraints.FreezeRotation;
        ResetScale();
    }
    private void OnCollisionEnter(Collision colide)
    {
        if (colide.gameObject.tag == "Wood" || colide.gameObject.tag == "Crocobody") attach(colide.transform);
    }
    private void OnBecameInvisible()
    {
        GetComponent<Drown>().restart();
    }

    private void CheckBelow()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            switch (hit.transform.tag)
            {
                case "Water":
                    particle.Play();
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponent<Drown>().restart();
                    detach();
                    enabled = false;
                    break;
                case "Finish":
                    timer.count = false;
                    controlable = false;
                    inputName.SetActive(true);
                    break;
                case "Crocohead":
                case "Crocotail":
                    detach();
                    rgb.AddForce(new Vector3(0, force, 0));
                    GetComponent<Drown>().restart();
                    break;
            }
        }
        
        Debug.DrawRay(transform.position, Vector3.down * distance, Color.red);
    }

    public void saveRecord()
    {
        DataPersistanceManager.instance.SaveGame();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Restart>().resetgame();
    }

    private void PcControl()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            forward();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            left();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            back();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            right();
        }
    }
    private void forward()
    {
        jumpAni.JumpAnim();
        detach();
        transform.rotation = Quaternion.Euler(-90, -90, 0);
        checkTree(Vector3.forward);
        playon();
    }

    private void left()
    {
        jumpAni.JumpAnim();
        checkTree(Vector3.left);
        transform.rotation = Quaternion.Euler(-90, -90, -90);
        playon();
    }

    private void back()
    {
        jumpAni.JumpAnim();
        detach();
        checkTree(Vector3.back);
        transform.rotation = Quaternion.Euler(-90, -90, 180);
        playon();
    }

    private void right()
    {
        jumpAni.JumpAnim();
        checkTree(Vector3.right);
        transform.rotation = Quaternion.Euler(-90, -90, 90);
        playon();
    }
    private void ResetScale()
    {
        transform.localScale = originalScale;
    }
    private void playon()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            switch (hit.transform.tag)
            {
                case "Wood":
                    SoundManager.instance.Play(SoundManager.SoundName.WoodLog);
                    break;

                case "Crocobody":
                    SoundManager.instance.Play(SoundManager.SoundName.Crocodile);
                    break;

                case "Finish":
                    SoundManager.instance.Play(SoundManager.SoundName.Finish);
                    break;

                case "Grass":
                    SoundManager.instance.Play(SoundManager.SoundName.Grass);
                    break;
                case "Water":
                    SoundManager.instance.Play(SoundManager.SoundName.Water);
                    break;
                case "Crocohead":
                case "CrocoTail":
                    SoundManager.instance.Play(SoundManager.SoundName.Bounce);
                    break;
            }
        }
    }
    private void checkTree(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, blockScale, treeLayer)) { }
        else
        {
            transform.position += direction;
        }
    }
}


