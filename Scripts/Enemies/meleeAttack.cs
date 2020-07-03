using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : Attack
{
    bool attacking;
    bool recovery;
    bool engaged;
    public float attackRange;
    Vector3 attackEnd; //Point where the attack comes back
    Vector3 initialPosition;
    public float attackSpeed;

    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        recovery = false;
        engaged = false;
        damage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (attacking)
        {
            engaged = true;
            Vector3 forward = transform.parent.transform.forward;
            forward.Normalize();
            attackEnd = initialPosition + forward * attackRange;
            Vector3 dir = transform.parent.transform.forward;
            dir.Normalize();

            transform.position += dir * attackSpeed * Time.deltaTime;

            float distanceToEnd = Vector3.Magnitude(transform.position - attackEnd);

            if (distanceToEnd < 0.1)
            {
                attacking = false;
                recovery = true;

            }
        }
        else if (recovery)
        {
            engaged = true;
            Vector3 dir = transform.parent.position - transform.position;
            float distanceToEnd = Vector3.Magnitude(dir);
            dir.Normalize();

            transform.position += dir * attackSpeed * Time.deltaTime;

            

            if (distanceToEnd < 0.1)
            {
                attacking = false;
                recovery = false;
            }

        }
        else
        {
            engaged = false;
        }

    }

    void OnTriggerEnter(Collider other) //When something enters a trigger
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ATTACK a;
            a.damageTaken = damage;
            a.element = Element.PHYSICAL;
            a.force = force;
            Vector3 dir = transform.forward + transform.up;
            a.damageDirection = dir;

            Player p = other.gameObject.GetComponent<Player>();
            p.GetDamage(a);
        }
    }

    public bool attack(int Damage, int force)
    {
        if (engaged) return false;

        attacking = true;
        engaged = true;
        damage = Damage;
        this.force = force;
        initialPosition = transform.position;

        return true;
    }

    public bool Engaged()
    {
        return engaged;
    }
}
