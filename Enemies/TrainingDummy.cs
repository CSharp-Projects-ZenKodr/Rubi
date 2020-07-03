using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : Enemy
{
    Animator animator;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        health = 9999;

        kEffect_ = StatusEffect.NULL;

        animator = GetComponent<Animator>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


    }

    void OnTriggerEnter(Collider other) //When something enters a trigger
    {

    }


    void OnTriggerExit(Collider other) //When something exists a trigger
    {

    }

    public override void GetDamage(Attack.ATTACK a)
    {
        base.GetDamage(a);

        if (a.damageTaken > 50)
        {
            animator.SetTrigger("Damaged");
        }

        switch (a.element)
        {
            case global::Attack.Element.FIRE:
           //     health -= a.damageTaken;
           //     kEffect_ = StatusEffect.BURNED;
                break;
            case global::Attack.Element.WATER:
           //     health += a.damageTaken;
                break;
            case global::Attack.Element.PHYSICAL:
            //    health -= a.damageTaken;
            //    if (canBeStunned_) stunHealth_ += a.damageTaken;
                break;
        }

    }

}
