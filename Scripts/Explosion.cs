using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Attack
{
    SphereCollider collider_;
    public float speed;
    ParticleSystem system;
    public float powerX, powerY;

    // Start is called before the first frame update
    void Start()
    {
        collider_ = GetComponent<SphereCollider>();
        system = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        collider_.radius += speed * Time.deltaTime;


        if (!system.isPlaying)
        {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            ATTACK a;
            a.damageTaken = damage;
            a.element = Element.FIRE;
            a.force = force;
            Vector3 dir = c.gameObject.transform.position - transform.position;          
            
            a.damageDirection = dir;


            Player p = c.gameObject.GetComponent<Player>();
            p.GetDamage(a);
        }
        if (c.gameObject.CompareTag("Enemy"))
        {
            ATTACK a;
            a.damageTaken = damage;
            a.element = Element.FIRE;
            a.force = force;

            Vector3 dir = Vector3.zero;

            a.damageDirection = dir;

            //c.SendMessage("GetDamage", a);

            Enemy e = c.gameObject.GetComponent<Enemy>();
            e.GetDamage(a);

        }



    }
}
