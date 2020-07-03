using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerControl : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        float speed = Random.Range(0.8f, 1f);
        float time = Random.Range(0f, 0.1f);
        anim.SetFloat("Speed", speed);
        //anim.SetFloat("Time", time);
        // .wrapMode = WrapMode.PingPong;
    }
        

    // Update is called once per frame
    void Update()
    {
        


    }
}
