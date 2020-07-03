using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public string location;
    Animator animator;
    public int Animation;

    void Awake()
    {
        animator = GetComponent<Animator>();
       
    }

    // Start is called before the first frame update
    void Start()
    {
        Trigger.trigger += startAnimation;
        animator.SetInteger("Animation", Animation);
    }



    void OnDestroy()
    {
        Trigger.trigger -= startAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startAnimation(string location)
    {
        if (this.location == location)
        {
            animator.SetBool("canStart", true);
        }
    }



}
