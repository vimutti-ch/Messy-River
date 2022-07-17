using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject start;
    public GameObject timer;
    void Start()
        
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space)|| Input.GetTouch(0).phase == TouchPhase.Began)
        {
            start.SetActive(false);
            timer.GetComponent<Timer>().SetStatus(true);
                SoundManager.instance.Play(SoundManager.SoundName.Play);
        }
    }
}
