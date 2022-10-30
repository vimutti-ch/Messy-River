using System;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class Move : MonoBehaviour
{
    #region - Variable Declaration (การประกาศตัวแปร) -

    [Header("Attribute Settings")]
    public float distance;
    public float blockScale = 1f;
    public float force;

    [Header("Object Assign")]
    [FormerlySerializedAs("particle")]
    public ParticleSystem waterParticle;

    [FormerlySerializedAs("jumpAni")]
    public Jump jumpAnimation;
    
    [Space(10)]
    public Timer timer;
    public GameObject inputName;
    public Animator animator;
    
    [Space(10)]
    public LayerMask treeLayers;

    [Header("Debug")]
    public bool showBelow;
    
    //Back-end Variable
    private PlayerControl _controller;
    
    private bool _controllable = true;
    
    private Rigidbody _rigidbody;
    
    private Vector3 _originalScale;

    private bool _isEnd = false;

    #endregion

    #region - Unity's Method (คำสั่งของ Unity เอง) -

    private void Awake()
    {
        _controller = new PlayerControl();
    }

    private void OnEnable()
    {
        _controller.Enable();
    }

    private void OnDisable()
    {
        _controller.Disable();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _originalScale = transform.localScale;
        
        _controller.Move.Forward.performed += ctx => Forward();
        _controller.Move.Backward.performed += ctx => Back();
        _controller.Move.Left.performed += ctx => Left();
        _controller.Move.Right.performed += ctx => Right();
    }
    
    void Update()
    {
        if(!_controllable) DisableControl();

        Debug.DrawRay(transform.position, Vector3.forward * blockScale, Color.red);

        if(!_isEnd) CheckBelow();
    }

    private void OnBecameInvisible()
    {
        GetComponent<Drown>().Restart();
    }
    
    private void OnCollisionEnter(Collision colide)
    {
        if (colide.gameObject.CompareTag("Wood") || colide.gameObject.CompareTag("Crocobody"))
            Attach(colide.transform);
    }

    #endregion

    #region - Custom Method (คำสั่งที่เขียนขึ้นมาเอง) -

    private void EnableControl()
    {
        _controller.Enable();
    }

    private void DisableControl()
    {
        _controller.Disable();
    }

    private void Attach(Transform obj)
    {
        transform.SetParent(obj);
        _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        ResetScale();
    }

    private void Detach()
    {
        transform.SetParent(null);
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        ResetScale();
    }


    private void CheckBelow()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, distance))
        {
            if(showBelow) Debug.Log($"Below is {hit.transform.tag}");
            switch (hit.transform.tag)
            {
                case "Water":
                    GroundDrown();
                    break;
                case "Finish":
                    Finish();
                    break;
                case "Crocohead":
                case "Crocotail":
                    Detach();
                    _rigidbody.AddForce(new Vector3(0, force, 0));
                    GetComponent<Drown>().Restart();
                    break;
            }
        }

        Debug.DrawRay(transform.position, Vector3.down * distance, Color.red);
    }

    public void GroundDrown()
    {
        waterParticle.Play();
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Drown>().Restart();
        Detach();
        enabled = false;
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
        /*GameObject.FindGameObjectWithTag("GameController").GetComponent<Restart>().ResetGame();*/
    }

    // private void KeyboardControl()
    // {
    //     if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Forward();
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         Left();
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         Back();
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.D))
    //     {
    //         Right();
    //     }
    // }

    // private void TouchControl()
    // {
    //     if (Input.touchCount > 0) //Is there any touch?
    //     {
    //         Touch touch = Input.GetTouch(0); //Use which touch?
    //
    //         switch (touch.phase)
    //         {
    //             case TouchPhase.Began:
    //                 _startPos = Input.GetTouch(0).position;
    //                 break;
    //             case TouchPhase.Ended:
    //                 _endPos = Input.GetTouch(0).position;
    //
    //                 if (_endPos.x < _startPos.x - deadZone)
    //                 {
    //                     Left();
    //                 }
    //
    //                 else if (_endPos.x > _startPos.x + deadZone)
    //                 {
    //                     Right();
    //                 }
    //
    //                 else if (_endPos.y < _startPos.y - (deadZone / 1.5f))
    //                 {
    //                     Back();
    //                 }
    //
    //                 else
    //                 {
    //                     Forward();
    //                 }
    //
    //                 break;
    //         }
    //     }
    // }

    public void Forward()
    {
        jumpAnimation.JumpAnim();
        Detach();
        transform.rotation = Quaternion.Euler(-90, -90, 0);

        CheckTree(Vector3.forward);

        PlayOn();
    }

    public void Left()
    {
        jumpAnimation.JumpAnim();
        transform.rotation = Quaternion.Euler(-90, -90, -90);

        CheckTree(Vector3.left);

        PlayOn();
    }

    public void Back()
    {
        jumpAnimation.JumpAnim();
        Detach();
        transform.rotation = Quaternion.Euler(-90, -90, 180);

        CheckTree(Vector3.back);

        PlayOn();
    }

    public void Right()
    {
        jumpAnimation.JumpAnim();
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
                    SoundManager.Instance.Play(SoundManager.SoundName.WoodLog);
                    break;
            }
        }
    }

    private void CheckTree(Vector3 direction)
    {
        if (!Physics.Raycast(transform.position, direction, blockScale, treeLayers))
        {
            transform.position += direction;
        }
    }

    [ContextMenu("Move to End's Z")]
    void ToEndLine()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 4);
    }

    [ContextMenu("Finish")]
    void Finish()
    {
        timer.PassResult();
        timer.SetStatus(false);
        _controllable = false;
        inputName.SetActive(true);
        animator.SetBool("open", true);
        _isEnd = true;
    }

    #endregion
}