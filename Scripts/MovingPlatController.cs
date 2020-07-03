using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatController : MonoBehaviour
{
    public int Animation;
    Animator animator;
    public bool startFromBeginning;

    void Awake()
    {
        animator = GetComponent<Animator>();

        animator.SetInteger("Animation", Animation);
        animator.SetBool("canStart", startFromBeginning);

        MovingPlat plat = GetComponentInChildren<MovingPlat>();
       if (plat != null) plat.setStart(startFromBeginning);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
    }

    public void startMoving()
    {
        animator.SetBool("canStart", true);
    }



}
