using UnityEngine;
using Debug = UnityEngine.Debug;

public class Move : MonoBehaviour
{
    private Rigidbody _rgb;
    public float distance;
    public float blockScale = 1f;
    public ParticleSystem particle;
    public Timer timer;
    public GameObject inputName;
    public float force;
    public Animator animator;

    public LayerMask treeLayers;
    
    public Jump jumpAni;

    private Vector2 _startPos, _endPos;
    public float deadZone;

    private bool _controllable = true;
    private Vector3 _originalScale;

    void Update()
    {
        if (_controllable)
        {
            KeyboardControl();
            TouchControl();
        }
        
        Debug.DrawRay(transform.position, Vector3.forward*blockScale, Color.red);
        
        CheckBelow();
    }

    private void Start()
    {
        _rgb = GetComponent<Rigidbody>();
        _originalScale = transform.localScale;
    }

    private void Attach(Transform obj)
    {
        transform.SetParent(obj);
        _rgb.constraints = RigidbodyConstraints.FreezePosition;
        ResetScale();
    }

    private void Detach()
    {
        transform.SetParent(null);
        _rgb.constraints = RigidbodyConstraints.None;
        _rgb.constraints = RigidbodyConstraints.FreezeRotation;
        ResetScale();
    }

    private void OnCollisionEnter(Collision colide)
    {
        if (colide.gameObject.CompareTag("Wood") || colide.gameObject.CompareTag("Crocobody")) Attach(colide.transform);
    }

    private void OnBecameInvisible()
    {
        GetComponent<Drown>().Restart();
    }

    private void CheckBelow()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, distance))
        {
            switch (hit.transform.tag)
            {
                case "Water":
                    particle.Play();
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponent<Drown>().Restart();
                    Detach();
                    enabled = false;
                    break;
                case "Finish":
                    timer.SetStatus(false);
                    _controllable = false;
                    inputName.SetActive(true);
                    animator.SetBool("open", true);
                    break;
                case "Crocohead":
                case "Crocotail":
                    Detach();
                    _rgb.AddForce(new Vector3(0, force, 0));
                    GetComponent<Drown>().Restart();
                    break;
            }
        }

        Debug.DrawRay(transform.position, Vector3.down * distance, Color.red);
    }

    // public void saveRecord()
    // {
    //     RecordInfo info = timer.SetRecord();
    //     Record.AddInfo(info);
    //     GameObject.FindGameObjectWithTag("GameController").GetComponent<Restart>().resetgame();
    // }

    public void SaveRecord()
    {
        DataPersistanceManager.Instance.SaveGame();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Restart>().resetgame();
    }

    private void KeyboardControl()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            Forward();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Left();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Back();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Right();
        }
    }

    private void TouchControl()
    {
        if (Input.touchCount > 0) //Is there any touch?
        {
            Touch touch = Input.GetTouch(0); //Use which touch?
        
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPos = Input.GetTouch(0).position;
                    break;
                case TouchPhase.Ended:
                    _endPos = Input.GetTouch(0).position;

                    if (_endPos.x < _startPos.x - deadZone)
                    {
                        Left();
                    }

                    else if(_endPos.x > _startPos.x + deadZone)
                    {
                        Right();
                    }

                    else if(_endPos.y < _startPos.y - (deadZone/1.5f))
                    {
                        Back();
                    }

                    else
                    {
                        Forward();
                    }
                    
                    break;
            }
        } 
    }

    private void Forward()
    {
        jumpAni.JumpAnim();
        Detach();
        transform.rotation = Quaternion.Euler(-90, -90, 0);

        CheckTree(Vector3.forward);
        
        PlayOn();
    }

    private void Left()
    {
        jumpAni.JumpAnim();
        transform.rotation = Quaternion.Euler(-90, -90, -90);
        
        CheckTree(Vector3.left);
        
        PlayOn();
    }

    private void Back()
    {
        jumpAni.JumpAnim();
        Detach();
        transform.rotation = Quaternion.Euler(-90, -90, 180);
        
        CheckTree(Vector3.back);
        
        PlayOn();
    }

    private void Right()
    {
        jumpAni.JumpAnim();
        transform.rotation = Quaternion.Euler(-90, -90, 90);
        
        CheckTree(Vector3.right);
        
        PlayOn();
    }

    private void ResetScale()
    {
        transform.localScale = _originalScale;
    }

    private void PlayOn()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            switch (hit.transform.tag)
            {
                case "Wood":
                    SoundManager.instance.Play(SoundManager.SoundName.WoodLog);
                    break;
            }
        }
    }

    private void CheckTree(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, blockScale, treeLayers))
        {
        }
        else transform.position += direction;
    }
}