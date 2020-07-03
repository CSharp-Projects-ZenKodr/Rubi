﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;
    public GameObject SFX;


    protected virtual void Awake()
    {
        
    }

    // Start is called before the first frame update
    protected virtual void  Start()
    {


    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void GetDamage(Attack.ATTACK a)
    {

    }



}
